resource "azurerm_resource_group" "umbracorg" {
  name     = var.rgName
  location = var.location
  tags = var.tags
}

resource "azurerm_user_assigned_identity" "gwIdentity" {
  resource_group_name = azurerm_resource_group.umbracorg.name
  location = azurerm_resource_group.umbracorg.location

  name = var.identityNameGw
}

resource "azurerm_virtual_network" "gwVNet" {
  resource_group_name = azurerm_resource_group.umbracorg.name
  location            = azurerm_resource_group.umbracorg.location

  name                = var.vnetName
  address_space       = ["10.1.0.0/16"]
  tags = var.tags
}

resource "azurerm_subnet" "frontend" {
  name                 = var.frontSubnetName
  resource_group_name  = azurerm_resource_group.umbracorg.name
  virtual_network_name = azurerm_virtual_network.gwVNet.name
  address_prefixes     = ["10.1.0.0/24"]
}
