# Help Support System

A Help Support System where people can chat in real time

## Tools

1. install git: https://git-scm.com/downloads;
2. install \[dotnet 6 sdk\] (https://dotnet.microsoft.com/en-us/download/dotnet/6.0);
3. install [nodejs](https://nodejs.org/en/download/) and [npm](https://www.npmjs.com/package/npm);
4. install and configure [azure cli](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli);
5. install [kubectl](https://kubernetes.io/docs/tasks/tools/);
6. install [helm](https://helm.sh/docs/intro/install/);
7. install [terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli).

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

start local instances by running(**IDP service need to start first**):

```
## start all services in the background
kill $(jobs -p) &
dotnet run --project ./HSS.IdentityServer/HSS.IdentityServer.csproj &
dotnet run --project ./HSS.HubServer/HSS.HubServer.csproj &
dotnet run --project ./HSS.SignalRDemo/HSS.SignalRDemo.csproj &
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
hubApiAddress=https://172.31.160.1:7133
userApiAddress=http://172.31.160.1:5269
idpAuthority=172.31.160.1:8443
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
sh hss_services.sh prod ## prod/dev
```

#### local deployment

There is another folder "./hss-chart" which is for deploying to a local kind cluster, it doesn't depend on cert-manager, and consul.

#### Docker build & push to azure container registry
```
  az acr build -t hss-idp-api:0.4.0 -f HSS.IdentityServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-hub-api:0.4.1 -f HSS.HubServer/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-user-api:0.4.1 -f HSS.UserApi/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-message-api:0.4.1 -f HSS.MessageApi/Dockerfile -r hssdevacr -g hss-configuration .
  az acr build -t hss-signalrdemo-api:0.4.1 -f HSS.SignalRDemo/Dockerfile -r hssdevacr -g hss-configuration .

  docker build -t hss-idp-api:0.4.0 -f HSS.IdentityServer/Dockerfile . 
  docker build -t hss-hub-api:0.4.0 -f HSS.HubServer/Dockerfile . 
  docker build -t hss-user-api:0.4.0 -f HSS.UserApi/Dockerfile .
  docker build -t hss-message-api:0.4.0 -f HSS.MessageApi/Dockerfile .
  docker build -t hss-signalrdemo-api:0.4.1 -f HSS.SignalRDemo/Dockerfile .
 ```