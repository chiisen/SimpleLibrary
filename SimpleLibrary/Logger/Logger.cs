using System.Drawing;

namespace SimpleLibrary.Logger
{
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
