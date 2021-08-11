using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    public class SearchFlagEntity : DataEntity
    {
        public string ColumnName { get; set; }
        public FlagCheckValue Value { get; set; }
        public bool EnableThreeState { get; set; }
        public bool ControlsText { get; set; }

        public bool BitColumn { get; set; }
        public string UncheckedValue { get; set; }
        
    }

    public enum FlagCheckValue
    {
        //
        // Summary:
        //     The control is unchecked.
        Unchecked = 0,
        //
        // Summary:
        //     The control is checked.
        Checked = 1,
        //
        // Summary:
        //     The control is indeterminate. An indeterminate control generally has a shaded
        //     appearance.
        Indeterminate = 2
    }

}
