// File: Services/Implementations/WiaConstants.cs
namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Identificadores de propiedades de WIA 2.0.
    /// </summary>
    public static class WIA2PropertyIDs
    {
        /// <summary>Brillo de la imagen.</summary>
        public const int WIA_IPS_BRIGHTNESS = 6154;

        /// <summary>Contraste de la imagen.</summary>
        public const int WIA_IPS_CONTRAST = 6155;

        /// <summary>Intención de color.</summary>
        public const int WIA_IPS_CUR_INTENT = 6146;

        /// <summary>Resolución horizontal (DPI).</summary>
        public const int WIA_IPS_XRES = 6147;

        /// <summary>Resolución vertical (DPI).</summary>
        public const int WIA_IPS_YRES = 6148;

        /// <summary>Selección de manejo de documento (alimentador/plano).</summary>
        public const int WIA_IPS_DOCUMENT_HANDLING_SELECT = 3086;

        /// <summary>Tamaño de página.</summary>
        public const int WIA_IPS_PAGE_SIZE = 3096;

        /// <summary>Ancho de página (mm).</summary>
        public const int WIA_IPS_PAGE_WIDTH = 3097;

        /// <summary>Alto de página (mm).</summary>
        public const int WIA_IPS_PAGE_HEIGHT = 3098;
    }

    /// <summary>
    /// Intenciones de color soportadas por WIA 2.0.
    /// </summary>
    public enum WIA2_IPS_CUR_INTENT
    {
        /// <summary>Color completo.</summary>
        COLOR = 1,

        /// <summary>Escala de grises.</summary>
        GRAYSCALE = 2,

        /// <summary>Solo texto (blanco y negro).</summary>
        TEXT = 4
    }

    /// <summary>
    /// Tamaños de página soportados por WIA 2.0.
    /// </summary>
    public enum WIA2_PAGE_SIZE
    {
        /// <summary>Tamaño A4.</summary>
        A4 = 0,

        /// <summary>Tamaño Letter.</summary>
        LETTER = 1,

        /// <summary>Tamaño Legal.</summary>
        LEGAL = 5,

        /// <summary>Tamaño A5.</summary>
        A5 = 6,

        /// <summary>Tamaño B4.</summary>
        B4 = 7,

        /// <summary>Tamaño B5.</summary>
        B5 = 8
    }
}
