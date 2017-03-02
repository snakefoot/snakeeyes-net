using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

// http://msdn.microsoft.com/en-us/library/cc681221.aspx
namespace SnakeEyes
{
    [ServiceContract]
    public interface IFileHostService
    {
        [OperationContract, WebGet(UriTemplate = "/{filename}")]
        Stream Files(string filename);
    }
}
