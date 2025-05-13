// Examples/Frontend/React/useScanner.js
import { useState } from 'react';
import axios from 'axios';

const useScanner = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [data, setData] = useState(null);

  const startScan = async (options) => {
    setLoading(true);
    setError(null);
    try {
      const token = localStorage.getItem('jwtToken');
      const response = await axios.post('/api/scanner/scan', options, {
        headers: { Authorization: `Bearer ${token}` }
      });
      setData(response.data.data);
    } catch (err) {
      setError(err.response?.data?.error?.message || err.message);
    } finally {
      setLoading(false);
    }
  };

  return { startScan, loading, error, data };
};

export default useScanner;