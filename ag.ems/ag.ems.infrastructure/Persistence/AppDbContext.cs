using ag.ems.domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ag.ems.application.Interfaces;
using ag.ems.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ag.ems.infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<
    AppIdentityUser,
    IdentityRole,
    string,
    IdentityUserClaim<string>,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>> , IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Salary> Salary { get; set; }
    }
    
    
}
