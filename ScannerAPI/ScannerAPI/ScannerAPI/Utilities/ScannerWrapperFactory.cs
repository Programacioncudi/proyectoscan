namespace ScannerAPI.Utilities
{
    public static class ScannerWrapperFactory
    {
        public static IScannerWrapper Create(string type)
        {
            return type.ToLower() switch
            {
                "wia"   => new WiaWrapper(),
                "twain" => new TwainWrapper(),
                _       => throw new ArgumentException($"Tipo de escáner desconocido: {type}")
            };
        }
    }
}
