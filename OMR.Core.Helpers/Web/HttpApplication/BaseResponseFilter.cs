using System;
using System.IO;

namespace OMR.Core.Helpers.Web.HttpApplication
{
    public abstract class BaseReponseFilter : Stream
    {
        private Stream _stream;

        public BaseReponseFilter(Stream stream)
        {
            _stream = stream;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        private long _position;
        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);

            string html = System.Text.Encoding.Default.GetString(buffer);

            html = FilterString(html);

            var filteredData = System.Text.Encoding.Default.GetBytes(html);

            _stream.Write(filteredData, 0, filteredData.GetLength(0));
        }

        public abstract string FilterString(string input);
    }
}
