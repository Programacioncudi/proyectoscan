// Examples/Frontend/React/ScannerComponent.jsx
import React from 'react';
import useScanner from './useScanner';

const ScannerComponent = () => {
    const { startScan, loading, error, data } = useScanner();

    const handleScan = () => {
        startScan({ deviceId: 'Scanner123', dpi: 300, format: 'PDF', duplex: true });
    };

    return (
        <div className="p-4">
            <button onClick={handleScan} disabled={loading} className="px-4 py-2 rounded shadow">
                {loading ? 'Escaneando...' : 'Iniciar Escaneo'}
            </button>
            {error && <div className="mt-2 text-red-500">Error: {error}</div>}
            {data && <a href={data.filePath} target="_blank" rel="noreferrer">Ver resultado</a>}
        </div>
    );
};

export default ScannerComponent;