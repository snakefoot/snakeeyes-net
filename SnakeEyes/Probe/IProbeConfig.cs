using System;
using System.Configuration;
using System.Text;

namespace SnakeEyes
{
    public interface IProbeConfig
    {
        bool AcceptType(Type assemblyType);
        bool LaunchConfig(Configuration configFile, string configName);
    }
}
