namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class EditUserProfileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUserProfileDialog));
            this.lblUserProfileProperty = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbUserProfileProperty = new LSOne.Controls.DualDataComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUserProfileProperty
            // 
            resources.ApplyResources(this.lblUserProfileProperty, "lblUserProfileProperty");
            this.lblUserProfileProperty.Name = "lblUserProfileProperty";
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
            // cmbUserProfileProperty
            // 
            this.cmbUserProfileProperty.AddList = null;
            this.cmbUserProfileProperty.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUserProfileProperty, "cmbUserProfileProperty");
            this.cmbUserProfileProperty.MaxLength = 32767;
            this.cmbUserProfileProperty.Name = "cmbUserProfileProperty";
            this.cmbUserProfileProperty.NoChangeAllowed = false;
            this.cmbUserProfileProperty.OnlyDisplayID = false;
            this.cmbUserProfileProperty.RemoveList = null;
            this.cmbUserProfileProperty.RowHeight = ((short)(22));
            this.cmbUserProfileProperty.SecondaryData = null;
            this.cmbUserProfileProperty.SelectedData = null;
            this.cmbUserProfileProperty.SelectedDataID = null;
            this.cmbUserProfileProperty.SelectionList = null;
            this.cmbUserProfileProperty.SkipIDColumn = true;
            this.cmbUserProfileProperty.RequestData += new System.EventHandler(this.cmbUserProfileProperty_RequestData);
            this.cmbUserProfileProperty.SelectedDataChanged += new System.EventHandler(this.cmbUserProfileProperty_SelectedDataChanged);
            this.cmbUserProfileProperty.RequestClear += new System.EventHandler(this.cmbUserProfileProperty_RequestClear);
            // 
            // EditUserProfileDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbUserProfileProperty);
            this.Controls.Add(this.lblUserProfileProperty);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.HasHelp = true;
            this.Name = "EditUserProfileDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblUserProfileProperty, 0);
            this.Controls.SetChildIndex(this.cmbUserProfileProperty, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblUserProfileProperty;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.DualDataComboBox cmbUserProfileProperty;
    }
}