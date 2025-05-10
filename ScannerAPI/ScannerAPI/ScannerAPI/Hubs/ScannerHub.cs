using Microsoft.AspNetCore.SignalR;
using ScannerAPI.Models.Events;
using System.Security.Claims;

namespace ScannerAPI.Hubs;

/// <summary>
/// Hub SignalR para notificaciones en tiempo real del escáner
/// </summary>
[Authorize]
public class ScannerHub : Hub
{
    private readonly IEventBusService _eventBus;
    private readonly ILogger<ScannerHub> _logger;

    public ScannerHub(IEventBusService eventBus, ILogger<ScannerHub> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    /// <summary>
    /// Suscribe al cliente a los eventos de una sesión específica
    /// </summary>
    public async Task SubscribeToSession(string sessionId)
    {
        try
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
            _logger.LogInformation("User {UserId} subscribed to session {SessionId}", userId, sessionId);
            
            // Enviar estado actual si existe
            var lastEvent = await _eventBus.GetLastEventAsync(sessionId);
            if (lastEvent != null)
            {
                await Clients.Caller.SendAsync("ScanEvent", lastEvent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subscribing to session");
            throw;
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Client connected: {ConnectionId}, User: {UserId}", Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Client disconnected: {ConnectionId}, User: {UserId}", Context.ConnectionId, userId);
        await base.OnDisconnectedAsync(exception);
    }
}