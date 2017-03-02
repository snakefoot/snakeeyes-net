using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace SnakeEyes
{
    public partial class SystemService : ServiceBase
    {
        public delegate void ProgramStart(ref bool serviceRunning);
        ProgramStart _programStart;
        System.Threading.Thread _programThread;
        bool _serviceRunning;

        public SystemService(ProgramStart programStart)
        {
            InitializeComponent();
            _programStart = programStart;
        }

        protected override void OnStart(string[] args)
        {
            _serviceRunning = true;
            _programThread = new System.Threading.Thread(ThreadStart);
            _programThread.Start();
        }

        protected override void OnStop()
        {
            _serviceRunning = false;
            while(_programThread.IsAlive)
                System.Threading.Thread.Sleep(10);
        }

        void ThreadStart()
        {
            try
            {
                _programStart(ref _serviceRunning);
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
}
