namespace ScannerAPI.Models.Events
{
    /// <summary>
    /// Representa un evento de progreso emitido durante un proceso (como escaneo o carga).
    /// </summary>
    public class ProgressEvent
    {
        /// <summary>
        /// Porcentaje completado del proceso.
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// Mensaje opcional de estado.
        /// </summary>
        public string? StatusMessage { get; set; }

        /// <summary>
        /// Identificador opcional del proceso al que pertenece el progreso.
        /// </summary>
        public string? TaskId { get; set; }

        public ProgressEvent(int percentage, string? statusMessage = null, string? taskId = null)
        {
            Percentage = percentage;
            StatusMessage = statusMessage;
            TaskId = taskId;
        }
    }
}
