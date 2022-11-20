resource "azurerm_resource_group" "demo_rg" {
  name     = var.rgName
  location = var.location
  tags = var.tags
}

resource "azurerm_public_ip" "gwIp" {
  resource_group_name = azurerm_kubernetes_cluster.demoCluster.node_resource_group
  location            = azurerm_resource_group.demo_rg.location

  name                = var.ipaddrName
  allocation_method   = "Static"
  sku = "Standard"
}
