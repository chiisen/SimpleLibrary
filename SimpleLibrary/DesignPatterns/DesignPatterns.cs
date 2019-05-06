using System.Diagnostics;

namespace SimpleLibrary.DesignPatterns
{
    /// <summary>
    /// 給人繼承成為獨體
    /// </summary>
    /// <typeparam name="T">要繼承的類別</typeparam>
    public class Singleton<T> where T : class, new()
    {
        protected Singleton()
        {
            Debug.Assert(null == _instance);
        }
        private static readonly T _instance = new T();

        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
