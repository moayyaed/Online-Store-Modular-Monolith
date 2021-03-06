using System.Linq;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Modules.Identity.Application.Authentication.Dtos;

namespace OnlineStore.Modules.Identity.Infrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static SecurityResult ToSecurityResult(this IdentityResult identityResult)
        {
            return new SecurityResult()
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(x => x.Description)
            };
        }
    }
}
