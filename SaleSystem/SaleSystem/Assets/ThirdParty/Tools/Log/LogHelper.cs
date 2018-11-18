using System;
using System.Text;
using UnityEngine;

namespace MyTools
{
    public class LogHelper
    {
        private static readonly StringBuilder SStringBuilderUnity = new StringBuilder(500);
        private static readonly object SLock = new object();
        private static string _sOutput = "";
        private static ELogLevel _logLevel = ELogLevel.Info;

        public static ELogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        public static void SetLogHandler(ILogHandler logHandler)
        {
            UnityEngine.Debug.unityLogger.logHandler = logHandler;
        }

        public static void Assert(string format, params object[] parameters)
        {
            if (_logLevel < ELogLevel.Assert)
            {
                return;
            }

            lock (SLock)
            {
                Output(ELogLevel.Assert, format, parameters);
            }
        }

        public static void Fatal(string format, params object[] parameters)
        {
            lock (SLock)
            {
                Output(ELogLevel.Fatal, format, parameters);
            }
        }

        public static void Error(string format, params object[] parameters)
        {
            lock (SLock)
            {
                Output(ELogLevel.Error, format, parameters);
            }
        }

        /// <summary>
        ///     带格式的输出
        /// </summary>
        /// <param name="format"></param>
        /// <param name="parameters"></param>
        public static void Warning(string format, params object[] parameters)
        {
            if (_logLevel < ELogLevel.Warning)
            {
                return;
            }

            lock (SLock)
            {
                Output(ELogLevel.Warning, format, parameters);
            }
        }

        public static void Info(string format, params object[] parameters)
        {
            if (_logLevel < ELogLevel.Info)
            {
                return;
            }

            lock (SLock)
            {
                Output(ELogLevel.Info, format, parameters);
            }
        }

        public static void Debug(string format, params object[] parameters)
        {
            if (_logLevel < ELogLevel.Debug)
            {
                return;
            }

            lock (SLock)
            {
                Output(ELogLevel.Debug, format, parameters);
            }
        }

        private static void Output(ELogLevel eLogLevel, string format, params object[] parameters)
        {
            if (parameters.Length == 0)
            {
                SStringBuilderUnity.AppendFormat("{0}:{1}:{2}", eLogLevel, DateTime.Now, format);
            }
            else
            {
                SStringBuilderUnity.AppendFormat("{0}:{1}:{2}", eLogLevel, DateTime.Now,
                    string.Format(format, parameters));
            }

            if (!SStringBuilderUnity.ToString().Equals(_sOutput))
            {
                _sOutput = SStringBuilderUnity.ToString();
                switch (eLogLevel)
                {
                    case ELogLevel.Assert:
                    case ELogLevel.Fatal:
                    case ELogLevel.Error:
                        UnityEngine.Debug.LogError(_sOutput);
                        break;
                    case ELogLevel.Warning:
                        UnityEngine.Debug.LogWarning(_sOutput);
                        break;
                    case ELogLevel.Info:
                    case ELogLevel.Debug:
                        UnityEngine.Debug.Log(_sOutput);
                        break;
                }
            }

            SStringBuilderUnity.Length = 0;
        }

        public enum ELogLevel
        {
            None,
            Fatal,
            Error,
            Warning,
            Info,
            Debug,
            Assert,
            All
        }
    }
}