using BepInEx.Logging;
namespace WolfoQoL_Client
{
    internal static class Log
    {
        public static ManualLogSource log;

        public static void LogMessage(object message)
        {
            log.LogMessage(message);
        }
        public static void LogWarning(object message)
        {
            log.LogWarning(message);
        }
        public static void LogError(object message)
        {
            log.LogError(message);
        }
    }
}