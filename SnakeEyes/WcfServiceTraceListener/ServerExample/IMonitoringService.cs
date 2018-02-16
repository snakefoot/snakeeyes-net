using System.ServiceModel;
using Monitoring.Models;

namespace Monitoring.WcfServices
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMonitoringService" in both code and config file together.
	[ServiceContract]
	public interface IMonitoringService
	{
		[OperationContract]
		void AddProbeResult(ProbeResultMessage ProbeResultMessage);
	}
}
