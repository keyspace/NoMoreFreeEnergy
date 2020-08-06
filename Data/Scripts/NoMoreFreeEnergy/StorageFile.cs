using Sandbox.ModAPI;
using System;
using VRage.Utils;

namespace Keyspace.NoMoreFreeEnergy
{
    /// <summary>
    /// Represents the configuration, loadable from a file.
    /// </summary>
    public class Config
    {
        // TODO

        public Config()
        {
            // Defaults; these properties will remain as below if the config couldn't be loaded.
            // TODO
        }
    }

    /// <summary>
    /// Helper class to load/save other simple class instances from/to XML files.
    /// </summary>
    public static class StorageFile
    {
        /// <summary>
        /// Loads a class instance that has a constructor from an XML file.
        /// </summary>
        /// <typeparam name="T">Type of the class to be loaded.</typeparam>
        /// <param name="fileName">
        /// Name of the file in per-save file storage, likely in:
        /// %appdata%\SpaceEngineers\Saves\%steamuserid%\%savename%\Storage\%steammodid%_Stamina
        /// </param>
        /// <returns></returns>
        public static T Load<T>(string fileName) where T: new()
        {
            T obj = new T();

            if (MyAPIGateway.Utilities.FileExistsInWorldStorage(fileName, typeof(T)))
            {
                try
                {
                    string contents;
                    using (var reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(fileName, typeof(T)))
                    {
                        contents = reader.ReadToEnd();
                    }

                    obj = MyAPIGateway.Utilities.SerializeFromXML<T>(contents);

                    MyLog.Default.WriteLineAndConsole($"Loaded {fileName}.");

                    return obj;
                }
                catch (Exception e)
                {
                    MyLog.Default.WriteLineAndConsole($"ERROR: Could not load {fileName}. Defaults will be used. Exception:");
                    MyLog.Default.WriteLineAndConsole(e.ToString());
                }
            }
            else
            {
                MyLog.Default.WriteLineAndConsole($"{fileName} not found. Defaults will be used.");
            }

            return obj;
        }

        /// <summary>
        /// Save a class instance to an XML file.
        /// </summary>
        /// <typeparam name="T">Type of the class to be saved.</typeparam>
        /// <param name="fileName">
        /// Name of the file in per-save file storage, likely in:
        /// %appdata%\SpaceEngineers\Saves\%steamuserid%\%savename%\Storage\%steammodid%_Stamina
        /// </param>
        /// <param name="obj">Instance to be saved.</param>
        public static void Save<T>(string fileName, T obj)
        {
            try
            {
                using (var writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(fileName, typeof(T)))
                {
                    writer.Write(MyAPIGateway.Utilities.SerializeToXML(obj));
                }

                MyLog.Default.WriteLineAndConsole($"Saved {fileName}.");
            }
            catch (Exception e)
            {
                MyLog.Default.WriteLineAndConsole($"ERROR: Could not save {fileName}. Exception:");
                MyLog.Default.WriteLineAndConsole(e.ToString());
            }
        }
    }
}
