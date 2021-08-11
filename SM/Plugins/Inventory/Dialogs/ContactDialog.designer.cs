using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class ContactDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbCompanyName = new System.Windows.Forms.TextBox();
            this.lblLoginName = new System.Windows.Forms.Label();
            this.fullNameField1 = new LSOne.Controls.FullNameControl();
            this.optCompanyAndPerson = new System.Windows.Forms.RadioButton();
            this.optCompany = new System.Windows.Forms.RadioButton();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbOtherPhone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.addressField = new LSOne.Controls.AddressControl();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // lblReason
            // 
            resources.ApplyResources(this.lblReason, "lblReason");
            this.lblReason.Name = "lblReason";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tbCompanyName
            // 
            this.errorProvider1.SetIconAlignment(this.tbCompanyName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tbCompanyName.IconAlignment"))));
            resources.ApplyResources(this.tbCompanyName, "tbCompanyName");
            this.tbCompanyName.Name = "tbCompanyName";
            this.tbCompanyName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblLoginName
            // 
            this.errorProvider1.SetIconAlignment(this.lblLoginName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblLoginName.IconAlignment"))));
            resources.ApplyResources(this.lblLoginName, "lblLoginName");
            this.lblLoginName.Name = "lblLoginName";
            // 
            // fullNameField1
            // 
            this.fullNameField1.Alias = "";
            resources.ApplyResources(this.fullNameField1, "fullNameField1");
            this.fullNameField1.BackColor = System.Drawing.Color.Transparent;
            this.fullNameField1.FirstName = "";
            this.errorProvider1.SetIconAlignment(this.fullNameField1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fullNameField1.IconAlignment"))));
            this.fullNameField1.LastName = "";
            this.fullNameField1.MiddleName = "";
            this.fullNameField1.Name = "fullNameField1";
            this.fullNameField1.Prefix = "";
            this.fullNameField1.ShowAlias = false;
            this.fullNameField1.Suffix = "";
            this.fullNameField1.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // optCompanyAndPerson
            // 
            resources.ApplyResources(this.optCompanyAndPerson, "optCompanyAndPerson");
            this.optCompanyAndPerson.Checked = true;
            this.optCompanyAndPerson.Name = "optCompanyAndPerson";
            this.optCompanyAndPerson.TabStop = true;
            this.optCompanyAndPerson.UseVisualStyleBackColor = true;
            this.optCompanyAndPerson.CheckedChanged += new System.EventHandler(this.optCompanyAndPerson_CheckedChanged);
            // 
            // optCompany
            // 
            resources.ApplyResources(this.optCompany, "optCompany");
            this.optCompany.Name = "optCompany";
            this.optCompany.UseVisualStyleBackColor = true;
            this.optCompany.CheckedChanged += new System.EventHandler(this.optCompany_CheckedChanged);
            // 
            // tbEmail
            // 
            resources.ApplyResources(this.tbEmail, "tbEmail");
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbFax
            // 
            resources.ApplyResources(this.tbFax, "tbFax");
            this.tbFax.Name = "tbFax";
            this.tbFax.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbPhone
            // 
            resources.ApplyResources(this.tbPhone, "tbPhone");
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbOtherPhone
            // 
            resources.ApplyResources(this.tbOtherPhone, "tbOtherPhone");
            this.tbOtherPhone.Name = "tbOtherPhone";
            this.tbOtherPhone.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.Name = "addressField";
            this.addressField.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // ContactDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tbOtherPhone);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbFax);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.addressField);
            this.Controls.Add(this.tbCompanyName);
            this.Controls.Add(this.lblLoginName);
            this.Controls.Add(this.fullNameField1);
            this.Controls.Add(this.optCompany);
            this.Controls.Add(this.optCompanyAndPerson);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ContactDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblReason, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.optCompanyAndPerson, 0);
            this.Controls.SetChildIndex(this.optCompany, 0);
            this.Controls.SetChildIndex(this.fullNameField1, 0);
            this.Controls.SetChildIndex(this.lblLoginName, 0);
            this.Controls.SetChildIndex(this.tbCompanyName, 0);
            this.Controls.SetChildIndex(this.addressField, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.tbPhone, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.tbFax, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbEmail, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.tbOtherPhone, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.RadioButton optCompany;
        private System.Windows.Forms.RadioButton optCompanyAndPerson;
        private System.Windows.Forms.TextBox tbCompanyName;
        private System.Windows.Forms.Label lblLoginName;
        private FullNameControl fullNameField1;
        private AddressControl addressField;
        private System.Windows.Forms.TextBox tbOtherPhone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label label5;
    }
}
