using System.Threading.Tasks;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Inicializador de base de datos que aplica migraciones pendientes y realiza la siembra de datos.
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ejecuta de forma as�ncrona la aplicaci�n de migraciones y la inserci�n de datos iniciales.
        /// </summary>
        /// <returns>Una tarea que representa la operaci�n de inicializaci�n de la base de datos.</returns>
        Task InitializeAsync();
    }
}
