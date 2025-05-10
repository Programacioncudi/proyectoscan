using ScannerAPI.Models.Scanner;
using System;

namespace ScannerAPI.Services.Interfaces;

public interface IScannerWrapper
{
    Task<ScanResult> ScanAsync(ScanOptions options, IProgress<ScanProgress> progress);
    Task<DeviceInfo[]> GetDevicesAsync();
    Task<DeviceCapabilities> GetDeviceCapabilitiesAsync(string deviceId);
}