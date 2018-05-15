using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DishesHierarchy
{
    static class Serializator
    {
        private const string XMLPath = "menu.xml";
        private const string BinPath = "menu.dat";

        public static string XMLSerialize(DishList list)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DishList), new XmlRootAttribute("dish_list"));
                using (FileStream fs = new FileStream(XMLPath, FileMode.Create))
                {
                    serializer.Serialize(fs, list);
                }
                return XMLPath;
            }
            catch
            {
                return null;
            }
        }

        public static DishList XMLDeserialize()
        {
            try
            {
                var list = new DishList();
                XmlSerializer serializer = new XmlSerializer(typeof(DishList), new XmlRootAttribute("dish_list"));
                using (FileStream fs = new FileStream(XMLPath, FileMode.Open))
                {
                    list = (DishList)serializer.Deserialize(fs);
                }
                return list;
            }
            catch
            {
                return null;
            }
            
        }

        public static string BinarySerialize(DishList list)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(BinPath, FileMode.Create))
                {
                    formatter.Serialize(fs, list);
                    fs.Flush();
                }
                return BinPath;
            }
            catch
            {
                return null;
            }
        }

        public static DishList BinaryDeserialize()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var list = new DishList();
                using (FileStream fs = new FileStream(BinPath, FileMode.Open))
                {
                    list = (DishList)formatter.Deserialize(fs);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

    }
}
