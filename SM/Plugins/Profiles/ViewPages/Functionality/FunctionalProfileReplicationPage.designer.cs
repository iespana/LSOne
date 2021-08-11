using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    partial class FunctionalProfileReplicationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileReplicationPage));
            this.lblPostTransactionDD = new System.Windows.Forms.Label();
            this.cmbDDJobs = new System.Windows.Forms.ComboBox();
            this.lblDDJobDestination = new System.Windows.Forms.Label();
            this.ddcbSchedulerLocation = new LSOne.Controls.DualDataComboBox();
            this.SuspendLayout();
            // 
            // lblPostTransactionDD
            // 
            this.lblPostTransactionDD.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblPostTransactionDD, "lblPostTransactionDD");
            this.lblPostTransactionDD.Name = "lblPostTransactionDD";
            // 
            // cmbDDJobs
            // 
            this.cmbDDJobs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDDJobs.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDDJobs, "cmbDDJobs");
            this.cmbDDJobs.Name = "cmbDDJobs";
            this.cmbDDJobs.SelectedValueChanged += new System.EventHandler(this.cmbDDJobs_SelectedValueChanged);
            // 
            // lblDDJobDestination
            // 
            this.lblDDJobDestination.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDDJobDestination, "lblDDJobDestination");
            this.lblDDJobDestination.Name = "lblDDJobDestination";
            // 
            // ddcbSchedulerLocation
            // 
            this.ddcbSchedulerLocation.AddList = null;
            this.ddcbSchedulerLocation.AllowKeyboardSelection = false;
            this.ddcbSchedulerLocation.EnableTextBox = true;
            resources.ApplyResources(this.ddcbSchedulerLocation, "ddcbSchedulerLocation");
            this.ddcbSchedulerLocation.MaxLength = 32767;
            this.ddcbSchedulerLocation.Name = "ddcbSchedulerLocation";
            this.ddcbSchedulerLocation.NoChangeAllowed = false;
            this.ddcbSchedulerLocation.OnlyDisplayID = false;
            this.ddcbSchedulerLocation.RemoveList = null;
            this.ddcbSchedulerLocation.RowHeight = ((short)(22));
            this.ddcbSchedulerLocation.SecondaryData = null;
            this.ddcbSchedulerLocation.SelectedData = null;
            this.ddcbSchedulerLocation.SelectedDataID = null;
            this.ddcbSchedulerLocation.SelectionList = null;
            this.ddcbSchedulerLocation.SkipIDColumn = false;
            this.ddcbSchedulerLocation.RequestData += new System.EventHandler(this.ddcbSchedulerLocation_RequestData);
            this.ddcbSchedulerLocation.SelectedDataChanged += new System.EventHandler(this.ddcbSchedulerLocation_SelectedDataChanged);
            this.ddcbSchedulerLocation.RequestClear += new System.EventHandler(this.ddcbSchedulerLocation_RequestClear);
            // 
            // FunctionalProfileReplicationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ddcbSchedulerLocation);
            this.Controls.Add(this.lblDDJobDestination);
            this.Controls.Add(this.cmbDDJobs);
            this.Controls.Add(this.lblPostTransactionDD);
            this.DoubleBuffered = true;
            this.Name = "FunctionalProfileReplicationPage";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPostTransactionDD;
        private System.Windows.Forms.ComboBox cmbDDJobs;
        private System.Windows.Forms.Label lblDDJobDestination;
        private DualDataComboBox ddcbSchedulerLocation;
    }
}
