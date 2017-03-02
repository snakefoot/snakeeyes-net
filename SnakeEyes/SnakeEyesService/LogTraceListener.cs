using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SnakeEyes
{
    // Forwards global Trace-interface to a log file with cleanup ability
    public class LogTraceListener : TextWriterTraceListener
    {
        string _logFileName; 
        DateTime _lastLogCheck = DateTime.MinValue;
        TimeSpan _freqLogCheck = TimeSpan.FromHours(1);

        public LogTraceListener(string fileName)
            :base(new StreamWriter(fileName, true))
        {
            _logFileName = fileName;
        }

        public override void WriteLine(string message)
        {
            message = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss' '") + message;
            base.WriteLine(message);
        }

        public override void Flush()
        {
            base.Flush();
            if (DateTime.Now - _lastLogCheck > _freqLogCheck)
            {
                StreamWriter streamWriter = Writer as StreamWriter;
                if (streamWriter.BaseStream.Length > 50 * 1024 * 1024)
                {
                    Writer.Close();
                    string backupFileName = System.IO.Path.GetDirectoryName(_logFileName) + "\\";
                    backupFileName += System.IO.Path.GetFileNameWithoutExtension(_logFileName);
                    backupFileName += DateTime.Now.ToString("'_'yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
                    backupFileName += System.IO.Path.GetExtension(_logFileName);
                    FileInfo fInfo = new FileInfo(_logFileName);
                    fInfo.MoveTo(backupFileName);
                    Writer = new StreamWriter(_logFileName, true);
                }
            }
        }
    }
}
