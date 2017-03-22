using System.Configuration;
using System.Diagnostics;
using NUnit.Framework;
using SnakeEyes;

namespace nUnitTest
{
    internal class PingProbeTestListener : TraceListener
    {
        public TraceEventType TriggeredEvent { get; set; }

        public PingProbeTestListener()
        {
            TriggeredEvent = TraceEventType.Start;
        }

        public override void Write(string msg)
        {
        }

        public override void WriteLine(string msg)
        {
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TriggeredEvent = eventType;
        }
    }

    [TestFixture]
    class PingProbeTest
    {
        [Test]
        public void Test_PingLocalhost()
        {
            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Clear();
            config.SectionGroups.Clear();
            DefaultSection section = new DefaultSection();
            string rawXml =
            @"<Test.PingProbe>
                <add key=""HostName"" value=""localhost""/>
                <add key=""TimeoutMs"" value=""1000""/>
                <add key=""SampleCount"" value=""3""/>
                <add key=""TTL"" value=""10""/>
                <add key=""MaxValue"" value=""10""/>
                <add key=""EventId"" value=""2""/>
                <add key=""EventType"" value=""Warning""/>
                <add key=""ProbeFrequency"" value=""1""/>
            </Test.PingProbe>";
            section.SectionInformation.SetRawXml(rawXml);
            section.SectionInformation.Type = typeof(NameValueSectionHandler).FullName;
            config.Sections.Add("Test.PingProbe", section);
            config.Save();
            ConfigurationManager.RefreshSection("Test.PingProbe");

            PingProbe pingProbe = new PingProbe();
            var traceSource = pingProbe.ConfigureProbe("Test.PingProbe");
            traceSource.Switch = new SourceSwitch("Test.PingProbe") { Level = SourceLevels.All };

            PingProbeTestListener probeListener = new PingProbeTestListener();
            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(probeListener);
            pingProbe.ExecuteProbe();

            Assert.AreEqual(TraceEventType.Information, probeListener.TriggeredEvent, "Failed to hear from PingProbe");
        }

        [Test]
        public void Test_PingUnknownhost()
        {
            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Clear();
            config.SectionGroups.Clear();
            DefaultSection section = new DefaultSection();
            string rawXml =
            @"<Test.PingProbe>
                <add key=""HostName"" value=""UnknownHost""/>
                <add key=""TimeoutMs"" value=""1000""/>
                <add key=""SampleCount"" value=""3""/>
                <add key=""TTL"" value=""10""/>
                <add key=""MaxValue"" value=""10""/>
                <add key=""EventId"" value=""2""/>
                <add key=""EventType"" value=""Warning""/>
                <add key=""ProbeFrequency"" value=""1""/>
            </Test.PingProbe>";
            section.SectionInformation.SetRawXml(rawXml);
            section.SectionInformation.Type = typeof(NameValueSectionHandler).FullName;
            config.Sections.Add("Test.PingProbe", section);
            config.Save();
            ConfigurationManager.RefreshSection("Test.PingProbe");

            PingProbe pingProbe = new PingProbe();
            var traceSource = pingProbe.ConfigureProbe("Test.PingProbe");
            traceSource.Switch = new SourceSwitch("Test.PingProbe") { Level = SourceLevels.All };

            PingProbeTestListener probeListener = new PingProbeTestListener();
            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(probeListener);
            pingProbe.ExecuteProbe();

            Assert.AreEqual(TraceEventType.Critical, probeListener.TriggeredEvent, "Failed to hear from PingProbe");
        }
    }
}
