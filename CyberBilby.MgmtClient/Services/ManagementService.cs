using CyberBilby.Shared.Network;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace CyberBilby.MgmtClient.Services;

public class ManagementService
{
    private TcpClient _tcpClient;
    private SslStream _sslStream;

    public ManagementService()
    {
        _tcpClient = new TcpClient();
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
}
