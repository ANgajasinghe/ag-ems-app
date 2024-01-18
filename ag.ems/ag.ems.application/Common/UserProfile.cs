
using ag.ems.domain.Entities.Identity;
using cube360.vbs.application.Common.Mappings;
using Throw;

namespace cube360.vbs.application.Common.Models.Identity;

public class UserProfile 
{ 
    public string?  Id { get; set; } 
    public string FullName { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public decimal? Salary { get; set; }
    public DateTime? JoinDate { get; set; }
    public string Address { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public bool Lock { get; set; }
    
    public void Validate()
    {
        Email.ThrowIfNull();
    }
    
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LockRequest
{
    public string Email { get; set; }
}


public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}

public record SalaryRequest(string Email, decimal Amount, DateTime PaidDate);
public record SalaryResponse(string FullName, string Email, string Month, string Year , decimal Amount, DateTime PaidDate);
