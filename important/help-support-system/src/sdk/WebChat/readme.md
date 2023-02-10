# SDK - WebChat

## Get Started

At first, enter into sdk/WebChat folder, and then type the followed commands:

1. Open a new terminal and type `npm run start:watch` to debug.

2. Open a new terminal and type `npm run start:serve` to boot a http server. 
_The default uri is `http://localhost:3100`_    

3. Open a new terminal and type `npm run start:mock` to boot a mock server. 
_The mock data is placed in `sdk/WebChat/mock/db.json`_

## Environment Variables

Local Environment, create file `.env.developement.local` that content as below and put it in `sdk/WebChat/`

```env
ENDPOINT=http://localhost:8080
HUB_API_SERVICE=https://api-prod-hss.metadlw.com/hub/chat
```

Online Environment, create file `.env.production.local` that content as below and put it in `sdk/WebChat/`

```env
HUB_API_SERVICE=https://api-prod-hss.metadlw.com/hub/chat
```