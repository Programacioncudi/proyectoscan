// File: Database/IDatabaseInitializer.cs
using System.Threading.Tasks;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Inicializador de base de datos: aplica migraciones y semillas.
    /// </summary>
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}