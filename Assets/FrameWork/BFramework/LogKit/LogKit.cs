using System.Text;

namespace BFramework
{
    using System;
    using UnityEngine;
    [Obsolete("QConsole=>ConsoleWindow",true)]
    public class QConsole : ConsoleWindow
    {
    }

    public class LogKit
    {

        public static void I(object msg, params object[] args)
        {
            if (mLogLevel < LogLevel.Normal)
            {
                return;
            }

            if (args == null || args.Length == 0)
            {
                Debug.Log(msg);
            }
            else
            {
                Debug.LogFormat(msg.ToString(), args);
            }
        }
        


        
        public static void W(object msg,params object[] args)
        {
            if (mLogLevel < LogLevel.Warning)
            {
                return;
            }

            if (args == null || args.Length == 0)
            {

                Debug.LogWarning(msg);
            }
            else
            {
                Debug.LogWarningFormat(msg.ToString(), args);
            }
        }
        

        public static void E(object msg, params object[] args)
        {
            if (mLogLevel < LogLevel.Error)
            {
                return;
            }

            if (args == null || args.Length == 0)
            {
                Debug.LogError(msg);
            }
            else
            {
                Debug.LogError(string.Format(msg.ToString(), args));
            }
        }


        public static void E(Exception e)
        {
            if (mLogLevel < LogLevel.Exception)
            {
                return;
            }

            Debug.LogException(e);
        }

        public static StringBuilder Builder()
        {
            return new StringBuilder();
        }
        

        public enum LogLevel
        {
            None = 0,
            Exception = 1,
            Error = 2,
            Warning = 3,
            Normal = 4,
            Max = 5,
        }

        private static LogLevel mLogLevel = LogLevel.Normal;

        public static LogLevel Level
        {
            get => mLogLevel;
            set => mLogLevel = value;
        }
    }

    public static class LogKitExtension
    {
        public static StringBuilder GreenColor(this StringBuilder self, Action<StringBuilder> childContent)
        {
            self.Append("<color=green>");
            childContent?.Invoke(self);
            self.Append("</color>");
            return self;
        }
        
        public static void LogInfo<T>(this T selfMsg)
        {
            LogKit.I(selfMsg);
        }

        public static void LogWarning<T>(this T selfMsg)
        {
            LogKit.W(selfMsg);
        }

        public static void LogError<T>(this T selfMsg)
        {
            LogKit.E(selfMsg);
        }

        public static void LogException(this Exception selfExp)
        {
            LogKit.E(selfExp);
        }
    }
}