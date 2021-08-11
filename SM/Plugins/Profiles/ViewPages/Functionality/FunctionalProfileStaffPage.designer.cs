using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    partial class FunctionalProfileStaffPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileStaffPage));
            this.chkMustKeyInPriceIfZero = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkDisplayEmployeeListAtlogin = new System.Windows.Forms.CheckBox();
            this.chkOnlyDisplayStoreEmployees = new System.Windows.Forms.CheckBox();
            this.chkClearUser = new System.Windows.Forms.CheckBox();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblSalesPerson = new System.Windows.Forms.Label();
            this.cmbSalesPerson = new System.Windows.Forms.ComboBox();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkMustKeyInPriceIfZero
            // 
            resources.ApplyResources(this.chkMustKeyInPriceIfZero, "chkMustKeyInPriceIfZero");
            this.chkMustKeyInPriceIfZero.BackColor = System.Drawing.Color.Transparent;
            this.chkMustKeyInPriceIfZero.Name = "chkMustKeyInPriceIfZero";
            this.chkMustKeyInPriceIfZero.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkDisplayEmployeeListAtlogin
            // 
            resources.ApplyResources(this.chkDisplayEmployeeListAtlogin, "chkDisplayEmployeeListAtlogin");
            this.chkDisplayEmployeeListAtlogin.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayEmployeeListAtlogin.Name = "chkDisplayEmployeeListAtlogin";
            this.chkDisplayEmployeeListAtlogin.UseVisualStyleBackColor = false;
            this.chkDisplayEmployeeListAtlogin.CheckedChanged += new System.EventHandler(this.chkDisplayEmployeeListAtlogin_CheckedChanged);
            // 
            // chkOnlyDisplayStoreEmployees
            // 
            resources.ApplyResources(this.chkOnlyDisplayStoreEmployees, "chkOnlyDisplayStoreEmployees");
            this.chkOnlyDisplayStoreEmployees.BackColor = System.Drawing.Color.Transparent;
            this.chkOnlyDisplayStoreEmployees.Name = "chkOnlyDisplayStoreEmployees";
            this.chkOnlyDisplayStoreEmployees.UseVisualStyleBackColor = false;
            // 
            // chkClearUser
            // 
            resources.ApplyResources(this.chkClearUser, "chkClearUser");
            this.chkClearUser.BackColor = System.Drawing.Color.Transparent;
            this.chkClearUser.Name = "chkClearUser";
            this.chkClearUser.UseVisualStyleBackColor = false;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.chkMustKeyInPriceIfZero);
            this.groupPanel1.Controls.Add(this.chkClearUser);
            this.groupPanel1.Controls.Add(this.chkDisplayEmployeeListAtlogin);
            this.groupPanel1.Controls.Add(this.chkOnlyDisplayStoreEmployees);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblSalesPerson
            // 
            this.lblSalesPerson.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblSalesPerson, "lblSalesPerson");
            this.lblSalesPerson.Name = "lblSalesPerson";
            // 
            // cmbSalesPerson
            // 
            this.cmbSalesPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesPerson.FormattingEnabled = true;
            this.cmbSalesPerson.Items.AddRange(new object[] {
            resources.GetString("cmbSalesPerson.Items"),
            resources.GetString("cmbSalesPerson.Items1"),
            resources.GetString("cmbSalesPerson.Items2")});
            resources.ApplyResources(this.cmbSalesPerson, "cmbSalesPerson");
            this.cmbSalesPerson.Name = "cmbSalesPerson";
            // 
            // FunctionalProfileStaffPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbSalesPerson);
            this.Controls.Add(this.lblSalesPerson);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label7);
            this.DoubleBuffered = true;
            this.Name = "FunctionalProfileStaffPage";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkMustKeyInPriceIfZero;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkDisplayEmployeeListAtlogin;
        private System.Windows.Forms.CheckBox chkOnlyDisplayStoreEmployees;
        private System.Windows.Forms.CheckBox chkClearUser;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblSalesPerson;
        private System.Windows.Forms.ComboBox cmbSalesPerson;
    }
}
