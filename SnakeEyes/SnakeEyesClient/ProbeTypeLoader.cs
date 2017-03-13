using System;
using System.Collections.Generic;

namespace SnakeEyesClient
{
    static class ProbeTypeLoader
    {
        public static List<Type> LoadList(Type interfaceType, string path)
        {
            List<Type> typeList = new List<Type>();

            string[] files = null;
            try
            {
                if (!System.IO.Directory.Exists(path))
                    return typeList;

                files = System.IO.Directory.GetFiles(path, "*.dll");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return typeList;
            }
            foreach (string file in files)
            {
                System.Reflection.Assembly plugin = System.Reflection.Assembly.LoadFile(file);
                foreach (Type type in plugin.GetTypes())
                {
                    if (type.GetInterface(interfaceType.FullName) != null)
                        typeList.Add(type);
                }
            }
            return typeList;
        }
    }
}
