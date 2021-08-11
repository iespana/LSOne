namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class EditItemRoutingConnectionDialog
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbInclude = new System.Windows.Forms.ComboBox();
            this.lblInclude = new System.Windows.Forms.Label();
            this.cmbConnection = new LSOne.Controls.DualDataComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblConnection = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(-4, 213);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 46);
            this.panel2.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(289, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(370, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbInclude
            // 
            this.cmbInclude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInclude.FormattingEnabled = true;
            this.cmbInclude.Items.AddRange(new object[] {
            "Include",
            "Exclude"});
            this.cmbInclude.Location = new System.Drawing.Point(170, 154);
            this.cmbInclude.Name = "cmbInclude";
            this.cmbInclude.Size = new System.Drawing.Size(237, 21);
            this.cmbInclude.TabIndex = 6;
            this.cmbInclude.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblInclude
            // 
            this.lblInclude.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblInclude.Location = new System.Drawing.Point(17, 157);
            this.lblInclude.Name = "lblInclude";
            this.lblInclude.Size = new System.Drawing.Size(151, 19);
            this.lblInclude.TabIndex = 5;
            this.lblInclude.Text = "Include/exclude:";
            this.lblInclude.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbConnection
            // 
            this.cmbConnection.AddList = null;
            this.cmbConnection.AllowKeyboardSelection = false;
            this.cmbConnection.Location = new System.Drawing.Point(170, 127);
            this.cmbConnection.MaxLength = 32767;
            this.cmbConnection.Name = "cmbConnection";
            this.cmbConnection.NoChangeAllowed = false;
            this.cmbConnection.OnlyDisplayID = false;
            this.cmbConnection.RemoveList = null;
            this.cmbConnection.RowHeight = ((short)(22));
            this.cmbConnection.SecondaryData = null;
            this.cmbConnection.SelectedData = null;
            this.cmbConnection.SelectedDataID = null;
            this.cmbConnection.SelectionList = null;
            this.cmbConnection.Size = new System.Drawing.Size(237, 21);
            this.cmbConnection.SkipIDColumn = false;
            this.cmbConnection.TabIndex = 4;
            this.cmbConnection.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbConnection_DropDown);
            this.cmbConnection.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Enabled = false;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "Retail group",
            "Item",
            "Special group"});
            this.cmbType.Location = new System.Drawing.Point(170, 100);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(237, 21);
            this.cmbType.TabIndex = 2;
            // 
            // lblConnection
            // 
            this.lblConnection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConnection.Location = new System.Drawing.Point(17, 130);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(151, 19);
            this.lblConnection.TabIndex = 3;
            this.lblConnection.Text = "Connection:";
            this.lblConnection.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(17, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Type:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EditItemRoutingConnectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 255);
            this.Controls.Add(this.cmbInclude);
            this.Controls.Add(this.lblInclude);
            this.Controls.Add(this.cmbConnection);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Header = "Edit the selected item routing information";
            this.Name = "EditItemRoutingConnectionDialog";
            this.Text = "Edit item routing";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblConnection, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbConnection, 0);
            this.Controls.SetChildIndex(this.lblInclude, 0);
            this.Controls.SetChildIndex(this.cmbInclude, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbInclude;
        private System.Windows.Forms.Label lblInclude;
        private LSOne.Controls.DualDataComboBox cmbConnection;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Label label3;
    }
}