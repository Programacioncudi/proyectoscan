// File: Utilities/Constants.cs
namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Constantes utilizadas en la aplicación.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Carpeta predeterminada donde se guardan las exploraciones.
        /// </summary>
        public const string DefaultScanFolder = "Scans";

        /// <summary>
        /// Densidad de escaneo predeterminada en DPI.
        /// </summary>
        public const int DefaultScanDpi = 300;

        /// <summary>
        /// Clave utilizada en JWT para almacenar el identificador de usuario.
        /// </summary>
        public const string JwtUserIdKey = "UserId";

        /// <summary>
        /// Tamaño máximo permitido para archivos subidos, en megabytes.
        /// </summary>
        public const int MaxUploadFileSizeMb = 50;

        /// <summary>
        /// Nombre de la política CORS predeterminada.
        /// </summary>
        public const string CorsPolicyName = "DefaultCorsPolicy";
    }
}
