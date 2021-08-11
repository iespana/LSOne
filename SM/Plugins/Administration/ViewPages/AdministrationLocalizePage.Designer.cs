namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationLocalizePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationLocalizePage));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cmbAddressConvention = new System.Windows.Forms.ComboBox();
            this.cmbNamingConvention = new System.Windows.Forms.ComboBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.lblAddress);
            this.groupBox4.Controls.Add(this.lblName);
            this.groupBox4.Controls.Add(this.cmbAddressConvention);
            this.groupBox4.Controls.Add(this.cmbNamingConvention);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.Name = "lblAddress";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // cmbAddressConvention
            // 
            this.cmbAddressConvention.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddressConvention.FormattingEnabled = true;
            resources.ApplyResources(this.cmbAddressConvention, "cmbAddressConvention");
            this.cmbAddressConvention.Name = "cmbAddressConvention";
            // 
            // cmbNamingConvention
            // 
            this.cmbNamingConvention.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNamingConvention.FormattingEnabled = true;
            resources.ApplyResources(this.cmbNamingConvention, "cmbNamingConvention");
            this.cmbNamingConvention.Name = "cmbNamingConvention";
            // 
            // AdministrationLocalizePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox4);
            this.Name = "AdministrationLocalizePage";
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cmbAddressConvention;
        private System.Windows.Forms.ComboBox cmbNamingConvention;
    }
}
