// ScannerAPI/Services/ScannerService.cs
using System;
using System.Collections.Generic;
using WIA;
using ScannerAPI.Interfaces;
using Microsoft.Extensions.Logging;
using WIA2 = WIA; // Especificamos que usamos WIA 2.0

namespace ScannerAPI.Services
{
    public partial class ScannerService : IScannerService
    {
        private WIA2.DeviceManager _wia2DeviceManager;
        private WIA2.Device _wia2Device;
        private readonly ILogger<ScannerService> _logger;

        public ScannerService(ILogger<ScannerService> logger)
        {
            _logger = logger;
            InitializeWIA2();
        }

        private void InitializeWIA2()
        {
            try
            {
                _wia2DeviceManager = new WIA2.DeviceManager();
                _logger.LogInformation("WIA 2.0 inicializado correctamente");
            }
            catch (COMException ex)
            {
                _logger.LogError(ex, "Error al inicializar WIA 2.0");
                throw new ScannerException("No se pudo inicializar WIA 2.0. Asegúrese que está instalado.", ex);
            }
        }

        public void SelectWIA2Device(string deviceId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    // Seleccionar el primer dispositivo por defecto
                    _wia2Device = _wia2DeviceManager.DeviceInfos[1].Connect();
                }
                else
                {
                    foreach (WIA2.DeviceInfo deviceInfo in _wia2DeviceManager.DeviceInfos)
                    {
                        if (deviceInfo.DeviceID == deviceId)
                        {
                            _wia2Device = deviceInfo.Connect();
                            break;
                        }
                    }
                }

                if (_wia2Device == null)
                    throw new ScannerException("Dispositivo WIA 2.0 no encontrado");

                _logger.LogInformation($"Dispositivo WIA 2.0 seleccionado: {_wia2Device.DeviceID}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al seleccionar dispositivo WIA 2.0");
                throw;
            }
        }

        public void ConfigureWIA2Scan(
            int brightness = 0,
            int contrast = 0,
            int dpi = 300,
            bool useADF = false,
            bool duplex = false,
            string colorMode = "Color",
            string paperSize = "A4")
        {
            try
            {
                if (_wia2Device == null)
                    SelectWIA2Device();

                var item = _wia2Device.Items[1];

                // Configuración avanzada específica de WIA 2.0
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_BRIGHTNESS, brightness);
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_CONTRAST, contrast);
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_XRES, dpi);
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_YRES, dpi);

                // Configuración de manejo de documentos (ADF/Duplex)
                int handling = (int)WIA2_DOCUMENT_HANDLING_SELECT.FLATBED;
                if (useADF) handling |= (int)WIA2_DOCUMENT_HANDLING_SELECT.FEEDER;
                if (duplex) handling |= (int)WIA2_DOCUMENT_HANDLING_SELECT.DUPLEX;
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_DOCUMENT_HANDLING_SELECT, handling);

                // Configuración de color
                int intent = colorMode.ToLower() switch
                {
                    "grayscale" => (int)WIA2_IPS_CUR_INTENT.GRAYSCALE,
                    "blackandwhite" => (int)WIA2_IPS_CUR_INTENT.TEXT,
                    _ => (int)WIA2_IPS_CUR_INTENT.COLOR
                };
                SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_CUR_INTENT, intent);

                // Configuración de tamaño de papel (solo para ADF)
                if (useADF)
                {
                    SetWIA2Property(item.Properties, WIA2PropertyIDs.WIA_IPS_PAGE_SIZE, 
                        (int)GetWIA2PageSize(paperSize));
                }

                _logger.LogInformation("Configuración WIA 2.0 aplicada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al configurar WIA 2.0");
                throw;
            }
        }

        private WIA2_PAGE_SIZE GetWIA2PageSize(string paperSize)
        {
            return paperSize.ToUpper() switch
            {
                "A4" => WIA2_PAGE_SIZE.A4,
                "LETTER" => WIA2_PAGE_SIZE.LETTER,
                "LEGAL" => WIA2_PAGE_SIZE.LEGAL,
                _ => WIA2_PAGE_SIZE.A4
            };
        }

        private void SetWIA2Property(WIA2.IProperties properties, int propertyId, object value)
        {
            try
            {
                WIA2.Property property = properties.get_Item(ref propertyId);
                property.set_Value(ref value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"No se pudo establecer la propiedad WIA 2.0 {propertyId}: {ex.Message}");
                throw new ScannerException($"Error de configuración WIA 2.0: {ex.Message}", ex);
            }
        }

        public List<WIA2.ImageFile> ScanWithWIA2(int pageCount = 1, string format = "jpeg")
        {
            var images = new List<WIA2.ImageFile>();
            try
            {
                if (_wia2Device == null)
                    SelectWIA2Device();

                var item = _wia2Device.Items[1];
                string wiaFormat = format.ToLower() switch
                {
                    "png" => FormatID.wiaFormatPNG,
                    "tiff" => FormatID.wiaFormatTIFF,
                    "bmp" => FormatID.wiaFormatBMP,
                    _ => FormatID.wiaFormatJPEG
                };

                for (int i = 0; i < pageCount; i++)
                {
                    try
                    {
                        var image = (WIA2.ImageFile)item.Transfer(wiaFormat);
                        images.Add(image);
                        _logger.LogInformation($"Página {i + 1} escaneada correctamente");
                    }
                    catch (COMException ex) when (ex.ErrorCode == unchecked((int)0x80210006))
                    {
                        _logger.LogInformation("No hay más páginas en el alimentador");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el escaneo WIA 2.0");
                throw;
            }

            return images;
        }
    }
}