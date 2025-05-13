import { useState } from 'react';
import { scan, getScanResult } from '../scanner-client';

/**
 * Hook React para escanear documentos.
 */
export default function useScanner() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [data, setData] = useState(null);

  const startScan = async (options, token) => {
    setLoading(true);
    setError(null);
    try {
      const result = await scan(options, token);
      setData(result);
    } catch (err) {
      setError(err.response?.data?.error?.message || err.message);
    } finally {
      setLoading(false);
    }
  };

  return { startScan, loading, error, data };
}