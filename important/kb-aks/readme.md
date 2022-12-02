# demo deploy existing micro-service to aks

## provision and de-provision infrastructure

## install basic services

#### Azure CLI
- [Install](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- [Login](https://learn.microsoft.com/en-us/cli/azure/authenticate-azure-cli)

#### Terraform
- [Install](https://developer.hashicorp.com/terraform/downloads)
- [Backend](https://developer.hashicorp.com/terraform/language/settings/backends/azurerm)

####  Consul
- [Consul](https://github.com/hashicorp/consul)
- [Consul and kubernetes deployment guide](https://developer.hashicorp.com/consul/tutorials/kubernetes/kubernetes-deployment-guide)

####  Ingress
- [Nginx](https://kubernetes.github.io/ingress-nginx/)
- [Create an ingress controller in AKS](https://learn.microsoft.com/en-us/azure/aks/ingress-basic?tabs=azure-cli#create-an-ingress-controller)

#### cert-manager
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