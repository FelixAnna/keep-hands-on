# Help Support System

A Help Support System where people can chat in real time

## Getting started
1. install azure cli, and configure;
2. install and configure helm;
3. install and configure kubectl (client only);

4. configure terraform;
5. configure aws cli (for dns record manager).


## Docker build & push to azure container registry

```sh
  az acr build -t hss-idp-api:0.2.3 -f HSS.IdentityServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-hub-api:0.2.3 -f HSS.HubServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-user-api:0.2.3 -f HSS.UserApi/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-signalrdemo-api:0.2.4 -f HSS.SignalRDemo/Dockerfile -r hssdevacr -g hss-configuration .
```

## Deploy & update
```sh
cd devops/aks
sh install dev ## dev or prod

## update our service only
cd services
sh hss_service.sh dev ## dev or prod
```

## Destory
```
cd devops/aks
sh uninstall.sh dev ## dev or prod
```

## Known issues:

1. Password login only return access_token by default, doesn't return id_token: 
[OpenID Connect does not specify the resource owner flow](https://stackoverflow.com/questions/41421160/how-to-get-id-token-along-with-access-token-from-identityserver4-via-password)
