using System.Net.Security;

namespace CyberBilby.Shared.Network;

public static class StreamExtensions
{
    public static void SendPacket(this SslStream stream, IPacket packet)
    {
        stream.Write(packet.Serialize());
    }

    public static async Task SendPacketAsync(this SslStream stream, IPacket packet)
    {
        await stream.WriteAsync(packet.Serialize());
    }
}
