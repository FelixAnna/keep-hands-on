trojan-go官方文档：https://p4gefau1t.github.io/trojan-go/

trojan配置文件：
{
    "run_type": "server",
    "local_addr": "0.0.0.0",
    "local_port": 443,
    "remote_addr": "192.83.167.78",
    "remote_port": 80,
    "password": [
        "your_awesome_password"
    ],
    "ssl": {
        "cert": "server.crt",
        "key": "server.key"
    }
}

申请证书：
    安装acme：curl https://get.acme.sh | sh
    安装socat：apt install socat
    添加软链接：ln -s  /root/.acme.sh/acme.sh /usr/local/bin/acme.sh
    注册账号： acme.sh --register-account -m my@example.com
    开放80端口：ufw allow 80
    申请证书： acme.sh  --issue -d 你的域名  --standalone -k ec-256
    安装证书： acme.sh --installcert -d 你的域名 --ecc  --key-file   /root/trojan/server.key   --fullchain-file /root/trojan/server.crt 
 
    如果默认CA无法颁发，则可以切换下列CA：
    切换 Let’s Encrypt：acme.sh --set-default-ca --server letsencrypt
    切换 Buypass：acme.sh --set-default-ca --server buypass
    切换 ZeroSSL：acme.sh --set-default-ca --server zerossl


自签证书：
    生成私钥：openssl ecparam -genkey -name prime256v1 -out ca.key
    生成证书：openssl req -new -x509 -days 36500 -key ca.key -out ca.crt  -subj "/CN=bing.com"