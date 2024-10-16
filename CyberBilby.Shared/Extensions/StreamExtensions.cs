namespace CyberBilby.Shared;

public static class StreamExtensions
{
    public static async Task<int> ReadIntAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        byte[] buffer = new byte[sizeof(int)];
        await stream.ReadAtLeastAsync(buffer, sizeof(int), cancellationToken: cancellationToken);

        return BitConverter.ToInt32(buffer);
    }
}
