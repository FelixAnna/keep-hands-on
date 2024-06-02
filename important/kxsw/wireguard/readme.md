# Wireguard VPN in azure

If you need to access the global internet for learning purpose, you can setup your own vpn server, here is the guide for wireguard vpn.

## Provision Infrastructure
Apply the **azure_arm.json** file in any resource group to create virtual machine, network, open firewall for ssh(22), udp(like 51820), and admin ui(80).

Provision by Azure Portal
1. in azure portal, select an empty resource group or create one **in a nearby region**
2. open the resource group, scroll down to the bottom
3. click "Automation -> Export template", wait until the default template load complete
4. click "Deploy", then "Edit template", paste with content from **azure_arm.json**
5. fill the parameters, then review and create, wait until complete

Provision by Azure Command-Line Interface (CLI):
```
## login and then switch subscription if you have multiple subscriptions
az account set --subscription "your-azure-subscription-id"
```

```
## start provisioning

## prepare westus.sh with following parameters
##    username=admin
##    password=Passw0rd1
##    resourceGroup=test-rg
##    location=westus
##    udpport=51820
##    tcpport=80

sh deploy_azure.sh westus

```

## Deploy Wireguard

After the infrastructure deployed, you will get some **command** for this step, just follow it to login to the vm, setup docker and then start the container:

## Access the Admin UI

After the infrastructure deployed, you will also get the **admin url**, now you can open the Admin UI, input the password and login.

After you success login, you can add client profile, and you phone can scan the QR code to add the profile to local client (You need WireGuard client mobile app installed, please searh from google play).

You can use the similar way to add client for PC (still need you download the client app: https://www.wireguard.com/install/)


* If your udpport is not the default value(51820), you need edit in the client profile you downloaded to change to the actual udp port you specified.

## Close Unused firewall rules

It is necessary to close the ssh and tcp port after you finished configured all the clients, so you vpn server and clients stay safe.

1. Go to the resource group in azure portal
2. select "vpnservernsg" (Network security group)
3. click "Inbound security rules"
4. click "default-tcp" and "default-ssh", change their action to "Deny" and save

If you want to add new client or connect to the server later, you need to Allow those 2 rules in advance.

## Delete the resource

If you don't need the vpn service any more, just delete all from the resource group

1. Go to the resource group in azure portal
2. Click "Delete resource group" and confirm, wait until it complete

```
az group delete -n $resourceGroup --force-deletion-types Microsoft.Compute/virtualMachines
```

## Refer

The content here are based on existing popular repositories:

wireguard reference: [wg-easy](https://github.com/wg-easy/wg-easy)

arm reference: [setup-ipsec-vpn](https://github.com/hwdsl2/setup-ipsec-vpn)
