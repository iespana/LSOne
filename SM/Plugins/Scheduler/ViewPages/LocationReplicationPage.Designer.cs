using LSOne.Controls;
using LSOne.ViewPlugins.Scheduler.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class LocationReplicationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationReplicationPage));
            this.tbDDHost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkDBServerIsUsed = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.cmbNetMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDDPort = new ShadeTextBox();
            this.databaseStringFieldsControl = new DatabaseStringFieldsControl();
            this.groupPanel1 = new GroupPanel();
            this.cmbDatabaseDesign = new DatabaseDesignComboBox();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDDHost
            // 
            resources.ApplyResources(this.tbDDHost, "tbDDHost");
            this.tbDDHost.Name = "tbDDHost";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // chkDBServerIsUsed
            // 
            resources.ApplyResources(this.chkDBServerIsUsed, "chkDBServerIsUsed");
            this.chkDBServerIsUsed.Name = "chkDBServerIsUsed";
            this.chkDBServerIsUsed.UseVisualStyleBackColor = true;
            this.chkDBServerIsUsed.CheckedChanged += new System.EventHandler(this.chkDBServerIsUsed_CheckedChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmbNetMode
            // 
            this.cmbNetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetMode.FormattingEnabled = true;
            this.cmbNetMode.Items.AddRange(new object[] {
            resources.GetString("cmbNetMode.Items"),
            resources.GetString("cmbNetMode.Items1"),
            resources.GetString("cmbNetMode.Items2"),
            resources.GetString("cmbNetMode.Items3"),
            resources.GetString("cmbNetMode.Items4"),
            resources.GetString("cmbNetMode.Items5"),
            resources.GetString("cmbNetMode.Items6")});
            resources.ApplyResources(this.cmbNetMode, "cmbNetMode");
            this.cmbNetMode.Name = "cmbNetMode";
            this.cmbNetMode.SelectedIndexChanged += new System.EventHandler(this.cmbNetMode_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // tbDDPort
            // 
            resources.ApplyResources(this.tbDDPort, "tbDDPort");
            this.tbDDPort.Name = "tbDDPort";
            // 
            // databaseStringFieldsControl
            // 
            this.databaseStringFieldsControl.BackColor = System.Drawing.Color.Transparent;
            this.databaseStringFieldsControl.DatabaseDriverTypes = null;
            this.databaseStringFieldsControl.DatabaseString = null;
            this.databaseStringFieldsControl.IsDDString = false;
            resources.ApplyResources(this.databaseStringFieldsControl, "databaseStringFieldsControl");
            this.databaseStringFieldsControl.Name = "databaseStringFieldsControl";
            this.databaseStringFieldsControl.TestButtonVisible = false;
            this.databaseStringFieldsControl.DatabaseStringChanged += new System.EventHandler(this.databaseStringFieldsControl_DatabaseStringChanged);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.databaseStringFieldsControl);
            this.groupPanel1.Controls.Add(this.cmbDatabaseDesign);
            this.groupPanel1.Controls.Add(this.label8);
            this.groupPanel1.Controls.Add(this.chkDBServerIsUsed);
            this.groupPanel1.Controls.Add(this.label11);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // cmbDatabaseDesign
            // 
            this.cmbDatabaseDesign.DatabaseDesignId = null;
            this.cmbDatabaseDesign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseDesign.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDatabaseDesign, "cmbDatabaseDesign");
            this.cmbDatabaseDesign.Name = "cmbDatabaseDesign";
            // 
            // LocationReplicationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tbDDPort);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmbNetMode);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbDDHost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "LocationReplicationPage";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDDHost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkDBServerIsUsed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbNetMode;
        private System.Windows.Forms.Label label13;
        private DatabaseDesignComboBox cmbDatabaseDesign;
        private DatabaseStringFieldsControl databaseStringFieldsControl;
        private ShadeTextBox tbDDPort;
        private GroupPanel groupPanel1;
    }
}
