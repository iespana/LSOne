using LSOne.Utilities.DataTypes;
using LSOne.Utilities.EMail;

namespace LSOne.DataLayer.BusinessObjects.EMails
{
	public class EMailSetting : DataEntity
	{
        public string DataAreaID { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpEMailAddress { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpDisplayName { get; set; }
        public bool UseSSL { get; set; }
        public bool TextOnly { get; set; }
        public string Signature { get; set; }

        // Accessors
	    public int EMailSettingID
	    {
	        get { return (int) ID; }
            set { ID = value; }
	    }

		/// <summary>
        /// Initializes a new instance of the <see cref="EMailSetting" /> class.
		/// </summary>
        public EMailSetting()
		{
            DataAreaID = "";
		    StoreID = RecordIdentifier.Empty;
		    SmtpServer = "";
		    SmtpPort = 0;
		    SmtpEMailAddress = "";
		    SmtpPassword = "";
		    SmtpDisplayName = "";
            UseSSL = false;
		    TextOnly = false;
		    Signature = "";
		}

        #region Convert to worker class
        public SmtpServerData ToSmtpServerData()
        {
            return new SmtpServerData
                {
                    SmtpServer = this.SmtpServer,
                    SmtpPort = this.SmtpPort,
                    SmtpEMailAddress = this.SmtpEMailAddress,
                    SmtpPassword = this.SmtpPassword,
                    SmtpDisplayName = this.SmtpDisplayName,
                    UseSSL = this.UseSSL,
                    TextOnly = this.TextOnly,
                    Signature = this.Signature
                };
        }
        #endregion
    }
}

