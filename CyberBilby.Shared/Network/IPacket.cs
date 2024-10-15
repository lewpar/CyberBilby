namespace CyberBilby.Shared.Network;

public interface IPacket
{
    public PacketType PacketType { get; }

    public byte[] Serialize();
}
