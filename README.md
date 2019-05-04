# SimpleLibrary
Simple program using the.net Standard


```csharp
// 檔案上傳到 S3
string bucketName_     = ConfigurationManager.AppSettings["BucketName"];
string accessKeyID     = ConfigurationManager.AppSettings["AccessKeyID"];
string secretAccessKey = ConfigurationManager.AppSettings["SecretAccessKey"];

// 透過 AutoFac 特化專案的 Logger 方法給 SimpleLibrary.S3 使用
ContainerBuilder builder_ = new ContainerBuilder();
builder_.RegisterType<MyLogger>().As<ILogger>();

S3 s3_ = new S3(bucketName_, accessKeyID, secretAccessKey, builder_);
string urlS3_ = s3_.UploadFile(@"C:\CSharpDLL\SimpleLibrary.nupkg");
```

MyLogger 是使用 DI 框架 AutoFac 主要目的是要傳遞目前專案的 Logger 方式給 SimpleLibrary.S3 底層使用
使用方法如下程式碼範例：

```csharp
public class MyLogger : ILogger
{
    public void Print(string msg, Color color)
    {
        // 寫入專案呼叫 Logger 的方式傳遞給 SimpleLibrary.S3 底層使用
        Log.Print(msg, color);
    }
}
```

