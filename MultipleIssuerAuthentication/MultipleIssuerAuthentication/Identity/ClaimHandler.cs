using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Helpers.Enums;

namespace Helpers.Identity
{
    public static class ClaimHandler
    {
        public static List<string> GetProgramIds(IEnumerable<Claim> claims)
        {
            return GetClaim(claims, "program_");
        }

        public static string GetOrganizationId(IEnumerable<Claim> claims)
        {
            return GetClaim(claims, "organization_").FirstOrDefault();
        }

        public static EnumOrganization GetOrganizationValue(IEnumerable<Claim> claims)
        {
            string organizationIdFromToken = GetOrganizationId(claims);

            if (int.TryParse(organizationIdFromToken, out int organizationId) && Enum.TryParse(organizationIdFromToken, out EnumOrganization result))
            {
                return result;
            }

            return EnumOrganization.ValleyMetro;
        }

        public static string GetPassengerId(IEnumerable<Claim> claims)
        {
            if (claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "relay_passenger"))
            {
                return claims.Where(x => x.Type == "alcPassengerId").Select(x => x.Value).FirstOrDefault() ?? string.Empty;
            }

            return string.Empty;
        }

        private static List<string> GetClaim(IEnumerable<Claim> claims, string property)
        {
            return claims.Where(x => x.Type == ClaimTypes.Role && x.Value.Contains(property)).Select(x => x.Value.Substring(x.Value.LastIndexOf("_") + 1)).ToList() ?? new List<string>();
        }
    }
}
