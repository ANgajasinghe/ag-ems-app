using Microsoft.AspNetCore.Identity;

namespace ag.ems.domain.Entities.Identity;

public sealed class AppIdentityUser : IdentityUser
{
    public AppIdentityUser(string email, string fullName) : base(email)
    {
        FullName = fullName;
        Email = email;
        UserName = email;
    }
    public string? FullName { get; set; }
    public decimal? Salary { get; set; }
    public DateTime? JoinDate { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }

    public bool Lock { get; set; }
}