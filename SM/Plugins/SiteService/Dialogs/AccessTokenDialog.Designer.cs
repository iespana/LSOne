namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    partial class AccessTokenDialog
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescrition = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblUser = new System.Windows.Forms.Label();
            this.tbSender = new System.Windows.Forms.TextBox();
            this.lblSender = new System.Windows.Forms.Label();
            this.lblStore = new System.Windows.Forms.Label();
            this.cmbUser = new LSOne.Controls.DualDataComboBox();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.grpToken = new System.Windows.Forms.GroupBox();
            this.txtCopyToken = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.tbSecurityToken = new System.Windows.Forms.TextBox();
            this.lblAccessToken = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.grpToken.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-3, 281);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 46);
            this.panel2.TabIndex = 11;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(361, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(442, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(159, 82);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(253, 20);
            this.tbDescription.TabIndex = 2;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // lblDescrition
            // 
            this.lblDescrition.Location = new System.Drawing.Point(4, 85);
            this.lblDescrition.Name = "lblDescrition";
            this.lblDescrition.Size = new System.Drawing.Size(151, 19);
            this.lblDescrition.TabIndex = 1;
            this.lblDescrition.Text = "Description:";
            this.lblDescrition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(2, 139);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(151, 19);
            this.lblUser.TabIndex = 5;
            this.lblUser.Text = "User:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbSender
            // 
            this.tbSender.Location = new System.Drawing.Point(159, 109);
            this.tbSender.Name = "tbSender";
            this.tbSender.Size = new System.Drawing.Size(253, 20);
            this.tbSender.TabIndex = 4;
            this.tbSender.TextChanged += new System.EventHandler(this.tbSender_TextChanged);
            // 
            // lblSender
            // 
            this.lblSender.Location = new System.Drawing.Point(4, 112);
            this.lblSender.Name = "lblSender";
            this.lblSender.Size = new System.Drawing.Size(151, 19);
            this.lblSender.TabIndex = 3;
            this.lblSender.Text = "Sender:";
            this.lblSender.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblStore
            // 
            this.lblStore.Location = new System.Drawing.Point(2, 167);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(151, 19);
            this.lblStore.TabIndex = 7;
            this.lblStore.Text = "Store:";
            this.lblStore.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbUser
            // 
            this.cmbUser.AddList = null;
            this.cmbUser.AllowKeyboardSelection = false;
            this.cmbUser.Location = new System.Drawing.Point(159, 136);
            this.cmbUser.MaxLength = 32767;
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.NoChangeAllowed = false;
            this.cmbUser.OnlyDisplayID = false;
            this.cmbUser.RemoveList = null;
            this.cmbUser.RowHeight = ((short)(22));
            this.cmbUser.SecondaryData = null;
            this.cmbUser.SelectedData = null;
            this.cmbUser.SelectedDataID = null;
            this.cmbUser.SelectionList = null;
            this.cmbUser.Size = new System.Drawing.Size(253, 21);
            this.cmbUser.SkipIDColumn = false;
            this.cmbUser.TabIndex = 6;
            this.cmbUser.RequestData += new System.EventHandler(this.cmbUser_RequestData);
            this.cmbUser.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbUser_DropDown);
            this.cmbUser.SelectedDataChanged += new System.EventHandler(this.cmbUser_SelectedDataChanged);
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            this.cmbStore.Location = new System.Drawing.Point(159, 164);
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.Size = new System.Drawing.Size(253, 21);
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.TabIndex = 8;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            // 
            // grpToken
            // 
            this.grpToken.Controls.Add(this.txtCopyToken);
            this.grpToken.Controls.Add(this.btnCopy);
            this.grpToken.Controls.Add(this.tbSecurityToken);
            this.grpToken.Controls.Add(this.lblAccessToken);
            this.grpToken.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpToken.Location = new System.Drawing.Point(13, 192);
            this.grpToken.Name = "grpToken";
            this.grpToken.Size = new System.Drawing.Size(504, 84);
            this.grpToken.TabIndex = 10;
            this.grpToken.TabStop = false;
            this.grpToken.Text = "Token";
            this.grpToken.Visible = false;
            // 
            // txtCopyToken
            // 
            this.txtCopyToken.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCopyToken.Location = new System.Drawing.Point(25, 22);
            this.txtCopyToken.Name = "txtCopyToken";
            this.txtCopyToken.Size = new System.Drawing.Size(457, 25);
            this.txtCopyToken.TabIndex = 0;
            this.txtCopyToken.Text = "Press the \'Copy\' button to copy the token to the clipboard";
            this.txtCopyToken.Visible = false;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(387, 48);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(76, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // tbSecurityToken
            // 
            this.tbSecurityToken.Enabled = false;
            this.tbSecurityToken.ForeColor = System.Drawing.SystemColors.Window;
            this.tbSecurityToken.Location = new System.Drawing.Point(182, 50);
            this.tbSecurityToken.Name = "tbSecurityToken";
            this.tbSecurityToken.Size = new System.Drawing.Size(199, 20);
            this.tbSecurityToken.TabIndex = 2;
            // 
            // lblAccessToken
            // 
            this.lblAccessToken.Location = new System.Drawing.Point(6, 48);
            this.lblAccessToken.Name = "lblAccessToken";
            this.lblAccessToken.Size = new System.Drawing.Size(170, 23);
            this.lblAccessToken.TabIndex = 1;
            this.lblAccessToken.Text = "Access token:";
            this.lblAccessToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(418, 163);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(76, 23);
            this.btnGenerate.TabIndex = 9;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // AccessTokenDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(529, 326);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.grpToken);
            this.Controls.Add(this.cmbStore);
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.tbSender);
            this.Controls.Add(this.lblSender);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescrition);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Header = "Enter token information";
            this.Name = "AccessTokenDialog";
            this.Text = " Integration framework token";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblDescrition, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.lblUser, 0);
            this.Controls.SetChildIndex(this.lblStore, 0);
            this.Controls.SetChildIndex(this.lblSender, 0);
            this.Controls.SetChildIndex(this.tbSender, 0);
            this.Controls.SetChildIndex(this.cmbUser, 0);
            this.Controls.SetChildIndex(this.cmbStore, 0);
            this.Controls.SetChildIndex(this.grpToken, 0);
            this.Controls.SetChildIndex(this.btnGenerate, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpToken.ResumeLayout(false);
            this.grpToken.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescrition;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox tbSender;
        private System.Windows.Forms.Label lblSender;
        private System.Windows.Forms.Label lblStore;
        private Controls.DualDataComboBox cmbStore;
        private Controls.DualDataComboBox cmbUser;
        private System.Windows.Forms.GroupBox grpToken;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox tbSecurityToken;
        private System.Windows.Forms.Label lblAccessToken;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label txtCopyToken;
    }
}