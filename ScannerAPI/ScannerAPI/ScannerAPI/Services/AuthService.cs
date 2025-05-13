
// File: Services/AuthService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models;

namespace ScannerAPI.Services
{
    /// <inheritdoc/>
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ApplicationDbContext context,
            IJwtTokenService tokenService,
            IPasswordHasher<User> passwordHasher,
            ILogger<AuthService> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Username))
                throw new DomainException("UserExists", "El email ya está registrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Username,
                Roles = new List<UserRole> { UserRole.User }
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Usuario {UserId} registrado exitosamente.", user.Id);

            var token = _tokenService.GenerateToken(user);
            return new AuthResponse { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenService.ExpiryMinutes) };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Username);
            if (user == null)
                throw new DomainException("InvalidCredentials", "Credenciales inválidas.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new DomainException("InvalidCredentials", "Credenciales inválidas.");

            _logger.LogInformation("Usuario {UserId} inició sesión.", user.Id);
            var token = _tokenService.GenerateToken(user);
            return new AuthResponse { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(_tokenService.ExpiryMinutes) };
        }
    }
}

