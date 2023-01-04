# Felix Technology Research

## Umbraco

Deploy umbraco CMS to azure web app following: [umbraco/readme.md](./umbraco/readme.md).

# Demo Project

A project for demo

## Tools

1. install git: https://git-scm.com/downloads;
2. install \[dotnet 6 sdk\] (https://dotnet.microsoft.com/en-us/download/dotnet/6.0);
3. install [nodejs](https://nodejs.org/en/download/) and [npm](https://www.npmjs.com/package/npm);
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


## DevOps

in "devops" folder, you can find how to deploy the microservices to azure Kubernetes service.

### Docker build & push to azure container registry

```
  ## if you have docker

  tag=latest

  cd kb-aks/src/product-service
  docker build -t demo-product-api:$tag -f Dockerfile . 
  docker image tag demo-product-api:$tag hssdevacr.azurecr.io/demo-product-api:$tag
  docker image push hssdevacr.azurecr.io/demo-product-api:$tag
  
  cd ../price-service
  docker build -t demo-price-api:$tag -f Dockerfile . 
  docker image tag demo-price-api:$tag hssdevacr.azurecr.io/demo-price-api:$tag
  docker image push hssdevacr.azurecr.io/demo-price-api:$tag
```
```
  ## if you do not have docker
    
  tag=latest
  cd kb-aks/src/product-service
  az acr build -t demo-product-api:$tag -f Dockerfile -r hssdevacr -g configuration-rg .
  cd ../price-service
  az acr build -t demo-price-api:$tag -f Dockerfile -r hssdevacr -g configuration-rg .

 ```
 
### microservices helm chart

microservice helm chart is located in "./demo-chart".

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

There is another folder "./demo-chart-nossl" which is for deploying to a local kind cluster, it doesn't depend on cert-manager, and consul.

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

----
-- TODO

ansible:
need work on linux(of subsystem): https://stackoverflow.com/questions/45228395/error-no-module-named-fcntl 
example: https://www.ansible.com/blog/automating-helm-using-ansible, https://docs.ansible.com/ansible/latest/getting_started/get_started_playbook.html

sudo apt-get update
sudo apt install python3

curl https://bootstrap.pypa.io/get-pip.py -o get-pip.py
python3 get-pip.py --user

sudo apt-get install python3-distutils --reinstall
sudo python3 -m pip install --user ansible

export PATH="/root/.local/bin:$PATH"
source .profile

ansible --version
python3 -m pip show ansible

apt install git

install helm: https://helm.sh/docs/intro/install/ 

install azure cli:
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
## install our services

## buid CI/CD pipeline

## demo

## thank you
