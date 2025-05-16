// File: Services/Interfaces/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Operaciones de administración de usuarios.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtiene un usuario por su identificador único.
        /// </summary>
        /// <param name="userId">Identificador único del usuario.</param>
        /// <returns>El <see cref="User"/> correspondiente o lanza excepción si no existe.</returns>
        Task<User> GetByIdAsync(Guid userId);

        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        /// <returns>Una colección de <see cref="User"/>.</returns>
        Task<IEnumerable<User>> GetAllAsync();
    }
}
