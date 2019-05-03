using Autofac;
using SimpleLibrary.Logger;
using System.Drawing;

namespace SimpleLibrary.S3
{
    public class S3Info
    {
        private ILogger _Logger = new ConsoleLogger();

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

        public S3Info(string bucketName, string accessKeyID, string secretAccessKey, ContainerBuilder builder = null)
        {
            InitLogger(builder);

            _BucketName      = bucketName;
            _AccessKeyID     = accessKeyID;
            _SecretAccessKey = secretAccessKey;

            Print($"BucketName : {_BucketName}", Color.Yellow);
            Print($"AccessKeyID : {_AccessKeyID}", Color.Yellow);
        }

        private void InitLogger(ContainerBuilder builder)
        {
            if (builder != null)
            {
                IContainer container_ = builder.Build();
                _Logger = container_.Resolve<ILogger>();
            }
        }

        private void Print(string msg, Color color)
        {
            if (_Logger != null)
            {
                _Logger.Print(msg, color);
            }
        }
    }
}
