using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.Dialogs
{
    partial class TenderdeclarationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TenderdeclarationDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTotalAmountCounted = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.paCountObjects = new LSOne.Controls.GroupPanel();
            this.ntbCountedQuantity = new LSOne.Controls.NumericTextBox();
            this.lvCountResult = new LSOne.Controls.ExtendedListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblQtyTotal = new System.Windows.Forms.Label();
            this.cmbDenominator = new LSOne.Controls.DualDataComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblGroupHeader = new System.Windows.Forms.Label();
            this.cmbPayments = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbTerminal = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            this.paCountObjects.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblTotalAmountCounted);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lblTotalAmountCounted
            // 
            resources.ApplyResources(this.lblTotalAmountCounted, "lblTotalAmountCounted");
            this.lblTotalAmountCounted.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalAmountCounted.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblTotalAmountCounted.Name = "lblTotalAmountCounted";
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
            // paCountObjects
            // 
            resources.ApplyResources(this.paCountObjects, "paCountObjects");
            this.paCountObjects.Controls.Add(this.ntbCountedQuantity);
            this.paCountObjects.Controls.Add(this.lvCountResult);
            this.paCountObjects.Controls.Add(this.btnRemove);
            this.paCountObjects.Controls.Add(this.btnAdd);
            this.paCountObjects.Controls.Add(this.lblQtyTotal);
            this.paCountObjects.Controls.Add(this.cmbDenominator);
            this.paCountObjects.Controls.Add(this.label3);
            this.paCountObjects.Controls.Add(this.cmbCurrency);
            this.paCountObjects.Controls.Add(this.label2);
            this.paCountObjects.Controls.Add(this.lblGroupHeader);
            this.paCountObjects.Name = "paCountObjects";
            // 
            // ntbCountedQuantity
            // 
            this.ntbCountedQuantity.AllowDecimal = true;
            this.ntbCountedQuantity.AllowNegative = true;
            this.ntbCountedQuantity.CultureInfo = null;
            this.ntbCountedQuantity.DecimalLetters = 0;
            this.ntbCountedQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbCountedQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbCountedQuantity, "ntbCountedQuantity");
            this.ntbCountedQuantity.MaxValue = 100000D;
            this.ntbCountedQuantity.MinValue = 0D;
            this.ntbCountedQuantity.Name = "ntbCountedQuantity";
            this.ntbCountedQuantity.Value = 0D;
            this.ntbCountedQuantity.TextChanged += new System.EventHandler(this.ntbCountedQuantity_TextChanged);
            // 
            // lvCountResult
            // 
            resources.ApplyResources(this.lvCountResult, "lvCountResult");
            this.lvCountResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader6,
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4});
            this.lvCountResult.FullRowSelect = true;
            this.lvCountResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCountResult.HideSelection = false;
            this.lvCountResult.LockDrawing = false;
            this.lvCountResult.MultiSelect = false;
            this.lvCountResult.Name = "lvCountResult";
            this.lvCountResult.SortColumn = -1;
            this.lvCountResult.SortedBackwards = false;
            this.lvCountResult.UseCompatibleStateImageBehavior = false;
            this.lvCountResult.UseEveryOtherRowColoring = true;
            this.lvCountResult.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAddFast_Click);
            // 
            // lblQtyTotal
            // 
            this.lblQtyTotal.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblQtyTotal, "lblQtyTotal");
            this.lblQtyTotal.Name = "lblQtyTotal";
            // 
            // cmbDenominator
            // 
            this.cmbDenominator.AddList = null;
            this.cmbDenominator.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDenominator, "cmbDenominator");
            this.cmbDenominator.MaxLength = 32767;
            this.cmbDenominator.Name = "cmbDenominator";
            this.cmbDenominator.NoChangeAllowed = false;
            this.cmbDenominator.OnlyDisplayID = false;
            this.cmbDenominator.RemoveList = null;
            this.cmbDenominator.RowHeight = ((short)(22));
            this.cmbDenominator.SecondaryData = null;
            this.cmbDenominator.SelectedData = null;
            this.cmbDenominator.SelectedDataID = null;
            this.cmbDenominator.SelectionList = null;
            this.cmbDenominator.SkipIDColumn = false;
            this.cmbDenominator.RequestData += new System.EventHandler(this.cmbDenominator_RequestData);
            this.cmbDenominator.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbDenominator_FormatData);
            this.cmbDenominator.SelectedDataChanged += new System.EventHandler(this.cmbDenominator_SelectedDataChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbCurrency
            // 
            this.cmbCurrency.AddList = null;
            this.cmbCurrency.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCurrency, "cmbCurrency");
            this.cmbCurrency.MaxLength = 32767;
            this.cmbCurrency.Name = "cmbCurrency";
            this.cmbCurrency.NoChangeAllowed = false;
            this.cmbCurrency.OnlyDisplayID = false;
            this.cmbCurrency.RemoveList = null;
            this.cmbCurrency.RowHeight = ((short)(22));
            this.cmbCurrency.SecondaryData = null;
            this.cmbCurrency.SelectedData = null;
            this.cmbCurrency.SelectedDataID = null;
            this.cmbCurrency.SelectionList = null;
            this.cmbCurrency.SkipIDColumn = false;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.cmbCurrency_SelectedDataChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblGroupHeader
            // 
            resources.ApplyResources(this.lblGroupHeader, "lblGroupHeader");
            this.lblGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblGroupHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.lblGroupHeader.Name = "lblGroupHeader";
            // 
            // cmbPayments
            // 
            this.cmbPayments.AddList = null;
            this.cmbPayments.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbPayments, "cmbPayments");
            this.cmbPayments.MaxLength = 32767;
            this.cmbPayments.Name = "cmbPayments";
            this.cmbPayments.NoChangeAllowed = false;
            this.cmbPayments.OnlyDisplayID = false;
            this.cmbPayments.RemoveList = null;
            this.cmbPayments.RowHeight = ((short)(22));
            this.cmbPayments.SecondaryData = null;
            this.cmbPayments.SelectedData = null;
            this.cmbPayments.SelectedDataID = null;
            this.cmbPayments.SelectionList = null;
            this.cmbPayments.SkipIDColumn = false;
            this.cmbPayments.RequestData += new System.EventHandler(this.cmbPayments_RequestData);
            this.cmbPayments.SelectedDataChanged += new System.EventHandler(this.cmbPayments_SelectedDataChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbTerminal
            // 
            this.cmbTerminal.AddList = null;
            this.cmbTerminal.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTerminal, "cmbTerminal");
            this.cmbTerminal.MaxLength = 32767;
            this.cmbTerminal.Name = "cmbTerminal";
            this.cmbTerminal.NoChangeAllowed = false;
            this.cmbTerminal.OnlyDisplayID = false;
            this.cmbTerminal.RemoveList = null;
            this.cmbTerminal.RowHeight = ((short)(22));
            this.cmbTerminal.SecondaryData = null;
            this.cmbTerminal.SelectedData = null;
            this.cmbTerminal.SelectedDataID = null;
            this.cmbTerminal.SelectionList = null;
            this.cmbTerminal.SkipIDColumn = false;
            this.cmbTerminal.RequestData += new System.EventHandler(this.cmbTerminal_RequestData);
            this.cmbTerminal.SelectedDataChanged += new System.EventHandler(this.cmbTerminal_SelectedDataChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
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
            this.cmbStore.SkipIDColumn = false;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            this.cmbStore.SelectedDataChanged += new System.EventHandler(this.cmbStore_SelectedDataChanged);
            this.cmbStore.TextChanged += new System.EventHandler(this.cmbStore_TextChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.cmbPayments);
            this.groupPanel1.Controls.Add(this.label6);
            this.groupPanel1.Controls.Add(this.label11);
            this.groupPanel1.Controls.Add(this.cmbTerminal);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.label5);
            this.groupPanel1.Controls.Add(this.cmbStore);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label11.Name = "label11";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // TenderdeclarationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.paCountObjects);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "TenderdeclarationDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.paCountObjects, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.paCountObjects.ResumeLayout(false);
            this.paCountObjects.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private GroupPanel paCountObjects;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblQtyTotal;
        private DualDataComboBox cmbDenominator;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblGroupHeader;
        private DualDataComboBox cmbPayments;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbTerminal;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label label5;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label label11;
        private ExtendedListView lvCountResult;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label lblTotalAmountCounted;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private NumericTextBox ntbCountedQuantity;
    }
}