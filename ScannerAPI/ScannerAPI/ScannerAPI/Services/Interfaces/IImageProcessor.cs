// File: Services/Interfaces/IImageProcessor.cs
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define operaciones para procesar im�genes, como conversi�n de formato y redimensionamiento.
    /// </summary>
    public interface IImageProcessor
    {
        /// <summary>
        /// Convierte los datos de imagen al formato especificado.
        /// </summary>
        /// <param name="imageData">Bytes de la imagen de entrada.</param>
        /// <param name="format">Formato de salida deseado.</param>
        /// <returns>Bytes de la imagen convertida.</returns>
        Task<byte[]> ConvertToFormatAsync(byte[] imageData, FileFormat format);

        /// <summary>
        /// Cambia el tama�o de la imagen a las dimensiones indicadas.
        /// </summary>
        /// <param name="imageData">Bytes de la imagen de entrada.</param>
        /// <param name="width">Ancho deseado en p�xeles.</param>
        /// <param name="height">Alto deseado en p�xeles.</param>
        /// <returns>Bytes de la imagen redimensionada.</returns>
        Task<byte[]> ResizeAsync(byte[] imageData, int width, int height);
    }
}
