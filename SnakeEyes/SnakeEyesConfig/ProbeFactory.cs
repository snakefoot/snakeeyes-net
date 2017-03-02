using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeEyes
{
    internal class ProbeFactory
    {
        public List<Type> ConfigList { get; private set; }
        public List<Type> ProbeList { get; private set; }
        public List<Type> ListenerList { get; private set; }
        public List<Type> FilterList { get; private set; }

        List<Type> LoadList(Type interfaceType, string path)
        {
            List<Type> typeList = new List<Type>();

            string[] files = null;
            try
            {
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

        public ProbeFactory()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);

            ConfigList = LoadList(typeof(IProbeConfig), path);
            ProbeList = LoadList(typeof(IProbe), path);
            ProbeList.AddRange(LoadList(typeof(IProbe), System.IO.Path.Combine(path, "Probes")));
            ListenerList = LoadList(typeof(System.Diagnostics.TraceListener), path);
            ListenerList.Add(typeof(System.Diagnostics.EventLogTraceListener));
            FilterList = LoadList(typeof(System.Diagnostics.TraceFilter), path);
        }
    }
}
