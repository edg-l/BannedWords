using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BannedWords
{
    public class ConfigFile
    {
        public char WordReplacement = '*';
        public List<string> BannedWords = new List<string>();

        public static ConfigFile Read(string path)
        {
            if (!File.Exists(path))
            {
                ConfigFile config = new ConfigFile();

                config.BannedWords.Add("noob");
                config.BannedWords.Add("idiot");
                config.BannedWords.Add("fuck");

                File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented));
                return config;
            }

            return JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path));
        }
    }
}