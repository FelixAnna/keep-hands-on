data "azurerm_mssql_server" "sqlserver" {
  name                = "kse-server"
  resource_group_name = "dotnet-kse-rg"
}

/*
resource "azurerm_mssql_virtual_network_rule" "webapprule" {
  name                = "sql-vnet-rule-webapp"
  server_id           = data.azurerm_mssql_server.sqlserver.id
  subnet_id           = data.azurerm_linux_web_app.cmsapp.outbound_ip_address_list[0]
}

resource "azurerm_mssql_virtual_network_rule" "aksrule" {
  name                = "sql-vnet-rule-aks"
  server_id           = data.azurerm_mssql_server.sqlserver.id
  subnet_id           = azurerm_kubernetes_cluster.kseCluster.default_node_pool[0].vnet_subnet_id
}
*/