using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;

namespace SnakeEyes
{
    // http://ukadcdiagnostics.codeplex.com/wikipage?title=LoggingPrimer
    // http://stackoverflow.com/questions/576185/Logging-best-practices
    class Program
    {
        static bool IsAdministrator
        {
            get
            {
                System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal wp = new System.Security.Principal.WindowsPrincipal(wi);
                return wp.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (Array.Exists(args, delegate(string arg) { return arg == "/install" || arg == "/INSTALL"; }))
            {
                try
                {
                    System.Configuration.Install.TransactedInstaller ti = new System.Configuration.Install.TransactedInstaller();
                    ti.Installers.Add(new ProjectInstaller());
                    ti.Context = new System.Configuration.Install.InstallContext("", null);
                    ti.Context.Parameters["assemblypath"] = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    ti.Install(new System.Collections.Hashtable());
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!IsAdministrator)
                        Console.WriteLine("Install requires administrator rights");
                }
                return;
            }

            if (Array.Exists(args, delegate(string arg) { return arg == "/uninstall" || arg == "/UNINSTALL"; }))
            {
                try
                {
                    System.Configuration.Install.TransactedInstaller ti = new System.Configuration.Install.TransactedInstaller();
                    ti.Installers.Add(new ProjectInstaller());
                    ti.Context = new System.Configuration.Install.InstallContext("", null);
                    ti.Context.Parameters["assemblypath"] = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    ti.Uninstall(null);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!IsAdministrator)
                        Console.WriteLine("Unnstall requires administrator rights");
                }
                return;
            }

            if (Array.Exists(args, delegate(string arg) { return arg == "/service" || arg == "/SERVICE"; }))
            {
                ServiceBase[] ServicesToRun;

                // More than one user Service may run within the same process. To add
                // another service to this process, change the following line to
                // create a second service object. For example,
                //
                //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
                //
                ServicesToRun = new ServiceBase[] { new SystemService(ProgramStart) };

                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                bool serviceRunning = true;
                try
                {
                    ProgramStart(ref serviceRunning);
                }
                catch (Exception ex)
                {
                    if (Trace.Listeners.Count > 1)
                    {
                        Trace.WriteLine("Program Failed: " + ex.Message);
                        if (ex.InnerException != null)
                            Trace.WriteLine(ex.InnerException.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                    else
                    {
                        Console.WriteLine("Program Failed: " + ex.Message);
                        if (ex.InnerException != null)
                            Console.WriteLine(ex.InnerException.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
        }

        static void ProgramStart(ref bool serviceRunning)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new LogTraceListener(path + "\\SnakeEyes.txt"));
            Trace.AutoFlush = true;
            Trace.WriteLine("Starting SnakeEyes...");

            EventLog eventLog = null;
            try
            {
                if (EventLog.SourceExists("SnakeEyes"))
                {
                    eventLog = new EventLog();
                    eventLog.Source = "SnakeEyes";
                }
                else
                    Trace.WriteLine("Warning not installed properly. Cannot write to EventLog");
            }
            catch(System.Security.SecurityException ex)
            {
                Trace.WriteLine("Warning not installed properly. Cannot write to EventLog : %1", ex.Message);
            }

            var builder = new Autofac.Builder.ContainerBuilder();
            builder.RegisterCollection<IProbe>().As<IEnumerable<IProbe>>();
            builder.Register<PerfMonProbe>().As<IProbe>().ExternallyOwned().FactoryScoped().Named(typeof(PerfMonProbe).FullName).MemberOf<IEnumerable<IProbe>>();
            builder.Register<EventLogProbe>().As<IProbe>().ExternallyOwned().FactoryScoped().Named(typeof(EventLogProbe).FullName).MemberOf<IEnumerable<IProbe>>();
            var container = builder.Build();
            Microsoft.Practices.ServiceLocation.IServiceLocator locator = new AutofacServiceLocator(container);

            Configuration exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection diagnosticsSection = exeConfiguration.GetSection("system.diagnostics");

            ConfigurationElementCollection tracesources = diagnosticsSection.ElementInformation.Properties["sources"].Value as ConfigurationElementCollection;

            List<KeyValuePair<string, IProbeMonitor>> probeMonitors = new List<KeyValuePair<string, IProbeMonitor>>();
            foreach (TraceListener traceListener in Trace.Listeners)
            {
                IProbeMonitor probeMonitor = traceListener as IProbeMonitor;
                if (probeMonitor != null)
                {
                    Trace.WriteLine("Configures ProbeMonitor: " + traceListener.Name);
                    probeMonitors.Add( new KeyValuePair<string,IProbeMonitor>(traceListener.Name,probeMonitor) );
                }
            }

            ProbeList probes = new ProbeList();
            ForwardTraceListener forwardTrace = new ForwardTraceListener();
            foreach (ConfigurationElement tracesource in tracesources)
            {
                string name = tracesource.ElementInformation.Properties["name"].Value.ToString();
                Trace.WriteLine("Configures TraceSource: " + name);

                if (name.IndexOf('.') != -1)
                {
                    string typeName = name.Substring(name.IndexOf('.') + 1);

                    IProbe probe = locator.GetInstance<IProbe>("SnakeEyes." + typeName);
                    if (probe == null)
                    {
                        Trace.WriteLine("Unknown Probe Type: " + typeName);
                        if (eventLog != null)
                            eventLog.WriteEntry("Unknown Probe Type: " + typeName, EventLogEntryType.Error);
                        continue;
                    }
                    TraceSource probeSource = probe.ConfigureProbe(name);
                    probeSource.Listeners.Add(forwardTrace);

                    foreach (KeyValuePair<string, IProbeMonitor> probeMonitor in probeMonitors)
                        probeMonitor.Value.RegisterProbe(probeSource);

                    probes.AddProbe(DateTime.MinValue, probe);
                }
            }

            if (probes.Count == 0)
            {
                Trace.WriteLine("SnakeEyes have no probes to poll");
                if (eventLog != null)
                    eventLog.WriteEntry("SnakeEyes have no probes to poll", EventLogEntryType.Error);
                return;
            }

            Trace.WriteLine("SnakeEyes starts monitoring...");

            foreach (KeyValuePair<string, IProbeMonitor> probeMonitor in probeMonitors)
                probeMonitor.Value.StartMonitor(probeMonitor.Key);

            using (probes)
            {
                // Implement an email trace listener:
                //  - http://www.barebonescoder.com/2010/04/writing-an-email-custom-trace-listener-for-a-wcf-service/
                while (serviceRunning)
                {
                    if (probes.GetNextPollTime() >= DateTime.UtcNow)
                    {
                        System.Threading.Thread.Sleep(10);
                        continue;
                    }

                    IProbe probe = probes.GetNextPollProbe();
                    try
                    {
                        TimeSpan nextPollTime = probe.ExecuteProbe();
                        probes.PollExecuted(nextPollTime, probe);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("Probe Failure: " + ex.Message);
                        if (ex.InnerException != null)
                            System.Diagnostics.Trace.TraceError("Probe Failure: " + ex.InnerException.Message);
                        System.Diagnostics.Trace.WriteLine("Probe Failure: " + ex.StackTrace);

                        probes.PollExecuted(TimeSpan.FromSeconds(5), probe);

                        if (eventLog != null)
                            eventLog.WriteEntry("Probe Failure: " + ex.Message, EventLogEntryType.Error);
                    }
                }
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("UnhandledException");
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Trace.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Trace.WriteLine(ex.InnerException.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Trace.WriteLine("ProcessExit");
        }
    }
}
