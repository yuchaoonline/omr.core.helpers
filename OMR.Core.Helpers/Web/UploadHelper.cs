namespace OMR.Core.Helpers
{
    using System.IO;
    using System.Web;

    public class UploadHelper
    {
        #region Properties

        public long MaxFileSizeKb { get; set; }

        public string[] AllowedFileExtensions { get; set; }

        #endregion

        public void PostedFileWriteToStream(HttpPostedFileBase postedFile, Stream stream, int readBufferSize = 2048)
        {
            if (postedFile == null || postedFile.InputStream == null)
                throw new InvalidDataException("Unable to recognize posted file");

            if (postedFile.ContentLength == 0)
                throw new InvalidDataException("postedFile.ContentLength");

            if ((postedFile.ContentLength / 8) > MaxFileSizeKb * 1024)
            {
                throw new InvalidDataException(
                        "File is too big. Maximum upload file size is " + MaxFileSizeKb +
                        "kb. File is " + (postedFile.ContentLength / 8) + "kb."
                );
            }

            string fileNameWithoutExtension, fileNameExtension;
            CheckFileName(postedFile.FileName, out fileNameWithoutExtension, out fileNameExtension);

            if (AllowedFileExtensions != null && AllowedFileExtensions.Length > 0)
            {
                bool isAllowedExtension = false;
                foreach (var item in AllowedFileExtensions)
                {
                    if (string.Equals(string.Concat(".", item), fileNameExtension, System.StringComparison.OrdinalIgnoreCase))
                    {
                        isAllowedExtension = true;
                        break;
                    }
                }

                if (!isAllowedExtension)
                    throw new InvalidDataException("File extension is not allowed");
            }

            //TODO: check ContentType

            if (!postedFile.InputStream.CanRead)
                throw new InvalidDataException("Unable to read from posted file stream");

            if (!stream.CanWrite)
                throw new InvalidDataException("Unable to write to output stream");

            postedFile.InputStream.CopyTo(stream, readBufferSize);

            if (stream.CanSeek)
                stream.Position = 0;

        }

        public void SaveAs(HttpPostedFileBase postedFile, string directory, out string newFileName, int readWriteBufferSize = 4096)
        {
            string fileNameWithoutExtension, fileNameExtension;
            CheckFileName(postedFile.FileName, out fileNameWithoutExtension, out fileNameExtension);

            string randomFileName = RandomHelper.RandomString(15, true);
            newFileName = string.Concat(randomFileName, fileNameExtension);

            string path = Path.Combine(directory, newFileName);

            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
            {
                PostedFileWriteToStream(postedFile, fileStream, readWriteBufferSize);
            }
        }

        private void CheckFileName(string fileName, out string fileNameWithoutExtension, out string fileNameExtension)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new InvalidDataException("File name is null or empty");

            fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            if (string.IsNullOrWhiteSpace(fileNameWithoutExtension))
                throw new InvalidDataException("File name is invalid");

            fileNameExtension = Path.GetExtension(fileName);

            if (string.IsNullOrWhiteSpace(fileNameExtension))
                throw new InvalidDataException("File extension is invalid");
        }

    }
}