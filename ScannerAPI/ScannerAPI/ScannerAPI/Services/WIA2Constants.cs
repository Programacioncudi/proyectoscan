// ScannerAPI/Services/WIA2Constants.cs
namespace ScannerAPI.Services
{
    public static class WIA2PropertyIDs
    {
        // Propiedades estándar de WIA 2.0
        public const int WIA_IPS_BRIGHTNESS = 6154;
        public const int WIA_IPS_CONTRAST = 6155;
        public const int WIA_IPS_CUR_INTENT = 6146;
        public const int WIA_IPS_XRES = 6147;
        public const int WIA_IPS_YRES = 6148;
        public const int WIA_IPS_DOCUMENT_HANDLING_SELECT = 3086;
        public const int WIA_IPS_PAGE_SIZE = 3096;
        public const int WIA_IPS_PAGE_WIDTH = 3097;
        public const int WIA_IPS_PAGE_HEIGHT = 3098;
    }

    public enum WIA2_DOCUMENT_HANDLING_SELECT
    {
        FLATBED = 0x0001,
        FEEDER = 0x0002,
        DUPLEX = 0x0004,
        FRONT_FIRST = 0x0010,
        BACK_FIRST = 0x0020
    }

    public enum WIA2_IPS_CUR_INTENT
    {
        COLOR = 1,
        GRAYSCALE = 2,
        TEXT = 4
    }

    public enum WIA2_PAGE_SIZE
    {
        A4 = 0,
        LETTER = 1,
        LEGAL = 5,
        A5 = 6,
        B4 = 7,
        B5 = 8
    }
}