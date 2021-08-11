

using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class DatabaseStringFieldsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseStringFieldsControl));
            this.cmbDBDriverType = new System.Windows.Forms.ComboBox();
            this.tbDBPathName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDBServerHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUserId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.gbFields = new System.Windows.Forms.GroupBox();
            this.cmbConnectionType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.linkFields1 = new LinkFields();
            this.btnTest = new System.Windows.Forms.Button();
            this.databaseStringControl = new SimpleDatabaseStringControl();
            this.gbFields.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbDBDriverType
            // 
            this.cmbDBDriverType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBDriverType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDBDriverType, "cmbDBDriverType");
            this.cmbDBDriverType.Name = "cmbDBDriverType";
            this.cmbDBDriverType.SelectedIndexChanged += new System.EventHandler(this.cmbDBDriverType_SelectedIndexChanged);
            // 
            // tbDBPathName
            // 
            resources.ApplyResources(this.tbDBPathName, "tbDBPathName");
            this.tbDBPathName.Name = "tbDBPathName";
            this.tbDBPathName.TextChanged += new System.EventHandler(this.tbDBPathName_TextChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tbDBServerHost
            // 
            resources.ApplyResources(this.tbDBServerHost, "tbDBServerHost");
            this.tbDBServerHost.Name = "tbDBServerHost";
            this.tbDBServerHost.TextChanged += new System.EventHandler(this.tbDBServerHost_TextChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbUserId
            // 
            resources.ApplyResources(this.tbUserId, "tbUserId");
            this.tbUserId.Name = "tbUserId";
            this.tbUserId.TextChanged += new System.EventHandler(this.tbUserId_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbCompany
            // 
            resources.ApplyResources(this.tbCompany, "tbCompany");
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.TextChanged += new System.EventHandler(this.tbCompany_TextChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // gbFields
            // 
            resources.ApplyResources(this.gbFields, "gbFields");
            this.gbFields.Controls.Add(this.cmbConnectionType);
            this.gbFields.Controls.Add(this.label4);
            this.gbFields.Controls.Add(this.cmbDBDriverType);
            this.gbFields.Controls.Add(this.tbDBPathName);
            this.gbFields.Controls.Add(this.label6);
            this.gbFields.Controls.Add(this.tbDBServerHost);
            this.gbFields.Controls.Add(this.label5);
            this.gbFields.Controls.Add(this.tbPassword);
            this.gbFields.Controls.Add(this.label2);
            this.gbFields.Controls.Add(this.tbUserId);
            this.gbFields.Controls.Add(this.label1);
            this.gbFields.Controls.Add(this.tbCompany);
            this.gbFields.Controls.Add(this.label3);
            this.gbFields.Controls.Add(this.label7);
            this.gbFields.Name = "gbFields";
            this.gbFields.TabStop = false;
            // 
            // cmbConnectionType
            // 
            this.cmbConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectionType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbConnectionType, "cmbConnectionType");
            this.cmbConnectionType.Name = "cmbConnectionType";
            this.cmbConnectionType.SelectedIndexChanged += new System.EventHandler(this.cmbConnectionType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // linkFields1
            // 
            resources.ApplyResources(this.linkFields1, "linkFields1");
            this.linkFields1.BackColor = System.Drawing.Color.Transparent;
            this.linkFields1.Name = "linkFields1";
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // databaseStringControl
            // 
            resources.ApplyResources(this.databaseStringControl, "databaseStringControl");
            this.databaseStringControl.BackColor = System.Drawing.Color.Transparent;
            this.databaseStringControl.DatabaseString = null;
            this.databaseStringControl.DatabaseStringTextBoxToolTip = "";
            this.databaseStringControl.IsDDString = true;
            this.databaseStringControl.Name = "databaseStringControl";
            this.databaseStringControl.ShowPasswordCheckboxVisible = true;
            this.databaseStringControl.DatabaseStringChanged += new System.EventHandler(this.databaseStringControl_DatabaseStringChanged);
            // 
            // DatabaseStringFieldsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.databaseStringControl);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.linkFields1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.gbFields);
            this.Name = "DatabaseStringFieldsControl";
            this.gbFields.ResumeLayout(false);
            this.gbFields.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDBDriverType;
        private System.Windows.Forms.TextBox tbDBPathName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDBServerHost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUserId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox gbFields;
        private LinkFields linkFields1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ComboBox cmbConnectionType;
        private System.Windows.Forms.Label label4;
        private SimpleDatabaseStringControl databaseStringControl;
    }
}
