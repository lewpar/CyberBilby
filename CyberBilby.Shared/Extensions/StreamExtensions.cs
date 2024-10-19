using CyberBilby.Shared.Network;
using System.Net.Security;
using System.Text;
using System.Text.Json;

namespace CyberBilby.Shared.Extensions;

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

    public static async Task<int> ReadIntAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[sizeof(int)];
        await stream.ReadAtLeastAsync(buffer, sizeof(int), cancellationToken: cancellationToken);

        return BitConverter.ToInt32(buffer);
    }

    public static async Task<byte[]> ReadBytesAsync(this Stream stream, int length, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[length];
        await stream.ReadAtLeastAsync(buffer, length);

        return buffer;
    }

    public static async Task<bool> ReadBoolAsync(this Stream stream)
    {
        byte[] buffer = new byte[sizeof(bool)];
        await stream.ReadAtLeastAsync(buffer, sizeof(bool));

        return BitConverter.ToBoolean(buffer);
    }

    public static async Task<string> ReadStringAsync(this Stream stream)
    {
        var msgLen = await stream.ReadIntAsync();
        byte[] buffer = new byte[msgLen];

        var msg = await stream.ReadBytesAsync(msgLen);

        return Encoding.UTF8.GetString(msg);
    }

    public static async Task<T?> DeserializeAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
    {
        var dataLen = await stream.ReadIntAsync(cancellationToken);
        byte[] dataBuf = await stream.ReadBytesAsync(dataLen, cancellationToken);

        using var ms = new MemoryStream(dataBuf);

        return await JsonSerializer.DeserializeAsync<T>(ms);
    }
}
