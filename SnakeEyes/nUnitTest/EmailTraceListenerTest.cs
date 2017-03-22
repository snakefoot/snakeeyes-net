using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Configuration;
using NUnit.Framework;

namespace nUnitTest
{
    [TestFixture]
    public class EmailTraceListenerTest
    {
        void ResetPickupDirectory(string directory)
        {
            if (System.IO.Directory.Exists(directory))
            {
                for (int i = 1; i <= 10; ++i)
                {
                    try
                    {
                        System.IO.Directory.Delete(directory, true);
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        break;
                    }
                    catch
                    {
                        if (i == 10)
                            throw;
                        System.Threading.Thread.Sleep(100);
                    }
                }
                Assert.IsFalse(System.IO.Directory.Exists(directory));
            }
            System.IO.Directory.CreateDirectory(directory);
            Assert.IsTrue(System.IO.Directory.Exists(directory));
            Assert.IsTrue(System.IO.Directory.GetFiles(directory).Length == 0);
        }

        [Test]
        public void Test_BundleSource()
        {
            // Setup ConfigurationManager
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Clear();
            config.SectionGroups.Clear();
            NetSectionGroup netSectionGroup = config.SectionGroups.Get("system.net") as NetSectionGroup;
            MailSettingsSectionGroup smtpSectionGroup = netSectionGroup.SectionGroups.Get("mailSettings") as MailSettingsSectionGroup;
            SmtpSection smtpSection = smtpSectionGroup.Sections.Get("smtp") as SmtpSection;
            smtpSection.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory;
            smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Test";
            ResetPickupDirectory(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation);
            config.Save();
            ConfigurationManager.RefreshSection("system.net");
            ConfigurationManager.RefreshSection("mailSettings");

            SnakeEyes.EmailTraceListener listener = new SnakeEyes.EmailTraceListener();
            listener.Attributes.Add("fromAddress", "example@example.com");
            listener.Attributes.Add("toAddress", "example@example.com");
            listener.Attributes.Add("throttleSeconds", "1");

            var source = new System.Diagnostics.TraceSource("TestSource", SourceLevels.Error);
            source.Listeners.Add(listener);

            {
                DateTime startTime = DateTime.UtcNow;
                for (int i = 0; i < 5; ++i)
                    source.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "Damn");
                System.Threading.Thread.Sleep(1000);
                Assert.GreaterOrEqual(DateTime.UtcNow.Subtract(startTime).TotalSeconds, 5);    // See throttle time works
                Assert.AreEqual(5, System.IO.Directory.GetFiles(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation).Length);
            }

            source.Listeners.Remove(listener);

            listener = new SnakeEyes.EmailTraceListener();
            listener.Attributes.Add("fromAddress", "example@example.com");
            listener.Attributes.Add("toAddress", "example@example.com");
            listener.Attributes.Add("throttleSeconds", "1");
            listener.Attributes.Add("formatBundleSource", "{{/TraceEvent/EventLogSource}}");
            listener.Attributes.Add("formatBody", "Message = {{/TraceEvent/EventLogSource}}");
            source.Listeners.Add(listener);
            ResetPickupDirectory(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation);
            {
                DateTime startTime = DateTime.UtcNow;
                for (int i = 0; i < 10; ++i)
                    source.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "<TraceEvent><EventLogSource>Damn</EventLogSource></TraceEvent>");
                System.Threading.Thread.Sleep(1000);
                Assert.Less(DateTime.UtcNow.Subtract(startTime).TotalSeconds, 2);    // See bundle time works
                Assert.AreEqual(2, System.IO.Directory.GetFiles(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation).Length);
            }
            source.Listeners.Remove(listener);

            // See that it can also bundle when receiving messages from different sources
            listener = new SnakeEyes.EmailTraceListener();
            listener.Attributes.Add("fromAddress", "example@example.com");
            listener.Attributes.Add("toAddress", "example@example.com");
            listener.Attributes.Add("throttleSeconds", "1");
            listener.Attributes.Add("formatBundleSource", "{{/TraceEvent/EventLogSource}}");
            listener.Attributes.Add("formatBody", "Message = {{/TraceEvent/EventLogSource}}");
            source.Listeners.Add(listener);
            ResetPickupDirectory(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation);
            {
                DateTime startTime = DateTime.UtcNow;
                for (int i = 0; i < 5; ++i)
                {
                    source.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "<TraceEvent><EventLogSource>Damn</EventLogSource></TraceEvent>");
                    source.TraceEvent(System.Diagnostics.TraceEventType.Error, 0, "<TraceEvent><EventLogSource>Again</EventLogSource></TraceEvent>");
                }
                System.Threading.Thread.Sleep(1000);
                Assert.Less(DateTime.UtcNow.Subtract(startTime).TotalSeconds, 3);    // See bundle time works
                Assert.AreEqual(3, System.IO.Directory.GetFiles(smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation).Length);
            }
            source.Listeners.Remove(listener);
        }
    }
}
