
## refer

certbot: https://certbot.eff.org/instructions?ws=other&os=ubuntufocal&tab=wildcard
router53: https://certbot-dns-route53.readthedocs.io/en/stable/

## install certbot and dns plugin
```
sudo snap install --classic certbot
sudo snap set certbot trust-plugin-with-root=ok
sudo snap install certbot-dns-route53
```
## Config AWS credentials
```
## inside config is the aws id and key:
## [default]
## aws_access_key_id=AKIAIOSFODNN7EXAMPLE
## aws_secret_access_key=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY

export AWS_CONFIG_FILE=/home/felix/.aws/config
```
## generate cert
```
certbot certonly   --dns-route53   -d metadlw.com   -d www.metadlw.com
```

