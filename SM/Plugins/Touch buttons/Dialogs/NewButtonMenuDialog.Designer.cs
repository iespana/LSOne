using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.ViewPlugins.TouchButtons.Controls;
using Style = LSOne.DataLayer.BusinessObjects.TouchButtons.Style;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
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
            this.pnlBottom = new LSOne.Controls.DialogBottomPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ntbRows = new LSOne.Controls.NumericTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ntbColumns = new LSOne.Controls.NumericTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbOperation = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStyle = new LSOne.Controls.DualDataComboBox();
            this.lblStyle = new System.Windows.Forms.Label();
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.lblMainMenu = new System.Windows.Forms.Label();
            this.chkMainMenu = new System.Windows.Forms.CheckBox();
            this.btnEditStyle = new LSOne.Controls.ContextButton();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Name = "pnlBottom";
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            this.ntbRows.MaxValue = 99999999D;
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
            this.ntbColumns.MaxValue = 99999999D;
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
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            this.cmbCopyFrom.RequestClear += new System.EventHandler(this.cmbCopyFrom_RequestClear);
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // lblMainMenu
            // 
            this.lblMainMenu.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblMainMenu, "lblMainMenu");
            this.lblMainMenu.Name = "lblMainMenu";
            // 
            // chkMainMenu
            // 
            resources.ApplyResources(this.chkMainMenu, "chkMainMenu");
            this.chkMainMenu.Name = "chkMainMenu";
            this.chkMainMenu.UseVisualStyleBackColor = true;
            // 
            // btnEditStyle
            // 
            this.btnEditStyle.BackColor = System.Drawing.Color.Transparent;
            this.btnEditStyle.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditStyle, "btnEditStyle");
            this.btnEditStyle.Name = "btnEditStyle";
            this.btnEditStyle.Click += new System.EventHandler(this.btnEditStyle_Click);
            // 
            // NewButtonMenuDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
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
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ntbColumns);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pnlBottom);
            this.HasHelp = true;
            this.Name = "NewButtonMenuDialog";
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.ntbColumns, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
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
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DialogBottomPanel pnlBottom;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private NumericTextBox ntbRows;
        private System.Windows.Forms.Label label7;
        private NumericTextBox ntbColumns;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbOperation;
        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label lblCopyFrom;
        private DualDataComboBox cmbStyle;
        private System.Windows.Forms.Label lblStyle;
        private System.Windows.Forms.CheckBox chkMainMenu;
        private System.Windows.Forms.Label lblMainMenu;
        private ContextButton btnEditStyle;
    }
}