﻿using System.Security.Claims;

namespace back_end.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.Sid);
            return id;
        }
        public static bool IsCustomer(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);
            return roles.Contains("CUSTOMER");
        }

        public static string GetGivenName(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.GivenName);
            return id;
        }
    }
}
