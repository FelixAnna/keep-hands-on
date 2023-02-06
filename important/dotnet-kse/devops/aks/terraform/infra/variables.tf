variable "rgName" {
    type = string
    description = "(optional) resource group to deploy your infrastructure"
    default="kse_rg"
}

variable "location" {
    type = string
    description = "(optional) Location"
    default = "eastus"
}

variable "clusterName" {
    type = string
    description = "(optional) azure kubernetes cluster name"
    default = "kseCluster"
}

variable "ipaddrName" {
    type = string
    description = "(optional) public ip address name for application gateway"
    default = "nginxIp"
}

variable "ns" {
    type = string
    description = "(optional) kubernetes namespace to deploy our microservices"
    default = "ksens"
}

variable "backendDNS" {
    type = string
    description = "(optional) dns record to binding to gateway ipaddress"
    default = "api-kse.metadlw.com"
}

variable "tags" {
    type = map
    description = "(optional) tags for resources"
    default = {
        Application = "kse"
        Group = "kse_rg"
    }
}
