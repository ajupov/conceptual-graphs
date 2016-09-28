using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elan.Models.Implementations.Others;

namespace Elan.Services
{
    public static class SettingsManager
    {
        private const string SettingsFileName = "DbSettings.ini";
        private const string ConnectionStringPattern = "ConnectionString";
        private const string InitialCatalogPattern = "Initial Catalog";
        private const string DefaultDbName = "elan";

        public static void SaveDbSettings(DbSettings dbSettings)
        {
            using (var fileStream = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine($"{ConnectionStringPattern}={dbSettings.ConnectionString}");
                }
            }
        }

        public static DbSettings LoadDbSettings()
        {
            var dbSettings = new DbSettings();
            var lines = File.ReadAllLines(SettingsFileName);
            dbSettings.ConnectionString = FormatSetting(lines, ConnectionStringPattern);
            return dbSettings;
        }

        public static void ReplaceDefaultDbName()
        {
            const char delimiter = ';';

            var dbSettings = LoadDbSettings();
            var splitSettings = dbSettings.ConnectionString.Split(delimiter);
            var newSplitSettings = splitSettings.Where(s => !s.StartsWith(InitialCatalogPattern)).ToList();
            newSplitSettings.Add($"{InitialCatalogPattern}={DefaultDbName}");
            dbSettings.ConnectionString = string.Join(delimiter.ToString(), newSplitSettings);
            SaveDbSettings(dbSettings);
        }

        private static string FormatSetting(IEnumerable<string> settingLines, string settingName)
        {
            return settingLines.LastOrDefault(l => l.StartsWith(settingName))
                ?.Replace(settingName, string.Empty).Trim(' ').TrimStart('=').TrimStart(' ');
        }
    }
}