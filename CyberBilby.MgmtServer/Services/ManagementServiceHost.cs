using CyberBilby.Shared.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace CyberBilby.MgmtServer.Services;

public class ManagementServiceHost : BackgroundService
{
    private TcpListener _listener;
    private readonly ILogger<ManagementServiceHost> logger;

    public ManagementServiceHost(ILogger<ManagementServiceHost> logger)
    {
        _listener = new TcpListener(IPAddress.Any, 44123);
        this.logger = logger;
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

    private async Task HandleClientConnectAsync(TcpClient client)
    {
        var sslStream = new SslStream(client.GetStream(), false);
        await sslStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions()
        {
            ServerCertificate = X509Cert2.LoadFromFile("./server.pfx"),
            ClientCertificateRequired = true
        });
    }
}
