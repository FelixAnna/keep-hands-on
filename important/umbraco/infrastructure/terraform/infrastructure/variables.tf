variable "rgName" {
    type = string
    description = "(optional) resource group to deploy your infrastructure"
    default="umbraco-rg"
}

variable "location" {
    type = string
    description = "(optional) Location"
    default = "eastus"
}

variable "identityNameGw" {
    type = string
    description = "(optional) name of the user managed identity"
    default = "appgwIdentity"
}

variable "valutName" {
    type = string
    description = "(optional) name of key vault"
    default = "umbVault"
}

variable "ipaddrName" {
    type = string
    description = "(optional) public ip address name for application gateway"
    default = "umbAppGWIp"
}

variable "vnetName" {
    type = string
    description = "(optional) name of virtual network for application gateway"
    default = "appGWVnet"
}

variable "frontSubnetName" {
    type = string
    description = "(optional) subnet name to deploy application gateway"
    default = "gwSubnet"
}

variable "appgwName" {
    type = string
    description = "(optional) name of the applicatoon gateway"
    default = "umbAppGateway"
}

variable "appServiceName" {
    type = string
    description = "(optional) name of the web app"
    default = "umb-app"
}

variable "admin_record" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress (admin)"
    default = "umb-admin"
}

variable "customer_record" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress (customer)"
    default = "umb"
}

variable "admin_instance_id" {
    type = string
    description = "(optional) admin instance id for admin site"
    default = "37935eb6fafb49a468fb68d92286356e22c8bc7a579b471826676d3dcd718252"
}

variable "sslCertName" {
    type = string
    description = "(optional) ssl cert name in application gateway http listener"
    default = "umbkvsslcert"
}

variable "tags" {
    type = map
    description = "(optional) tags for resources"
    default = {
        Application = "umb"
        Group = "umbraco-rg"
    }
}
