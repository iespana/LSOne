using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class RestaurantStationView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestaurantStationView));
			this.tbStationName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbStationType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnEditRemoteHost = new LSOne.Controls.ContextButton();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbWindowsPrinter = new LSOne.Controls.DualDataComboBox();
			this.cmbRemoteHost = new LSOne.Controls.DualDataComboBox();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
			this.cmbDeviceName = new LSOne.Controls.DualDataComboBox();
			this.ntbMaxCharacters = new LSOne.Controls.NumericTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cmbVerticalFontSize = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.cmbHorizontalFontSize = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.btnEditWindowsPrinter = new LSOne.Controls.ContextButton();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.btnEditWindowsPrinter);
			this.pnlBottom.Controls.Add(this.cmbHorizontalFontSize);
			this.pnlBottom.Controls.Add(this.label8);
			this.pnlBottom.Controls.Add(this.label7);
			this.pnlBottom.Controls.Add(this.cmbVerticalFontSize);
			this.pnlBottom.Controls.Add(this.label6);
			this.pnlBottom.Controls.Add(this.ntbMaxCharacters);
			this.pnlBottom.Controls.Add(this.cmbDeviceName);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.cmbStationType);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.btnEditRemoteHost);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.tbStationName);
			this.pnlBottom.Controls.Add(this.cmbRemoteHost);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.cmbWindowsPrinter);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			this.pnlBottom.Controls.Add(this.label12);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// tbStationName
			// 
			resources.ApplyResources(this.tbStationName, "tbStationName");
			this.tbStationName.Name = "tbStationName";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// cmbStationType
			// 
			this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStationType.FormattingEnabled = true;
			this.cmbStationType.Items.AddRange(new object[] {
            resources.GetString("cmbStationType.Items"),
            resources.GetString("cmbStationType.Items1"),
            resources.GetString("cmbStationType.Items2")});
			resources.ApplyResources(this.cmbStationType, "cmbStationType");
			this.cmbStationType.Name = "cmbStationType";
			this.cmbStationType.SelectedIndexChanged += new System.EventHandler(this.cmbStationType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// btnEditRemoteHost
			// 
			this.btnEditRemoteHost.BackColor = System.Drawing.Color.Transparent;
			this.btnEditRemoteHost.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditRemoteHost, "btnEditRemoteHost");
			this.btnEditRemoteHost.Name = "btnEditRemoteHost";
			this.btnEditRemoteHost.Click += new System.EventHandler(this.btnAddRemoteHost_Click);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cmbWindowsPrinter
			// 
			this.cmbWindowsPrinter.AddList = null;
			this.cmbWindowsPrinter.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbWindowsPrinter, "cmbWindowsPrinter");
			this.cmbWindowsPrinter.MaxLength = 32767;
			this.cmbWindowsPrinter.Name = "cmbWindowsPrinter";
			this.cmbWindowsPrinter.NoChangeAllowed = false;
			this.cmbWindowsPrinter.OnlyDisplayID = false;
			this.cmbWindowsPrinter.RemoveList = null;
			this.cmbWindowsPrinter.RowHeight = ((short)(22));
			this.cmbWindowsPrinter.SecondaryData = null;
			this.cmbWindowsPrinter.SelectedData = null;
			this.cmbWindowsPrinter.SelectedDataID = null;
			this.cmbWindowsPrinter.SelectionList = null;
			this.cmbWindowsPrinter.SkipIDColumn = true;
			this.cmbWindowsPrinter.RequestData += new System.EventHandler(this.cmbWindowsPrinter_RequestData);
			// 
			// cmbRemoteHost
			// 
			this.cmbRemoteHost.AddList = null;
			this.cmbRemoteHost.AllowKeyboardSelection = false;
			this.cmbRemoteHost.EnableTextBox = true;
			resources.ApplyResources(this.cmbRemoteHost, "cmbRemoteHost");
			this.cmbRemoteHost.MaxLength = 32767;
			this.cmbRemoteHost.Name = "cmbRemoteHost";
			this.cmbRemoteHost.NoChangeAllowed = false;
			this.cmbRemoteHost.OnlyDisplayID = false;
			this.cmbRemoteHost.RemoveList = null;
			this.cmbRemoteHost.RowHeight = ((short)(22));
			this.cmbRemoteHost.SecondaryData = null;
			this.cmbRemoteHost.SelectedData = null;
			this.cmbRemoteHost.SelectedDataID = null;
			this.cmbRemoteHost.SelectionList = null;
			this.cmbRemoteHost.SkipIDColumn = true;
			this.cmbRemoteHost.RequestData += new System.EventHandler(this.cmbRemoteHost_RequestData);
			this.cmbRemoteHost.SelectedDataChanged += new System.EventHandler(this.cmbRemoteHost_SelectedDataChanged);
			this.cmbRemoteHost.RequestClear += new System.EventHandler(this.ClearData);
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// cmbDeviceName
			// 
			this.cmbDeviceName.AddList = null;
			this.cmbDeviceName.AllowKeyboardSelection = false;
			this.cmbDeviceName.EnableTextBox = true;
			resources.ApplyResources(this.cmbDeviceName, "cmbDeviceName");
			this.cmbDeviceName.MaxLength = 32767;
			this.cmbDeviceName.Name = "cmbDeviceName";
			this.cmbDeviceName.NoChangeAllowed = false;
			this.cmbDeviceName.OnlyDisplayID = false;
			this.cmbDeviceName.RemoveList = null;
			this.cmbDeviceName.RowHeight = ((short)(22));
			this.cmbDeviceName.SecondaryData = null;
			this.cmbDeviceName.SelectedData = null;
			this.cmbDeviceName.SelectedDataID = null;
			this.cmbDeviceName.SelectionList = null;
			this.cmbDeviceName.SkipIDColumn = true;
			this.cmbDeviceName.RequestData += new System.EventHandler(this.cmbDeviceName_RequestData);
			// 
			// ntbMaxCharacters
			// 
			this.ntbMaxCharacters.AllowDecimal = false;
			this.ntbMaxCharacters.AllowNegative = false;
			this.ntbMaxCharacters.CultureInfo = null;
			this.ntbMaxCharacters.DecimalLetters = 0;
			this.ntbMaxCharacters.ForeColor = System.Drawing.Color.Black;
			this.ntbMaxCharacters.HasMinValue = false;
			resources.ApplyResources(this.ntbMaxCharacters, "ntbMaxCharacters");
			this.ntbMaxCharacters.MaxValue = 55D;
			this.ntbMaxCharacters.MinValue = 1D;
			this.ntbMaxCharacters.Name = "ntbMaxCharacters";
			this.ntbMaxCharacters.Value = 0D;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// cmbVerticalFontSize
			// 
			this.cmbVerticalFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbVerticalFontSize.FormattingEnabled = true;
			this.cmbVerticalFontSize.Items.AddRange(new object[] {
            resources.GetString("cmbVerticalFontSize.Items"),
            resources.GetString("cmbVerticalFontSize.Items1"),
            resources.GetString("cmbVerticalFontSize.Items2")});
			resources.ApplyResources(this.cmbVerticalFontSize, "cmbVerticalFontSize");
			this.cmbVerticalFontSize.Name = "cmbVerticalFontSize";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// cmbHorizontalFontSize
			// 
			this.cmbHorizontalFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHorizontalFontSize.FormattingEnabled = true;
			this.cmbHorizontalFontSize.Items.AddRange(new object[] {
            resources.GetString("cmbHorizontalFontSize.Items"),
            resources.GetString("cmbHorizontalFontSize.Items1"),
            resources.GetString("cmbHorizontalFontSize.Items2")});
			resources.ApplyResources(this.cmbHorizontalFontSize, "cmbHorizontalFontSize");
			this.cmbHorizontalFontSize.Name = "cmbHorizontalFontSize";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// btnEditWindowsPrinter
			// 
			this.btnEditWindowsPrinter.BackColor = System.Drawing.Color.Transparent;
			this.btnEditWindowsPrinter.Context = LSOne.Controls.ButtonType.Edit;
			resources.ApplyResources(this.btnEditWindowsPrinter, "btnEditWindowsPrinter");
			this.btnEditWindowsPrinter.Name = "btnEditWindowsPrinter";
			this.btnEditWindowsPrinter.Click += new System.EventHandler(this.btnEditWindowsPrinter_Click);
			// 
			// RestaurantStationView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "RestaurantStationView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbStationName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private TabControl tabSheetTabs;
        private System.Windows.Forms.ComboBox cmbStationType;
        private DualDataComboBox cmbWindowsPrinter;
        private DualDataComboBox cmbRemoteHost;
        private System.Windows.Forms.Label label1;
        private ContextButton btnEditRemoteHost;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbDeviceName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbVerticalFontSize;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbMaxCharacters;
        private System.Windows.Forms.ComboBox cmbHorizontalFontSize;
        private System.Windows.Forms.Label label8;
        private ContextButton btnEditWindowsPrinter;
    }
}
