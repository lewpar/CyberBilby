using CyberBilby.MgmtClient.Events;
using CyberBilby.MgmtClient.Network;

using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Extensions;
using CyberBilby.Shared.Network;

using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace CyberBilby.MgmtClient.Services;

public class ManagementService
{
    private TcpClient _tcpClient;
    private SslStream? _sslStream;

    private Dictionary<PacketType, Func<SslStream, Task>> packetHandlers;
    private readonly ILogger<ManagementService> logger;

    public event EventHandler<RespondPostsEventArgs> OnRespondPosts;

    private CancellationTokenSource _cancellationTokenSource;
    private CancellationToken _cancellationToken;

    public ManagementService(ILogger<ManagementService> logger)
    {
        _tcpClient = new TcpClient();
        packetHandlers = new Dictionary<PacketType, Func<SslStream, Task>>()
        {
            { PacketType.SMSG_POSTS, HandleRespondPostsAsync }
        };
        this.logger = logger;

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public async Task ConnectToMgmtServerAsync(string host, int port, X509Certificate2 localCertificate)
    {
        _tcpClient.Connect(host, port);
        _sslStream = new SslStream(_tcpClient.GetStream(), false);

        await _sslStream.AuthenticateAsClientAsync(new SslClientAuthenticationOptions()
        {
            ClientCertificates = new X509CertificateCollection()
            {
                localCertificate
            },
            CertificateRevocationCheckMode = X509RevocationMode.NoCheck,
            TargetHost = host,
        });
    }

    public async Task<AuthProfile> AuthenticateAsync()
    {
        if(_sslStream is null)
        {
            throw new Exception("Tried to authenticate client with unitialize ssl stream.");
        }

        byte[] packetTypeBuf = new byte[sizeof(int)];
        await _sslStream.ReadAsync(packetTypeBuf);

        int packetType = BitConverter.ToInt32(packetTypeBuf);
        if(packetType != (int)PacketType.SMSG_AUTH)
        {
            throw new Exception($"Received unexpected packet type '{packetType}' when '{(int)PacketType.SMSG_AUTH}' expected.");
        }

        byte[] jsonLenBuf = new byte[sizeof(int)];
        await _sslStream.ReadAsync(jsonLenBuf);

        int jsonLen = BitConverter.ToInt32(jsonLenBuf);
        if(jsonLen <= 0)
        {
            throw new Exception("Received invalid data length for auth profile.");
        }

        byte[] jsonData = new byte[jsonLen];
        await _sslStream.ReadAsync(jsonData, 0, jsonLen);

        var ms = new MemoryStream(jsonData);

        var profile = await JsonSerializer.DeserializeAsync<AuthProfile>(ms);
        if(profile is null)
        {
            throw new Exception("Failed to deserialize auth profile.");
        }

        return profile;
    }

    public async Task StartListeningAsync()
    {
        if(_sslStream is null)
        {
            throw new Exception("Failed to start listening for data: SslStream is null.");
        }

        _ = ListenForDataAsync(_sslStream);
    }

    private async Task ListenForDataAsync(SslStream stream)
    {
        try
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var packetType = (PacketType)await stream.ReadIntAsync(_cancellationToken);
                if (!packetHandlers.ContainsKey(packetType))
                {
                    throw new Exception($"Received invalid packet type '{packetType}' from server.");
                }

                await packetHandlers[packetType].Invoke(stream);
            }
        }
        catch(Exception ex)
        {
            logger.LogCritical($"An error occured while trying to handle incoming packet: {ex.Message} {ex.StackTrace}");
        }
    }

    public async Task RequestPostsAsync()
    {
        if (_sslStream is null)
        {
            throw new Exception("Tried to authenticate client with unitialize ssl stream.");
        }

        logger.LogInformation("Requesting posts from server");

        await _sslStream.SendPacketAsync(new RequestPostsPacket());
    }

    private async Task HandleRespondPostsAsync(SslStream stream)
    {
        logger.LogInformation("Received posts from server");

        var dataLength = await stream.ReadIntAsync();
        byte[] jsonData = new byte[dataLength];

        await stream.ReadAtLeastAsync(jsonData, dataLength);

        var ms = new MemoryStream(jsonData);

        var posts = await JsonSerializer.DeserializeAsync<List<BlogPost>>(ms);
        if(posts is null)
        {
            throw new Exception("Failed to deserialize posts.");
        }

        OnRespondPosts?.Invoke(this, new RespondPostsEventArgs(posts));
    }
}
