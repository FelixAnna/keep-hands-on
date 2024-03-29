variable "rgName" {
    type = string
    description = "(optional) resource group to deploy your infrastructure"
    default="hss_rg"
}

variable "location" {
    type = string
    description = "(optional) Location"
    default = "eastus"
}

variable "identityNameCluster" {
    type = string
    description = "(optional) name of the user managed identity"
    default = "clusterIdentity"
}

variable "clusterName" {
    type = string
    description = "(optional) azure kubernetes cluster name"
    default = "hssCluster"
}

variable "ipaddrName" {
    type = string
    description = "(optional) public ip address name for application gateway"
    default = "nginxIp"
}

variable "ns" {
    type = string
    description = "(optional) kubernetes namespace to deploy our microservices"
    default = "hssns"
}

variable "backendDNS" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress"
    default = "api-hss.metadlw.com"
}

variable "idpDNS" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress"
    default = "idp-hss.metadlw.com"
}

variable "AdminDNS" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress"
    default = "admin-hss.metadlw.com"
}

variable "tags" {
    type = map
    description = "(optional) tags for resources"
    default = {
        Application = "hss"
        Group = "hss_rg"
    }
}
