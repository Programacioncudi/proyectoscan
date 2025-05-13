import axios from 'axios';

/**
 * Inicia un escaneo.
 */
export async function scan(options, token) {
  const response = await axios.post('/api/scanner/scan', options, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data.data;
}

/**
 * Solicita el resultado de un escaneo.
 */
export async function getScanResult(scanId, token) {
  const response = await axios.get(`/api/scanner/${scanId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data.data;
}