using System;

namespace LSOne.DataLayer.BusinessObjects
{
    public class HelpSettings
    {
        private const string SearchIdentifier = "#cshid=";

        public string OnlineHelpUrl { get; set; }
        public string ViewKey { get; set; }

        public HelpSettings(string onlineHelpUrl, string viewKey)
        {
            OnlineHelpUrl = onlineHelpUrl;
            ViewKey = viewKey;
        }

        public HelpSettings(string viewKey)
        {
            //OnlineHelpUrl = "http://hq-lsoneweb-d01/LSOneHelp/"; //Test url for development
            OnlineHelpUrl = "http://help.ls-one.com/"; // Live url
            //OnlineHelpUrl = "http://localhost/LSOnlineHelp/"; //Local url for development
            ViewKey = viewKey;
        }

        public bool IsUrlValid()
        {
            return !string.IsNullOrWhiteSpace(OnlineHelpUrl) && Uri.IsWellFormedUriString(OnlineHelpUrl, UriKind.Absolute);
        }

        public override string ToString()
        {
            return OnlineHelpUrl + (string.IsNullOrWhiteSpace(ViewKey) ? "" : SearchIdentifier + ViewKey);
        }
    }
}
