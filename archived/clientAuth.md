// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

// Load the client certificate from the personal store
X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
store.Open(OpenFlags.ReadOnly);
X509Certificate2Collection certs = store.Certificates.Find(
    X509FindType.FindBySubjectName, "ASP.NET Core HTTPS development certificate", false);
X509Certificate2 cert = certs[0];

// Create an HTTP client with the client certificate
HttpClientHandler handler = new()
{
    SslProtocols = System.Security.Authentication.SslProtocols.Tls12
};
handler.ClientCertificates.Add(cert);
HttpClient client = new(handler);

// Send an HTTP request to the URL
HttpResponseMessage response = await client.GetAsync("https://your-url.com");

// Print the response content
string content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);
