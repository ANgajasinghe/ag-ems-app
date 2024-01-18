namespace cube360.vbs.application.Common.Models.Identity;

public class TokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}