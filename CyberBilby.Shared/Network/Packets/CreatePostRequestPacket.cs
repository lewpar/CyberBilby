using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Network;

using System.Text.Json;
using System.Text;

namespace CyberBilby.MgmtClient.Network;

public class CreatePostRequestPacket : IPacket
{
    public PacketType PacketType => PacketType.CMSG_CREATE_POST;

    public required BlogPost Post { get; set; }

    public byte[] Serialize()
    {
        var ms = new MemoryStream();

        ms.Write(BitConverter.GetBytes((int)this.PacketType), 0, sizeof(int));

        var json = JsonSerializer.Serialize<BlogPost>(Post);
        var data = Encoding.UTF8.GetBytes(json);

        // Packet Length
        ms.Write(BitConverter.GetBytes(data.Length), 0, sizeof(int));

        // Packet Data
        ms.Write(data);

        return ms.ToArray();
    }
}
