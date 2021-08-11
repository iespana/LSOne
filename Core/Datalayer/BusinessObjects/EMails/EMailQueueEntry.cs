using System;
using System.Collections.Generic;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.EMail;

namespace LSOne.DataLayer.BusinessObjects.EMails
{
	public class EMailQueueEntry : DataEntity
	{
        //public string DataAreaID { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public DateTime Sent { get; set; }
        public int Priority { get; set; }
        public int Attempts { get; set; }
        public string LastError { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool BodyIsHTML { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // Accessors
	    public int EMailID
	    {
	        get { return (int) ID; }
            set { ID = value; }
	    }

		/// <summary>
        /// Initializes a new instance of the <see cref="EMailQueueEntry" /> class.
		/// </summary>
        public EMailQueueEntry()
		{
            //DataAreaID = "";
        }

        #region Convert to / from worker class
        public EMailMessage ToEMailMessage(List<EMailQueueAttachment> attachments)
        {
            var message = new EMailMessage
                {
                    To = this.To,
                    CC = this.CC,
                    BCC = this.BCC,
                    Subject = this.Subject,
                    Body = this.Body,
                    BodyIsHTML = this.BodyIsHTML
                };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var attch = new EMailAttachment
                        {
                            Name = attachment.Name,
                            Attachment = new byte[attachment.Attachment.Length]
                        };
                    attachment.Attachment.CopyTo(attch.Attachment, 0);

                    message.Attachments.Add(attch);
                }
            }

            return message;
        }

        public EMailQueueEntry(EMailMessage message)
        {
            To = message.To;
            CC = message.CC;
            BCC = message.BCC;
            Subject = message.Subject;
            Body = message.Body;
            BodyIsHTML = message.BodyIsHTML;
        }
        #endregion
    }

    public enum EMailSortEnum
    {
        Priority,
        Sent,
        Created,
        Store,
        Attempts,
        Error,
        To,
        Subject
    }
}

