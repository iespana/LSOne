using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    public class ReportContext : DataEntity
    {
        public ReportContext()
            : base()
        {
            ReportID = RecordIdentifier.Empty;
            Active = true;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ReportID { get; set; }

        [StringLength(20)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public bool Active { get; set; }

        public string Description 
        {
            get
            {
                try
                {
                    //ResourceManager rm = new ResourceManager("LSOne.ViewPlugins.ReportViewer.Properties.Resources", Assembly.GetExecutingAssembly());
                    ResourceManager rm = new global::System.Resources.ResourceManager("LSOne.DataLayer.BusinessObjects.Properties.Resources", typeof(Resources).Assembly);
                    return rm.GetString("Context" + Text);
                }
                catch (Exception)
                {
                    return Resources.NotTranslated;
                }
            }
        }
    }
}
