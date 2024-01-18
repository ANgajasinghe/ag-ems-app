using ag.ems.application.Common;
using Microsoft.AspNetCore.Identity;

namespace cube360.vbs.Infrastructure.Services.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description).ToArray());
    }
}