using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace NPP.DE.Misc
{
    public interface ISerializable { }

    public class BinarySerializer
    {
        public void Serialize<T>(T data, string folder, string fileName) where T : ISerializable
        {
            string filePath = Path.Combine(Application.persistentDataPath, folder, $"{fileName}-save.sv");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(filePath, FileMode.Create);

            bf.Serialize(fs, (T)data);
        }

        public T Deserialize<T>(string folder, string fileName) where T : ISerializable
        {
            string filePath = Path.Combine(Application.persistentDataPath, folder, $"{fileName}-save.sv");

            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(filePath, FileMode.Open);

                return (T)bf.Deserialize(fs);
            }

            return default(T);
        }
    }
}