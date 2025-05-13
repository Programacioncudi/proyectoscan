namespace ScannerAPI.Services
{
    /// <summary>
    /// Identificadores y enums de WIA 2.0.
    /// </summary>
    public static class WIA2PropertyIDs
    {
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

    /// <summary>
    /// Intents de color para WIA 2.0.
    /// </summary>
    public enum WIA2_IPS_CUR_INTENT
    {
        COLOR = 1,
        GRAYSCALE = 2,
        TEXT = 4
    }

    /// <summary>
    /// Tamaños de página soportados en WIA 2.0.
    /// </summary>
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