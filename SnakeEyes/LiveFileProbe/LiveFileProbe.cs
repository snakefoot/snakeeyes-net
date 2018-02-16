using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeEyes
{
	[XmlRoot("TraceEvent")]
	public class LiveFileTraceEvent
	{
		public string Source { get; set; }
		public string MachineName { get; set; }
		public string Timestamp { get; set; }
		public int EventId { get; set; }
		public TraceEventType EventType { get; set; }
		public string FileName { get; set; }
		public long Value { get; set; }
		public long MaxValue { get; set; }
		public string Message { get; set; }
	};


	public class LiveFileProbe : IProbe
	{
		TraceSource _traceSource;

		public string FileName { get; set; }
		public TimeSpan ProbeFrequency { get; set; }
		public string RequiredStatus { get; set; }
		public TimeSpan? MaxAge { get; set; }
		public int EventId { get; set; }
		public TraceEventType EventType { get; set; }
		public string DefaultStatus { get; set; }
		public int? DefaultAge { get; set; }

		public LiveFileProbe()
		{
			ProbeFrequency = TimeSpan.FromSeconds(5);
			EventType = TraceEventType.Warning;
		}

		public void Dispose()
		{
			// NOOP
		}

		public TraceSource ConfigureProbe(string configName)
		{
			_traceSource = new TraceSource(configName);
			NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection(configName);
			if (config != null)
			{
				FileName = config["FileName"];
				if (config["RequiredStatus"] != null)
					RequiredStatus = config["RequiredStatus"];
				if (config["MaxAge"] != null)
					MaxAge = TimeSpan.FromSeconds(Int32.Parse(config["MaxAge"]));
				if (config["DefaultStatus"] != null)
					DefaultStatus = config["DefaultStatus"];
				if (config["DefaultAge"] != null)
					DefaultAge = int.Parse(config["DefaultAge"]);
				if (config["ProbeFrequency"] != null)
					ProbeFrequency = TimeSpan.FromSeconds(Int32.Parse(config["ProbeFrequency"]));
				if (config["EventId"] != null)
					EventId = Int32.Parse(config["EventId"]);
				if (config["EventType"] != null)
					EventType = (TraceEventType)Enum.Parse(typeof(TraceEventType), config["EventType"]);
			}

			return _traceSource;
		}

		public TimeSpan ExecuteProbe()
		{
			try
			{
				if (!File.Exists(FileName))
				{
					if (!String.IsNullOrWhiteSpace(DefaultStatus) || DefaultAge.HasValue)
					{
						TraceEvent(TraceEventType.Information, DefaultStatus ?? "", DefaultAge ?? 0, "Life File not found");
						return ProbeFrequency;
					}
					else
					{
						throw new FileNotFoundException(_traceSource.Name + " missing file", FileName);
					}
				}

				string status = "";
				int age = 0;
				AnalyzeLiveFile(ref status, ref age);

				TraceEvent(TraceEventType.Information, status, age, "Ok");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.Message);
				TraceEvent(TraceEventType.Critical, "", 0, ex.Message); // Report initial exception
			}
			return ProbeFrequency;
		}

		void AnalyzeLiveFile(ref string status, ref int age)
		{
			var fileInfo = new FileInfo(FileName);
			long fileSize = fileInfo.Length;

			if (fileSize > 1024)
			{
				throw new Exception("Maximal file size for a life file is 1024 bytes");
			}

			string xmlText = File.ReadAllText(FileName);
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(xmlText);
			status = xml.SelectSingleNode("//Live/Status").InnerText;
			age = (int)Math.Max(0.0, (DateTime.UtcNow - DateTime.Parse(xml.SelectSingleNode("//Live/NewTimeStamp").InnerText)).TotalSeconds);
		}

		void TraceEvent(TraceEventType eventType, string status, int age, string message)
		{
			if (!String.IsNullOrWhiteSpace(RequiredStatus) && status != RequiredStatus)
			{
				message = String.Format("Life file status ({0}) does not match required status ({1})", status, RequiredStatus);
				eventType = EventType;
			}
			else if (age > MaxAge.Value.TotalSeconds)
			{
				message = "Timestamp in life file is too old";
				eventType = EventType;
			}

			LiveFileTraceEvent traceEvent = new LiveFileTraceEvent
			{
				Source = _traceSource.Name,
				MachineName = Environment.MachineName,
				Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
				Value = age,
				MaxValue = (int)MaxAge.Value.TotalSeconds,
				EventId = EventId,
				EventType = eventType,
				Message = message
			};

			string formatMessage = "";

			using (StringWriter writer = new StringWriter())
			using (XmlTextWriter xmlwriter = new XmlTextWriter(writer))
			{
				xmlwriter.Formatting = Formatting.Indented;
				XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
				xmlnsEmpty.Add("", "");
				XmlSerializer serializer = new XmlSerializer(typeof(LiveFileTraceEvent));
				serializer.Serialize(xmlwriter, traceEvent, xmlnsEmpty);
				formatMessage = writer.ToString();
			}

			try
			{
				_traceSource.TraceEvent(eventType, EventId, formatMessage);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(_traceSource.Name + " " + ex.Message);
				if (ex.InnerException != null)
					System.Diagnostics.Trace.TraceError(_traceSource.Name + " " + ex.InnerException.Message);
				System.Diagnostics.Trace.WriteLine(_traceSource.Name + " failed to trace event. Check TraceListeners:");
				foreach (TraceListener listener in _traceSource.Listeners)
					System.Diagnostics.Trace.WriteLine(_traceSource.Name + " has listener: " + listener.Name + " (" + listener.ToString() + ")");
				System.Diagnostics.Trace.WriteLine(_traceSource.Name + " " + ex.ToString());
			}
		}
	}
}
