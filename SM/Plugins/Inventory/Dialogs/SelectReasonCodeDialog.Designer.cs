namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class SelectReasonCodeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectReasonCodeDialog));
            this.btnAddReasonCode = new LSOne.Controls.ContextButton();
            this.btnEditReasonCode = new LSOne.Controls.ContextButton();
            this.cmbReason = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddReasonCode
            // 
            this.btnAddReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnAddReasonCode.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddReasonCode, "btnAddReasonCode");
            this.btnAddReasonCode.Name = "btnAddReasonCode";
            this.btnAddReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // btnEditReasonCode
            // 
            this.btnEditReasonCode.BackColor = System.Drawing.Color.Transparent;
            this.btnEditReasonCode.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditReasonCode, "btnEditReasonCode");
            this.btnEditReasonCode.Name = "btnEditReasonCode";
            this.btnEditReasonCode.Click += new System.EventHandler(this.btnAddEditReasonCode_Click);
            // 
            // cmbReason
            // 
            this.cmbReason.AddList = null;
            this.cmbReason.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbReason, "cmbReason");
            this.cmbReason.MaxLength = 32767;
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.NoChangeAllowed = false;
            this.cmbReason.OnlyDisplayID = false;
            this.cmbReason.RemoveList = null;
            this.cmbReason.RowHeight = ((short)(22));
            this.cmbReason.SecondaryData = null;
            this.cmbReason.SelectedData = null;
            this.cmbReason.SelectedDataID = null;
            this.cmbReason.SelectionList = null;
            this.cmbReason.SkipIDColumn = true;
            this.cmbReason.RequestData += new System.EventHandler(this.cmbReason_RequestData);
            this.cmbReason.SelectedDataChanged += new System.EventHandler(this.cmbReason_SelectedDataChanged);
            this.cmbReason.SelectedDataCleared += new System.EventHandler(this.cmbReason_SelectedDataCleared);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SelectReasonCodeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnAddReasonCode);
            this.Controls.Add(this.btnEditReasonCode);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.label8);
            this.HasHelp = true;
            this.Name = "SelectReasonCodeDialog";
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbReason, 0);
            this.Controls.SetChildIndex(this.btnEditReasonCode, 0);
            this.Controls.SetChildIndex(this.btnAddReasonCode, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButton btnAddReasonCode;
        private Controls.ContextButton btnEditReasonCode;
        private Controls.DualDataComboBox cmbReason;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}