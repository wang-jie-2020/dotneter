namespace Hugin.Infrastructure
{
    public class Singleton<T> where T : new()
    {
        public static T Instance => SingletonCreator.Instance;

        private class SingletonCreator
        {
            static T _instance;

            internal static T Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (typeof(T))
                        {
                            _instance ??= new T();
                        }
                    }
                    return _instance;
                }
            }
        }
    }
}
