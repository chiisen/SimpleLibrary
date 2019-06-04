using Autofac;
using System.Collections.Generic;
using System.Drawing;

namespace SimpleLibrary.Logger
{
    public class PrintLogger
    {
        /// <summary>
        /// Logger 的 DI 物件容器
        /// </summary>
        private List<ILogger> _Logger = new List<ILogger>() { new ConsoleLogger() };

        public void AddLogger(ILogger log)
        {
            if (log != null)
            {
                _Logger.Add(log);
            }
        }

        protected void Print(string msg, Color color)
        {
            _Logger.ForEach(x => x.Print(msg, color));
        }

        protected ILogger InitLogger(ContainerBuilder builder)
        {
            if (builder != null)
            {
                IContainer container_ = builder.Build();
                ILogger log_ = container_.Resolve<ILogger>();
                AddLogger(log_);
                return log_;
            }
            return null;
        }
    }

    /// <summary>
    /// Logger 的介面
    /// </summary>
    public interface ILogger
    {
        void Print(string msg, Color color);
    }

    /// <summary>
    /// 預設的 Console Logger
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Print(string msg, Color color)
        {
            System.Console.WriteLine(msg);
        }
    }

    /// <summary>
    /// 彩色的 Console Logger
    /// </summary>
    public class ColorfulLogger : ILogger
    {
        public void Print(string msg, Color color)
        {
            Colorful.Console.WriteLine(msg, color);
        }
    }
}
