namespace ScannerAPI.Utilities
{
    public interface IScannerWrapper
    {
        /// <summary>
        /// Inicia un escaneo y devuelve la imagen en un array de bytes (por ejemplo PNG).
        /// </summary>
        byte[] Scan(string deviceId);
    }
}
