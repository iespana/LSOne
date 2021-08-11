using System;
using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    public class ReportEnumValue : DataEntity
    {
        public ReportEnumValue()
        {
            Label = "";
            EnumName = "";
            LanguageID = "en";
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid, Depth=4)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(ReportID,
                    new RecordIdentifier(LanguageID,
                    new RecordIdentifier(EnumName, EnumValue)));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ReportID { get; set; }

        [StringLength(10)]
        public string LanguageID { get; set; }

        [StringLength(40)]
        public string EnumName { get; set; }

        public int EnumValue { get; set; }

        [StringLength(40)]
        public string Label { get; set; }

        [StringLength(40)]
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
    }
}
