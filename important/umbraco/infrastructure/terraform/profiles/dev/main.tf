terraform {
  # need setup backend in azure storage account first
  backend "azurerm" {
    resource_group_name  = "configuration-rg"
    storage_account_name = "configstoragefelix"
    container_name       = "tfstate"
    key                  = "umbraco.terraform.tfstate"
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
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

provider "aws" {
  region = "us-east-1"
}

locals {
  environment_name = "dev"
}

module "infrastructure" {
  source = "../../infrastructure"

  # Input Variables
  rgName = "umbraco-rg2"
  tags = {
      Application = "umbraco"
      Group = "umbraco"
      Environment="${local.environment_name}"
  }
}
