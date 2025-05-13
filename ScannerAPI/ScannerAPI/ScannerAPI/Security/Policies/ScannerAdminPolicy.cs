using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    public static class ScannerAdminPolicy
    {
        public const string PolicyName = "ScannerAdmin";
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireClaim("role", "ADMIN"));
        }
    }
}