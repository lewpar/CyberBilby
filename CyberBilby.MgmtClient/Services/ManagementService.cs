using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace CyberBilby.MgmtClient.Services;

public class ManagementService
{
    private TcpClient _tcpClient;

    public ManagementService()
    {
        _tcpClient = new TcpClient();
    }

    public async Task ConnectToMgmtServerAsync(string host, int port, X509Certificate2 localCertificate)
    {
        _tcpClient.Connect(host, port);

        var sslStream = new SslStream(_tcpClient.GetStream(), false);
        await sslStream.AuthenticateAsClientAsync(new SslClientAuthenticationOptions()
        {
            ClientCertificates = new X509CertificateCollection()
            {
                localCertificate
            },
            CertificateRevocationCheckMode = X509RevocationMode.NoCheck,
            TargetHost = host,
        });
    }
}
