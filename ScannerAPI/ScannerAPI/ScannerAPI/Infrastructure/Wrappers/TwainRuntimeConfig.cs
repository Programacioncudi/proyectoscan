using NTwain.Data;

namespace ScannerAPI.Infrastructure.Wrappers;

public interface ITwainConfig
{
    TransferMode TransferMode { get; }
    bool ShowUI { get; }
    int DefaultResolution { get; }
    int MaxWaitSeconds { get; }
}

public class TwainRuntimeConfig : ITwainConfig
{
    public TransferMode TransferMode { get; set; } = TransferMode.Native;
    public bool ShowUI { get; set; } = false;
    public int DefaultResolution { get; set; } = 300;
    public int MaxWaitSeconds { get; set; } = 30;
}