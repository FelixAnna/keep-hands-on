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


using System.Runtime.InteropServices;

// This method gets the Outlook cookies from the Internet Explorer cookie store
public string GetOutlookCookies()
{
    string url = "https://outlook.office.com";
    string cookieName = "MSPAuth";

    int size = 4096;
    StringBuilder cookieData = new StringBuilder(size);

    bool success = InternetGetCookieEx(url, cookieName, cookieData, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero);
    if (!success)
    {
        int error = Marshal.GetLastWin32Error();
        throw new Exception("Failed to get cookie: " + error);
    }

    return cookieData.ToString();
}

[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
private static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, int flags, IntPtr reserved);

private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
