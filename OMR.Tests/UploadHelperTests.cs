using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;
using System.Text;
using OMR.Core.Helpers;

namespace OMR.Tests
{
    [TestClass]
    public class UploadHelperTests
    {
        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStreamTest1()
        {
            var expected = "exepected";

            var postedFile = new MockHttpPostedFileBase("test.txt", Encoding.UTF8.GetBytes(expected));

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt", "rtf" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            uh.PostedFileWriteToStream(postedFile, ms, 1024);

            var sr = new StreamReader(ms);
            var actual = sr.ReadLine();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_NullExtension()
        {
            var postedFile = new MockHttpPostedFileBase("test.", new byte[] { 0 });

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_DotlessExtension()
        {
            var postedFile = new MockHttpPostedFileBase("testjpg", new byte[] { 0 });

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_EmptyName()
        {
            var postedFile = new MockHttpPostedFileBase(".jpg", new byte[] { 0 });

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_NullName()
        {
            var postedFile = new MockHttpPostedFileBase(null, new byte[] { 0 });

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_BadExtension()
        {
            var postedFile = new MockHttpPostedFileBase("test.jpg", new byte[] { 0 });

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_ContentTooBig()
        {
            var bytes = new byte[8 * 1024 * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }

            var postedFile = new MockHttpPostedFileBase("test.txt", bytes);

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt" };
            uh.MaxFileSizeKb = 1;

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_PostedFileWriteToStream_NullContent()
        {
            var postedFile = new MockHttpPostedFileBase("test.txt", null);

            UploadHelper uh = new UploadHelper();

            var ms = new MemoryStream();
            try
            {
                uh.PostedFileWriteToStream(postedFile, ms, 0);
            }
            catch (InvalidDataException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void UplaodHelper_Save1()
        {
            var expected = "exepected";

            var postedFile = new MockHttpPostedFileBase("test.txt", Encoding.UTF8.GetBytes(expected));

            UploadHelper uh = new UploadHelper();
            uh.AllowedFileExtensions = new string[] { "txt", "rtf" };
            uh.MaxFileSizeKb = 100;

            var ms = new MemoryStream();
            string fileName;
            uh.SaveAs(postedFile, "c:\\tests\\", out fileName, 4096);

            Assert.IsNotNull(fileName);

            var actual = File.ReadAllText("c:\\tests\\" + fileName);

            Assert.AreEqual(expected, actual);
        }

    }
}

internal class MockHttpPostedFileBase : HttpPostedFileBase
{
    private string _fileName;
    private MemoryStream _stream;

    public MockHttpPostedFileBase(string fileName, byte[] content)
    {
        _fileName = fileName;

        if (content != null)
            _stream = new MemoryStream(content);
    }

    public override int ContentLength
    {
        get
        {
            return (int)_stream.Length;
        }
    }

    public override string ContentType
    {
        get
        {
            return "test/x";
        }
    }

    public override string FileName
    {
        get
        {
            return _fileName;
        }
    }

    public override System.IO.Stream InputStream
    {
        get
        {
            return _stream;
        }
    }
}
