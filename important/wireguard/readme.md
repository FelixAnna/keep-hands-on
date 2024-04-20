# Wireguard VPN in azure

If you need to access the global internet for learning purpose, you can setup your own vpn server, here is the guide for wireguard vpn.

## Provision Infrastructure
Apply the **azure_arm.yml** file in any resource group to create virtual machine, network, open firewall for ssh(22), udp(51820), and admin ui(80).

1. in azure portal, select an empty resource group or create one **in a nearby region**
2. open the resource group, scroll down to the bottom
3. click "Automation -> Export template", wait until the default template load complete
4. click "Deploy", then "Edit template", paste with content from **azure_arm.yml**
5. fill the parameters, then review and create, wait until complete

## Deploy Wireguard

After the infrastructure deployed, you can get the public ip of the vpnserver, also remember the parameter

## Refer

The content here are based on existing popular repositories:

wireguard reference: [wg-easy](https://github.com/wg-easy/wg-easy)

arm reference: [setup-ipsec-vpn](https://github.com/hwdsl2/setup-ipsec-vpn)