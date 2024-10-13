using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;

using CyberBilby.Shared.Security;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CyberBilby.MgmtServer.Services;

namespace CyberBilby.MgmtServer;

class Program()
{
    static async Task Main(string[] args)
    {
        GenerateTestCertificates();

        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddHostedService<ManagementServiceHost>();

        var app = builder.Build();
        await app.StartAsync();
        await app.WaitForShutdownAsync();
    }

    static void GenerateTestCertificates()
    {
        if(!File.Exists(@"./server.pfx") ||
            !File.Exists(@"./client.pfx"))
        {
            Console.WriteLine("Generating server and client test certificates..");

            var rootCert = X509Cert2.CreateRootCertificate("cyberbilby.com");
            var clientCert = X509Cert2.CreateCertificate(rootCert, "testuser");

            var rootPfx = rootCert.Export(X509ContentType.Pfx);
            File.WriteAllBytes(@"./server.pfx", rootPfx);

            var clientPfx = clientCert.Export(X509ContentType.Pfx);
            File.WriteAllBytes(@"./client.pfx", clientPfx);

            Console.WriteLine("Generated.");
        }
    }
}