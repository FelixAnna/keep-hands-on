// See https://aka.ms/new-console-template for more information
using IdentityModel;
using IdentityServer4.Models;

Console.WriteLine("Hello, World!");


var secret = new Secret("SomethingUnknown@1116".ToSha256());
Console.WriteLine(secret.Value);
