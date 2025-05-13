// File: Services/IUserService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Operaciones de administración de usuarios.
    /// </summary>
    public interface IUserService
    {
        Task<User> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
    }
}