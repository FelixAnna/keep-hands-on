# KSE Project

A project for dotnet KSE

## Tools

1. install git: https://git-scm.com/downloads;
2. install \[dotnet 7 sdk\] (https://dotnet.microsoft.com/en-us/download/dotnet/7.0);
3. install [nodejs](https://nodejs.org/en/download/) and [npm](https://www.npmjs.com/package/npm); ####
4. install and configure [azure cli](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli);
5. install [kubectl](https://kubernetes.io/docs/tasks/tools/);
6. install [helm](https://helm.sh/docs/intro/install/);
7. install [terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli);
8. install [consul](https://developer.hashicorp.com/consul/downloads?host=www.consul.io).

---

1. \[optional\] install [postman](https://www.postman.com/downloads/);
2. \[optional\] install [draw.io](https://github.com/jgraph/drawio-desktop/releases);
3. \[optional\] install [docker](https://www.docker.com/);
4. \[optional\] install [kind](https://kubernetes.io/docs/tasks/tools/#kind);
5. \[optional\] install and configure [aws cli](https://aws.amazon.com/cli/).

## Projects

After you download the code from the code repo, you can debug them in your local env.

### backend

open the "backend" folder in vs code, to check it works, run:

```
dotnet restore && dotnet build
```

(Optional) start consul 
```
consul agent -dev
```

start local instances by running(**IDP service need to start first**):
```
## start all services in the background
kill $(jobs -p) &
dotnet run --project ./EStore.IdentityServer/EStore.IdentityServer.csproj &
dotnet run --project ./EStore.CartAPI/EStore.CartAPI.csproj &
dotnet run --project ./EStore.OrderAPI/EStore.OrderAPI.csproj &
dotnet run --project ./EStore.ProductAPI/EStore.ProductAPI.csproj &
dotnet run --project ./EStore.UserAPI/EStore.UserAPI.csproj &
jobs -p
```
After that, you need to open the below URL in a browser: **https://localhost:7214**, so the IDP service starts working

## DevOps

in "devops" folder, you can find how to deploy the microservices to azure Kubernetes service.

### Docker build & push to azure container registry

```
  ## if you have docker

  tag=latest

  docker build -t kse-idp-api:$tag -f EStore.IdentityServer/Dockerfile . 
  docker build -t kse-cart-api:$tag -f EStore.CartAPI/Dockerfile . 
  docker build -t kse-order-api:$tag -f EStore.OrderAPI/Dockerfile . 
  docker build -t kse-product-api:$tag -f EStore.ProductAPI/Dockerfile . 
  docker build -t kse-user-api:$tag -f EStore.UserAPI/Dockerfile . 
  
  docker image tag kse-idp-api:$tag hssdevacr.azurecr.io/kse-idp-api:$tag
  docker image push hssdevacr.azurecr.io/kse-idp-api:$tag
  
  docker image tag kse-cart-api:$tag hssdevacr.azurecr.io/kse-cart-api:$tag
  docker image push hssdevacr.azurecr.io/kse-cart-api:$tag
  
  docker image tag kse-order-api:$tag hssdevacr.azurecr.io/kse-order-api:$tag
  docker image push hssdevacr.azurecr.io/kse-order-api:$tag
  
  docker image tag kse-product-api:$tag hssdevacr.azurecr.io/kse-product-api:$tag
  docker image push hssdevacr.azurecr.io/kse-product-api:$tag
  
  docker image tag kse-user-api:$tag hssdevacr.azurecr.io/kse-user-api:$tag
  docker image push hssdevacr.azurecr.io/kse-user-api:$tag
```
```
  ## if you do not have docker
    
  tag=latest
  
  az acr build -t kse-idp-api:$tag -f EStore.IdentityServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t kse-cart-api:$tag -f EStore.CartAPI/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t kse-order-api:$tag -f EStore.OrderAPI/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t kse-product-api:$tag -f EStore.ProductAPI/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t kse-user-api:$tag -f EStore.UserAPI/Dockerfile -r hssdevacr -g hss-configuration .

 ```
 
### microservices helm chart

microservice helm chart is located in "./kse-chart".

### aks deployment

deploy to aks with "prod" argument will create valid cert by cert-manager, and use consul as service register,

```
## deploy (need AWS CLI configured)
cd aks
sh install.sh prod  ## prod/dev
```

```
## destroy (need AWS CLI configured)
cd aks
sh uninstall.sh prod  ## prod/dev
```

```
## install/upgrade our microservices only (don't need to configure AWS CLI)
cd aks/services
sh main_services.sh prod ## prod/dev
```

### local deployment

There is another folder "./kse-chart-nossl" which is for deploying to a local kind cluster, it doesn't depend on cert-manager, and consul.

follow: [./devops/kind/readme.md](./devops/kind/readme.md)

## other

### Terraform Backend
- [Backend](https://developer.hashicorp.com/terraform/language/settings/backends/azurerm)

###  Consul
- [Consul](https://github.com/hashicorp/consul)
- [Consul and kubernetes deployment guide](https://developer.hashicorp.com/consul/tutorials/kubernetes/kubernetes-deployment-guide)

###  Ingress
- [Nginx](https://kubernetes.github.io/ingress-nginx/)
- [Create an ingress controller in AKS](https://learn.microsoft.com/en-us/azure/aks/ingress-basic?tabs=azure-cli#create-an-ingress-controller)

### cert-manager
- [Cert-Manager](https://artifacthub.io/packages/helm/cert-manager/cert-manager)
- [CustomResourceDefinition prerequisite](https://artifacthub.io/packages/helm/cert-manager/cert-manager#installing-the-chart)

