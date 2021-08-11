using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    /// <summary>
    /// Class that has the relation mapping to type of dining tables. In LS One this is
    /// just hospitality type but on other systems this might have different meaning
    /// </summary>
    public class SectionRelationType : DataEntity
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}
