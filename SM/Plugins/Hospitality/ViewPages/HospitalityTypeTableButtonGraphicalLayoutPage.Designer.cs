using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityTypeTableButtonGraphicalLayoutPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeTableButtonGraphicalLayoutPage));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkRequestNoOfGuests = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDiningTableLayoutID = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAutomaticJoiningCheck = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbTableButtonDescription = new System.Windows.Forms.ComboBox();
            this.cmbTableButtonStaffDescription = new System.Windows.Forms.ComboBox();
            this.tbSettingsFromRestaurant = new System.Windows.Forms.TextBox();
            this.cmbSettingsFromHospitalityType = new LSOne.Controls.DualDataComboBox();
            this.tbSharingSalesTypeFilter = new System.Windows.Forms.TextBox();
            this.tbSettingsFromSequence = new System.Windows.Forms.TextBox();
            this.btnsEditAddDiningTableLayout = new LSOne.Controls.ContextButtons();
            this.chkUpdateTableFromPOS = new System.Windows.Forms.CheckBox();
            this.chkPromptForCustomer = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbCustomerNameOnTable = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMaxNoOfGuests = new LSOne.Controls.NumericTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkRequestNoOfGuests
            // 
            resources.ApplyResources(this.chkRequestNoOfGuests, "chkRequestNoOfGuests");
            this.chkRequestNoOfGuests.BackColor = System.Drawing.Color.Transparent;
            this.chkRequestNoOfGuests.Name = "chkRequestNoOfGuests";
            this.chkRequestNoOfGuests.UseVisualStyleBackColor = false;
            this.chkRequestNoOfGuests.CheckedChanged += new System.EventHandler(this.chkRequestNoOfGuests_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbDiningTableLayoutID
            // 
            this.cmbDiningTableLayoutID.AddList = null;
            this.cmbDiningTableLayoutID.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDiningTableLayoutID, "cmbDiningTableLayoutID");
            this.cmbDiningTableLayoutID.MaxLength = 32767;
            this.cmbDiningTableLayoutID.Name = "cmbDiningTableLayoutID";
            this.cmbDiningTableLayoutID.NoChangeAllowed = false;
            this.cmbDiningTableLayoutID.OnlyDisplayID = false;
            this.cmbDiningTableLayoutID.RemoveList = null;
            this.cmbDiningTableLayoutID.RowHeight = ((short)(22));
            this.cmbDiningTableLayoutID.SecondaryData = null;
            this.cmbDiningTableLayoutID.SelectedData = null;
            this.cmbDiningTableLayoutID.SelectedDataID = null;
            this.cmbDiningTableLayoutID.SelectionList = null;
            this.cmbDiningTableLayoutID.SkipIDColumn = true;
            this.cmbDiningTableLayoutID.RequestData += new System.EventHandler(this.cmbDiningTableLayoutID_RequestData);
            this.cmbDiningTableLayoutID.SelectedDataChanged += new System.EventHandler(this.cmbDiningTableLayoutID_SelectedDataChanged);
            this.cmbDiningTableLayoutID.RequestClear += new System.EventHandler(this.ClearData);
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
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAutomaticJoiningCheck);
            this.groupBox1.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkAutomaticJoiningCheck
            // 
            resources.ApplyResources(this.chkAutomaticJoiningCheck, "chkAutomaticJoiningCheck");
            this.chkAutomaticJoiningCheck.Name = "chkAutomaticJoiningCheck";
            this.chkAutomaticJoiningCheck.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmbTableButtonDescription
            // 
            this.cmbTableButtonDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableButtonDescription.FormattingEnabled = true;
            this.cmbTableButtonDescription.Items.AddRange(new object[] {
            resources.GetString("cmbTableButtonDescription.Items"),
            resources.GetString("cmbTableButtonDescription.Items1"),
            resources.GetString("cmbTableButtonDescription.Items2"),
            resources.GetString("cmbTableButtonDescription.Items3")});
            resources.ApplyResources(this.cmbTableButtonDescription, "cmbTableButtonDescription");
            this.cmbTableButtonDescription.Name = "cmbTableButtonDescription";
            // 
            // cmbTableButtonStaffDescription
            // 
            this.cmbTableButtonStaffDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableButtonStaffDescription.FormattingEnabled = true;
            this.cmbTableButtonStaffDescription.Items.AddRange(new object[] {
            resources.GetString("cmbTableButtonStaffDescription.Items"),
            resources.GetString("cmbTableButtonStaffDescription.Items1")});
            resources.ApplyResources(this.cmbTableButtonStaffDescription, "cmbTableButtonStaffDescription");
            this.cmbTableButtonStaffDescription.Name = "cmbTableButtonStaffDescription";
            // 
            // tbSettingsFromRestaurant
            // 
            resources.ApplyResources(this.tbSettingsFromRestaurant, "tbSettingsFromRestaurant");
            this.tbSettingsFromRestaurant.Name = "tbSettingsFromRestaurant";
            this.tbSettingsFromRestaurant.ReadOnly = true;
            // 
            // cmbSettingsFromHospitalityType
            // 
            this.cmbSettingsFromHospitalityType.AddList = null;
            this.cmbSettingsFromHospitalityType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSettingsFromHospitalityType, "cmbSettingsFromHospitalityType");
            this.cmbSettingsFromHospitalityType.MaxLength = 32767;
            this.cmbSettingsFromHospitalityType.Name = "cmbSettingsFromHospitalityType";
            this.cmbSettingsFromHospitalityType.NoChangeAllowed = false;
            this.cmbSettingsFromHospitalityType.OnlyDisplayID = false;
            this.cmbSettingsFromHospitalityType.RemoveList = null;
            this.cmbSettingsFromHospitalityType.RowHeight = ((short)(22));
            this.cmbSettingsFromHospitalityType.SecondaryData = null;
            this.cmbSettingsFromHospitalityType.SelectedData = null;
            this.cmbSettingsFromHospitalityType.SelectedDataID = null;
            this.cmbSettingsFromHospitalityType.SelectionList = null;
            this.cmbSettingsFromHospitalityType.SkipIDColumn = true;
            this.cmbSettingsFromHospitalityType.RequestData += new System.EventHandler(this.cmbSettingsFromHospitalityType_RequestData);
            this.cmbSettingsFromHospitalityType.SelectedDataChanged += new System.EventHandler(this.cmbSettingsFromHospitalityType_SelectedDataChanged);
            this.cmbSettingsFromHospitalityType.RequestClear += new System.EventHandler(this.ClearData);
            // 
            // tbSharingSalesTypeFilter
            // 
            resources.ApplyResources(this.tbSharingSalesTypeFilter, "tbSharingSalesTypeFilter");
            this.tbSharingSalesTypeFilter.Name = "tbSharingSalesTypeFilter";
            // 
            // tbSettingsFromSequence
            // 
            resources.ApplyResources(this.tbSettingsFromSequence, "tbSettingsFromSequence");
            this.tbSettingsFromSequence.Name = "tbSettingsFromSequence";
            this.tbSettingsFromSequence.ReadOnly = true;
            // 
            // btnsEditAddDiningTableLayout
            // 
            this.btnsEditAddDiningTableLayout.AddButtonEnabled = true;
            this.btnsEditAddDiningTableLayout.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddDiningTableLayout.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsEditAddDiningTableLayout.EditButtonEnabled = false;
            resources.ApplyResources(this.btnsEditAddDiningTableLayout, "btnsEditAddDiningTableLayout");
            this.btnsEditAddDiningTableLayout.Name = "btnsEditAddDiningTableLayout";
            this.btnsEditAddDiningTableLayout.RemoveButtonEnabled = false;
            this.btnsEditAddDiningTableLayout.EditButtonClicked += new System.EventHandler(this.btnsEditAddDiningTableLayout_EditButtonClicked);
            this.btnsEditAddDiningTableLayout.AddButtonClicked += new System.EventHandler(this.btnsEditAddDiningTableLayout_AddButtonClicked);
            // 
            // chkUpdateTableFromPOS
            // 
            resources.ApplyResources(this.chkUpdateTableFromPOS, "chkUpdateTableFromPOS");
            this.chkUpdateTableFromPOS.Name = "chkUpdateTableFromPOS";
            this.chkUpdateTableFromPOS.UseVisualStyleBackColor = true;
            // 
            // chkPromptForCustomer
            // 
            resources.ApplyResources(this.chkPromptForCustomer, "chkPromptForCustomer");
            this.chkPromptForCustomer.Name = "chkPromptForCustomer";
            this.chkPromptForCustomer.UseVisualStyleBackColor = true;
            this.chkPromptForCustomer.CheckedChanged += new System.EventHandler(this.chkPromptForCustomer_CheckedChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmbCustomerNameOnTable
            // 
            this.cmbCustomerNameOnTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerNameOnTable.FormattingEnabled = true;
            this.cmbCustomerNameOnTable.Items.AddRange(new object[] {
            resources.GetString("cmbCustomerNameOnTable.Items"),
            resources.GetString("cmbCustomerNameOnTable.Items1"),
            resources.GetString("cmbCustomerNameOnTable.Items2")});
            resources.ApplyResources(this.cmbCustomerNameOnTable, "cmbCustomerNameOnTable");
            this.cmbCustomerNameOnTable.Name = "cmbCustomerNameOnTable";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // txtMaxNoOfGuests
            // 
            this.txtMaxNoOfGuests.AllowDecimal = false;
            this.txtMaxNoOfGuests.AllowNegative = false;
            this.txtMaxNoOfGuests.CultureInfo = null;
            this.txtMaxNoOfGuests.DecimalLetters = 0;
            this.txtMaxNoOfGuests.ForeColor = System.Drawing.Color.Black;
            this.txtMaxNoOfGuests.HasMinValue = true;
            resources.ApplyResources(this.txtMaxNoOfGuests, "txtMaxNoOfGuests");
            this.txtMaxNoOfGuests.MaxValue = 0D;
            this.txtMaxNoOfGuests.MinValue = 0D;
            this.txtMaxNoOfGuests.Name = "txtMaxNoOfGuests";
            this.txtMaxNoOfGuests.Value = 0D;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // HospitalityTypeTableButtonGraphicalLayoutPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtMaxNoOfGuests);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbCustomerNameOnTable);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkPromptForCustomer);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkUpdateTableFromPOS);
            this.Controls.Add(this.btnsEditAddDiningTableLayout);
            this.Controls.Add(this.tbSettingsFromSequence);
            this.Controls.Add(this.tbSharingSalesTypeFilter);
            this.Controls.Add(this.cmbSettingsFromHospitalityType);
            this.Controls.Add(this.tbSettingsFromRestaurant);
            this.Controls.Add(this.cmbTableButtonStaffDescription);
            this.Controls.Add(this.cmbTableButtonDescription);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbDiningTableLayoutID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkRequestNoOfGuests);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "HospitalityTypeTableButtonGraphicalLayoutPage";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkRequestNoOfGuests;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbDiningTableLayoutID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAutomaticJoiningCheck;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbTableButtonDescription;
        private System.Windows.Forms.ComboBox cmbTableButtonStaffDescription;
        private System.Windows.Forms.TextBox tbSettingsFromRestaurant;
        private DualDataComboBox cmbSettingsFromHospitalityType;
        private System.Windows.Forms.TextBox tbSharingSalesTypeFilter;
        private System.Windows.Forms.TextBox tbSettingsFromSequence;
        private ContextButtons btnsEditAddDiningTableLayout;
        private System.Windows.Forms.CheckBox chkUpdateTableFromPOS;
        private System.Windows.Forms.CheckBox chkPromptForCustomer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbCustomerNameOnTable;
        private System.Windows.Forms.Label label12;
        private NumericTextBox txtMaxNoOfGuests;
        private System.Windows.Forms.Label label13;
    }
}
