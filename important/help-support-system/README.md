# Help Support System

A Help Support System where people can chat in real time

## Tools

1. install git: https://git-scm.com/downloads;
2. install \[dotnet 6 sdk\] (https://dotnet.microsoft.com/en-us/download/dotnet/6.0);
3. install [nodejs](https://nodejs.org/en/download/) and [npm](https://www.npmjs.com/package/npm);
4. install and configure [azure cli](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli);
5. install [kubectl](https://kubernetes.io/docs/tasks/tools/);
6. install [helm](https://helm.sh/docs/intro/install/);
7. install [terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli);
8. install [consul](https://developer.hashicorp.com/consul/downloads?host=www.consul.io).

---

1. \[optional\] install [postman](https://www.postman.com/downloads/);
2. \[optional\] install [draw.io](https://github.com/jgraph/drawio-desktop/releases);
3. \[optional\] install [docker](https://www.docker.com/);
4. \[optional\] install [kind](https://kubernetes.io/docs/tasks/tools/#kind);
5. \[optional\] install and configure [aws cli](https://aws.amazon.com/cli/).

## Software

1. install vscode with extensions: C#, dart, flutter;
2. install [flutter, android SDK or android studio](https://docs.flutter.dev/get-started/install);


## Projects

After you download the code from the code repo, you can debug them in your local env.

### backend

open the "backend" folder in vs code, to check it works, run:

```
dotnet restore && dotnet build
```

(Optional) start consul 
```
consul agent -dev
```

start local instances by running(**IDP service need to start first**):
```
npx kill-port 7283 5226 7133 5133 5268 5266 8443 5075 7075 7076
```

```
## start all services in the background

kill $(jobs -p) &
dotnet run --project ./HSS.IdentityServer/HSS.IdentityServer.csproj &
dotnet run --project ./HSS.HubServer/HSS.HubServer.csproj &
dotnet run --project ./HSS.Admin/HSS.Admin.csproj &
dotnet run --project ./HSS.UserApi/HSS.UserApi.csproj &
dotnet run --project ./HSS.MessageApi/HSS.MessageApi.csproj &
jobs -p
```
After that, you need to open the below URL in a browser: **https://localhost:7283**, so the IDP service starts working


### Flutter

flutter code in the "FlutterClient\\signalr_demo_app" folder, open it in ide.

1. duplicate env/.env.dist to 3 copies: .env, prod.env, stage.env;
2. change the URL in stage.env to your local address, like:

```
hubApiAddress=https://api-prod-hss.metadlw.com/hub
userApiAddress=https://api-prod-hss.metadlw.com/user
messageApiAddress=https://api-prod-hss.metadlw.com/message
idpAuthority=idp-prod-hss.metadlw.com
```

3. Press **Control + shift + P**, select **Tasks: Run Task**, Select **Build APK Prod** or **Build APK Stage** to update the ".env" configure file.
4. select ./lib/main.dart, then start debugging your app.

### DevOps

in "devops" folder, you can find how to deploy the microservices to azure Kubernetes service.

#### microservices helm chart

microservice helm chart is located in "./hss-chart".

#### aks deployment

deploy to aks with "prod" argument will create valid cert by cert-manager, and use consul as service register,

```
## deploy (need AWS CLI configured)
cd aks
sh install.sh prod  ## prod/dev
```

```
## destroy (need AWS CLI configured)
cd aks
sh uninstall.sh prod  ## prod/dev
```

```
## install/upgrade our microservices only (don't need to configure AWS CLI)
cd aks/services
sh main_services.sh prod hss ## prod/dev, appName: hss
```

#### local deployment

There is another folder "./hss-chart-nossl" which is for deploying to a local kind cluster, it doesn't depend on cert-manager, and consul.

#### Docker build & push to azure container registry

```
  ## if you have docker
  
  tag=0.4.2

  docker build -t hss-idp-api:$tag -f HSS.IdentityServer/Dockerfile . 
  docker build -t hss-hub-api:$tag -f HSS.HubServer/Dockerfile . 
  docker build -t hss-user-api:$tag -f HSS.UserApi/Dockerfile .
  docker build -t hss-message-api:$tag -f HSS.MessageApi/Dockerfile .
  docker build -t hss-admin:$tag -f HSS.Admin/Dockerfile .
  docker build -t hss-signalrdemo-api:$tag -f HSS.SignalRDemo/Dockerfile .


  docker image tag hss-idp-api:$tag hssdevacr.azurecr.io/hss-idp-api:$tag
  docker image push hssdevacr.azurecr.io/hss-idp-api:$tag

  docker image tag hss-hub-api:$tag hssdevacr.azurecr.io/hss-hub-api:$tag
  docker image push hssdevacr.azurecr.io/hss-hub-api:$tag

  docker image tag hss-user-api:$tag hssdevacr.azurecr.io/hss-user-api:$tag
  docker image push hssdevacr.azurecr.io/hss-user-api:$tag

  docker image tag hss-message-api:$tag hssdevacr.azurecr.io/hss-message-api:$tag
  docker image push hssdevacr.azurecr.io/hss-message-api:$tag

  docker image tag hss-admin:$tag hssdevacr.azurecr.io/hss-admin:$tag
  docker image push hssdevacr.azurecr.io/hss-admin:$tag

  docker image tag hss-signalrdemo-api:$tag hssdevacr.azurecr.io/hss-signalrdemo-api:$tag
  docker image push hssdevacr.azurecr.io/hss-signalrdemo-api:$tag
```
```
  ## if you do not have docker
    
  tag=0.4.2
  
  az acr build -t hss-idp-api:$tag -f HSS.IdentityServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-hub-api:$tag -f HSS.HubServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-user-api:$tag -f HSS.UserApi/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-message-api:$tag -f HSS.MessageApi/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-admin:$tag -f HSS.Admin/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-signalrdemo-api:$tag -f HSS.SignalRDemo/Dockerfile -r hssdevacr -g hss-configuration .

 ```