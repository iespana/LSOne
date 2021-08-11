namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class AddUsersToUserProfileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUsersToUserProfileDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUsers = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
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
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbUsers
            // 
            this.cmbUsers.AddList = null;
            this.cmbUsers.AllowKeyboardSelection = false;
            this.cmbUsers.EnableTextBox = true;
            resources.ApplyResources(this.cmbUsers, "cmbUsers");
            this.cmbUsers.MaxLength = 32767;
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.NoChangeAllowed = false;
            this.cmbUsers.OnlyDisplayID = false;
            this.cmbUsers.RemoveList = null;
            this.cmbUsers.RowHeight = ((short)(22));
            this.cmbUsers.SecondaryData = null;
            this.cmbUsers.SelectedData = null;
            this.cmbUsers.SelectedDataID = null;
            this.cmbUsers.SelectionList = null;
            this.cmbUsers.ShowDropDownOnTyping = true;
            this.cmbUsers.SkipIDColumn = true;
            this.cmbUsers.RequestData += new System.EventHandler(this.cmbUsers_RequestData);
            this.cmbUsers.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbUsers_DropDown);
            this.cmbUsers.SelectedDataChanged += new System.EventHandler(this.cmbUsers_SelectedDataChanged);
            // 
            // AddUsersToUserProfileDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "AddUsersToUserProfileDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbUsers, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private Controls.DualDataComboBox cmbUsers;
    }
}