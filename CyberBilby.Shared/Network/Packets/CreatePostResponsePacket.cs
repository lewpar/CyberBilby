using CyberBilby.Shared.Network;
using System.Text;

namespace CyberBilby.MgmtServer.Network;

public class CreatePostResponsePacket : IPacket
{
    public PacketType PacketType => PacketType.SMSG_CREATE_POST;

    public required bool Result { get; set; }
    public required string Message { get; set; }

    public byte[] Serialize()
    {
        var ms = new MemoryStream();

        // Packet Type
        ms.Write(BitConverter.GetBytes((int)this.PacketType), 0, sizeof(int));

        // Result
        ms.Write(BitConverter.GetBytes(Result), 0, sizeof(bool));

        // MsgLen
        ms.Write(BitConverter.GetBytes(Message.Length), 0, sizeof(int));

        // Msg
        ms.Write(Encoding.UTF8.GetBytes(Message));

        return ms.ToArray();
    }
}
