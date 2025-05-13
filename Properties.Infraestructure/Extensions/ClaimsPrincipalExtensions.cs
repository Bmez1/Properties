using Microsoft.IdentityModel.JsonWebTokens;

using System.Security.Claims;

namespace Properties.Infraestructure.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ApplicationException("User id is unavailable");
    }

    public static string GetEmail(this ClaimsPrincipal? principal)
    {
        var email = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        return !string.IsNullOrEmpty(email) ? email :
            throw new ApplicationException("Email is unavailable");
    }
}
