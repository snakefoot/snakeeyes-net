using System.Collections.Generic;
using System.Configuration;

namespace SnakeEyesClient
{
    class ConfigItem
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    class ConfigManager
    {
        public List<ConfigItem> ProbeList { get; private set; }
        public List<ConfigItem> ListenerList { get; private set; }

        public ConfigManager(Configuration config)
        {
            // Find all trace sources (These are probes)
            ConfigurationSection diagnosticsSection = config.GetSection("system.diagnostics");

            ConfigurationElementCollection tracesources = diagnosticsSection.ElementInformation.Properties["sources"].Value as ConfigurationElementCollection;

            List<ConfigItem> probeList = new List<ConfigItem>();
            foreach (ConfigurationElement tracesource in tracesources)
            {
                string name = tracesource.ElementInformation.Properties["name"].Value.ToString();
                if (name.IndexOf('.') != -1)
                {
                    ConfigItem configItem = new ConfigItem();
                    configItem.Name = name.Substring(0, name.IndexOf('.'));
                    configItem.Type = name.Substring(name.IndexOf('.') + 1);
                    probeList.Add(configItem);
                }
            }

            ProbeList = probeList;

            // Find all shared listeners
            List<ConfigItem> listenerList = new List<ConfigItem>();
            ConfigurationElementCollection traceListeners = diagnosticsSection.ElementInformation.Properties["sharedListeners"].Value as ConfigurationElementCollection;
            foreach (ConfigurationElement listener in traceListeners)
            {
                ConfigItem configItem = new ConfigItem();
                configItem.Name = listener.ElementInformation.Properties["name"].Value.ToString();
                configItem.Type = listener.ElementInformation.Properties["type"].Value.ToString();
                listenerList.Add(configItem);
            }

            ListenerList = listenerList;
        }
    }
}
