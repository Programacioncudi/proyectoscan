// File: Config/JwtConfig.cs
namespace ScannerAPI.Config
{
    /// <summary>
    /// Mapea la sección "Jwt" en appsettings.json.
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// Emisor (issuer) que firma los tokens JWT.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audiencia (audience) para la que el token JWT es válido.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Clave secreta usada para firmar y validar los tokens JWT.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Duración en minutos antes de que el token JWT expire.
        /// </summary>
        public int ExpirationMinutes { get; set; }
    }
}

