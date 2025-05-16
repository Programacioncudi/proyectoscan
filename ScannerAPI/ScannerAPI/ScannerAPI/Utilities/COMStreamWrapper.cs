// File: Utilities/COMStreamWrapper.cs
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Adaptador de COM <see cref="IStream"/> a <see cref="Stream"/>.
    /// </summary>
    public class COMStreamWrapper : Stream
    {
        private readonly IStream _comStream;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="COMStreamWrapper"/> con el flujo COM dado.
        /// </summary>
        /// <param name="comStream">Instancia de <see cref="IStream"/> a envolver.</param>
        public COMStreamWrapper(IStream comStream)
        {
            _comStream = comStream ?? throw new ArgumentNullException(nameof(comStream));
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanSeek => true;

        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override long Length
        {
            get
            {
                _comStream.Stat(out var stat, 1);
                return stat.cbSize;
            }
        }

        /// <inheritdoc/>
        public override long Position
        {
            get
            {
                // No se puede obtener la posición actual sin punteros nativos,
                // devolvemos 0 como posición predeterminada.
                return 0;
            }
            set
            {
                // Solo movemos el puntero sin leer la nueva posición.
                _comStream.Seek(value, 0, IntPtr.Zero);
            }
        }

        /// <inheritdoc/>
        public override void Flush()
        {
            _comStream.Commit(0);
        }

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            _comStream.Read(buffer, count, IntPtr.Zero);
            return count;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin)
        {
            var originEnum = origin switch
            {
                SeekOrigin.Begin => 0,
                SeekOrigin.Current => 1,
                SeekOrigin.End => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };
            _comStream.Seek(offset, originEnum, IntPtr.Zero);
            return 0;
        }

        /// <inheritdoc/>
        public override void SetLength(long value)
        {
            _comStream.SetSize(value);
        }

        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _comStream.Write(buffer, count, IntPtr.Zero);
        }
    }
}
