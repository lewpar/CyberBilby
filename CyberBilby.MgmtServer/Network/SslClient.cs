using System.Net;
using System.Net.Security;
using System.Net.Sockets;

namespace CyberBilby.MgmtServer.Network;

public class SslClient
{
    public required EndPoint? Endpoint { get; set; }
    public required string Fingerprint { get; set; }
    public required SslStream Stream { get; set; }
    public required TcpClient Client { get; set; }

    public async Task DisconnectAsync(string message = "")
    {

    }
}
