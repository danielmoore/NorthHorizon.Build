using System;
using System.Collections.Generic;
using System.IO;

namespace NorthHorizon.Build.Tasks
{
    internal class ConcatenatedStream : Stream
    {
        private readonly IEnumerator<Stream> _enumerator;
        private bool _hasMore;

        public ConcatenatedStream(IEnumerable<Stream> streams)
        {
            _enumerator = streams.GetEnumerator();
            _hasMore = _enumerator.MoveNext();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (!_hasMore)
                return 0;

            int bytesRead = _enumerator.Current.Read(buffer, offset, count);
            if (bytesRead < count)
            {
                _enumerator.Current.Dispose();

                _hasMore = _enumerator.MoveNext();
                bytesRead += Read(buffer, offset + bytesRead, count - bytesRead);
            }
            return bytesRead;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_enumerator.Current != null)
                    _enumerator.Current.Dispose();

                _enumerator.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Not Supported

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}