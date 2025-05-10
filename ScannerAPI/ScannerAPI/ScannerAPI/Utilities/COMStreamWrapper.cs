using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace ScannerAPI.Utilities
{
    public class COMStreamWrapper : Stream
    {
        private readonly IStream _comStream;
        public COMStreamWrapper(IStream comStream) => _comStream = comStream;
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() { }
        public override int Read(byte[] buffer, int offset, int count)
        {
            _comStream.Read(buffer, count, IntPtr.Zero);
            return count;
        }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
