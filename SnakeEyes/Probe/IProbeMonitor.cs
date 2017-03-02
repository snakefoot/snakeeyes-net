using System;
using System.Diagnostics;
using System.Text;

namespace SnakeEyes
{
    public interface IProbeMonitor : IDisposable
    {
        TraceSource StartMonitor(string configName);
        void RegisterProbe(TraceSource probe);
    }
}
