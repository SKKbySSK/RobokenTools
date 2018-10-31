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
        public static string SerializeToText<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static void SerializeToFile<T>(T data, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.Write(SerializeToText(data));
            }
        }

        public static T DeserializeFromText<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
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
