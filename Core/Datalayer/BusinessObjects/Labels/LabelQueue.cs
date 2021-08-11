using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Labels
{
	public class LabelQueue : DataEntity
	{
        public string DataAreaID { get; set; }
        public int NumberOfLabels { get; set; }
        public RecordIdentifier LabelTemplateID { get; set; }
        public RecordIdentifier EntityID { get; set; }
        public string Batch { get; set; }
        public DateTime Printed { get; set; }
        public string Message { get; set; }

        // Accessors
	    public int LabelQueueID
	    {
	        get { return (int) ID; }
            set { ID = value; }
	    }

	    public string Printer
	    {
	        get { return Text; }
            set { Text = value; }
	    }

		/// <summary>
        /// Initializes a new instance of the <see cref="LabelQueue" /> class.
		/// </summary>
        public LabelQueue()
		{
            DataAreaID = "";
		    Batch = "";
		}
	}
}

