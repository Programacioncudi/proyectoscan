using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Factories;

public interface IScannerFactory
{
    IScannerWrapper CreateScanner(string scannerType);
}