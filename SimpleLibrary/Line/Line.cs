using Autofac;
using Newtonsoft.Json;
using SimpleLibrary.Logger;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace SimpleLibrary.Line
{
    public class Line
    {
        private ILogger _Logger = new ConsoleLogger();
        private string _Url     = "";
        private string _UserId  = "";        
        private string _ApiKey  = "";

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

        public Line(string url, string userId, string apiKey, ContainerBuilder builder = null)
        {
            _Url    = url;
            _UserId = userId;            
            _ApiKey = apiKey;

            InitLogger(builder);
        }

        /// <summary>
        /// 送出 Line 的通知訊息
        /// </summary>
        /// <param name="message">訊息的內容字串</param>
        public void Notify(string message)
        {
            string word_ = message;

            //JSON
            var msg_ = new
            {
                to = _UserId,
                messages = new[] {
                    new {
                        type = "text",
                        text = word_
                    }
                }
            };

            //POST
            string msgStr_ = JsonConvert.SerializeObject(msg_);
            Uri myUri_ = new Uri(_Url);
            var data_ = Encoding.UTF8.GetBytes(msgStr_);
            SendRequest(myUri_, data_, "application/json", "POST", _ApiKey);
        }

        /// <summary>
        /// 透過網頁方式傳送資訊給伺服器
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="jsonDataBytes"></param>
        /// <param name="contentType"></param>
        /// <param name="method"></param>
        /// <param name="authorization">授權碼</param>
        private static void SendRequest(Uri uri, byte[] jsonDataBytes, string contentType, string method, string authorization)
        {
            WebRequest req_ = WebRequest.Create(uri);
            {
                req_.ContentType = contentType;
                req_.Method = method;
                req_.ContentLength = jsonDataBytes.Length;
                req_.Headers.Add("Authorization", $"Bearer {authorization}");

                var stream = req_.GetRequestStream();
                stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);
                stream.Close();

                WebResponse response = req_.GetResponse();
                {
                    stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    reader.ReadToEnd();
                }
            }
        }

    }
}
