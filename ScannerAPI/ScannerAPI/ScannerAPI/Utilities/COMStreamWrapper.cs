using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de COM IStream a System.IO.Stream.
    /// </summary>
    public class COMStreamWrapper : Stream
    {
        private readonly IStream _comStream;

        public COMStreamWrapper(IStream comStream)
        {
            _comStream = comStream;
        }

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length
        {
            get
            {
                _comStream.Stat(out var stat, 1);
                return stat.cbSize;
            }
        }
        public override long Position
        {
            get { _comStream.Seek(0, 1, out var pos); return pos; }
            set { _comStream.Seek(value, 0, out _); }
        }
        public override void Flush() => _comStream.Commit(0);
        public override int Read(byte[] buffer, int offset, int count)
        {
            _comStream.Read(buffer, count, IntPtr.Zero);
            return count;
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            var originEnum = origin switch
            {
                SeekOrigin.Begin => 0,
                SeekOrigin.Current => 1,
                SeekOrigin.End => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };
            _comStream.Seek(offset, originEnum, out var newPos);
            return newPos;
        }
        public override void SetLength(long value) => _comStream.SetSize(value);
        public override void Write(byte[] buffer, int offset, int count) => _comStream.Write(buffer, count, IntPtr.Zero);
    }
}