namespace Hugin.Platform
{
    /*
     *  DEBUG时，注册唯一的EventName（相当于RabbitMQ的Routing Key）
     */
    internal static class EventNameConsts
    {
#if DEBUG
        public const string PlatformEvent = "platformEventDebug";
#else
        public const string PlatformEvent = "platformEvent";
#endif
    }
}
