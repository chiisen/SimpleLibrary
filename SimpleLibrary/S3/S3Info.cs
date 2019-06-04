using SimpleLibrary.Logger;
using System.Drawing;

namespace SimpleLibrary.S3
{
    public class S3Info : PrintLogger
    {
        private readonly string _BucketName = "";
        private readonly string _AccessKeyID = "";
        private readonly string _SecretAccessKey = "";

        public string BucketName
        {
            get
            {
                return _BucketName;
            }
        }

        public string AccessKeyID
        {
            get
            {
                return _AccessKeyID;
            }
        }

        public string SecretAccessKey
        {
            get
            {
                return _SecretAccessKey;
            }
        }

        public S3Info(string bucketName, string accessKeyID, string secretAccessKey, ILogger logger = null)
        {
            AddLogger(logger);

            _BucketName      = bucketName;
            _AccessKeyID     = accessKeyID;
            _SecretAccessKey = secretAccessKey;

            Print($"BucketName : {_BucketName}", Color.Yellow);
            Print($"AccessKeyID : {_AccessKeyID}", Color.Yellow);
        }
    }
}
