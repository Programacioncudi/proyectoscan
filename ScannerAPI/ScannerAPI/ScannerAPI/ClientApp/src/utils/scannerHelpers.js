/**
 * Valida las opciones de escaneo antes de enviarlas.
 */
export function validateScanOptions(options) {
  const errors = [];
  if (!options.deviceId) errors.push('deviceId es requerido.');
  if (options.dpi < 1 || options.dpi > 10000) errors.push('dpi fuera de rango (1-10000).');
  const allowedFormats = ['JPEG','PNG','PDF','TIFF','BMP'];
  if (!allowedFormats.includes(options.format))
    errors.push(`format no válido: ${options.format}`);
  return errors;
}