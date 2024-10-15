using CyberBilby.Shared.Network;
using System.Text;
using System.Text.Json;

namespace CyberBilby.MgmtServer.Network;

public class AuthPacket : IPacket
{
    public PacketType PacketType { get; } = PacketType.SMSG_AUTH;

    public required AuthProfile Profile { get; set; }

    public byte[] Serialize()
    {
        var ms = new MemoryStream();

        ms.Write(BitConverter.GetBytes((int)this.PacketType), 0, sizeof(int));

        var json = JsonSerializer.Serialize<AuthProfile>(Profile);
        var data = Encoding.UTF8.GetBytes(json);

        ms.Write(BitConverter.GetBytes(data.Length), 0, sizeof(int));
        ms.Write(data);

        return ms.ToArray();
    }
}
