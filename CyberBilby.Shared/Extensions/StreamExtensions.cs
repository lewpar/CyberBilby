using System.Text.Json;

namespace CyberBilby.Shared.Extensions;

public static class StreamExtensions
{
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

    public static async Task<T?> DeserializeAsync<T>(this Stream stream, CancellationToken cancellationToken = default)
    {
        var dataLen = await stream.ReadIntAsync(cancellationToken);
        byte[] dataBuf = await stream.ReadBytesAsync(dataLen, cancellationToken);

        using var ms = new MemoryStream(dataBuf);

        return await JsonSerializer.DeserializeAsync<T>(ms);
    }
}
