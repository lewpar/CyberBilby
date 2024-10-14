using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CyberBilby.Shared.Security;

public static class X509Cert2
{
    public static X509Certificate2 CreateRootCertificate(string issuer)
    {
        using var rsa = RSA.Create(2048);

        var subject = new X500DistinguishedName($"CN={issuer}");

        var request = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyCertSign |
                                                                    X509KeyUsageFlags.DigitalSignature |
                                                                    X509KeyUsageFlags.NonRepudiation |
                                                                    X509KeyUsageFlags.CrlSign, true));

        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection
        {
            new Oid("1.3.6.1.5.5.7.3.2"), // Client Authentication
            new Oid("1.3.6.1.5.5.7.3.1") // Server Authentication
        }, false));

        return request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
    }

    public static X509Certificate2 CreateCertificate(X509Certificate2 issuer, string subject)
    {
        using var rsa = RSA.Create(2048);

        var subjectName = new X500DistinguishedName($"CN={subject}");

        var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment |
                                                                    X509KeyUsageFlags.DigitalSignature |
                                                                    X509KeyUsageFlags.NonRepudiation, true));

        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection
        {
            new Oid("1.3.6.1.5.5.7.3.2") // Client Authentication
        }, false));

        var certificate = request.Create(issuer, DateTimeOffset.Now, issuer.NotAfter, Guid.NewGuid().ToByteArray());

        return certificate.CopyWithPrivateKey(rsa);
    }

    public static X509Certificate2 LoadFromFile(string path)
    {
        return new X509Certificate2(path);
    }

    public static X509Certificate2 LoadFromStore(string thumprint)
    {
        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);

        if(store.Certificates.Count < 1)
        {
            throw new Exception("No certificates found in store.");
        }

        return store.Certificates.First(c => c.Thumbprint == thumprint);
    }
}
