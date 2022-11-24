terraform {
  # need setup backend in azure storage account first
  backend "azurerm" {
    resource_group_name  = "hss-configuration"
    storage_account_name = "hssconfigstore"
    container_name       = "tfstate"
    key                  = "hss-dev.nginx.tfstate"
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "3.20.0"
    }

    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  features  {
  }
}

provider "aws" {
  region = "us-east-1"
}

locals {
  environment_name = "dev"
}

module "infrastructure" {
  source = "../../infra"

  # Input Variables
  clusterName = "${local.environment_name}Cluster"
  rgName = "hss-${local.environment_name}-rg"
  backendDNS = "api-${local.environment_name}-hss.metadlw.com"
  idpDNS = "idp-${local.environment_name}-hss.metadlw.com"
  signalRDemoDNS="demo-${local.environment_name}-hss.metadlw.com"
  tags = {
      Application = "hss"
      Group = "hss"
      Environment="${local.environment_name}"
  }
}
