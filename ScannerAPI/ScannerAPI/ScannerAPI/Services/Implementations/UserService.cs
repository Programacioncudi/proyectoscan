using Microsoft.EntityFrameworkCore;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Implementación concreta del servicio de gestión de usuarios
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor principal que inicializa el contexto de base de datos
        /// </summary>
        /// <param name="dbContext">Contexto de base de datos inyectado</param>
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene un usuario específico por su identificador único incluyendo sus roles
        /// </summary>
        /// <param name="userId">Identificador único del usuario (GUID)</param>
        /// <returns>Usuario encontrado</returns>
        /// <exception cref="KeyNotFoundException">Se lanza cuando no se encuentra el usuario</exception>
        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException("Usuario no encontrado");
        }

        /// <summary>
        /// Obtiene todos los usuarios registrados en el sistema incluyendo sus roles
        /// </summary>
        /// <returns>Colección enumerable de usuarios</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Roles)
                .ToListAsync();
        }
    }
}