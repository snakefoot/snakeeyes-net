using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;

namespace SnakeEyes
{
    [ServiceContract]
    public interface ICrossDomainService
    {
        [OperationContract]
        [WebGet(UriTemplate = "ClientAccessPolicy.xml")]
        Message ProvidePolicyFile();
    }
}
