using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;



namespace TextDecorder
{
    class XMLConverter
    {
        public void Set(string Local, object Item)
        {
            XmlSerializer x = new XmlSerializer(Item.GetType());
            TextWriter writer = new StreamWriter(Local);
            x.Serialize(writer, Item);
            writer.Close();
        }

        public object Get(string Local, object ObjectType)
        {


            XmlSerializer x = new XmlSerializer(ObjectType.GetType());
            FileStream fs = new FileStream(Local, FileMode.Open);
            ObjectType = x.Deserialize(fs);
            fs.Close();
            return ObjectType;


        }
    }
}
