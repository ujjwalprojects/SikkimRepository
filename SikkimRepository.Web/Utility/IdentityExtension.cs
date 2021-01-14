using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SikkimRepository.Web.Utility
{
    public static class IdentityExtension
    {
        public static string GetProfileName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfileName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetRoleName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("RoleName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}