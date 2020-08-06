using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Utils;

namespace Keyspace.Stamina
{
    /// <summary>
    /// Represents the server-provided configuration, loadable from a file.
    /// </summary>
    public class Config
    {
        // Configurables - must be properties for easy save/load to/from XML.
        public float GainHigh { get; set; }
        public float GainMedium { get; set; }
        public float GainLow { get; set; }
        public float CostNone { get; set; }
        public float CostLow { get; set; }
        public float CostMedium { get; set; }
        public float CostHigh { get; set; }

        public Config()
        {
            // Defaults; these properties will remain as below if the config couldn't be loaded.
            GainHigh   =  0.0050f;
            GainMedium =  0.0025f;
            GainLow    =  0.0005f;
            CostNone   =  0.0000f;
            CostLow    = -0.0005f;
            CostMedium = -0.0025f;
            CostHigh   = -0.0050f;
        }
    }

    /// <summary>
    /// Helper, used by PlayerStatsStorage below to represent a single array element.
    /// </summary>
    public struct StatElement
    {
        public ulong Id;
        public PlayerStats Stats;
    }

    /// <summary>
    /// Represents how much of the tracked stats each player has.
    /// 
    /// Helper class to work around dictionaries being non-serialisable to XML. The
    /// dictionary is converted to an array, so that can be serialised instead.
    /// </summary>
    public class PlayerStatsStorage
    {
        public StatElement[] PlayerStatElements { get; set; }

        public PlayerStatsStorage()
        {
            PlayerStatElements = new StatElement[0];
        }

        internal PlayerStatsStorage(Dictionary<ulong, PlayerStats> playerStatsDict)
        {
            PlayerStatElements = new StatElement[playerStatsDict.Count];

            int i = 0;
            foreach (ulong steamId in playerStatsDict.Keys)
            {
                PlayerStatElements[i].Id = steamId;
                PlayerStatElements[i].Stats = playerStatsDict[steamId];
                i++;
            }
        }

        internal Dictionary<ulong, PlayerStats> ToDict()
        {
            Dictionary<ulong, PlayerStats> playerStatsDict = new Dictionary<ulong, PlayerStats>();

            for (int i = 0; i < PlayerStatElements.Length; i++)
            {
                playerStatsDict.Add(PlayerStatElements[i].Id, PlayerStatElements[i].Stats);
            }

            return playerStatsDict;
        }
    }

    // TODO: Detect if type passed to Save()/Load() of StorageFile is a dictionary
    // and convert to array on the fly, instead of expecting that a dictionary will
    // never be passed.

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
