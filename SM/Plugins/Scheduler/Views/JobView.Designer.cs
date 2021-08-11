using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class JobView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobView));
            this.tabControl = new LSOne.ViewCore.Controls.TabControl();
            this.cmbDestination = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.cmbSource = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbErrorHandling = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnEditSourceLocation = new LSOne.Controls.ContextButton();
            this.btnEditDestLocation = new LSOne.Controls.ContextButton();
            this.lblExternalCommand = new System.Windows.Forms.Label();
            this.cbExternalCommand = new System.Windows.Forms.CheckBox();
            this.tbJobTypeCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUseCurrentLocation = new System.Windows.Forms.CheckBox();
            this.cmbCompressionMode = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbIsolationLevel = new System.Windows.Forms.ComboBox();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.label9);
            this.pnlBottom.Controls.Add(this.cmbIsolationLevel);
            this.pnlBottom.Controls.Add(this.label8);
            this.pnlBottom.Controls.Add(this.cmbCompressionMode);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.cbUseCurrentLocation);
            this.pnlBottom.Controls.Add(this.tbJobTypeCode);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.lblExternalCommand);
            this.pnlBottom.Controls.Add(this.cbExternalCommand);
            this.pnlBottom.Controls.Add(this.btnEditDestLocation);
            this.pnlBottom.Controls.Add(this.btnEditSourceLocation);
            this.pnlBottom.Controls.Add(this.cmbErrorHandling);
            this.pnlBottom.Controls.Add(this.label7);
            this.pnlBottom.Controls.Add(this.label6);
            this.pnlBottom.Controls.Add(this.chkEnabled);
            this.pnlBottom.Controls.Add(this.tabControl);
            this.pnlBottom.Controls.Add(this.cmbDestination);
            this.pnlBottom.Controls.Add(this.cmbSource);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.label4);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.label3);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.TabStop = true;
            // 
            // cmbDestination
            // 
            this.cmbDestination.AddList = null;
            resources.ApplyResources(this.cmbDestination, "cmbDestination");
            this.cmbDestination.MaxLength = 32767;
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.NoChangeAllowed = false;
            this.cmbDestination.RemoveList = null;
            this.cmbDestination.RowHeight = ((short)(22));
            this.cmbDestination.SecondaryData = null;
            this.cmbDestination.SelectedData = null;
            this.cmbDestination.SelectionList = null;
            this.cmbDestination.DropDown += new LSOne.Controls.DropDownEventHandler(this.LocationSelectDropDown);
            this.cmbDestination.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.LocationSelectFormatData);
            this.cmbDestination.SelectedDataChanged += new System.EventHandler(this.cmbDestination_SelectedDataChanged);
            this.cmbDestination.RequestClear += new System.EventHandler(this.cmbDestination_RequestClear);
            // 
            // cmbSource
            // 
            this.cmbSource.AddList = null;
            resources.ApplyResources(this.cmbSource, "cmbSource");
            this.cmbSource.MaxLength = 32767;
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.NoChangeAllowed = false;
            this.cmbSource.RemoveList = null;
            this.cmbSource.RowHeight = ((short)(22));
            this.cmbSource.SecondaryData = null;
            this.cmbSource.SelectedData = null;
            this.cmbSource.SelectionList = null;
            this.cmbSource.DropDown += new LSOne.Controls.DropDownEventHandler(this.LocationSelectDropDown);
            this.cmbSource.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.LocationSelectFormatData);
            this.cmbSource.SelectedDataChanged += new System.EventHandler(this.cmbSource_SelectedDataChanged);
            this.cmbSource.RequestClear += new System.EventHandler(this.cmbSource_RequestClear);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbDescription
            // 
            this.tbDescription.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Validating += new System.ComponentModel.CancelEventHandler(this.tbDescription_Validating);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbErrorHandling
            // 
            this.cmbErrorHandling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbErrorHandling.FormattingEnabled = true;
            resources.ApplyResources(this.cmbErrorHandling, "cmbErrorHandling");
            this.cmbErrorHandling.Name = "cmbErrorHandling";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnEditSourceLocation
            // 
            this.btnEditSourceLocation.BackColor = System.Drawing.Color.Transparent;
            this.btnEditSourceLocation.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSourceLocation, "btnEditSourceLocation");
            this.btnEditSourceLocation.Name = "btnEditSourceLocation";
            this.btnEditSourceLocation.Click += new System.EventHandler(this.btnEditSourceLocation_Click);
            // 
            // btnEditDestLocation
            // 
            this.btnEditDestLocation.BackColor = System.Drawing.Color.Transparent;
            this.btnEditDestLocation.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditDestLocation, "btnEditDestLocation");
            this.btnEditDestLocation.Name = "btnEditDestLocation";
            this.btnEditDestLocation.Click += new System.EventHandler(this.btnEditDestLocation_Click);
            // 
            // lblExternalCommand
            // 
            this.lblExternalCommand.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblExternalCommand, "lblExternalCommand");
            this.lblExternalCommand.Name = "lblExternalCommand";
            // 
            // cbExternalCommand
            // 
            resources.ApplyResources(this.cbExternalCommand, "cbExternalCommand");
            this.cbExternalCommand.Name = "cbExternalCommand";
            this.cbExternalCommand.UseVisualStyleBackColor = true;
            this.cbExternalCommand.CheckedChanged += new System.EventHandler(this.cbExternalCommand_CheckedChanged);
            // 
            // tbJobTypeCode
            // 
            this.tbJobTypeCode.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbJobTypeCode, "tbJobTypeCode");
            this.tbJobTypeCode.Name = "tbJobTypeCode";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbUseCurrentLocation
            // 
            resources.ApplyResources(this.cbUseCurrentLocation, "cbUseCurrentLocation");
            this.cbUseCurrentLocation.Name = "cbUseCurrentLocation";
            this.cbUseCurrentLocation.UseVisualStyleBackColor = true;
            this.cbUseCurrentLocation.CheckedChanged += new System.EventHandler(this.cbUseCurrentLocation_CheckedChanged);
            // 
            // cmbCompressionMode
            // 
            this.cmbCompressionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompressionMode.FormattingEnabled = true;
            this.cmbCompressionMode.Items.AddRange(new object[] {
            resources.GetString("cmbCompressionMode.Items"),
            resources.GetString("cmbCompressionMode.Items1"),
            resources.GetString("cmbCompressionMode.Items2"),
            resources.GetString("cmbCompressionMode.Items3"),
            resources.GetString("cmbCompressionMode.Items4")});
            resources.ApplyResources(this.cmbCompressionMode, "cmbCompressionMode");
            this.cmbCompressionMode.Name = "cmbCompressionMode";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmbIsolationLevel
            // 
            this.cmbIsolationLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIsolationLevel.FormattingEnabled = true;
            this.cmbIsolationLevel.Items.AddRange(new object[] {
            resources.GetString("cmbIsolationLevel.Items"),
            resources.GetString("cmbIsolationLevel.Items1"),
            resources.GetString("cmbIsolationLevel.Items2"),
            resources.GetString("cmbIsolationLevel.Items3"),
            resources.GetString("cmbIsolationLevel.Items4"),
            resources.GetString("cmbIsolationLevel.Items5"),
            resources.GetString("cmbIsolationLevel.Items6"),
            resources.GetString("cmbIsolationLevel.Items7")});
            resources.ApplyResources(this.cmbIsolationLevel, "cmbIsolationLevel");
            this.cmbIsolationLevel.Name = "cmbIsolationLevel";
            // 
            // JobView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "JobView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private System.Windows.Forms.ComboBox cmbErrorHandling;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private DropDownFormComboBox cmbDestination;
        private DropDownFormComboBox cmbSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private ContextButton btnEditDestLocation;
        private ContextButton btnEditSourceLocation;
        private System.Windows.Forms.Label lblExternalCommand;
        private System.Windows.Forms.CheckBox cbExternalCommand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbUseCurrentLocation;
        private System.Windows.Forms.TextBox tbJobTypeCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbIsolationLevel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbCompressionMode;
    }
}
