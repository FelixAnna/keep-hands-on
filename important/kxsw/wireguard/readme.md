# Wireguard VPN in azure

If you need to access the global internet for learning purpose, you can setup your own vpn server, here is the guide for wireguard vpn.

## Provision Infrastructure
Apply the **azure_arm.json** file in any resource group to create virtual machine, network, open firewall for ssh(22), udp(51820), and admin ui(80).

1. in azure portal, select an empty resource group or create one **in a nearby region**
2. open the resource group, scroll down to the bottom
3. click "Automation -> Export template", wait until the default template load complete
4. click "Deploy", then "Edit template", paste with content from **azure_arm.json**
5. fill the parameters, then review and create, wait until complete

```
username=admin123
password=Passw0rd
resourceGroup=my-rg
location=westus

az account set --subscription "xxxx-xxxx-xxxx-xxxx-xxxxxxxx"

az group create --name $resourceGroup --location $location

az deployment group create --name 'ExampleDeployment'$(date +"%d-%b-%Y") --resource-group $resourceGroup --template-uri "https://raw.githubusercontent.com/FelixAnna/keep-hands-on/master/important/kxsw/wireguard/azure_arm.json"  --parameters username=$username password=$password

```

## Deploy Wireguard

After the infrastructure deployed, you can get the public ip of the vpnserver, also remember the parameters you input before deployment, then login to the vm by ssh and then start the container:

```
## open command line, and connect to the vpnserver by ssh

ssh -i ~/.ssh/id_rsa.pem <TheUserNameYouInput>@<ThePublicIpAddress>
```

```
## Start the vpn and admin services in the vpnserver
## replace <ThePublicIpAddress> and <ThePasswordYouInput> with correct value

docker run -d \
  --name=wg-easy \
  -e LANG=en \
  -e WG_HOST=<ThePublicIpAddress> \
  -e PASSWORD=<ThePasswordYouInput> \
  -v ~/.wg-easy:/etc/wireguard \
  -p 51820:51820/udp \
  -p 80:51821/tcp \
  --cap-add=NET_ADMIN \
  --cap-add=SYS_MODULE \
  --sysctl="net.ipv4.conf.all.src_valid_mark=1" \
  --sysctl="net.ipv4.ip_forward=1" \
  --restart unless-stopped \
  ghcr.io/wg-easy/wg-easy
  ```

## Access the Admin UI

Now you can open the Admin UI, by accessing the http://<ThePublicIpAddress>, input the password when login.

After you success login, you can add client profile, and you phone can scan the QR code to add the profile to local client (You need WireGuard client mobile app installed, please searh from google play).

You can use the similar way to add client for PC (still need you download the client app: https://www.wireguard.com/install/)

## Close Unused firewall rules

It is necessary to close the ssh and tcp port after you finished configured all the clients, so you vpn server and clients stay safe.

1. Go to the resource group in azure portal
2. select "vpnservernsg" (Network security group)
3. click "Inbound security rules"
4. click "default-tcp-80" and "default-ssh", change their action to "Deny" and save

If you want to add new client or connect to the server later, you need to Allow those 2 rules in advance.

## Delete the resource

If you don't need the vpn service any more, just delete all from the resource group

1. Go to the resource group in azure portal
2. Click "Delete resource group" and confirm, wait until it complete


## Refer

The content here are based on existing popular repositories:

wireguard reference: [wg-easy](https://github.com/wg-easy/wg-easy)

arm reference: [setup-ipsec-vpn](https://github.com/hwdsl2/setup-ipsec-vpn)
