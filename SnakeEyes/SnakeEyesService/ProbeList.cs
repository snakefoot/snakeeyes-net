using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SnakeEyes
{
    class ProbeList : IDisposable
    {
        SortedList<DateTime, IProbe> _list = new SortedList<DateTime, IProbe>();

        public int Count { get { return _list.Count; } }

        public void Dispose()
        {
            foreach (KeyValuePair<DateTime, IProbe> probe in _list)
                probe.Value.Dispose();
            PerformanceCounter.CloseSharedResources();
            _list.Clear();
        }

        public void AddProbe(DateTime nextPoll, IProbe probe)
        {
            while(_list.IndexOfKey(nextPoll)!=-1)
                nextPoll += TimeSpan.FromMilliseconds(1);

            _list.Add(nextPoll, probe);
        }

        public DateTime GetNextPollTime()
        {
            return _list.Keys[0];
        }

        public IProbe GetNextPollProbe()
        {
            return _list.Values[0];
        }

        public void PollExecuted(TimeSpan nextPoll, IProbe probe)
        {
            _list.RemoveAt(0);
            AddProbe(DateTime.UtcNow + nextPoll, probe);
        }
    }
}
