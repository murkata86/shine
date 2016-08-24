namespace Shine.Core
{
    using System;
    using UI;

    public static class EventLogger
    {
        public static void Logger(string message)
        {
            DateTime dt = DateTime.Now;

            string log = string.Format("{0} - {1}", dt, message);

            OutputWriter.WriteToConsole(log);
        }
    }
}
