using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace OMR.Tests
{
    [TestClass]
    public class GenericComparerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text1 = "Hello world";
            var text2 = "Hello World!";

            var by1 = Encoding.UTF8.GetBytes(text1);
            var by2 = Encoding.UTF8.GetBytes(text2);

            var ba1 = ByteData.GetByteData(by1);
            var ba2 = ByteData.GetByteData(by2);

            var gc = new GenericComparer<ByteData>();
            gc.CompareFunc = (a, b) => { return a.Order.CompareTo(b.Order); };
            gc.EqualsFunc = (a, b) => { return a.Value.Equals(b.Value); };

            var result = gc.Compare(ba2.ToList(), ba1.ToList());

            var p = new Patcher(by1);
            var newBytes = p.ApplyChanges(result);

            var newText = Encoding.UTF8.GetString(newBytes);
        }

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

        public class Patcher
        {
            private List<byte> _bytes;

            public Patcher(byte[] bytes)
            {
                _bytes = new List<byte>(bytes);
            }

            public byte[] ApplyChanges(List<ComparisonResult<ByteData>> result)
            {
                var orderedResults = result.OrderByDescending(c => c.Value.Order);

                foreach (var item in result)
                {
                    if (item.Target == ComparisonResultType.CONFLICT)
                    {
                        _bytes[item.Value.Order] = item.Value.Value;
                    }
                    else if (item.Target == ComparisonResultType.WCREATE)
                    {
                        _bytes.Insert(item.Value.Order, item.Value.Value);
                    }
                    else if (item.Target == ComparisonResultType.WDELETE)
                    {
                        _bytes.RemoveAt(item.Value.Order);
                    }
                }

                return _bytes.ToArray();
            }
        }

    }

}
