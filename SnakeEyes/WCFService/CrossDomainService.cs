using System.IO;
using System.Xml;
using System.ServiceModel.Channels;

namespace SnakeEyes
{
    // http://www.dotnetcurry.com/ShowArticle.aspx?ID=208
    public class CrossDomainService : ICrossDomainService
    {
        public System.ServiceModel.Channels.Message ProvidePolicyFile()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            path = Path.Combine(path, @"ClientAccessPolicy.xml");

            FileStream filestream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlReader reader = XmlReader.Create(filestream);
            Message result = Message.CreateMessage(MessageVersion.None, "", reader);
            return result;
        }
    }
}
