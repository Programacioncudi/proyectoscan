using System.Threading.Tasks;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Inicializador de base de datos que aplica migraciones pendientes y realiza la siembra de datos.
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ejecuta de forma asíncrona la aplicación de migraciones y la inserción de datos iniciales.
        /// </summary>
        /// <returns>Una tarea que representa la operación de inicialización de la base de datos.</returns>
        Task InitializeAsync();
    }
}
