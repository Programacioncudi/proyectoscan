using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Security
{
    public class JwtConfig
    {
        [Required]
        public string SecretKey { get; set; }
        [Required]
        public string Issuer { get; set; }
        [Required]
        public string Audience { get; set; }
        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; }
    }
}