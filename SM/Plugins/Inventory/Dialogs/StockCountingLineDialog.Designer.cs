using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    partial class StockCountingLineDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockCountingLineDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkCreateAnother = new LSOne.Controls.DoubleBufferedCheckbox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblItem = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbQty = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbBarcode = new System.Windows.Forms.TextBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.cmbItem = new LSOne.Controls.DualDataComboBox();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.lblFeedback = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.chkCreateAnother);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // chkCreateAnother
            // 
            resources.ApplyResources(this.chkCreateAnother, "chkCreateAnother");
            this.chkCreateAnother.Checked = true;
            this.chkCreateAnother.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateAnother.Name = "chkCreateAnother";
            this.chkCreateAnother.UseVisualStyleBackColor = true;
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // lblItem
            // 
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbQty
            // 
            this.ntbQty.AllowDecimal = true;
            this.ntbQty.AllowNegative = false;
            this.ntbQty.CultureInfo = null;
            this.ntbQty.DecimalLetters = 2;
            this.ntbQty.ForeColor = System.Drawing.Color.Black;
            this.ntbQty.HasMinValue = false;
            resources.ApplyResources(this.ntbQty, "ntbQty");
            this.ntbQty.MaxValue = 999999999999D;
            this.ntbQty.MinValue = 0D;
            this.ntbQty.Name = "ntbQty";
            this.ntbQty.Value = 0D;
            this.ntbQty.ValueChanged += new System.EventHandler(this.CheckEnabled);
            this.ntbQty.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbBarcode
            // 
            this.tbBarcode.AcceptsTab = true;
            resources.ApplyResources(this.tbBarcode, "tbBarcode");
            this.tbBarcode.Name = "tbBarcode";
            this.tbBarcode.Click += new System.EventHandler(this.tbBarcode_Click);
            this.tbBarcode.Enter += new System.EventHandler(this.tbBarcode_Enter);
            this.tbBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyDown);
            this.tbBarcode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbBarcode_KeyUp);
            this.tbBarcode.Leave += new System.EventHandler(this.tbBarcode_Leave);
            // 
            // lblBarcode
            // 
            this.lblBarcode.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarcode, "lblBarcode");
            this.lblBarcode.Name = "lblBarcode";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
            this.cmbVariant.HorizontalScrollbar = true;
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.ReceiveKeyboardEvents = true;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.SkipIDColumn = false;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // cmbUnit
            // 
            this.cmbUnit.AddList = null;
            this.cmbUnit.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbUnit, "cmbUnit");
            this.cmbUnit.MaxLength = 32767;
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.NoChangeAllowed = false;
            this.cmbUnit.OnlyDisplayID = false;
            this.cmbUnit.RemoveList = null;
            this.cmbUnit.RowHeight = ((short)(22));
            this.cmbUnit.SecondaryData = null;
            this.cmbUnit.SelectedData = null;
            this.cmbUnit.SelectedDataID = null;
            this.cmbUnit.SelectionList = null;
            this.cmbUnit.SkipIDColumn = true;
            this.cmbUnit.RequestData += new System.EventHandler(this.cmbUnit_RequestData);
            this.cmbUnit.SelectedDataChanged += new System.EventHandler(this.cmbUnit_SelectedDataChanged);
            // 
            // cmbItem
            // 
            this.cmbItem.AddList = null;
            this.cmbItem.AllowKeyboardSelection = false;
            this.cmbItem.EnableTextBox = true;
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.NoChangeAllowed = false;
            this.cmbItem.OnlyDisplayID = false;
            this.cmbItem.RemoveList = null;
            this.cmbItem.RowHeight = ((short)(22));
            this.cmbItem.SecondaryData = null;
            this.cmbItem.SelectedData = null;
            this.cmbItem.SelectedDataID = null;
            this.cmbItem.SelectionList = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = true;
            this.cmbItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            this.cmbItem.Leave += new System.EventHandler(this.cmbItem_Leave);
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.lblFeedback);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblFeedback
            // 
            this.lblFeedback.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblFeedback, "lblFeedback");
            this.lblFeedback.Name = "lblFeedback";
            // 
            // StockCountingLineDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.tbBarcode);
            this.Controls.Add(this.ntbQty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.HelpButton = true;
            this.KeyPreview = true;
            this.Name = "StockCountingLineDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbItem, 0);
            this.Controls.SetChildIndex(this.lblItem, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ntbQty, 0);
            this.Controls.SetChildIndex(this.tbBarcode, 0);
            this.Controls.SetChildIndex(this.lblBarcode, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label lblItem;
        private DualDataComboBox cmbItem;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbUnit;
        private NumericTextBox ntbQty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox tbBarcode;
        private System.Windows.Forms.Button btnOK;
        private DualDataComboBox cmbVariant;
        private System.Windows.Forms.Label label3;
        private DoubleBufferedCheckbox chkCreateAnother;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblFeedback;
    }
}