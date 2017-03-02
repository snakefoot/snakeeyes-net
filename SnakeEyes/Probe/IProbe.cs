using System;
using System.Diagnostics;
using System.Text;

namespace SnakeEyes
{
    public interface IProbe : IDisposable
    {
        TraceSource ConfigureProbe(string configName);
        TimeSpan ExecuteProbe();
    }
}
