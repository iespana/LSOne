using System;

namespace LSOne.DataLayer.BusinessObjects.Attributes
{
    /// <summary>
	/// Specifies a conditional attribute for mandatory customer fields used in the site service profile
    /// Properties marked with this attribute will cause the customer page of the site service profile to automatically add a checkbox field for the property
    /// This attribute should only be used for mandatory customer fields - ONE-6574
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomerMandatoryProperty : Attribute
    {
        private CustomerMandatoryPropertyTypeEnum type;

        public CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum type)
        {
            this.type = type;
        }

        public string DisplayName
        {
            get
            {
                switch(type)
                {
                    case CustomerMandatoryPropertyTypeEnum.Name:
                        return Properties.Resources.Name;
                    case CustomerMandatoryPropertyTypeEnum.SearchAlias:
                        return Properties.Resources.SearchAlias;
                    case CustomerMandatoryPropertyTypeEnum.Address:
                        return Properties.Resources.Address;
                    case CustomerMandatoryPropertyTypeEnum.Phone:
                        return Properties.Resources.Phone;
                    case CustomerMandatoryPropertyTypeEnum.EmailAddress:
                        return Properties.Resources.Email;
                    case CustomerMandatoryPropertyTypeEnum.ReceiptEmailAddress:
                        return Properties.Resources.ReceiptEmail;
                    case CustomerMandatoryPropertyTypeEnum.Gender:
                        return Properties.Resources.Gender;
                    case CustomerMandatoryPropertyTypeEnum.DateOfBirth:
                        return Properties.Resources.DateOfBirth;
                    default:
                        throw new Exception("Invalid CustomerMandatoryPropertyTypeEnum");
                }
            }
        }
    }

    public enum CustomerMandatoryPropertyTypeEnum
    {
        Name,
        SearchAlias,
        Address,
        Phone,
        EmailAddress,
        ReceiptEmailAddress,
        Gender,
        DateOfBirth
    }
}
