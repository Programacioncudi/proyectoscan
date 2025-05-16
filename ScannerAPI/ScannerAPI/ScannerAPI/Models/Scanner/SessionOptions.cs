// File: Models/Scanner/SessionOptions.cs
namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Opciones extra para la creación de una sesión (metadatos de sesión).
    /// </summary>
    public class SessionOptions
    {
        /// <summary>
        /// Identificador del dispositivo para la sesión.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        // Aquí puedes agregar más campos en el futuro, e.g.:
        /// <summary>
        /// Indica si se debe notificar por correo electrónico tras completar la sesión.
        /// </summary>
        public bool NotifyByEmail { get; set; } = false;

        /// <summary>
        /// Nombre de perfil de escaneo a aplicar en esta sesión.
        /// </summary>
        public string ProfileName { get; set; } = string.Empty;
    }
}
