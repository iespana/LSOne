namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    partial class NewButtonMenuDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewButtonMenuDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnEditStyle = new LSOne.Controls.ContextButton();
            this.chkMainMenu = new System.Windows.Forms.CheckBox();
            this.lblMainMenu = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.cmbStyle = new LSOne.Controls.DualDataComboBox();
            this.lblStyle = new System.Windows.Forms.Label();
            this.cmbOperation = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ntbRows = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ntbColumns = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkUseNavOperation = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAppliesTo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnEditStyle
            // 
            this.btnEditStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnEditStyle.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditStyle, "btnEditStyle");
            this.btnEditStyle.Name = "btnEditStyle";
            this.btnEditStyle.Click += new System.EventHandler(this.btnEditStyle_Click);
            // 
            // chkMainMenu
            // 
            resources.ApplyResources(this.chkMainMenu, "chkMainMenu");
            this.chkMainMenu.Name = "chkMainMenu";
            this.chkMainMenu.UseVisualStyleBackColor = true;
            // 
            // lblMainMenu
            // 
            this.lblMainMenu.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMainMenu, "lblMainMenu");
            this.lblMainMenu.Name = "lblMainMenu";
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_RequestClear);
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // cmbStyle
            // 
            this.cmbStyle.AddList = null;
            this.cmbStyle.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStyle, "cmbStyle");
            this.cmbStyle.MaxLength = 32767;
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.NoChangeAllowed = false;
            this.cmbStyle.OnlyDisplayID = false;
            this.cmbStyle.RemoveList = null;
            this.cmbStyle.RowHeight = ((short)(22));
            this.cmbStyle.SecondaryData = null;
            this.cmbStyle.SelectedData = null;
            this.cmbStyle.SelectedDataID = null;
            this.cmbStyle.SelectionList = null;
            this.cmbStyle.SkipIDColumn = true;
            this.cmbStyle.RequestData += new System.EventHandler(this.cmbStyle_RequestData);
            this.cmbStyle.SelectedDataChanged += new System.EventHandler(this.cmbStyle_SelectedDataChanged);
            this.cmbStyle.RequestClear += new System.EventHandler(this.cmbStyle_RequestClear);
            // 
            // lblStyle
            // 
            this.lblStyle.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblStyle, "lblStyle");
            this.lblStyle.Name = "lblStyle";
            // 
            // cmbOperation
            // 
            this.cmbOperation.AddList = null;
            this.cmbOperation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbOperation, "cmbOperation");
            this.cmbOperation.MaxLength = 32767;
            this.cmbOperation.Name = "cmbOperation";
            this.cmbOperation.NoChangeAllowed = false;
            this.cmbOperation.OnlyDisplayID = false;
            this.cmbOperation.RemoveList = null;
            this.cmbOperation.RowHeight = ((short)(22));
            this.cmbOperation.SecondaryData = null;
            this.cmbOperation.SelectedData = null;
            this.cmbOperation.SelectedDataID = null;
            this.cmbOperation.SelectionList = null;
            this.cmbOperation.SkipIDColumn = true;
            this.cmbOperation.RequestData += new System.EventHandler(this.cmbOperation_RequestData);
            this.cmbOperation.RequestClear += new System.EventHandler(this.cmbOperation_RequestClear);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // ntbRows
            // 
            this.ntbRows.AllowDecimal = false;
            this.ntbRows.AllowNegative = false;
            this.ntbRows.CultureInfo = null;
            this.ntbRows.DecimalLetters = 2;
            this.ntbRows.ForeColor = System.Drawing.Color.Black;
            this.ntbRows.HasMinValue = false;
            resources.ApplyResources(this.ntbRows, "ntbRows");
            this.ntbRows.MaxValue = 0D;
            this.ntbRows.MinValue = 0D;
            this.ntbRows.Name = "ntbRows";
            this.ntbRows.Value = 0D;
            this.ntbRows.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // ntbColumns
            // 
            this.ntbColumns.AllowDecimal = false;
            this.ntbColumns.AllowNegative = false;
            this.ntbColumns.CultureInfo = null;
            this.ntbColumns.DecimalLetters = 2;
            this.ntbColumns.ForeColor = System.Drawing.Color.Black;
            this.ntbColumns.HasMinValue = false;
            resources.ApplyResources(this.ntbColumns, "ntbColumns");
            this.ntbColumns.MaxValue = 0D;
            this.ntbColumns.MinValue = 0D;
            this.ntbColumns.Name = "ntbColumns";
            this.ntbColumns.Value = 0D;
            this.ntbColumns.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkUseNavOperation
            // 
            resources.ApplyResources(this.chkUseNavOperation, "chkUseNavOperation");
            this.chkUseNavOperation.BackColor = System.Drawing.Color.Transparent;
            this.chkUseNavOperation.Name = "chkUseNavOperation";
            this.chkUseNavOperation.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbAppliesTo
            // 
            this.cmbAppliesTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppliesTo.FormattingEnabled = true;
            this.cmbAppliesTo.Items.AddRange(new object[] {
            resources.GetString("cmbAppliesTo.Items"),
            resources.GetString("cmbAppliesTo.Items1"),
            resources.GetString("cmbAppliesTo.Items2"),
            resources.GetString("cmbAppliesTo.Items3")});
            resources.ApplyResources(this.cmbAppliesTo, "cmbAppliesTo");
            this.cmbAppliesTo.Name = "cmbAppliesTo";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // NewButtonMenuDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkUseNavOperation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAppliesTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnEditStyle);
            this.Controls.Add(this.chkMainMenu);
            this.Controls.Add(this.lblMainMenu);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.lblCopyFrom);
            this.Controls.Add(this.cmbStyle);
            this.Controls.Add(this.lblStyle);
            this.Controls.Add(this.cmbOperation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ntbRows);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ntbColumns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "NewButtonMenuDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbColumns, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.ntbRows, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbOperation, 0);
            this.Controls.SetChildIndex(this.lblStyle, 0);
            this.Controls.SetChildIndex(this.cmbStyle, 0);
            this.Controls.SetChildIndex(this.lblCopyFrom, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.lblMainMenu, 0);
            this.Controls.SetChildIndex(this.chkMainMenu, 0);
            this.Controls.SetChildIndex(this.btnEditStyle, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbAppliesTo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.chkUseNavOperation, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private LSOne.Controls.ContextButton btnEditStyle;
        private System.Windows.Forms.CheckBox chkMainMenu;
        private System.Windows.Forms.Label lblMainMenu;
        private LSOne.Controls.DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label lblCopyFrom;
        private LSOne.Controls.DualDataComboBox cmbStyle;
        private System.Windows.Forms.Label lblStyle;
        private LSOne.Controls.DualDataComboBox cmbOperation;
        private System.Windows.Forms.Label label5;
        private LSOne.Controls.NumericTextBox ntbRows;
        private System.Windows.Forms.Label label7;
        private LSOne.Controls.NumericTextBox ntbColumns;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkUseNavOperation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAppliesTo;
        private System.Windows.Forms.Label label4;
    }
}