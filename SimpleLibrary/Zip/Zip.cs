using ICSharpCode.SharpZipLib.Zip;
using SimpleLibrary.Logger;
using System;
using System.Drawing;
using System.IO;
using Autofac;

namespace SimpleLibrary.Zip
{
    public class Zip
    {
        private ILogger _Logger = new ConsoleLogger();

        private void Print(string msg, Color color)
        {
            if (_Logger != null)
            {
                _Logger.Print(msg, color);
            }
        }

        private void InitLogger(ContainerBuilder builder)
        {
            if (builder != null)
            {
                IContainer container_ = builder.Build();
                _Logger = container_.Resolve<ILogger>();
            }
        }

        public Zip(ContainerBuilder builder = null)
        {
            InitLogger(builder);
        }

        /// <summary>
        /// 檢查並提示 S3 的下載網址不能有 ' 分號
        /// </summary>
        /// <param name="filePath">上傳 S3 的檔案路徑</param>
        private void CheckS3Path(string filePath)
        {
            string path_ = filePath;
            if (path_.Contains("'") == true)
            {
                // S3 的下載網址不能有 ' 分號
                string error_ = $@"S3 的下載網址不能有 ' 分號 path = {path_}";
                Print(error_, Color.OrangeRed);
            }
        }

        /// <summary>
        /// 壓縮指定目錄為 zip 檔案
        /// </summary>
        /// <param name="outputZipPath">輸出的 zip 檔案路徑</param>
        /// <param name="inputDirectory">要壓縮的目錄路徑</param>
        public void ZipTo(string outputZipPath, string inputDirectory)
        {
            // 修正 S3 的下載網址不能有 ' 分號
            CheckS3Path(outputZipPath);

            ZipOutputStream zipStream_ = new ZipOutputStream(File.Create(outputZipPath));
            zipStream_.SetLevel(9);

            ZipFolder(inputDirectory, inputDirectory, zipStream_);

            zipStream_.Finish();
            zipStream_.Close();
        }

        /// <summary>
        /// 壓縮指定目錄
        /// </summary>
        /// <param name="rootFolder">指定要壓縮的目錄同 currentFolder</param>
        /// <param name="currentFolder">指定要壓縮的目錄同 rootFolder</param>
        /// <param name="zipStream">ZipOutputStream 的參考</param>
        private static void ZipFolder(string rootFolder, string currentFolder, ZipOutputStream zipStream)
        {
            string[] SubFolders_ = Directory.GetDirectories(currentFolder);

            foreach (string Folder in SubFolders_)
            {
                ZipFolder(rootFolder, Folder, zipStream);
            }

            string relativePath_ = currentFolder.Substring(rootFolder.Length) + "/";

            if (relativePath_.Length > 1)
            {
                ZipEntry dirEntry_;

                dirEntry_ = new ZipEntry(relativePath_)
                {
                    DateTime = DateTime.Now
                };
            }

            foreach (string file in Directory.GetFiles(currentFolder))
            {
                AddFileToZip(zipStream, relativePath_, file);
            }
        }

        /// <summary>
        /// 新增檔案到 zip 檔案內
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="relativePath"></param>
        /// <param name="file"></param>
        private static void AddFileToZip(ZipOutputStream zipStream, string relativePath, string file)
        {
            byte[] buffer_ = new byte[4096];
            string fileRelativePath_ = (relativePath.Length > 1 ? relativePath : string.Empty) + Path.GetFileName(file);
            ZipEntry entry_ = new ZipEntry(fileRelativePath_)
            {
                DateTime = DateTime.Now
            };
            zipStream.PutNextEntry(entry_);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes_;

                do
                {
                    sourceBytes_ = fs.Read(buffer_, 0, buffer_.Length);
                    zipStream.Write(buffer_, 0, sourceBytes_);
                } while (sourceBytes_ > 0);
            }
        }
    }
}
