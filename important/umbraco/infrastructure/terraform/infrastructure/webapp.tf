data "azurerm_linux_web_app" "umbapp" {
  name                = "umbracoapp10"
  resource_group_name = "umbraco-rg"
}

resource "azurerm_app_service_custom_hostname_binding" "admin" {
  hostname            = aws_route53_record.admin.fqdn
  app_service_name    = data.azurerm_linux_web_app.umbapp.name
  resource_group_name = data.azurerm_linux_web_app.umbapp.resource_group_name
}

resource "azurerm_app_service_custom_hostname_binding" "customer" {
  hostname            = aws_route53_record.customer.fqdn
  app_service_name    = data.azurerm_linux_web_app.umbapp.name
  resource_group_name = data.azurerm_linux_web_app.umbapp.resource_group_name
}

/*
resource "azurerm_service_plan" "umbplan" {
  name                = "${var.appServiceName}-plan"
  location            = azurerm_resource_group.umbracorg.location
  resource_group_name = azurerm_resource_group.umbracorg.name

  os_type             = "Linux"
  sku_name            = "P1v2"
}

resource "azurerm_linux_web_app" "umbapp" {
  name                = var.appServiceName
  location            = azurerm_resource_group.umbracorg.location
  resource_group_name = azurerm_resource_group.umbracorg.name
  service_plan_id     = azurerm_service_plan.umbplan.id
  
  site_config {
    application_stack {
        dotnet_version = "v6.0"
    }
    worker_count = 1
  }

  client_affinity_enabled = true
}
*/
