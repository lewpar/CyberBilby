using System.Net.Sockets;

namespace CyberBilby.MgmtServer.Events;

public class ClientRejectedEventArgs : EventArgs
{
    public TcpClient Client { get; set; }
    public RejectionReason Reason { get; set; }

    public ClientRejectedEventArgs(TcpClient client, RejectionReason reason)
    {
        Client = client;
        Reason = reason;
    }
}
