using Autofac;
using SimpleLibrary.Logger;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Mail;

namespace SimpleLibrary.GMail
{
    public class GMail
    {
        /// <summary>
        /// 發信人的 Email
        /// </summary>
        private readonly string _EmailAddress = "";

        /// <summary>
        /// 發信人的 Email 密碼
        /// </summary>
        private readonly string _EmailPassword = "";

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

        public GMail(string emailAddress, string emailPassword, ContainerBuilder builder = null)
        {
            _EmailAddress  = emailAddress;
            _EmailPassword = emailPassword;

            InitLogger(builder);
        }

        public void SendMessage(string displayName, string subject, string body, List<string> ToAdd)
        {
            MailMessage mailMessage_ = new MailMessage
            {
                // 前面是發信 email 後面是顯示的名稱
                From = new MailAddress(_EmailAddress, displayName)
            };

            // 收信者 email
            for (int i = 0; i < ToAdd.Count; ++i)
            {
                mailMessage_.To.Add(ToAdd[i]);
            }

            if (mailMessage_.To.Count > 0)
            {

                // 設定優先權
                mailMessage_.Priority = MailPriority.Normal;

                // 標題
                mailMessage_.Subject = subject;

                // 內容
                mailMessage_.Body = body;

                // 內容使用 html
                mailMessage_.IsBodyHtml = true;

                // 設定 gmail 的 smtp (這是 google 的)
                SmtpClient smtpClient_ = new SmtpClient("smtp.gmail.com", 587)
                {
                    // 您在 gmail 的帳號密碼
                    Credentials = new NetworkCredential(_EmailAddress, _EmailPassword),

                    // 開啟ssl
                    EnableSsl = true
                };

                // 發送郵件
                smtpClient_.Send(mailMessage_);

                // 放掉宣告出來的MySmtp
                smtpClient_ = null;
            }

            // 放掉宣告出來的 mail
            mailMessage_.Dispose();
        }
    }
}
