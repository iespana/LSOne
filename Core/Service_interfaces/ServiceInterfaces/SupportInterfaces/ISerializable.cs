using System.Xml.Linq;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ISerializable
    {
        XElement ToXML(IErrorLog errorLogger = null);
        void ToClass(XElement xLoyalty, IErrorLog errorLogger = null);
    }
}
