# Wireguard VPN 部署到微软云

如果因为学习的原因（外面的世界也有很多造谣者，切勿轻信），你需要访问外网，这里是怎么搭建 Wireguard VPN 的方法.

## 创建和配置服务器
应用 **azure_arm.json** 到任意的azure（微软云） resource group, 以便创建 virtual machine, network, 打开防火墙端口：ssh(22), udp(如：51820), and admin ui(如：80).

从Azure portal 创建：
1. 打开portal.azure.com并登陆, 选择或创建新的 resource group （region 可以选择附近的，以确保延迟不大）；
2. 打开resource group 后，滚动到最后；
3. 点击 "Automation -> Export template", 等待默认模板加载完毕；
4. 点击 "Deploy", 然后 "Edit template", 把里面的内容替换为 **azure_arm.json**的内容；
5. 选择或者输入变量, 然后检查并开始创建, 等待创建结束。

使用 Azure Command-Line Interface (CLI)创建:
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

## 安装 Wireguard

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

## Restart
az vm start --resource-group $resourceGroup --name vpnserver
az vm stop --resource-group $resourceGroup --name vpnserver --no-wait
