using System.Text;
using Throw;

namespace ag.ems.application.Common;

public class ApplicationConfig
{
    public string Secret { get; set; } = null!;
    public string Environment { get; set; } = null!;
    public string ClientBaseUrl { get; set; } = null!;
    public string[] AllowedHosts { get; set; }  = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public EmailSettings EmailSettings { get; set; }  = null!;
    public string AzureBlobPath { get; set; } = null!;


    public bool IsEnabledDatabaseSeeding { get; set; }
    
    public byte[] Key => Encoding.UTF8.GetBytes(Secret);

    public void Validate()
    {
        //EmailSettings.ThrowIfNull();
        Secret.ThrowIfNull();
        AllowedHosts.ThrowIfNull();
    }
}