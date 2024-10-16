using System.Net.Security;
using System.Net.Sockets;

namespace CyberBilby.MgmtServer.Network;

public class SslClient
{
    public required SslStream Stream { get; set; }
    public required TcpClient Client { get; set; }

    public async Task DisconnectAsync(string message = "")
    {

    }
}
