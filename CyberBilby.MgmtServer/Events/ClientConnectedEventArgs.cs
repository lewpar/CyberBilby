using System.Net.Sockets;

namespace CyberBilby.MgmtServer.Events;

public class ClientConnectedEventArgs : EventArgs
{
    public TcpClient Client { get; set; }

    public ClientConnectedEventArgs(TcpClient client)
    {
        Client = client;
    }
}
