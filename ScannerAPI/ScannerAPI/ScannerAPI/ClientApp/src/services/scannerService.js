import axios from 'axios';

/**
 * Servicio para interactuar con la API de escaneo.
 */
const scannerService = {
  /**
   * Inicia un escaneo con las opciones dadas.
   */
  async scan(options) {
    const token = localStorage.getItem('jwtToken');
    const response = await axios.post('/api/scanner/scan', options, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data.data;
  },

  /**
   * Obtiene el resultado de un escaneo por ID.
   */
  async getResult(scanId) {
    const token = localStorage.getItem('jwtToken');
    const response = await axios.get(`/api/scanner/${scanId}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.data.data;
  }
};

export default scannerService;