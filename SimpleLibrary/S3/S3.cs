using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Autofac;
using SimpleLibrary.Logger;
using System;
using System.Drawing;
using System.IO;

namespace SimpleLibrary.S3
{
    public class S3 : PrintLogger
    {
        private S3Info _S3Info           = null;
        private AmazonS3Config _S3Config = new AmazonS3Config();
        private AmazonS3Client _S3Client = null;

        /// <summary>
        /// 初始化 AWS 的 S3 服務
        /// </summary>
        /// <param name="bucketName">AWS S3 的 Bucket Name 名稱不能重複</param>
        /// <param name="accessKeyID">AWS 在 IAM 申請的 AccessKeyID</param>
        /// <param name="secretAccessKey">AWS 在 IAM 申請的 SecretAccessKey</param>
        public S3(string bucketName, string accessKeyID, string secretAccessKey, ContainerBuilder builder = null)
        {
            ILogger log_ = InitLogger(builder);            

            _S3Info = new S3Info(bucketName, accessKeyID, secretAccessKey, log_);

            _S3Config.RegionEndpoint = Amazon.RegionEndpoint.USWest2;

            _S3Client = new AmazonS3Client(accessKeyID, secretAccessKey, _S3Config);
        }

        /// <summary>
        /// 指定檔案上傳到 S3 服務，並回傳下載網址
        /// </summary>
        /// <param name="fileFullPath">上傳檔案的路徑</param>
        /// <returns>回傳下載網址</returns>
        public string UploadFile(string fileFullPath)
        {
            string preSignedUrl_ = "";
            if (!File.Exists(fileFullPath))
            {
                Print("Try to upload " + fileFullPath + " not exist", Color.Red);
                return preSignedUrl_;
            }
            string fileName_ = Path.GetFileName(fileFullPath);

            TransferUtility fileTransferUtility_ = new TransferUtility(_S3Client);

            try
            {
                fileTransferUtility_.Upload(fileFullPath, _S3Info.BucketName, fileName_);

                GetPreSignedUrlRequest request_ = new GetPreSignedUrlRequest
                {
                    BucketName = _S3Info.BucketName,
                    Key        = fileName_,
                    Expires    = DateTime.Now.AddDays(5)
                };
                preSignedUrl_ = _S3Client.GetPreSignedURL(request_);
            }
            catch (AmazonS3Exception e)
            {
                Print("Error encountered on S3 server. Message: " + e.Message + " when upload an object: " + fileFullPath, Color.Red);
            }
            catch (Exception e)
            {
                Print("Error encountered on server. Message: " + e.Message + " when upload an object: " + fileFullPath, Color.Red);
            }

            return preSignedUrl_;
        }
    }
}
