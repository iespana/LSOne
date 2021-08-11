#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;


namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class SuspensionTypeAdditionalInfo : DataEntity
    {
        [DataContract(Name = "SuspensionInfoTypeEnum")]
        public enum InfoTypeEnum
        {
            [EnumMember]
            Text = 0,
            [EnumMember]
            Customer = 1,
            [EnumMember]
            Name = 2,
            [EnumMember]
            Address = 3,
            [EnumMember]
            Infocode = 4,
            [EnumMember]
            Date = 5,
            [EnumMember]
            Other = 6
        }

        public SuspensionTypeAdditionalInfo()
            :base()
        {
            AdditionalInfoID = RecordIdentifier.Empty;
            SuspensionTypeID = "";
        }

        [RecordIdentifierValidation(40,Depth=2)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(AdditionalInfoID,SuspensionTypeID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        [RecordIdentifierValidation(40)]
        public RecordIdentifier AdditionalInfoID{get; set;}

        [RecordIdentifierValidation(40)]
        public RecordIdentifier SuspensionTypeID{get; set;}


        [StringLength(60)]
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

        public int Order { get; set; }
        public InfoTypeEnum Infotype { get; set; }

        public string InfotypeText
        {
            get
            {
                switch (Infotype)
                {
                    case InfoTypeEnum.Text:
                        return Properties.Resources.Text;

                    case InfoTypeEnum.Customer:
                        return Properties.Resources.Customer;

                    case InfoTypeEnum.Name:
                        return Properties.Resources.Name;
                    
                    case InfoTypeEnum.Address:
                        return Properties.Resources.Address;

                    case InfoTypeEnum.Infocode:
                        return Properties.Resources.Infocode;

                    case InfoTypeEnum.Date:
                        return Properties.Resources.Date;

                    case InfoTypeEnum.Other:
                        return Properties.Resources.Other;

                    default:
                        return "";
                }
            }

            
        }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier InfoTypeSelectionID { get; set; }

        public string InfoTypeSelectionDescription { get; internal set; }

        public bool Required { get; set; }
        





        



    }
}
