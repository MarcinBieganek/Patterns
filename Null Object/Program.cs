using System;
using System.IO;

namespace Zadanie_1
{
    public interface ILogger
    {
        void Log(string Message);
    }

    public enum LogType { None, Console, File}

    public class NoneLogger : ILogger
    {
        public void Log(string Message)
        {

        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string Message)
        {
            Console.WriteLine(Message);
        }
    }

    public class FileLogger : ILogger
    {
        StreamWriter writer;

        public FileLogger(string filePath)
        {
            FileStream file = File.Create(filePath);
            writer = new StreamWriter(file);
        }
        public void Log(string Message)
        {
            writer.WriteLine(Message);
            writer.Flush();
        }
    }


    public class LoggerFactory
    {
        private static LoggerFactory _instance;
        public ILogger GetLogger(LogType LogType, string Parameters = null)
        {
            switch (LogType)
            {
                case LogType.None:
                    return new NoneLogger();
                case LogType.Console:
                    return new ConsoleLogger();
                case LogType.File:
                    if (Parameters != null) {
                        return new FileLogger(Parameters);
                    } else {
                        throw new ArgumentException();
                    }
                default:
                    throw new ArgumentException();
            }
        }

        public static LoggerFactory Instance
        {
            get 
            { 
                if (_instance == null)
                {
                    _instance = new LoggerFactory();
                } 
                return _instance; 
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            LoggerFactory loggerFactory = LoggerFactory.Instance;

            ILogger fileLogger = loggerFactory.GetLogger(LogType.File, "test.txt");
            fileLogger.Log("file logger log1");
            fileLogger.Log("file logger log2");

            ILogger noneLogger = loggerFactory.GetLogger(LogType.None);
            noneLogger.Log("none logger none");

            ILogger consoleLogger = loggerFactory.GetLogger(LogType.Console);
            consoleLogger.Log("console logger log1");

        }
    }
}
