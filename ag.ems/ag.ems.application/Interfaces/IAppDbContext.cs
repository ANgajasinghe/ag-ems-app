using ag.ems.domain.Entities;
using ag.ems.domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace ag.ems.application.Interfaces;

public interface IAppDbContext
{
    DbSet<Salary> Salary { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}