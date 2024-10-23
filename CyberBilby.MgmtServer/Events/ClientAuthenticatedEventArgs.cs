using CyberBilby.MgmtServer.Network;

namespace CyberBilby.MgmtServer.Events;

public class ClientAuthenticatedEventArgs
{
    public SslClient Client { get; set; }

    public ClientAuthenticatedEventArgs(SslClient client)
    {
        Client = client;
    }
}
