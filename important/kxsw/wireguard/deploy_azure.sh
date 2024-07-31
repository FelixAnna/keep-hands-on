region=$1

echo "start provisioning"

source d:/code/config/wg/$1.sh

## username=admin
## password=Passw0rd1
## resourceGroup=wg-$1-rg
## location=$1
## udpport=51820
## tcpport=80

echo "resource group: $resourceGroup, tcpport:$tcpport, udbport: $udpport, username: $username"

az group create --name $resourceGroup --location $location

echo "if following step fails, please following the menu guide to create the VPN server first, then skip following step and try again"
#az deployment group create --name 'ExampleDeployment'$(date +"%d-%b-%Y") --resource-group $resourceGroup --template-uri "https://raw.githubusercontent.com/FelixAnna/keep-hands-on/master/important/kxsw/wireguard/azure_arm.json"  --parameters username=$username password=$password udpport=$udpport tcpport=$tcpport 

publicIpAddress=$(az network public-ip show -g $resourceGroup -n vpnserverpip --query "ipAddress" --out tsv)

connnectvm="ssh $username@$publicIpAddress"
install_docker="curl -fsSL https://get.docker.com -o get-docker.sh && sudo sh get-docker.sh && sudo usermod -aG docker $username && exit"
start_wg="docker run -d --name=wg-easy -e LANG=en -e WG_HOST=$publicIpAddress -e PASSWORD=$password -v ~/.wg-easy:/etc/wireguard -p $udpport:51820/udp -p $tcpport:51821/tcp --cap-add=NET_ADMIN --cap-add=SYS_MODULE --sysctl=\"net.ipv4.conf.all.src_valid_mark=1\" --sysctl=\"net.ipv4.ip_forward=1\" --restart unless-stopped ghcr.io/wg-easy/wg-easy"
admin_address="http://$publicIpAddress:$tcpport/"

echo "========================================================================================================"
echo "====================Please run following command from shell (local or cloud shell)======================"
echo "========================================================================================================"

echo "$connnectvm"
echo "$install_docker"

echo "$connnectvm"
echo "$start_wg"

echo "========================================================================================================"
echo "===========================Please open below admin url and add client==================================="
echo "========================================================================================================"

echo "$admin_address"

##ssh -f $username@$publicIpAddress "$install_docker"
##ssh -f $username@$publicIpAddress $start_wg
