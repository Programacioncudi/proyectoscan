using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using System;
using System.Collections.Concurrent;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Servicio para manejar sesiones de escaneo activas por dispositivo.
    /// </summary>
    public class ScannerSessionService : IScannerSessionService
    {
        private readonly ConcurrentDictionary<string, ScanSession> _sessions = new();

        /// <summary>
        /// Inicia una nueva sesión para el escáner indicado.
        /// </summary>
        public void StartSession(string scannerId)
        {
            var session = new ScanSession
            {
                ScannerId = scannerId,
                StartTime = DateTime.UtcNow
            };

            _sessions[scannerId] = session;
        }

        /// <summary>
        /// Termina y elimina la sesión activa del escáner indicado.
        /// </summary>
        public void EndSession(string scannerId)
        {
            _sessions.TryRemove(scannerId, out _);
        }

        /// <summary>
        /// Obtiene los datos de una sesión activa por scannerId.
        /// </summary>
        public ScanSession? GetSession(string scannerId)
        {
            _sessions.TryGetValue(scannerId, out var session);
            return session;
        }
    }
}
