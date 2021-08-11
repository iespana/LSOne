using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.UserManagement.ListItems
{
    [Serializable]
    [RecordIdentifierConstruction("ID", typeof(Guid))]
    public class PermissionsAssignmentResult : DataEntity
    {
        private PermissionState hasPermission;
        private string permissionGroupName;
        private string permissionCode;


        public PermissionsAssignmentResult()
            : base(Guid.Empty, "")
        {
            this.hasPermission = PermissionState.ExclusiveDeny;
            this.permissionGroupName = "";
            this.permissionCode = "";
        }

        public PermissionsAssignmentResult(
            Guid permissionGuid,
            string description,
            PermissionState hasPermission,
            string permissionGroupName,
            string permissionCode)
        {
            this.ID = permissionGuid;
            this.Text = description;
            this.hasPermission = hasPermission;
            this.permissionGroupName = permissionGroupName;
            this.permissionCode = permissionCode;
        }

        public Guid PermissionGuid
        {
            get { return (Guid)ID; }
            internal set { ID = value; }
        }

        public string Description
        {
            get { return base.Text; }
            internal set { base.Text = value; }
        }

        public PermissionState HasPermission
        {
            get { return hasPermission; }
            set { hasPermission = value; }
        }

        public string PermissionGroupName
        {
            get { return permissionGroupName; }
            internal set { permissionGroupName = value; }
        }

        public string PermissionCode
        {
            get { return permissionCode; }
            internal set { permissionCode = value; }
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> currencyElements = element.Elements();
            foreach (XElement current in currencyElements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "Text":
                                Text = current.Value;
                                break;
                            case "ID":
                                ID = new Guid(current.Value);
                                break;
                            case "PermissionGroupName":
                                permissionGroupName = current.Value;
                                break;
                            case "PermissionCode":
                                permissionCode = current.Value;
                                break;
                            case "HasPermission":
                                hasPermission = (PermissionState)Convert.ToInt32(current.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("PermissionAssignmnet",
                    new XElement("ID", ((Guid)ID).ToString()),
                    new XElement("Text", Text),
                    new XElement("PermissionGroupName", permissionGroupName),
                    new XElement("PermissionCode", permissionCode),
                    new XElement("HasPermission", (int)hasPermission));
            return xml;
        }

        public override object Clone()
        {
            var o = new PermissionsAssignmentResult();
            Populate(o);
            return o;
        }

        protected void Populate(PermissionsAssignmentResult o)
        {
            o.ID = (RecordIdentifier)ID.Clone();
            o.Text = Text;
            o.PermissionGroupName = permissionGroupName;
            o.PermissionCode = permissionCode;
            o.HasPermission = hasPermission;
        }
    }
}
