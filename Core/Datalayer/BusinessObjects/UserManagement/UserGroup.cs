using System;
using System.Xml.Linq;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    [Serializable()]
    public class UserGroup : DataEntity
    {
        private string name;
        private bool isAdminGroup;
        private bool locked;

        public UserGroup()
        {
            ID = Guid.Empty;
            name = "";
            isAdminGroup = false;
            locked = false;
        }

        public UserGroup(Guid id, string name, bool isAdminGroup, bool locked)
        {
            ID = id;
            this.name = name;
            this.isAdminGroup = isAdminGroup;
            this.locked = locked;
        }

        public UserGroup(Guid id, string name)
        {
            ID = id;
            this.name = name;
            isAdminGroup = false;
            locked = false;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string Text
        {
            get
            {
                return name;
            }
        }

        public bool IsAdminGroup
        {
            get { return isAdminGroup; }
            internal set { isAdminGroup = value; }
        }

        public bool Locked
        {
            get { return locked; }
            internal set { locked = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "Text":
                                name = current.Value;
                                break;
                            case "GroupGuid" :
                                ID = new Guid(current.Value);
                                isAdminGroup = current.Value == "ff21a0e8-40e0-4bf6-8670-eb159de2b48c" || current.Value == "2D4CA208-31A3-45B6-8682-B665E6FA0FC3";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("UserGroup",
                    new XElement("GroupGuid", ((Guid)ID).ToString()),
                    new XElement("Text", name));
            return xml;
        }

    }
}
