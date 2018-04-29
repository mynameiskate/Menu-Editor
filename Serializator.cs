using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DishesHierarchy
{
    static class Serializator
    {
        private const string XMLPath = "menu.xml";
        private const string BinPath = "menu.dat";

        public static bool XMLSerialize(DishList list)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(list.GetType());
                using (FileStream fs = new FileStream(XMLPath, FileMode.Create))
                {
                    serializer.Serialize(fs, list);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DishList XMLDeserialize()
        {
            try
            {
                var list = new DishList();
                XmlSerializer serializer = new XmlSerializer(list.GetType());
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

        public static bool BinarySerialize(DishList list)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(BinPath, FileMode.Create))
                {
                    formatter.Serialize(fs, list);
                    fs.Flush();
                }
                return true;
            }
            catch
            {
                return false;
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
