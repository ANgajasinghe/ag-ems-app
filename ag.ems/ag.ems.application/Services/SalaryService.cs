using ag.ems.application.Interfaces;
using ag.ems.domain.Entities;
using cube360.vbs.application.Common.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ag.ems.application.Services;

public class SalaryService
{
    private readonly IAppDbContext _appDbContext;
    private readonly IIdentityService _identityService;

    public SalaryService(IAppDbContext appDbContext, IIdentityService identityService)
    {
        _appDbContext = appDbContext;
        _identityService = identityService;
    }
    
    public async Task<List<SalaryResponse>> GetSalaryAsync(string? email, string? month, CancellationToken cancellationToken = default)
    {
        if(email == null && month == null)
            return await GetAllSalaryAsync(cancellationToken);
        
        if(email != null && month == null)
            return await GetSalaryByEmailAsync(email, cancellationToken);
        
        if(email == null && month != null)
            return await GetSalaryByMonthAsync(month, cancellationToken);
        
        var salary = await GetEmployeeSalary()
            .Where(x=>x.AppIdentityUser.Email == email && string.Equals(x.Month, month, StringComparison.CurrentCultureIgnoreCase))
            .Select(x => new SalaryResponse(x.AppIdentityUser.FullName, x.AppIdentityUser.Email, x.Month, x.Year, x.Amount, x.PaidDate))
            .ToListAsync(cancellationToken: cancellationToken);
        return salary;
    }

    private async Task<List<SalaryResponse>> GetAllSalaryAsync(CancellationToken cancellationToken)
    {
        var salary = await GetEmployeeSalary()
            .Select(x => new SalaryResponse(x.AppIdentityUser.FullName,x.AppIdentityUser.Email, x.Month, x.Year, x.Amount, x.PaidDate))
            .ToListAsync(cancellationToken: cancellationToken);
        return salary;
    }


    public async Task<List<SalaryResponse>> GetSalaryByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var salary = await GetEmployeeSalary()
            .Where(x => x.AppIdentityUser.Email == email)
            .Select(x => new SalaryResponse(x.AppIdentityUser.FullName,x.AppIdentityUser.Email, x.Month, x.Year, x.Amount, x.PaidDate))
            .ToListAsync(cancellationToken: cancellationToken);
        return salary;
    }
    
    public async Task<List<SalaryResponse>> GetSalaryByMonthAsync(string month, CancellationToken cancellationToken = default)
    {
        var salary = await GetEmployeeSalary()
            .Where(x => string.Equals(x.Month, month, StringComparison.CurrentCultureIgnoreCase))
            .Select(x => new SalaryResponse(x.AppIdentityUser.FullName,x.AppIdentityUser.Email, x.Month, x.Year, x.Amount, x.PaidDate))
            .ToListAsync(cancellationToken: cancellationToken);
        return salary;
    }
    
    public async Task<Salary> CreateSalaryAsync(SalaryRequest salaryRequest,CancellationToken cancellationToken = default)
    {

        var user = await _identityService.GetUserAsync(salaryRequest.Email);
        
        var salary = Salary.Create(
            salaryRequest.Amount,
            salaryRequest.PaidDate, "Test", user);
        
        await _appDbContext.Salary.AddAsync(salary, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return salary;
    }


    public IQueryable<Salary> GetEmployeeSalary()
    {
        var employeeIds = _identityService.UserWithQuery().Select(x=>x.Id).ToList();

       return _appDbContext.Salary.Where(x=> employeeIds.Contains(x.AppIdentityUserId));
    }
}