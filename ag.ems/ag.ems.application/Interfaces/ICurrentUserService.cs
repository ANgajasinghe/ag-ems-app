using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ag.ems.application.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        List<string> UserRoles();
        string UserEmail { get; }

        string BranchId { get; }
        void SetUserId(string userId);
    }
}
