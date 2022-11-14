# demo deploy existing micro-service to aks

## provision and de-provision infrastructure

## install basic services

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
## install our services

## buid CI/CD pipeline

## demo

## thank you