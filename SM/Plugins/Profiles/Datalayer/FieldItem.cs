using LSOne.DataLayer.BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.Profiles.Datalayer
{
    public class FieldItem
    {
        public Field Field { get; set; }
        public string FieldName { get; set; }
        public FieldType FieldType { get; set; }
        public string FieldTypeName { get; set; }
    }
}
