﻿using Common.Identity;
using Microsoft.AspNetCore.Http;

namespace OnlineStore.Modules.Identity.Infrastructure
{
    public class HttpContextUserResolver : IUserNameResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserName()
        {
            string result = "unknown";

            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request != null && context.User != null)
            {
                var identity = context.User.Identity;
                if (identity != null && identity.IsAuthenticated)
                {
                    result = context.Request.Headers["VirtoCommerce-User-Name"];
                    if (string.IsNullOrEmpty(result))
                    {
                        result = identity.Name;
                    }
                }
            }
            return result;

        }
    }
}
