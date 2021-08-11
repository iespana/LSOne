using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.ValueProviders
{
    internal partial class CustomerLabelValueProvider : LabelValueProviderBase
    {
        private static List<string> macros;

        public override List<string> SupportedMacros
        {
            get
            {
                if (macros == null)
                {
                    var macroList = new List<string>
                        {
                            "${ID}",
                            "${SEARCHNAME}",
                            "${NAME}",
                            "${MOBILEPHONE}",
                            "${TELEPHONE}",
                            "${EMAIL}",
                            "${VATNUMBER}"
                        };

                    foreach (var prefix in new string[] {"", "SHIPPING.", "BILLING."})
                    {
                        macroList.Add("${" + prefix + "ADDRESS1}");
                        macroList.Add("${" + prefix + "ADDRESS2}");
                        macroList.Add("${" + prefix + "CITY}");
                        macroList.Add("${" + prefix + "COUNTY}");
                        macroList.Add("${" + prefix + "ZIP}");
                        macroList.Add("${" + prefix + "COUNTRY}");
                        macroList.Add("${" + prefix + "STATE}");
                    }

                    AddExtraMacros(macroList);
                    
                    macros = new List<string>();
                    macros.AddRange(SortMacros(macroList));
                }

                return macros;
            }
        }

        public override string ApplyMacros(IConnectionManager entry, int numLabels, string form, IDataEntity entity)
        {
            var customer = entity as Customer;

            var res = form;
            if (customer != null)
            {
                res = ApplyMacro(res, "${ID}", customer.ID.ToString());
                res = ApplyMacro(res, "${SEARCHNAME}", customer.SearchName);
                res = ApplyMacro(res, "${NAME}", customer.Text);
                res = ApplyMacro(res, "${MOBILEPHONE}", customer.MobilePhone);
                res = ApplyMacro(res, "${TELEPHONE}", customer.Telephone);
                res = ApplyMacro(res, "${EMAIL}", customer.Email);
                res = ApplyMacro(res, "${VATNUMBER}", customer.VatNum);
            }
            res = ApplyAddressMacros(entry, res, customer);
    
            return base.ApplyMacros(entry, numLabels, res, entity);
        }

        private string ApplyAddressMacros(IConnectionManager entry, string form, Customer customer)
        {
            if (customer.Addresses == null || customer.Addresses.Count == 0)
                customer.Addresses = Providers.CustomerAddressData.GetListForCustomer(entry, customer.ID);
            form = ApplyAddressMacros(entry, form, customer.DefaultShippingAddress, "SHIPPING.");
            form = ApplyAddressMacros(entry, form, customer.DefaultBillingAddress, "BILLING.");

            return form;
        }

        private string ApplyAddressMacros(IConnectionManager entry, string form, Address customerAddress, string prefix)
        {
            if (customerAddress == null)
                return form;

            form = ApplyMacro(form, "${" + prefix + "ADDRESS1}", customerAddress.Address1);
            form = ApplyMacro(form, "${" + prefix + "ADDRESS2}", customerAddress.Address2);
            form = ApplyMacro(form, "${" + prefix + "CITY}", customerAddress.City);
            form = ApplyMacro(form, "${" + prefix + "COUNTY}", customerAddress.County);
            form = ApplyMacro(form, "${" + prefix + "STATE}", customerAddress.State);
            form = ApplyMacro(form, "${" + prefix + "ZIP}", customerAddress.Zip);

            if (form.IndexOf("${" + prefix + "COUNTRY", StringComparison.CurrentCulture) > 0)
            {
                form = ApplyMacro(form, "${" + prefix + "COUNTRY}",
                                 entry.Cache.GetCountryName(customerAddress.Country));
            }

            return form;
        }
    }
}
