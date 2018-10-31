using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace RobokenTools
{
    public static class Exporter
    {
        public static string SerializeToText<T>(object data)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(data, Formatting.Indented, serializerSettings);
        }

        public static void SerializeToFile<T>(string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(SerializeToText<T>(path));
            }
        }

        public static T DeserializeFromText<T>(string text)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.DeserializeObject<T>(text, serializerSettings);
        }

        public static T DeserializeFromFile<T>(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return DeserializeFromText<T>(sr.ReadToEnd());
            }
        }
    }
}
