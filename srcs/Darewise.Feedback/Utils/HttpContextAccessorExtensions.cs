using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Darewise.Feedback.Utils
{
    public static class HttpContextAccessorExtensions
    {
        public static Guid GetUserId(this IHttpContextAccessor contextAccessor)
        {
            Claim tmp = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier) ?? throw new BadHttpRequestException("JWT was incorrect", 400);
            return !Guid.TryParse(tmp.Value, out Guid id) ? Guid.Empty : id;
        }
    }
}