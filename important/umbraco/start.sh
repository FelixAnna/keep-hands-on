# Ensure we have the latest Umbraco templates
dotnet new -i Umbraco.Templates

username="Administrator"
password="Passw0rd12"
email="yuecnu@hotmail.com"
connectionString="Your SQL Connection String"
# Create solution/project
dotnet new sln --name "EPAM.Umbraco.POC"
dotnet new umbraco -n "Umbraco.Site" \
 --friendly-name $username --email $email --password $password --connection-string "$connectionString" \
 --connection-string-provider-name "Microsoft.Data.SqlClient"
dotnet sln add "Umbraco.Site"

#Add starter kit
dotnet add "Umbraco.Site" package clean

#Add Packages
dotnet add "Umbraco.Site" package uSync
dotnet add "Umbraco.Site" package Umbraco.StorageProviders.AzureBlob

dotnet run --project "Umbraco.Site"
#Running
