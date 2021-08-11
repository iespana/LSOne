using System;
using System.CodeDom;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.DialogPanels
{
    public partial class FormPanel : DialogPageBase
    {
        public override bool BackEnabled
        {
            get { return false; }
        }

        public FormPanel()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;

            ddfcbIssueDate.DropDown += dateComboBox_DropDown;
        }
    }
}
