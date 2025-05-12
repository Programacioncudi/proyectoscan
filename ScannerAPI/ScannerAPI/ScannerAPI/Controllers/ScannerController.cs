// Añadir al inicio del archivo
using WIA2 = WIA; // Para claridad en el código que usaremos WIA 2.0
using System.Security;

public class ScannerController : IDisposable
{
    private WIA2.DeviceManager _wiaManager;
    private bool _disposed = false;
    
    // Constructor seguro
    public ScannerController()
    {
        try
        {
            _wiaManager = new WIA2.DeviceManagerClass(); // Usando WIA 2.0 explícitamente
        }
        catch (COMException ex)
        {
            throw new SecurityException("Error al acceder a WIA. Verifique permisos.", ex);
        }
    }

    // Método para listar dispositivos con validación
    public IEnumerable<ScannerDevice> ListDevices()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(ScannerController));
        
        try
        {
            return _wiaManager.DeviceInfos.Cast<WIA2.DeviceInfo>()
                .Select(d => new ScannerDevice
                {
                    Id = d.DeviceID,
                    Name = d.Properties["Name"].get_Value().ToString()
                })
                .ToList();
        }
        catch (Exception ex)
        {
            // Loggear el error adecuadamente
            throw new ScannerException("Error al listar dispositivos", ex);
        }
    }

    // Implementación IDisposable segura
    public void Dispose()
    {
        if (_disposed) return;
        
        if (_wiaManager != null)
        {
            Marshal.ReleaseComObject(_wiaManager);
            _wiaManager = null;
        }
        
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    // Destructor por seguridad
    ~ScannerController()
    {
        Dispose();
    }
}

// Nueva clase de excepción específica
public class ScannerException : Exception
{
    public ScannerException(string message, Exception inner) : base(message, inner) { }
}