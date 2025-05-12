namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Resultado de una operación de escaneo.
    /// </summary>
    public class ScanResult
    {
        /// <summary>
        /// Indica si el escaneo fue exitoso.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Ruta del archivo resultante del escaneo.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Mensaje adicional o de error, si corresponde.
        /// </summary>
        public string Message { get; set; }
    }
}
