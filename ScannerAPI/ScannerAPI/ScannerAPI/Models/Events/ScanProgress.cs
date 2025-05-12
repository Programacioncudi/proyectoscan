namespace ScannerAPI.Models.Events
{
    /// <summary>
    /// Modelo de evento que informa el progreso de un escaneo.
    /// </summary>
    public class ScanProgress
    {
        /// <summary>
        /// Porcentaje completado del escaneo.
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// Mensaje de estado del escaneo.
        /// </summary>
        public string Status { get; set; }
    }
}
