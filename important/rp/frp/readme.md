# Set up reverse proxy by frp

## about frp
It is an open source reverse proxy solution: https://github.com/fatedier/frp, we can access resources deployed in a private network with a reverse proxy as a jump server.

## Prequisite

To set up the reverse proxy, you need a virtual machine with public ip address as a server, and one or more client inside LAN with only private ip address. I use a VM in azure with public ip address for example here.

Open port: 7000,8080,9090,8443 in the firewall of the virtual machine with public ip.

## Server setup

Login to the server by ssh, and follow the [setup_server.sh](./server/setup_server.sh) to setup and start the service in server.

(The fpr might have newer version, check： https://github.com/fatedier/frp/releases )

## Client setup

Download the client frpc.exe from: https://github.com/fatedier/frp/releases for your operation system and architecture.

Then download everything from [client](./client/) folder, update the environment variables in [start_client.sh](./client/start_client.sh), include "FRP_SERVER_ADDR" refer to the public ip address of your VM, and update the CNAME to your own dns name.

Add dns record in the domain provider for the subdomain you just configured， then execute:

```
sh start_client.sh
```

If not error, below url should work:

Dashboard: http://yourIpAddress:9090  (input the username and password, default are admin and admin )
Website: http://yourDNSRecord:8080 or https://yourDNSRecord:8443

