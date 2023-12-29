namespace Hugin.BookStore
{
    /*
     *  DEBUG时，注册唯一的EventName（相当于RabbitMQ的Routing Key）
     */
    internal static class EventNameConsts
    {
#if DEBUG
        public const string BookStoreEvent = "bookStoreEventDebug";
#else
        public const string BookStoreEvent = "bookStoreEvent";
#endif
    }
}