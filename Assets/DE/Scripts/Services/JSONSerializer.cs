//Note for Installing Newtonsoft Json :
//Open < project >/ Packages / manifest.json, then add the package com.unity.nuget.newtonsoft-json in the list of dependencies.
//
//À la:
//
//{
//    "dependencies": {
//        "com.unity.nuget.newtonsoft-json": "3.0.2",
//
//    // ...
//  }
//}
//// ... signals the rest of the packages you have in the manifest.json, such as all the "com.unity.*" dependencies
//
//Version 3.0.1 was the latest at the time of writing (2022-03-03). You can find latest version here: https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html

using System.IO;
//using Unity.Plastic.Newtonsoft.Json;
using Newtonsoft.Json;
using NPP.DE.Core.Services;
using UnityEngine;

namespace NPP.DE.Misc
{
    public class JSONSerializer : IPersistent
    {
        public void WriteJSON<T>(T data, string folder, string fileName) where T : ISerializable
        {
            string filePath = Path.Combine(Application.persistentDataPath, folder, $"{fileName}.json");
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public T ReadJSON<T>(string folder, string fileName) where T : ISerializable
        {
            string filePath = Path.Combine(Application.persistentDataPath, folder, $"{fileName}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                T data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    ObjectCreationHandling = ObjectCreationHandling.Reuse
                });

                return data;
            }

            return default(T);
        }
    }
}