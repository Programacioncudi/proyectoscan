// Examples/VanillaJS/scanner.js
document.getElementById('scanForm').addEventListener('submit', async e => {
  e.preventDefault();
  const deviceId = document.getElementById('deviceId').value;
  const dpi = parseInt(document.getElementById('dpi').value, 10);
  const format = document.getElementById('format').value;
  const duplex = document.getElementById('duplex').checked;
  const token = localStorage.getItem('jwtToken');

  try {
    const res = await fetch('/api/scanner/scan', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({ deviceId, dpi, format, duplex })
    });
    const json = await res.json();
    document.getElementById('output').textContent = JSON.stringify(json, null, 2);
  } catch (err) {
    document.getElementById('output').textContent = err;
  }
});