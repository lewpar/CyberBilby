using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using CyberBilby.MgmtServer.Services;

using CyberBilby.Shared.Database;
using CyberBilby.Shared.Configuration;
using CyberBilby.Shared.Security;

namespace CyberBilby.MgmtServer;

class Program()
{
    static async Task Main(string[] args)
    {
        GenerateTestCertificates();

        var builder = Host.CreateApplicationBuilder();

        DotEnv.Load(Environment.CurrentDirectory);
        DotEnv.Ensure("MYSQL_CONNECTION");

        var connectionString = DotEnv.Get("MYSQL_CONNECTION");

        builder.Services.AddHostedService<ManagementServiceHost>();
        builder.Services.AddDbContext<BilbyDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        var app = builder.Build();

        await app.StartAsync();
        await app.WaitForShutdownAsync();
    }

    static void GenerateTestCertificates()
    {
        if(!File.Exists(@"./server.pfx") ||
            !File.Exists(@"./client.pfx") ||
            !File.Exists(@"./chain.pfx"))
        {
            Console.WriteLine("Generating server and client test certificates..");

            var rootCert = X509Cert2.CreateRootCertificate("localhost");
            var clientCert = X509Cert2.CreateCertificate(rootCert, "testuser");

            var rootPfx = rootCert.Export(X509ContentType.Pfx);
            File.WriteAllBytes(@"./server.pfx", rootPfx);

            var clientPfx = clientCert.Export(X509ContentType.Pfx);
            File.WriteAllBytes(@"./client.pfx", clientPfx);

            var chainPfx = new X509Certificate2Collection()
            {
                /* Exports the root cert without private key */
                new X509Certificate2(rootCert.Export(X509ContentType.Cert)),

                clientCert,
            }.Export(X509ContentType.Pfx);

            if(chainPfx is not null)
            {
                File.WriteAllBytes(@"./chain.pfx", chainPfx);
            }

            Console.WriteLine("Generated.");
        }
    }
}