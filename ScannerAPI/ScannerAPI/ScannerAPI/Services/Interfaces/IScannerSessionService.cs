// File: Services/Interfaces/IScannerSessionService.cs
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using SessionOptionsModel = ScannerAPI.Models.Scanner.SessionOptions;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define las operaciones para gestionar sesiones de escaneo.
    /// </summary>
    public interface IScannerSessionService
    {
        /// <summary>
        /// Crea una nueva sesión de escaneo para el usuario especificado.
        /// </summary>
        /// <param name="username">Nombre o identificador del usuario.</param>
        /// <param name="options">Opciones específicas de la sesión.</param>
        Task<ScanSession> CreateSessionAsync(string username, SessionOptionsModel options);
        /// <summary>
        /// Obtiene una sesión existente por su ID.
        /// </summary>
        Task<ScanSession> GetSessionAsync(string sessionId);

        /// <summary>
        /// Marca una sesión como finalizada.
        /// </summary>
        Task EndSessionAsync(string sessionId);
        /// <summary>
        /// Obtiene todas las sesiones de escaneo existentes.
        /// </summary>
        ///<returns>Una colección de objetos<see cref="ScanSession"/> representando cada sesión.</returns>
        Task<IEnumerable<ScanSession>> GetAllSessionsAsync();

    }
}
