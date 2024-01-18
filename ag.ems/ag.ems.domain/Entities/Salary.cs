using ag.ems.domain.Entities.Identity;
using Throw;

namespace ag.ems.domain.Entities;

public class Salary
{
    private Salary()
    {
        
    }
    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public string Month { get; private set; }
    public string Year { get; private set; }
    public DateTime PaidDate { get; private set; }
    public DateTime CreatedAt { get;private  set; }
    public string CreatedBy { get; private set; }
    public AppIdentityUser AppIdentityUser { get; private set; }
    public string AppIdentityUserId { get; private set; }
    
    public Salary(decimal amount, string month, string year, DateTime paidDate, AppIdentityUser appIdentityUser)
    {
        Amount = amount;
        Month = month;
        Year = year;
        PaidDate = paidDate;
        AppIdentityUserId = appIdentityUser.Id;
    }
    
    
    
    public static Salary Create(decimal amount, DateTime paidDate, string createdBy, AppIdentityUser user)
    {
        var (month, year) = GetMonthAndYear(paidDate);
        var salary = new Salary(amount, month, year, paidDate, user);
        salary.SetAudit(createdBy);
        return salary;
    }
    
    private static (string month, string year) GetMonthAndYear(DateTime paidDate)
    {
        var month = paidDate.ToString("MMMM");
        var year = paidDate.ToString("yyyy");
        return (month, year);
    }

    private void SetAudit(string createdBy)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Validate()
    {
        Amount.ThrowIfNull();
        Month.ThrowIfNull();
        Year.ThrowIfNull();
        PaidDate.ThrowIfNull();
        CreatedAt.ThrowIfNull();
        CreatedBy.ThrowIfNull();
    }
}