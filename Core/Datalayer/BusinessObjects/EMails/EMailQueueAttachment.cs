using System.IO;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.EMails
{
	public class EMailQueueAttachment : DataEntity
	{
        public string DataAreaID { get; set; }
        public RecordIdentifier EMailID { get; set; }
        public string Name { get; set; }
        public byte[] Attachment { get; set; }

        // Accessors
	    public int EMailAttachmentID
	    {
	        get { return (int) ID; }
            set { ID = value; }
	    }

		/// <summary>
        /// Initializes a new instance of the <see cref="EMailQueueEntry" /> class.
		/// </summary>
        public EMailQueueAttachment()
		{
            DataAreaID = "";
		}

        /// <summary>
        /// Initializes a new attachment from a file
        /// </summary>
        /// <param name="fileName"></param>
        public EMailQueueAttachment(string fileName)
            : this()
        {
            SetAttachmentFromFile(fileName);
        }

        public void SetAttachmentFromFile(string fileName)
        {
            Name = Path.GetFileName(fileName);

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int index = 0;
                const int buffer = 4096;
                Attachment = new byte[fs.Length];
                int left = (int)fs.Length;
                using (var br = new BinaryReader(fs))
                {
                    int read;
                    do
                    {
                        read = br.Read(Attachment, index, left >= buffer ? buffer : left);
                        index += read;
                        left -= read;
                    } while (read == buffer);
                }
            }
        }
	}
}

