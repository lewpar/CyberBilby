using CyberBilby.MgmtServer.Network;

using CyberBilby.Shared.Network;
using CyberBilby.Shared.Extensions;
using CyberBilby.Shared.Repositories;
using CyberBilby.Shared.Database.Entities;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Net.Sockets;

using CyberBilby.MgmtServer.Events;
using CyberBilby.Shared.Security;
using System.Security.Cryptography.X509Certificates;
using CyberBilby.Shared.Network.Packets;
using System.Net.Security;

namespace CyberBilby.MgmtServer.Services;

public class ManagementServiceHost : BackgroundService
{
    private readonly ILogger<ManagementServiceHost> logger;
    private readonly IBlogRepository blogRepo;

    private ManagementServer _managementServer;

    public ManagementServiceHost(ILogger<ManagementServiceHost> logger, IBlogRepository blogRepo)
    {
        _managementServer = new ManagementServer(IPAddress.Any, 44123, X509Cert2.LoadFromFile("./server.pfx"));
        _managementServer.CertificateValidationCallback = ValidateCertificateAsync;

        RegisterHandlers();
        RegisterEvents();

        this.logger = logger;
        this.blogRepo = blogRepo;
    }

    private void RegisterHandlers()
    {
        _managementServer.RegisterHandler(PacketType.CMSG_GET_POSTS, HandleRequestPostsAsync);
        _managementServer.RegisterHandler(PacketType.CMSG_CREATE_POST, HandleCreatePostAsync);
    }

    private void RegisterEvents()
    {
        _managementServer.ClientConnected += _managementServer_ClientConnected;
        _managementServer.ClientAuthenticated += _managementServer_ClientAuthenticated;
        _managementServer.ClientRejected += _managementServer_ClientRejected;
    }

    private void _managementServer_ClientRejected(object? sender, ClientRejectedEventArgs e)
    {
        logger.LogCritical($"Client '{e.Client.Client.RemoteEndPoint}' rejected for reason: {e.Reason}");
    }

    private async void _managementServer_ClientAuthenticated(object? sender, ClientAuthenticatedEventArgs e)
    {
        logger.LogInformation($"Client '{e.Client.Client.Client.RemoteEndPoint}' authenticated.");

        var author = await blogRepo.GetAuthorAsync(e.Client.Fingerprint);
        if (author is null)
        {
            return;
        }

        await e.Client.Stream.SendPacketAsync(new AuthenticateResponsePacket()
        {
            Profile = new AuthProfile()
            {
                Name = author.Name,
                Role = author.Role
            }
        });
    }

    private void _managementServer_ClientConnected(object? sender, ClientConnectedEventArgs e)
    {
        logger.LogInformation($"Client '{e.Client.Client.RemoteEndPoint}' connected.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _managementServer.ListenAsync(stoppingToken);
    }

    private async Task<bool> ValidateCertificateAsync(X509Certificate2 clientCertificate)
    {
        var isRevoked = await blogRepo.IsCertificateRevokedAsync(clientCertificate.Thumbprint);
        if (isRevoked)
        {
            return false;
        }

        var author = await blogRepo.GetAuthorAsync(clientCertificate.Thumbprint);
        if (author is null)
        {
            return false;
        }

        return true;
    }

    private async Task HandleRequestPostsAsync(SslClient client)
    {
        logger.LogInformation($"Client '{client.Endpoint}' is requesting posts.");

        var posts = await blogRepo.GetAllPostsAsync();
        var response = new GetPostsResponsePacket()
        {
            Posts = posts.ToList()
        };

        await client.Stream.SendPacketAsync(response);
    }

    private async Task HandleCreatePostAsync(SslClient client)
    {
        logger.LogInformation($"Client '{client.Endpoint}' is creating post.");

        var post = await client.Stream.DeserializeAsync<BlogPost>();
        if(post is null)
        {
            logger.LogCritical("Failed to deserialize blog post.");

            await client.Stream.SendPacketAsync(new CreatePostResponsePacket() 
            { 
                Result = false, 
                Message = "Internal server error occured" 
            });

            return;
        }

        post.Author = await blogRepo.GetAuthorAsync(client.Fingerprint);

        if(await blogRepo.PostWithSlugExistsAsync(post))
        {
            await client.Stream.SendPacketAsync(new CreatePostResponsePacket()
            {
                Result = false,
                Message = "A post with that slug already exists."
            });

            return;
        }

        await blogRepo.CreatePostAsync(post);
        await client.Stream.SendPacketAsync(new CreatePostResponsePacket() 
        { 
            Result = true,
            Message = "Post created."
        });
    }
}
