using NPP.DE.Core.Services;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NPP.DE.Misc
{
    public class AssetLoader : IPersistent
    {
        /// <summary>
        /// this one load assets using <see cref="Resources.Load(string)"/>
        /// </summary>
        /// <typeparam name="T">type of asset that going to load</typeparam>
        /// <param name="fileName">asset name</param>
        /// <param name="path">asset path, must inside <see cref="Resources"/> folder</param>
        /// <returns></returns>
        public T Load<T>(string fileName, string path) where T : Object
        {
            string fullpath = System.IO.Path.Combine(path, fileName);
            return Resources.Load<T>(fullpath);
        }
    }
}