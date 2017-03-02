using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SnakeEyes
{
    [DataContract]
    public class ProbeState
    {
        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public string Timestamp { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class ProbeInfo : ProbeState
    {
        [DataMember]
        public string Name { get; set; }

        public void UpdateState(ProbeState state)
        {
            EventId = state.EventId;
            Timestamp = state.Timestamp;
            Status = state.Status;
            Value = state.Value;
            Message = state.Message;
        }

        public override bool Equals(Object obj)
        {
            ProbeInfo otherProbe = obj as ProbeInfo;
            if (otherProbe == null)
                return false;

            return Name == otherProbe.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    };

    [DataContract]
    public class ProbeCollection
    {
        [DataMember]
        public List<ProbeInfo> Probes { get; set; }
    }

    [DataContract]
    public class ProbeHistory
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<ProbeState> History { get; set; }
    }

    [ServiceContract]
    public interface IProbeMonitorService
    {
        [OperationContract]
        ProbeCollection GetProbeCollection();

        [OperationContract]
        ProbeHistory GetProbeHistory(string name);
    }
}
