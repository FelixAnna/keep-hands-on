
## Setup With SQL Server

Please setup SQL Database in azure, and allow your ip to access or allow connections from a special vnet.

Install umbraco and required plugins, following: [start.sh](./start.sh) or [UMBRACO PACKAGES SCRIPT WRITER](https://psw.codeshare.co.uk/)

## Use Azure Blob Storage for Media files

Setup Azure Blob Storage in azure, and then following:
[Setup Your Site to use Azure Blob Storage for Media and ImageSharp Cache](https://our.umbraco.com/documentation/Extending/FileSystemProviders/Azure-Blob-Storage/)

## Build & Deploy

Recommand deployment target is Azure Web App (App services), 
configure first following: 
[Running Umbraco on Azure Web Apps](https://our.umbraco.com/Documentation/Fundamentals/Setup/server-setup/azure-web-apps)

Then use visual studio to publish to a existing/new web app, please keep the **Cookie ARRAffinity** enabled in configurations.


## Admin / Back Office

"You will designate a single server to be the backoffice server for which your editors will log into for editing content.
Umbraco will not work correctly if the backoffice is behind the load balancer."

Refer: https://our.umbraco.com/Documentation/Fundamentals/Setup/server-setup/Load-Balancing/

To make backoffice single instance,we can:


### [Recommanded] Deploy to only one site, use single instance for admin from application gateway

The first step is to get the list of instance names.we can use Resource Explorer (https://resources.azure.com/) to get one instance ID for admin.

    In Resource Explorer, find your Web App (in the tree or using search box)
    Under the app, click on Instances, which gives you an array of instances. Each instance has a long name like xxxx
    example path:
    https://resources.azure.com/subscriptions/xxx/resourceGroups/umbraco-rg/providers/Microsoft.Web/sites/umbracoapp10/instances/xxxxxxxxx

 
 Once we have the instance names, you can add a cookie in your requests to aim at a specific instance by setting the ARRAffinity cookie to that value.

    ARRAffinity=xxxx;Path=/;HttpOnly;Domain=yourdomain.com

Known Issue: 

    target server broken/scale down might impect availiability of admin: 
    refer: https://learn.microsoft.com/en-us/answers/questions/39015/azure-app-service-arr-affinity-auto-scaling-statef.html,
    
    We can update the cookie value after that changed(automatically or manually)

Apply Infrastructure changes:

Use the [terraform code](./infrastructure/terraform/profiles/dev/) to provision the infrastructure:

After that provided, binded domain(customer & admin) to the webapp, then you can access admin and customer sites.

* requestion to admin site always point to one of the single server you configured(instance id is ARRAffinity value)
* request to admin site with customer router have no restrictions, it can point to any instances(you can restrict it, but i didn't apply it for now)

* request to customer site can reach to any instances in the web app
* request to customer site with admin router will redirect to home page

-----------
### [Not Recommand] Deploy 2 web apps, use uSync plugin
use uSync plugin to export templates(or more) to a file, then import to other sides.

this need manual work, also need open admin function on both site.


### [Not Recommand] Deploy 2 web apps, sync folders
Web app use share files to host web sites insides, one of them with admin function blocked (ex: by application gateway)
We have checked there is not command to special a same share folder(ftp folder) when deploy/create a web app,
So we can use ftp sync tools (like FTP Sychronizer) to sync between 2 folders, 
only sync: /site/wwwroot/Views, and /site/wwwroot/wwwroot 2 folders,(not quite sure)
the initial sync is slow(1 hour at least), but incremental sync can be finished in minutes

this option can be down automaticaly, but due to cache issue, the site used by users need manually refresh when update existing template.
