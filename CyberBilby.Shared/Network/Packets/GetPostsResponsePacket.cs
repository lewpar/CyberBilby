using CyberBilby.Shared.Database.Entities;
using CyberBilby.Shared.Network;

using System.Text.Json;
using System.Text;

namespace CyberBilby.MgmtServer.Network;

public class GetPostsResponsePacket : IPacket
{
    public PacketType PacketType => PacketType.SMSG_GET_POSTS;

    public required List<BlogPost> Posts { get; set; }

    public byte[] Serialize()
    {
        var ms = new MemoryStream();

        // Packet Type
        ms.Write(BitConverter.GetBytes((int)this.PacketType), 0, sizeof(int));

        var json = JsonSerializer.Serialize<List<BlogPost>>(Posts);
        var data = Encoding.UTF8.GetBytes(json);

        // Packet Length
        ms.Write(BitConverter.GetBytes(data.Length), 0, sizeof(int));

        // Packet Data
        ms.Write(data);

        return ms.ToArray();
    }
}
