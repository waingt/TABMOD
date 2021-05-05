using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModLoader
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RunStaticConstructorAttribute : Attribute
    {
    }
    /// <summary>
    /// log message in Mods/ModLog.txt
    /// </summary>
    public static class Logger
    {
        public enum Level
        {
            INFO,
            ERROR
        }
        public static StreamWriter logger = new StreamWriter("Mods/ModLog.txt");
        static void write(Level level, string message)
        {
            logger.WriteLine(string.Format("{0} - {1} - {2} - {3}", DateTime.Now.ToShortTimeString(), new StackTrace().GetFrame(2).GetMethod().DeclaringType.FullName, Enum.GetName(typeof(Level), level), message));
            logger.Flush();
        }
        /// <summary>
        /// log message
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message) => write(Level.INFO, message);
        /// <summary>
        /// log error
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message) => write(Level.ERROR, message);
        /// <summary>
        /// log exception
        /// </summary>
        /// <param name="ex"></param>
        public static void Exception(Exception ex) =>
            write(Level.ERROR, ex.ToString());
        public static void Close() => logger.Close();
    }
    public class ModLoader
    {
        public static void Run()
        {
            try
            {
                Logger.Info("Start");
                int num = 0;
                if (Directory.Exists("Mods"))
                {
                    string[] directories = Directory.GetDirectories("Mods");
                    Logger.Info(string.Format("{0} mods found", directories.Length));
                    foreach (string directory in directories)
                    {
                        string path = Path.Combine(directory, Path.GetFileName(directory) + ".dll");
                        Logger.Info("Loading mod file: " + path);
                        foreach (var t in Assembly.LoadFrom(path).DefinedTypes)
                        {
                            if (t.GetCustomAttribute<RunStaticConstructorAttribute>() != null)
                            {
                                try
                                {
                                    Logger.Info("Running static constructor of: " + t.FullName);
                                    RuntimeHelpers.RunClassConstructor(t.TypeHandle);
                                    num++;
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("An exception occured durring running static constructor of: " + t.FullName);
                                    Logger.Exception(ex);
                                }
                            }
                        }
                    }
                    Logger.Info(string.Format("{0} mods successfully loaded", num));
                }
                else
                {
                    Logger.Info("No Mods folder, return");
                }
            }
            catch (Exception e) { Logger.Exception(e); }
            //Logger.Close();
        }
    }
}
