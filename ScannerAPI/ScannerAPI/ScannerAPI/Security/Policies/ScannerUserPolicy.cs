using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    public static class ScannerUserPolicy
    {
        public const string PolicyName = "ScannerUser";
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim(c => (c.Type == "role" && (c.Value == "USER" || c.Value == "ADMIN")) )));
        }
    }
}