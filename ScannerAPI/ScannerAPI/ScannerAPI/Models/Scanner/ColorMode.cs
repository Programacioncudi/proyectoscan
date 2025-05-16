namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Modos de color del escaneo.
    /// </summary>
    public enum ColorMode
    {
        /// <summary>
        /// Escaneo en color completo.
        /// </summary>
        Color,

        /// <summary>
        /// Escaneo en escala de grises.
        /// </summary>
        Grayscale,

        /// <summary>
        /// Escaneo en blanco y negro (1 bit por píxel).
        /// </summary>
        BlackAndWhite
    }
}
