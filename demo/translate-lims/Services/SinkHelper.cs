namespace Translate.Services
{
    public static class SinkHelper
    {
        public static long GetStringHashCode(string value)
        {
            long h = 0; // 默认值是0
            if (value.Length > 0)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    h = 31 * h + value[i]; // val[0]*31^(n-1) + val[1]*31^(n-2) + ... + val[n-1]
                }
            }
            return h;
        }
    }
}
