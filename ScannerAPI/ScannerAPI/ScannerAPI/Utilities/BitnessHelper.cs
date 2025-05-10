namespace ScannerAPI.Utilities
{
    public static class BitnessHelper
    {
        public static bool Is64BitProcess => IntPtr.Size == 8;
        public static bool Is32BitProcess => !Is64BitProcess;
    }
}