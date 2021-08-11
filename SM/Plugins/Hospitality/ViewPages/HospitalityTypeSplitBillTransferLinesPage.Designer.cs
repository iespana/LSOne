using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityTypeSplitBillTransferLinesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeSplitBillTransferLinesPage));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSplitBillLookupId = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTransferLinesLookupId = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbGuestButtons = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSelectGuestOnSplitting = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbCombineSplitLinesAction = new System.Windows.Forms.ComboBox();
            this.ntbMaxGuestButtonsShown = new LSOne.Controls.NumericTextBox();
            this.ntbMaxGuestsPerTable = new LSOne.Controls.NumericTextBox();
            this.btnsEditAddSplitBillLookup = new LSOne.Controls.ContextButtons();
            this.btnsEditAddTransferLinesLookup = new LSOne.Controls.ContextButtons();
            this.ntbMaxNoOfSplits = new LSOne.Controls.NumericTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbSplitBillLookupId
            // 
            this.cmbSplitBillLookupId.AddList = null;
            this.cmbSplitBillLookupId.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSplitBillLookupId, "cmbSplitBillLookupId");
            this.cmbSplitBillLookupId.MaxLength = 32767;
            this.cmbSplitBillLookupId.Name = "cmbSplitBillLookupId";
            this.cmbSplitBillLookupId.NoChangeAllowed = false;
            this.cmbSplitBillLookupId.OnlyDisplayID = false;
            this.cmbSplitBillLookupId.RemoveList = null;
            this.cmbSplitBillLookupId.RowHeight = ((short)(22));
            this.cmbSplitBillLookupId.SecondaryData = null;
            this.cmbSplitBillLookupId.SelectedData = null;
            this.cmbSplitBillLookupId.SelectedDataID = null;
            this.cmbSplitBillLookupId.SelectionList = null;
            this.cmbSplitBillLookupId.SkipIDColumn = true;
            this.cmbSplitBillLookupId.RequestData += new System.EventHandler(this.cmbSplitBillLookupId_RequestData);
            this.cmbSplitBillLookupId.SelectedDataChanged += new System.EventHandler(this.cmbSplitBillLookupId_SelectedDataChanged);
            this.cmbSplitBillLookupId.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbTransferLinesLookupId
            // 
            this.cmbTransferLinesLookupId.AddList = null;
            this.cmbTransferLinesLookupId.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTransferLinesLookupId, "cmbTransferLinesLookupId");
            this.cmbTransferLinesLookupId.MaxLength = 32767;
            this.cmbTransferLinesLookupId.Name = "cmbTransferLinesLookupId";
            this.cmbTransferLinesLookupId.NoChangeAllowed = false;
            this.cmbTransferLinesLookupId.OnlyDisplayID = false;
            this.cmbTransferLinesLookupId.RemoveList = null;
            this.cmbTransferLinesLookupId.RowHeight = ((short)(22));
            this.cmbTransferLinesLookupId.SecondaryData = null;
            this.cmbTransferLinesLookupId.SelectedData = null;
            this.cmbTransferLinesLookupId.SelectedDataID = null;
            this.cmbTransferLinesLookupId.SelectionList = null;
            this.cmbTransferLinesLookupId.SkipIDColumn = true;
            this.cmbTransferLinesLookupId.RequestData += new System.EventHandler(this.cmbTransferLinesLookupId_RequestData);
            this.cmbTransferLinesLookupId.SelectedDataChanged += new System.EventHandler(this.cmbTransferLinesLookupId_SelectedDataChanged);
            this.cmbTransferLinesLookupId.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbGuestButtons
            // 
            this.cmbGuestButtons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGuestButtons.FormattingEnabled = true;
            this.cmbGuestButtons.Items.AddRange(new object[] {
            resources.GetString("cmbGuestButtons.Items"),
            resources.GetString("cmbGuestButtons.Items1"),
            resources.GetString("cmbGuestButtons.Items2")});
            resources.ApplyResources(this.cmbGuestButtons, "cmbGuestButtons");
            this.cmbGuestButtons.Name = "cmbGuestButtons";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkSelectGuestOnSplitting
            // 
            resources.ApplyResources(this.chkSelectGuestOnSplitting, "chkSelectGuestOnSplitting");
            this.chkSelectGuestOnSplitting.Name = "chkSelectGuestOnSplitting";
            this.chkSelectGuestOnSplitting.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbCombineSplitLinesAction
            // 
            this.cmbCombineSplitLinesAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCombineSplitLinesAction.FormattingEnabled = true;
            this.cmbCombineSplitLinesAction.Items.AddRange(new object[] {
            resources.GetString("cmbCombineSplitLinesAction.Items"),
            resources.GetString("cmbCombineSplitLinesAction.Items1"),
            resources.GetString("cmbCombineSplitLinesAction.Items2")});
            resources.ApplyResources(this.cmbCombineSplitLinesAction, "cmbCombineSplitLinesAction");
            this.cmbCombineSplitLinesAction.Name = "cmbCombineSplitLinesAction";
            // 
            // ntbMaxGuestButtonsShown
            // 
            this.ntbMaxGuestButtonsShown.AllowDecimal = false;
            this.ntbMaxGuestButtonsShown.AllowNegative = false;
            this.ntbMaxGuestButtonsShown.CultureInfo = null;
            this.ntbMaxGuestButtonsShown.DecimalLetters = 2;
            resources.ApplyResources(this.ntbMaxGuestButtonsShown, "ntbMaxGuestButtonsShown");
            this.ntbMaxGuestButtonsShown.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxGuestButtonsShown.HasMinValue = false;
            this.ntbMaxGuestButtonsShown.MaxValue = 0D;
            this.ntbMaxGuestButtonsShown.MinValue = 0D;
            this.ntbMaxGuestButtonsShown.Name = "ntbMaxGuestButtonsShown";
            this.ntbMaxGuestButtonsShown.Value = 0D;
            // 
            // ntbMaxGuestsPerTable
            // 
            this.ntbMaxGuestsPerTable.AllowDecimal = false;
            this.ntbMaxGuestsPerTable.AllowNegative = false;
            this.ntbMaxGuestsPerTable.CultureInfo = null;
            this.ntbMaxGuestsPerTable.DecimalLetters = 2;
            resources.ApplyResources(this.ntbMaxGuestsPerTable, "ntbMaxGuestsPerTable");
            this.ntbMaxGuestsPerTable.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxGuestsPerTable.HasMinValue = false;
            this.ntbMaxGuestsPerTable.MaxValue = 0D;
            this.ntbMaxGuestsPerTable.MinValue = 0D;
            this.ntbMaxGuestsPerTable.Name = "ntbMaxGuestsPerTable";
            this.ntbMaxGuestsPerTable.Value = 0D;
            // 
            // btnsEditAddSplitBillLookup
            // 
            this.btnsEditAddSplitBillLookup.AddButtonEnabled = true;
            this.btnsEditAddSplitBillLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddSplitBillLookup.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddSplitBillLookup.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddSplitBillLookup, "btnsEditAddSplitBillLookup");
            this.btnsEditAddSplitBillLookup.Name = "btnsEditAddSplitBillLookup";
            this.btnsEditAddSplitBillLookup.RemoveButtonEnabled = false;
            this.btnsEditAddSplitBillLookup.EditButtonClicked += new System.EventHandler(this.btnsEditAddSplitBillLookup_EditButtonClicked);
            this.btnsEditAddSplitBillLookup.AddButtonClicked += new System.EventHandler(this.btnAddSplitBillLookup_Click);
            // 
            // btnsEditAddTransferLinesLookup
            // 
            this.btnsEditAddTransferLinesLookup.AddButtonEnabled = true;
            this.btnsEditAddTransferLinesLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddTransferLinesLookup.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddTransferLinesLookup.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddTransferLinesLookup, "btnsEditAddTransferLinesLookup");
            this.btnsEditAddTransferLinesLookup.Name = "btnsEditAddTransferLinesLookup";
            this.btnsEditAddTransferLinesLookup.RemoveButtonEnabled = false;
            this.btnsEditAddTransferLinesLookup.EditButtonClicked += new System.EventHandler(this.btnsEditAddTransferLinesLookup_EditButtonClicked);
            this.btnsEditAddTransferLinesLookup.AddButtonClicked += new System.EventHandler(this.btnAddTransferLinesLookup_Click);
            // 
            // ntbMaxNoOfSplits
            // 
            this.ntbMaxNoOfSplits.AllowDecimal = false;
            this.ntbMaxNoOfSplits.AllowNegative = false;
            this.ntbMaxNoOfSplits.CultureInfo = null;
            this.ntbMaxNoOfSplits.DecimalLetters = 2;
            this.ntbMaxNoOfSplits.ForeColor = System.Drawing.Color.Black;
            this.ntbMaxNoOfSplits.HasMinValue = false;
            resources.ApplyResources(this.ntbMaxNoOfSplits, "ntbMaxNoOfSplits");
            this.ntbMaxNoOfSplits.MaxValue = 0D;
            this.ntbMaxNoOfSplits.MinValue = 0D;
            this.ntbMaxNoOfSplits.Name = "ntbMaxNoOfSplits";
            this.ntbMaxNoOfSplits.Value = 0D;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // HospitalityTypeSplitBillTransferLinesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbMaxNoOfSplits);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnsEditAddTransferLinesLookup);
            this.Controls.Add(this.btnsEditAddSplitBillLookup);
            this.Controls.Add(this.ntbMaxGuestsPerTable);
            this.Controls.Add(this.ntbMaxGuestButtonsShown);
            this.Controls.Add(this.cmbCombineSplitLinesAction);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkSelectGuestOnSplitting);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbGuestButtons);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTransferLinesLookupId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSplitBillLookupId);
            this.Controls.Add(this.label1);
            this.Name = "HospitalityTypeSplitBillTransferLinesPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbSplitBillLookupId;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbTransferLinesLookupId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbGuestButtons;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSelectGuestOnSplitting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbCombineSplitLinesAction;
        private NumericTextBox ntbMaxGuestButtonsShown;
        private NumericTextBox ntbMaxGuestsPerTable;
        private ContextButtons btnsEditAddSplitBillLookup;
        private ContextButtons btnsEditAddTransferLinesLookup;
        private NumericTextBox ntbMaxNoOfSplits;
        private System.Windows.Forms.Label label8;
    }
}
