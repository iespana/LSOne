using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class LinkedItemsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LinkedItemsDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAddUnit = new LSOne.Controls.ContextButton();
            this.lblUnit = new System.Windows.Forms.Label();
            this.cmbUnit = new LSOne.Controls.DualDataComboBox();
            this.lblLinkedItem = new System.Windows.Forms.Label();
            this.cmbLinkedItem = new LSOne.Controls.DualDataComboBox();
            this.ntbQuantity = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBlocked = new System.Windows.Forms.CheckBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.cmbVariant = new LSOne.Controls.DualDataComboBox();
            this.lblVariant = new System.Windows.Forms.Label();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnAddUnit
            // 
            this.btnAddUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnAddUnit.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddUnit, "btnAddUnit");
            this.btnAddUnit.Name = "btnAddUnit";
            this.btnAddUnit.Click += new System.EventHandler(this.btnAddUnit_Click);
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
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
            // lblLinkedItem
            // 
            this.lblLinkedItem.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblLinkedItem, "lblLinkedItem");
            this.lblLinkedItem.Name = "lblLinkedItem";
            // 
            // cmbLinkedItem
            // 
            this.cmbLinkedItem.AddList = null;
            this.cmbLinkedItem.AllowKeyboardSelection = false;
            this.cmbLinkedItem.EnableTextBox = true;
            resources.ApplyResources(this.cmbLinkedItem, "cmbLinkedItem");
            this.cmbLinkedItem.MaxLength = 32767;
            this.cmbLinkedItem.Name = "cmbLinkedItem";
            this.cmbLinkedItem.NoChangeAllowed = false;
            this.cmbLinkedItem.OnlyDisplayID = false;
            this.cmbLinkedItem.RemoveList = null;
            this.cmbLinkedItem.RowHeight = ((short)(22));
            this.cmbLinkedItem.SecondaryData = null;
            this.cmbLinkedItem.SelectedData = null;
            this.cmbLinkedItem.SelectedDataID = null;
            this.cmbLinkedItem.SelectionList = null;
            this.cmbLinkedItem.ShowDropDownOnTyping = true;
            this.cmbLinkedItem.SkipIDColumn = false;
            this.cmbLinkedItem.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbLinkedItem_DropDown);
            this.cmbLinkedItem.SelectedDataChanged += new System.EventHandler(this.cmbLinkedItem_SelectedDataChanged);
            // 
            // ntbQuantity
            // 
            this.ntbQuantity.AllowDecimal = true;
            this.ntbQuantity.AllowNegative = true;
            this.ntbQuantity.CultureInfo = null;
            this.ntbQuantity.DecimalLetters = 0;
            this.ntbQuantity.ForeColor = System.Drawing.Color.Black;
            this.ntbQuantity.HasMinValue = false;
            resources.ApplyResources(this.ntbQuantity, "ntbQuantity");
            this.ntbQuantity.MaxValue = 100000D;
            this.ntbQuantity.MinValue = 0D;
            this.ntbQuantity.Name = "ntbQuantity";
            this.ntbQuantity.Value = 0D;
            this.ntbQuantity.TextChanged += new System.EventHandler(this.ntbQuantity_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkBlocked
            // 
            resources.ApplyResources(this.chkBlocked, "chkBlocked");
            this.chkBlocked.BackColor = System.Drawing.Color.Transparent;
            this.chkBlocked.Name = "chkBlocked";
            this.chkBlocked.UseVisualStyleBackColor = false;
            this.chkBlocked.CheckedChanged += new System.EventHandler(this.chkBlocked_CheckedChanged);
            // 
            // lblColor
            // 
            this.lblColor.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblColor, "lblColor");
            this.lblColor.Name = "lblColor";
            // 
            // cmbVariant
            // 
            this.cmbVariant.AddList = null;
            this.cmbVariant.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVariant, "cmbVariant");
            this.cmbVariant.EnableTextBox = true;
            this.cmbVariant.MaxLength = 32767;
            this.cmbVariant.Name = "cmbVariant";
            this.cmbVariant.NoChangeAllowed = false;
            this.cmbVariant.OnlyDisplayID = false;
            this.cmbVariant.RemoveList = null;
            this.cmbVariant.RowHeight = ((short)(22));
            this.cmbVariant.SecondaryData = null;
            this.cmbVariant.SelectedData = null;
            this.cmbVariant.SelectedDataID = null;
            this.cmbVariant.SelectionList = null;
            this.cmbVariant.ShowDropDownOnTyping = true;
            this.cmbVariant.SkipIDColumn = false;
            this.cmbVariant.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbVariant_DropDown);
            this.cmbVariant.SelectedDataChanged += new System.EventHandler(this.cmbVariant_SelectedDataChanged);
            // 
            // lblVariant
            // 
            this.lblVariant.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblVariant, "lblVariant");
            this.lblVariant.Name = "lblVariant";
            // 
            // LinkedItemsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblVariant);
            this.Controls.Add(this.cmbVariant);
            this.Controls.Add(this.chkBlocked);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.ntbQuantity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLinkedItem);
            this.Controls.Add(this.cmbLinkedItem);
            this.Controls.Add(this.btnAddUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "LinkedItemsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cmbUnit, 0);
            this.Controls.SetChildIndex(this.lblUnit, 0);
            this.Controls.SetChildIndex(this.btnAddUnit, 0);
            this.Controls.SetChildIndex(this.cmbLinkedItem, 0);
            this.Controls.SetChildIndex(this.lblLinkedItem, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ntbQuantity, 0);
            this.Controls.SetChildIndex(this.lblColor, 0);
            this.Controls.SetChildIndex(this.chkBlocked, 0);
            this.Controls.SetChildIndex(this.cmbVariant, 0);
            this.Controls.SetChildIndex(this.lblVariant, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblLinkedItem;
        private DualDataComboBox cmbLinkedItem;
        private ContextButton btnAddUnit;
        private System.Windows.Forms.Label lblUnit;
        private DualDataComboBox cmbUnit;
        private NumericTextBox ntbQuantity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBlocked;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblVariant;
        private DualDataComboBox cmbVariant;
    }
}