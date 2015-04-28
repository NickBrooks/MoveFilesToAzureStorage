using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveFilesToAzureStorage
{
    class ENV
    {

        private const string CONFIG = "config.json";
        public static string Get(string key)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), CONFIG);
            if (File.Exists(path))
            {
                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                if (config.ContainsKey(key))
                {
                    return config[key];
                }
            }
            else
            {
                try
                {
                    return Environment.GetEnvironmentVariable(key);
                }
                catch { }
            }
            return null;
        }

    }
}
