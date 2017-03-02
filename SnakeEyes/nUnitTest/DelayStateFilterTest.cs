using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace nUnitTest
{
    internal class DelayStateFilterTestListener : TraceListener
    {
        public bool TriggeredEvent { get; set; }

        public DelayStateFilterTestListener()
        {
            TriggeredEvent = false;
        }

        public override void Write(string msg)
        {
        }

        public override void WriteLine(string msg)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter != null && !Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                return;

            TriggeredEvent = true;
        }
    }

    [TestFixture]
    public class DelayStateFilterTest
    {
        [Test]
        public void Test_NextTriggerTime()
        {
            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DefaultSection section = new DefaultSection();
            string rawXml =
                @"<HelloWorld>
                <add key=""NextTriggerTime"" value=""2""/>
                <add key=""DelayTriggerTime"" value=""0""/>
                </HelloWorld>";
            section.SectionInformation.SetRawXml(rawXml);
            section.SectionInformation.Type = typeof(NameValueSectionHandler).FullName;
            config.Sections.Clear();
            config.Sections.Add("HelloWorld", section);
            config.Save();
            ConfigurationManager.RefreshSection("HelloWorld");

            SnakeEyes.DelayStateFilter filter = new SnakeEyes.DelayStateFilter("HelloWorld");
            Assert.AreEqual(filter.NextTriggerTime.TotalSeconds, 2);
            Assert.AreEqual(filter.DelayTriggerTime.TotalSeconds, 0);

            DelayStateFilterTestListener listener = new DelayStateFilterTestListener();
            listener.Filter = filter;

            TraceSource source = new TraceSource("TestSource", SourceLevels.All);
            source.Listeners.Add(listener);

            // Check first error is reported at once
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check any similar errors are not reported at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check any similar errors are not reported at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check that after a "long" time then the same error is reported again
            listener.TriggeredEvent = false;
            System.Threading.Thread.Sleep((int)filter.NextTriggerTime.TotalMilliseconds + 1);
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check first ok is reported at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check any following oks are not reported
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check that after a "long" time then the same ok is not reported again
            listener.TriggeredEvent = false;
            System.Threading.Thread.Sleep((int)filter.NextTriggerTime.TotalMilliseconds + 1);
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(!listener.TriggeredEvent);
        }

        [Test]
        public void Test_DelayTriggerTime()
        {
            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DefaultSection section = new DefaultSection();
            string rawXml =
                @"<HelloWorld>
                <add key=""NextTriggerTime"" value=""4""/>
                <add key=""DelayTriggerTime"" value=""2""/>
                </HelloWorld>";
            section.SectionInformation.SetRawXml(rawXml);
            section.SectionInformation.Type = typeof(NameValueSectionHandler).FullName;
            config.Sections.Clear();
            config.Sections.Add("HelloWorld", section);
            config.Save();
            ConfigurationManager.RefreshSection("HelloWorld");

            SnakeEyes.DelayStateFilter filter = new SnakeEyes.DelayStateFilter("HelloWorld");
            Assert.AreEqual(filter.NextTriggerTime.TotalSeconds, 4);
            Assert.AreEqual(filter.DelayTriggerTime.TotalSeconds, 2);

            DelayStateFilterTestListener listener = new DelayStateFilterTestListener();
            listener.Filter = filter;

            TraceSource source = new TraceSource("TestSource", SourceLevels.All);
            source.Listeners.Add(listener);

            // Check first error is not reported at once
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(!listener.TriggeredEvent);
            
            // Check any similar errors are not reported at once
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check that after a "long" time in the same error state, then it is reported
            System.Threading.Thread.Sleep((int)filter.DelayTriggerTime.TotalMilliseconds + 1);
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check any similar errors are not reported afterwards
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check that after an even "longer" time it is reported again (NextTriggerTime)
            System.Threading.Thread.Sleep((int)filter.NextTriggerTime.TotalMilliseconds + 1);
            source.TraceEvent(TraceEventType.Error, 0, "Damn");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check that the first good event doesn't trigger at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check any similar good news are not reported at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(!listener.TriggeredEvent);

            // Check that good news are first reported after a while
            System.Threading.Thread.Sleep((int)filter.DelayTriggerTime.TotalMilliseconds + 1);
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(listener.TriggeredEvent);

            // Check any following good news are not reported at once
            listener.TriggeredEvent = false;
            source.TraceEvent(TraceEventType.Information, 0, "Hurray");
            Assert.IsTrue(!listener.TriggeredEvent);
        }
    }
}
