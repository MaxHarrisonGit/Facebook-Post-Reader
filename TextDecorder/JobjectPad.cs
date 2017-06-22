using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDecorder
{
    class JobjectPad
    {
        public void Set(string filepath, object Obj)
        {
            using (StreamWriter file = File.CreateText(filepath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Obj);
            }
        }

        public object Get(string filepath, object Obj)
        {
            using (var reader = new StreamReader(filepath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer serializer = new JsonSerializer();
                JsonConvert.PopulateObject(serializer.Deserialize(jsonReader).ToString(), Obj);
            }
            return Obj;
        }
    }
}
