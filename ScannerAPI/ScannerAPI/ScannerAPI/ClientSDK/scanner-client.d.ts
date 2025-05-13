declare interface ScanOptions {
  deviceId: string;
  dpi: number;
  format: 'JPEG'|'PNG'|'PDF'|'TIFF'|'BMP';
  duplex: boolean;
  colorMode?: 'BlackAndWhite'|'Grayscale'|'Color';
  bitDepth?: number;
  orientation?: 'Portrait'|'Landscape';
  paperSize?: 'A4'|'Letter'|'Legal'|'Executive';
  quality?: number;
  useFeeder?: boolean;
  brightness?: number;
  contrast?: number;
  cropRegion?: { x: number; y: number; width: number; height: number };
}

declare interface ScanResult {
  scanId: string;
  filePath: string;
  success: boolean;
  errorMessage?: string;
}