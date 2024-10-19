using CyberBilby.Shared.Network;

namespace CyberBilby.MgmtClient.Network;

public class GetPostsRequestPacket : IPacket
{
    public PacketType PacketType => PacketType.CMSG_GET_POSTS;

    public byte[] Serialize()
    {
        var ms = new MemoryStream();

        ms.Write(BitConverter.GetBytes((int)this.PacketType), 0, sizeof(int));

        return ms.ToArray();
    }
}
