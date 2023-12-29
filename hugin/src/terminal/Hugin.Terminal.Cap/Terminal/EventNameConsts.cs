namespace Hugin.Terminal
{
    /*
     *  DEBUG时，注册唯一的EventName（相当于RabbitMQ的Routing Key）
     */
    internal static class EventNameConsts
    {
#if DEBUG
        public const string TerminalEvent = "terminalEventDebug";
#else
        public const string TerminalEvent = "terminalEvent";
#endif
    }
}