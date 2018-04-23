﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DXVisualTestFixer.Core.Configuration {
    public static class ConfigSerializer {
        static Config cached;

        static readonly string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DXVisualTestFixer\\";
        static readonly string SettingsFile = "ui_settings.config";
        static string SettingsFilePath {
            get { return SettingsPath + SettingsFile; }
        }

        public static int Version { get; set; } = 0;

        public static Config GetConfig(bool useCache = true) {
            if(useCache && cached != null)
                return cached;
            return cached = GetConfigCore();
        }
        static Config GetConfigCore() {
            if(!File.Exists(SettingsFilePath))
                return Config.GenerateDefault();
            try {
                return Config.Validate(Serializer.Deserialize<Config>(SettingsFilePath));
            }
            catch {
                return Config.GenerateDefault();
            }
        }

        public static void SaveConfig(Config options) {
            cached = null;
            try {
                Serializer.Serialize(SettingsFilePath, options);
            }
            catch {
            }
        }
        public static bool IsConfigEquals(Config left, Config right) {
            return GetConfigAsString(left) == GetConfigAsString(right);
        }
        static string GetConfigAsString(Config config) {
            using(MemoryStream stream = new MemoryStream()) {
                Serializer.Serialize(stream, config);
                stream.Seek(0, SeekOrigin.Begin);
                using(StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}