using CyberBilby.MgmtServer.Events;

using CyberBilby.Shared.Extensions;
using CyberBilby.Shared.Network;

using System.Net;
using System.Net.Security;
using System.Net.Sockets;

using System.Security.Cryptography.X509Certificates;

namespace CyberBilby.MgmtServer.Network;

public class ManagementServer
{
    public event EventHandler<ClientConnectedEventArgs>? ClientConnected;
    public event EventHandler<ClientAuthenticatedEventArgs>? ClientAuthenticated;
    public event EventHandler<ClientRejectedEventArgs>? ClientRejected;

    public Func<X509Certificate2, Task<bool>>? CertificateValidationCallback;

    private TcpListener _listener;
    private Dictionary<PacketType, Func<SslClient, Task>> _handlers;
    private List<SslClient> _connectedClients;
    private readonly X509Certificate2 serverCertificate;
    private CancellationToken _stoppingToken;

    public ManagementServer(IPAddress ipAddress, int port, X509Certificate2 serverCertificate)
    {
        _listener = new TcpListener(ipAddress, port);
        _handlers = new Dictionary<PacketType, Func<SslClient, Task>>();
        _connectedClients = new List<SslClient>();
        this.serverCertificate = serverCertificate;
    }

    public void RegisterHandler(PacketType packetType, Func<SslClient, Task> handler)
    {
        if(_handlers.ContainsKey(packetType))
        {
            return;
        }

        _handlers.Add(packetType, handler);
    }

    public async Task ListenAsync(CancellationToken stoppingToken)
    {
        _listener.Start();
        _stoppingToken = stoppingToken;

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync();

            _ = HandleClientConnectAsync(client);

            ClientConnected?.Invoke(this, new ClientConnectedEventArgs(client));
        }
    }

    private async Task HandleClientConnectAsync(TcpClient client)
    {
        try
        {
            var sslStream = new SslStream(client.GetStream(), false);
            await sslStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions()
            {
                ServerCertificate = serverCertificate,
                ClientCertificateRequired = true
            });

            var certificate = sslStream.RemoteCertificate as X509Certificate2;
            if (certificate is null)
            {
                return;
            }

            if (!await ValidateCertificateAsync(certificate))
            {
                sslStream.Close();
                return;
            }

            var sslClient = new SslClient()
            {
                Endpoint = client.Client.RemoteEndPoint,
                Fingerprint = certificate.Thumbprint,
                Stream = sslStream,
                Client = client
            };

            _connectedClients.Add(sslClient);

            ClientAuthenticated?.Invoke(this, new ClientAuthenticatedEventArgs(sslClient));

            await ListenForDataAsync(sslClient);
        }
        catch (Exception)
        {
            ClientRejected?.Invoke(this, new ClientRejectedEventArgs(client, RejectionReason.AuthenticationFailed));
        }
    }

    private async Task<bool> ValidateCertificateAsync(X509Certificate2? certificate)
    {
        if (certificate is null)
        {
            return false;
        }

        if(CertificateValidationCallback is not null)
        {
            return await CertificateValidationCallback(certificate);
        }

        return true;
    }

    private async Task ListenForDataAsync(SslClient client)
    {
        while (!_stoppingToken.IsCancellationRequested)
        {
            var stream = client.Stream;

            var packetType = (PacketType)await stream.ReadIntAsync(_stoppingToken);
            if (!_handlers.ContainsKey(packetType))
            {
                await client.DisconnectAsync("Invalid packet type.");
                break;
            }

            await _handlers[packetType].Invoke(client);
        }
    }
}
