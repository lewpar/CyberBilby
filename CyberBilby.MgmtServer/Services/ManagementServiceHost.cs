using CyberBilby.MgmtServer.Network;
using CyberBilby.Shared.Database;
using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Network;
using CyberBilby.Shared.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Net.Security;
using System.Net.Sockets;

using System.Security.Cryptography.X509Certificates;

namespace CyberBilby.MgmtServer.Services;

public class ManagementServiceHost : BackgroundService
{
    private TcpListener _listener;
    private readonly ILogger<ManagementServiceHost> logger;
    private readonly BilbyDbContext dbContext;

    public ManagementServiceHost(ILogger<ManagementServiceHost> logger, BilbyDbContext dbContext)
    {
        _listener = new TcpListener(IPAddress.Any, 44123);
        this.logger = logger;
        this.dbContext = dbContext;
    }

    private async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting listen server..");

        _listener.Start();

        logger.LogInformation("Started. Waiting for clients..");

        while(!cancellationToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync();

            _ = HandleClientConnectAsync(client);

            logger.LogInformation($"Client '{client.Client.RemoteEndPoint}' connected.");
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartListeningAsync(stoppingToken);
    }

    private async Task<bool> ValidateCertificateAsync(X509Certificate2? certificate)
    {
        if(certificate is null)
        {
            return false;
        }

        var isRevoked = await dbContext.RevokedCertificates
            .AnyAsync(c => c.Fingerprint.ToLower() == certificate.Thumbprint.ToLower());

        if(isRevoked)
        {
            return false;
        }

        if(await GetAuthorAsync(certificate) is null)
        {
            return false;
        }

        return true;
    }

    private async Task<BlogAuthor?> GetAuthorAsync(X509Certificate2 certificate)
    {
        return await dbContext.Authors.FirstOrDefaultAsync(a => a.Fingerprint == certificate.Thumbprint);
    }

    private async Task DisconnectAsync(SslStream stream)
    {
        stream.Close();
    }

    private async Task HandleClientConnectAsync(TcpClient client)
    {
        try
        {
            var sslStream = new SslStream(client.GetStream(), false);
            await sslStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions()
            {
                ServerCertificate = X509Cert2.LoadFromFile("./server.pfx"),
                ClientCertificateRequired = true
            });

            var certificate = sslStream.RemoteCertificate as X509Certificate2;
            if (certificate is null)
            {
                return;
            }

            if(!await ValidateCertificateAsync(certificate))
            {
                await DisconnectAsync(sslStream);
                return;
            }

            var author = await GetAuthorAsync(certificate);
            if(author is null)
            {
                await DisconnectAsync(sslStream);
                return;
            }

            sslStream.SendPacket(new AuthPacket()
            {
                Profile = new AuthProfile()
                {
                    Name = author.Name,
                    Role = author.Role
                }
            });

            logger.LogInformation("Client passed authentication.");
        }
        catch(Exception ex)
        {
            logger.LogCritical($"Client failed authentication: {ex.Message}");
        }
    }
}
