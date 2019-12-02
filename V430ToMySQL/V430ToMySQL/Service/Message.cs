using GalaSoft.MvvmLight.Messaging;
using System;

namespace V430ToMySQL.Service
{
    public class Message
    {
        static public void Write(string msg, string logger = "", bool timestamp = true)
        {
            if (string.IsNullOrEmpty(msg)) return;
            string message = "";
            if (timestamp)
                message = DateTime.Now.ToString(" HH:mm:ss.fff") + " | ";
            if (!string.IsNullOrEmpty(logger))
                message += logger + " | ";
                Messenger.Default.Send<String>(message + msg, "Message");
        }
        static public void WriteLine(string msg, string logger = "", bool timestamp = true)
        {
            if (string.IsNullOrEmpty(msg)) return;
            string message = "";
            if (timestamp)
                message = DateTime.Now.ToString(" HH:mm:ss.fff") + " | ";
            if (!string.IsNullOrEmpty(logger))
                message += logger + " | ";
            Messenger.Default.Send<String>(message + msg + "\r\n", "Message");
        }
    }
}
