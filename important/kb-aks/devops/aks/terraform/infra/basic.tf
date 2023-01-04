resource "azurerm_resource_group" "demo_rg" {
  name     = var.rgName
  location = var.location
  tags = var.tags
}

data "azurerm_container_registry" "hss_acr" {
  name                = "hssdevacr"
  resource_group_name = "configuration-rg"
}

data "azurerm_user_assigned_identity" "nodepoolIdentity" {
  resource_group_name = azurerm_kubernetes_cluster.demoCluster.node_resource_group

  name = "${azurerm_kubernetes_cluster.demoCluster.name}-agentpool"
}

resource "azurerm_role_assignment" "acr-pull-role" {
    scope = data.azurerm_container_registry.hss_acr.id
    principal_id                      = data.azurerm_user_assigned_identity.nodepoolIdentity.principal_id
    role_definition_name = "AcrPull"
    skip_service_principal_aad_check = true
}

resource "azurerm_role_assignment" "acr-sign-role" {
    scope = data.azurerm_container_registry.hss_acr.id
    principal_id                      = data.azurerm_user_assigned_identity.nodepoolIdentity.principal_id
    role_definition_name = "AcrImageSigner"
    skip_service_principal_aad_check = true
}

resource "azurerm_public_ip" "gwIp" {
  resource_group_name = azurerm_kubernetes_cluster.demoCluster.node_resource_group
  location            = azurerm_resource_group.demo_rg.location

  name                = var.ipaddrName
  allocation_method   = "Static"
  sku = "Standard"
}
