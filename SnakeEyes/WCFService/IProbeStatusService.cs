using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SnakeEyes
{
    [ServiceContract]
    public interface IProbeStatusService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        Stream CurrentStatus();
    }
}
