# Wireguard VPN 部署到微软云

如果因为学习的原因，您需要访问外网，这里是搭建 Wireguard VPN 的方法。由于特殊原因，您搭建的VPN 过一段时间就会被封锁，这时候您只需要删除所有资源并重新搭建即可。

出于好奇心的请慎重，外面的世界有很多造谣者，假新闻机构，切勿轻信！

## 创建服务器
部署 **azure_arm.json** 到任意的azure（微软云）resource group, 以便创建 virtual machine, network, 打开防火墙端口：ssh(22), udp(如：51820), 和 tcp(如：80).

从Azure portal 创建：

1. 打开portal.azure.com并登陆, 选择或创建新的 resource group （region 可以选择附近的，延迟小）；
2. 打开resource group 后，滚动到最后；
3. 点击 "Automation -> Export template", 等待默认模板加载完毕；
4. 点击 "Deploy", 然后 "Edit template", 把里面的内容替换为 **azure_arm.json** 的内容；
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

## 配置服务器

在执行完上面的命令（deploy_azure.sh）之后, 您会拿到接下来要执行的**命令** 和要访问的**链接**, 按顺序执行，就会安装好docker 和启动VPN服务。

## 配置客户端

最后访问输出的链接，输入您开始设置的密码。登陆之后就可以添加客户端（Client），手机可以扫码设置好客户端，电脑可以通过下载客户端配置文件导入设置（手机端和电脑端都需要装WireGuard VPN: https://www.wireguard.com/install/)


* 如果部署VPN服务时 udpport 不是默认值(51820), 您需要在客户端配置中把51820改成您设置的端口，才能连接成功。

## 关闭防火墙端口

当一切设置妥当之后，您可以关闭防火墙端口，以便没有人可以通过网页端、远程访问您的VPN服务器。

1. 打开您VPN所在的 resource group
2. 选择资源： "vpnservernsg" (Network security group)
3. 点击： "Inbound security rules"
4. 分别点击： "default-tcp" and "default-ssh", 把他们的 action 改成 "Deny" 并保存。

如果稍后您想要添加新的Client，您需要再打开这些防火墙端口（把 action 改成Allow）

## 删除资源

如果您不再需要这个VPN服务，可以直接删除它：

1. 打开您VPN所在的 resource group
2. 点击 "Delete resource group" 并确定, 等待删除完成提示。

```
az group delete -n $resourceGroup --force-deletion-types Microsoft.Compute/virtualMachines
```


## 重启

为了节省开销，在您不需要VPN的时候，您可以停止VPN服务器，可以在VPN Server 资源上通过Automation -> Task 来完成， 也可用下面命令手动重启。

```
az vm start --resource-group $resourceGroup --name vpnserver
az vm stop --resource-group $resourceGroup --name vpnserver --no-wait
```

## 切换端口
```

## 拿到conatinerId
## Stop the running Container
## 修改端口
## 重启容器

oldPort=1522
newPort=1523

containerId=$(docker ps -q)
docker stop $containerId
cd /var/lib/docker/containers/
cd $containerId
sed s/$oldPort/$newPort/g hostconfig.json -i
systemctl restart docker
docker start $containerId

## 在Azure Portal 上打开新的（如：1523）防火墙端口
## 在客户端软件上修改端口号（如：1523）重试链接

```

## Refer

The content here are based on existing popular repositories:

wireguard reference: [wg-easy](https://github.com/wg-easy/wg-easy)

arm reference: [setup-ipsec-vpn](https://github.com/hwdsl2/setup-ipsec-vpn)
