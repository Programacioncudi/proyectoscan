const form = document.getElementById('scanForm');
const output = document.getElementById('output');
form.addEventListener('submit', async e=>{
  e.preventDefault();
  const opts={deviceId:form.deviceId.value,dpi:+form.dpi.value,format:form.format.value,duplex:form.duplex.checked};
  try{const res=await ScannerAPI.scan(opts);output.textContent=JSON.stringify(res,null,2);}catch(err){output.textContent=err}}
);