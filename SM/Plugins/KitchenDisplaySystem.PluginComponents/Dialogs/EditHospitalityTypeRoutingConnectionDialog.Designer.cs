namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class EditHospitalityTypeRoutingConnectionDialog
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
            this.cmbHospitalityType = new LSOne.Controls.DualDataComboBox();
            this.lblConnection = new System.Windows.Forms.Label();
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
            this.panel2.Location = new System.Drawing.Point(-4, 186);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(460, 46);
            this.panel2.TabIndex = 5;
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
            this.cmbInclude.Location = new System.Drawing.Point(162, 127);
            this.cmbInclude.Name = "cmbInclude";
            this.cmbInclude.Size = new System.Drawing.Size(266, 21);
            this.cmbInclude.TabIndex = 4;
            this.cmbInclude.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblInclude
            // 
            this.lblInclude.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblInclude.Location = new System.Drawing.Point(5, 130);
            this.lblInclude.Name = "lblInclude";
            this.lblInclude.Size = new System.Drawing.Size(151, 19);
            this.lblInclude.TabIndex = 3;
            this.lblInclude.Text = "Include/exclude:";
            this.lblInclude.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbHospitalityType
            // 
            this.cmbHospitalityType.AddList = null;
            this.cmbHospitalityType.AllowKeyboardSelection = false;
            this.cmbHospitalityType.Location = new System.Drawing.Point(162, 100);
            this.cmbHospitalityType.MaxLength = 32767;
            this.cmbHospitalityType.Name = "cmbHospitalityType";
            this.cmbHospitalityType.NoChangeAllowed = false;
            this.cmbHospitalityType.OnlyDisplayID = false;
            this.cmbHospitalityType.RemoveList = null;
            this.cmbHospitalityType.RowHeight = ((short)(22));
            this.cmbHospitalityType.SecondaryData = null;
            this.cmbHospitalityType.SelectedData = null;
            this.cmbHospitalityType.SelectedDataID = null;
            this.cmbHospitalityType.SelectionList = null;
            this.cmbHospitalityType.Size = new System.Drawing.Size(266, 21);
            this.cmbHospitalityType.SkipIDColumn = true;
            this.cmbHospitalityType.TabIndex = 2;
            this.cmbHospitalityType.RequestData += new System.EventHandler(this.cmbHospitalityType_RequestData);
            this.cmbHospitalityType.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblConnection
            // 
            this.lblConnection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConnection.Location = new System.Drawing.Point(5, 103);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(151, 19);
            this.lblConnection.TabIndex = 1;
            this.lblConnection.Text = "Hospitailty type:";
            this.lblConnection.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EditHospitalityTypeRoutingConnectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 228);
            this.Controls.Add(this.cmbInclude);
            this.Controls.Add(this.lblInclude);
            this.Controls.Add(this.cmbHospitalityType);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Header = "Edit the selected hospitality type routing information";
            this.Name = "EditHospitalityTypeRoutingConnectionDialog";
            this.Text = "Edit hospitality type routing";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblConnection, 0);
            this.Controls.SetChildIndex(this.cmbHospitalityType, 0);
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
        private LSOne.Controls.DualDataComboBox cmbHospitalityType;
        private System.Windows.Forms.Label lblConnection;
    }
}