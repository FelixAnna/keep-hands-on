resource "azurerm_public_ip" "gwIp" {
  resource_group_name = azurerm_resource_group.umbracorg.name
  location            = azurerm_resource_group.umbracorg.location

  name                = var.ipaddrName
  allocation_method   = "Static"
  sku = "Standard"
}

locals {
  gateway_ip_configure_name      = "${azurerm_virtual_network.gwVNet.name}-gwipn"
  backend_address_pool_name      = "${azurerm_virtual_network.gwVNet.name}-beap"
  frontend_port_name             = "${azurerm_virtual_network.gwVNet.name}-feport"
  frontend_port_name_80             = "${azurerm_virtual_network.gwVNet.name}-feport80"
  frontend_ip_configuration_name = "${azurerm_virtual_network.gwVNet.name}-feip"
  http_setting_name              = "${azurerm_virtual_network.gwVNet.name}-be-htst"
  listener_name_admin                  = "${azurerm_virtual_network.gwVNet.name}-admin-lstn-443"
  listener_name_admin_80                 = "${azurerm_virtual_network.gwVNet.name}-admin-lstn-480"
  listener_name_customer_80                 = "${azurerm_virtual_network.gwVNet.name}-customer-lstn-480"
  request_routing_rule_name_admin      = "${azurerm_virtual_network.gwVNet.name}-admin-rqrt"
  request_routing_rule_name_admin_80      = "${azurerm_virtual_network.gwVNet.name}-admin-rqrt80"
  request_routing_rule_name_customer_80      = "${azurerm_virtual_network.gwVNet.name}-customer-rqrt80"
  probe_name ="${azurerm_virtual_network.gwVNet.name}-probe"
  rewrite_rule_set_name_admin = "${azurerm_virtual_network.gwVNet.name}-admin-rewrite-rule"
  rewrite_rule_set_name_customer = "${azurerm_virtual_network.gwVNet.name}-customer-rewrite-rule"
}

resource "azurerm_application_gateway" "appGW" {
  resource_group_name = azurerm_resource_group.umbracorg.name
  location            = azurerm_resource_group.umbracorg.location
  tags = var.tags

  name                = var.appgwName

  sku {
    name     = "Standard_v2"
    tier     = "Standard_v2"
    capacity = 1
  }

  gateway_ip_configuration {
    name      = local.gateway_ip_configure_name
    subnet_id = azurerm_subnet.frontend.id
  }

  frontend_port {
    name = local.frontend_port_name
    port = 443
  }

  frontend_port {
    name = local.frontend_port_name_80
    port = 80
  }

  frontend_ip_configuration {
    name                 = local.frontend_ip_configuration_name
    public_ip_address_id = azurerm_public_ip.gwIp.id
  }

  backend_address_pool {
    name = local.backend_address_pool_name
    fqdns = [data.azurerm_linux_web_app.umbapp.default_hostname]
  }
  
  identity {
    type = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.gwIdentity.id]
  }

  backend_http_settings {
    name                  = local.http_setting_name
    cookie_based_affinity = "Disabled"
    port                  = 80
    protocol              = "Http"
    request_timeout       = 15

    //pick_host_name_from_backend_address = true

    probe_name = local.probe_name
  }

  probe {
    host = data.azurerm_linux_web_app.umbapp.default_hostname
    interval = 30
    name = local.probe_name

    protocol = "Http"

    path = "/umbraco"
    timeout =  30
    unhealthy_threshold = 3
  }

  ssl_certificate {
    name = var.sslCertName
    key_vault_secret_id = azurerm_key_vault_certificate.sslcert.secret_id
  }

  //admin
  http_listener {
    name                           = local.listener_name_admin
    frontend_ip_configuration_name = local.frontend_ip_configuration_name
    frontend_port_name             = local.frontend_port_name
    protocol                       = "Https"

    host_name = aws_route53_record.admin.fqdn
    ssl_certificate_name = var.sslCertName
  }

  http_listener {
    name                           = local.listener_name_admin_80
    frontend_ip_configuration_name = local.frontend_ip_configuration_name
    frontend_port_name             = local.frontend_port_name_80
    protocol                       = "Http"

    host_name = aws_route53_record.admin.fqdn
  }

  //customer
  http_listener {
    name                           = local.listener_name_customer_80
    frontend_ip_configuration_name = local.frontend_ip_configuration_name
    frontend_port_name             = local.frontend_port_name_80
    protocol                       = "Http"

    host_name = aws_route53_record.customer.fqdn
  }

  //admin
  request_routing_rule {
    name                       = local.request_routing_rule_name_admin
    rule_type                  = "Basic"
    http_listener_name         = local.listener_name_admin
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.http_setting_name
    priority = 100

    rewrite_rule_set_name = local.rewrite_rule_set_name_admin
  }

  request_routing_rule {
    name                       = local.request_routing_rule_name_admin_80
    rule_type                  = "Basic"
    http_listener_name         = local.listener_name_admin_80
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.http_setting_name
    priority = 101

    rewrite_rule_set_name = local.rewrite_rule_set_name_admin
  }

  //customer
  request_routing_rule {
    name                       = local.request_routing_rule_name_customer_80
    rule_type                  = "Basic"
    http_listener_name         = local.listener_name_customer_80
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.http_setting_name
    priority = 102

    rewrite_rule_set_name = local.rewrite_rule_set_name_customer
  }

  //admin
  rewrite_rule_set {
    name = local.rewrite_rule_set_name_admin
    rewrite_rule {
      name = "admin_rule"

      rule_sequence = 1
      condition {
        ignore_case = true
        variable = "http_req_Cookie"  //this should be "Request Header -> Cookie"
        pattern ="(.)*${var.admin_instance_id}(.*)"
        negate = true
      }


      response_header_configuration {
        header_name = "Set-Cookie"
        header_value = "ARRAffinity=${var.admin_instance_id};Path=/;HttpOnly;Domain=${var.admin_record}.metadlw.com"
      }
    }
  }

  rewrite_rule_set {
    name = local.rewrite_rule_set_name_customer
    rewrite_rule {
      name = "customer_rule"

      rule_sequence = 2
      condition {
        ignore_case = true
        variable = "var_uri_path"
        pattern ="/umbraco(.*)"
      }

      url {
        path = "/"
        query_string = ""
      }
    }
  }
}
