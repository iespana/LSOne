using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Localization;

namespace LSOne.Services.Interfaces.SupportClasses.Employee
{
    [Serializable]
    public class Employee : ICloneable
    {
        private string name;
        private string nameOnReceipt;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier Login { get; set; }

        public string Name 
        {
            get { return name == "" ? (string)ID : name; }
            set { name = value; }
        }

        public string NameOnReceipt 
        {
            get { return nameOnReceipt != "" ? nameOnReceipt : Name; }
            set { nameOnReceipt = value; }
        }

        public bool Exists
        {
            get { return ID.StringValue != ""; }
        }        

        public Employee()
        {
            Clear();
        }

        public Employee(Employee employee)
        {
            Populate(employee);
        }

        public Employee(POSUser posUser, INameFormatter nameFormatter)
        {
            Clear();

            ID = posUser.ID;
            Login = posUser.Login;
            Name = nameFormatter.Format(posUser.Name);
            NameOnReceipt = posUser.NameOnReceipt;
        }

        public void Clear()
        {
            ID = RecordIdentifier.Empty;
            Name = "";
            NameOnReceipt = "";
            Login = RecordIdentifier.Empty;
        }

        protected void Populate(Employee employee)
        {
            employee.ID = ID;
            employee.Name = name;
            employee.NameOnReceipt = nameOnReceipt;
            employee.Login = Login;
        }

        public virtual object Clone()
        {
            Employee op = new Employee();
            Populate(op);
            return op;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xOperator = new XElement("Employee",
                    new XElement("ID", ID),
                    new XElement("Login", Login),
                    new XElement("Name", name),
                    new XElement("NameOnReceipt", nameOnReceipt)                    
                );

                return xOperator;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "Operator.ToXML", ex);

                throw;
            }
        }

        public void ToClass(XElement xOperator, IErrorLog errorLogger = null)
        {
            try
            {                
                if (xOperator.HasElements)
                {
                    IEnumerable<XElement> operatorElements = xOperator.Elements("Employee");
                    foreach (XElement operatorElem in operatorElements)
                    {
                        if(operatorElem.HasElements)
                        {
                            IEnumerable<XElement> xElements = operatorElem.Elements();
                            foreach(XElement xElement in xElements)
                            {
                                if (!xElement.IsEmpty)
                                {
                                    try
                                    {
                                        switch (xElement.Name.ToString())
                                        {
                                            case "ID":
                                                ID = xElement.Value;
                                                break;
                                            case "Login":
                                                Login = xElement.Value;
                                                break;
                                            case "Name":
                                                name = xElement.Value;
                                                break;
                                            case "NameOnReceipt":
                                                nameOnReceipt = xElement.Value;
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLogger?.LogMessage(LogMessageType.Error, "Employee:" + xElement.Name, ex);
                                    }
                                }
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "Employee.ToClass", ex);

                throw;
            }
        }
    }
}
