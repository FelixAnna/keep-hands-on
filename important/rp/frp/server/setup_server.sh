 
mkdir frp
cd frp
wget https://github.com/fatedier/frp/releases/download/v0.57.0/frp_0.57.0_linux_amd64.tar.gz -O frp057.gz
mkdir 057
tar -xvf frp057.gz -C 057

cd 057/frp_0.57.0_linux_amd64

mkdir server
cd server
mv ../frps* ./

cat > frps.toml << EOF

bindPort = 7000

transport.maxPoolCount = 5
vhostHTTPPort = 8080
vhostHTTPSPort = 8443

#transport.tls.enable = true
#transport.tls.certFile = "certificate.crt"
#transport.tls.keyFile = "certificate.key"
#transport.tls.trustedCaFile = "ca.crt"

webServer.addr = "0.0.0.0"
webServer.port = {{ .Envs.ADMIN_PORT }}
# dashboard's username and password are both optional
webServer.user = "{{ .Envs.ADMIN_USER }}"
webServer.password = "{{ .Envs.ADMIN_PASSWORD }}"

# webServer.tls.certFile = "server.crt"
# webServer.tls.keyFile = "server.key"

EOF

cat > start_server.sh << EOF
#!/bin/sh

export ADMIN_PORT=9090
export ADMIN_USER=admin
export ADMIN_PASSWORD=admin

./frps -c ./frps.toml

EOF

sh start_server.sh

## set as system deamon
sudo bash
touch /lib/systemd/system/frp.service
##
## copy and update from the [frp.service](./server/frp.service)
vim /lib/systemd/system/frp.service
##
systemctl enable frp.service --now