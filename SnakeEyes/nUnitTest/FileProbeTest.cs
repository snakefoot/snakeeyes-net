using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using SnakeEyes;

namespace nUnitTest
{
    internal class FileProbeTestListener : TraceListener
    {
        public TraceEventType TriggeredEvent { get; set; }

        public FileProbeTestListener()
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
    public class FileProbeTest
    {
        [Test]
        public void Test_MaxFileSize()
        {
            var testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test");
            Directory.CreateDirectory(testDirectory);
            var testFile = Path.Combine(testDirectory, "Hello.txt");
            if (File.Exists(testFile))
                File.Delete(testFile);

            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Clear();
            config.SectionGroups.Clear();
            DefaultSection section = new DefaultSection();
            string rawXml =
            @"<Test.FileProbe>
            <add key=""FileName"" value=""{0}""/>
            <add key=""MaxFileSize"" value=""0""/>
            <add key=""MaxFileAge"" value=""1""/>
            <add key=""EventId"" value=""30""/>
            <add key=""EventType"" value=""Warning""/>
            <add key=""ProbeFrequency"" value=""10""/>
            </Test.FileProbe>";
            rawXml = string.Format(rawXml, testFile);

            section.SectionInformation.SetRawXml(rawXml);
            section.SectionInformation.Type = typeof(NameValueSectionHandler).FullName;
            config.Sections.Add("Test.FileProbe", section);
            config.Save();
            ConfigurationManager.RefreshSection("Test.FileProbe");

            FileProbe fileProbe = new FileProbe();
            var traceSource = fileProbe.ConfigureProbe("Test.FileProbe");
            traceSource.Switch = new SourceSwitch("Test.FileProbe") { Level = SourceLevels.All };

            FileProbeTestListener probeListener = new FileProbeTestListener();
            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(probeListener);
            fileProbe.ExecuteProbe();

            Assert.AreEqual(TraceEventType.Critical, probeListener.TriggeredEvent, "Failed to detect missing file");

            File.Create(testFile).Dispose();
            fileProbe.ExecuteProbe();
            Assert.AreEqual(TraceEventType.Information, probeListener.TriggeredEvent, "Failed to detect file");

            System.Threading.Thread.Sleep(2000);
            fileProbe.ExecuteProbe();
            Assert.AreEqual(TraceEventType.Warning, probeListener.TriggeredEvent, "Failed to detect old file");

            File.WriteAllText(testFile, "");
            fileProbe.ExecuteProbe();
            Assert.AreEqual(TraceEventType.Information, probeListener.TriggeredEvent, "Failed to detect new file");

            File.WriteAllText(testFile, "Error");
            fileProbe.ExecuteProbe();
            Assert.AreEqual(TraceEventType.Warning, probeListener.TriggeredEvent, "Failed to detect large file");
        }
    }
}
