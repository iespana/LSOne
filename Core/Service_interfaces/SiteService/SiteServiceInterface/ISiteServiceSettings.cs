using System;
using System.Collections.Generic;
using System.Text;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public interface ISiteServiceSettings
    {
        void WriteKey(string key,string value);
        void WriteComment(string comment);
        void WriteEmptyLine();
    }
}
