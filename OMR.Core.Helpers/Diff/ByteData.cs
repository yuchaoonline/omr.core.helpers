using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMR.Core.Helpers.Diff
{
    public struct ByteData
    {
        public byte Value { get; set; }

        public int Order { get; set; }

        public static ByteData[] GetByteData(byte[] bytes)
        {
            var data = new ByteData[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                data[i].Order = i;
                data[i].Value = bytes[i];
            }

            return data;
        }
    }
}
