namespace OMR.Core.Helpers
{
    using System;
    using System.IO;

    public static class IOHelper
    {
        #region File

        #region CreateFile

        public static void CreateFile(byte[] content, string fullPath)
        {
            CreateFile(content, fullPath, false);
        }

        public static void CreateFile(Stream stream, string fullPath)
        {
            if (stream == null)
                throw new ArgumentException("stream");

            byte[] content = new byte[stream.Length];
            stream.Read(content, 0, (int)stream.Length);

            CreateFile(content, fullPath, false);
        }

        public static void CreateFile(byte[] content, string fullPath, bool canOverride)
        {
            if (content == null)
                throw new ArgumentException("content");
            if (fullPath == null)
                throw new ArgumentException("fullPath");
            if (fullPath == string.Empty)
                throw new ArgumentException("fullPath");

            if (File.Exists(fullPath) && !canOverride)
                throw new IOException("File already exists");

            File.WriteAllBytes(fullPath, content);
        }

        #endregion


        public static byte[] ReadFile(string fullPath)
        {
            if (fullPath == null || string.IsNullOrEmpty(fullPath))
                throw new ArgumentException("fullPath");

            return File.ReadAllBytes(fullPath);
        }

        public static void Rename(string fullPath, string newName)
        {
            if (fullPath == null)
                throw new ArgumentException("fullPath");
            if (newName == null)
                throw new ArgumentException("newName");

            FileInfo fileInfo = new FileInfo(fullPath);
            string newPath = Path.Combine(Path.GetDirectoryName(fullPath), newName);

            if (File.Exists(newPath))
                throw new IOException("File already exists");

            fileInfo.MoveTo(newPath);
        }

        public static void ChangeExtention(string fullPath, string newExtention)
        {
            if (fullPath == null)
                throw new ArgumentException("fullPath");
            if (newExtention == null)
                throw new ArgumentException("newExtention");

            FileInfo fileInfo = new FileInfo(fullPath);

            string newPath = Path.Combine(
                                            Path.Combine
                                            (
                                                Path.GetDirectoryName(fullPath),
                                                Path.GetFileNameWithoutExtension(fullPath)
                                            ),
                                            Path.Combine
                                            (
                                                ".",
                                                newExtention
                                            )
                                        );

            if (File.Exists(newPath))
                throw new IOException("File already exists");

            fileInfo.MoveTo(newPath);
        }

        #endregion

        public static void CreateDirectory(string fullPath, bool createIfNotExist)
        {
            AssertHelper.AreNotNullOrEmpty(fullPath);

            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

            if (Directory.Exists(fullPath) && !createIfNotExist)
                throw new IOException("File already exists");

            Directory.CreateDirectory(fullPath);
        }

        public static string GetReadableFileSize(long length)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            var i = 0;
            while (length >= 1024)
            {
                length /= 1024;
                ++i;
            }

            return string.Format("{0} {1}", length, units[i]);
        }
    }
}
