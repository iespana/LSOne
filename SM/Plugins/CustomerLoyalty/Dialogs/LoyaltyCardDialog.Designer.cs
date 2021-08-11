using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
	partial class LoyaltyCardDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoyaltyCardDialog));
            this.laCardsQty = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.laStartingPts = new System.Windows.Forms.Label();
            this.laCardType = new System.Windows.Forms.Label();
            this.cmbCardType = new System.Windows.Forms.ComboBox();
            this.dcbScheme = new DualDataComboBox();
            this.laScheme = new System.Windows.Forms.Label();
            this.dcbCustomer = new DualDataComboBox();
            this.laCustomer = new System.Windows.Forms.Label();
            this.btnAddScheme = new ContextButton();
            this.btnEditScheme = new ContextButton();
            this.ntbStartingPts = new NumericTextBox();
            this.ntbCardsQty = new NumericTextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // laCardsQty
            // 
            resources.ApplyResources(this.laCardsQty, "laCardsQty");
            this.laCardsQty.Name = "laCardsQty";
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
            // laStartingPts
            // 
            resources.ApplyResources(this.laStartingPts, "laStartingPts");
            this.laStartingPts.Name = "laStartingPts";
            // 
            // laCardType
            // 
            resources.ApplyResources(this.laCardType, "laCardType");
            this.laCardType.Name = "laCardType";
            // 
            // cmbCardType
            // 
            this.cmbCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCardType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCardType, "cmbCardType");
            this.cmbCardType.Name = "cmbCardType";
            this.cmbCardType.SelectedIndexChanged += new System.EventHandler(this.cmbCardType_SelectedIndexChanged);
            // 
            // dcbScheme
            // 
            this.dcbScheme.AddList = null;
            this.dcbScheme.AllowKeyboardSelection = false;
            resources.ApplyResources(this.dcbScheme, "dcbScheme");
            this.dcbScheme.MaxLength = 32767;
            this.dcbScheme.Name = "dcbScheme";
            this.dcbScheme.RemoveList = null;
            this.dcbScheme.RowHeight = ((short)(22));
            this.dcbScheme.SelectedData = null;
            this.dcbScheme.SelectedDataID = null;
            this.dcbScheme.SelectionList = null;
            this.dcbScheme.SkipIDColumn = false;
            this.dcbScheme.RequestData += new System.EventHandler(this.dcbScheme_RequestData);
            this.dcbScheme.SelectedDataChanged += new System.EventHandler(this.dcbScheme_SelectedDataChanged);
            this.dcbScheme.RequestClear += new System.EventHandler(this.DualDataComboBox_RequestClear);
            // 
            // laScheme
            // 
            this.laScheme.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laScheme, "laScheme");
            this.laScheme.Name = "laScheme";
            // 
            // dcbCustomer
            // 
            this.dcbCustomer.AddList = null;
            this.dcbCustomer.AllowKeyboardSelection = false;
            resources.ApplyResources(this.dcbCustomer, "dcbCustomer");
            this.dcbCustomer.MaxLength = 32767;
            this.dcbCustomer.Name = "dcbCustomer";
            this.dcbCustomer.RemoveList = null;
            this.dcbCustomer.RowHeight = ((short)(22));
            this.dcbCustomer.SelectedData = null;
            this.dcbCustomer.SelectedDataID = null;
            this.dcbCustomer.SelectionList = null;
            this.dcbCustomer.SkipIDColumn = false;
            this.dcbCustomer.DropDown += new DropDownEventHandler(this.dcbCustomer_DropDown);
            this.dcbCustomer.SelectedDataChanged += new System.EventHandler(this.dcbCustomer_SelectedDataChanged);
            this.dcbCustomer.RequestClear += new System.EventHandler(this.DualDataComboBox_RequestClear);
            // 
            // laCustomer
            // 
            this.laCustomer.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.laCustomer, "laCustomer");
            this.laCustomer.Name = "laCustomer";
            // 
            // btnAddScheme
            // 
            this.btnAddScheme.BackColor = System.Drawing.Color.Transparent;
            this.btnAddScheme.Context = ButtonType.Add;
            resources.ApplyResources(this.btnAddScheme, "btnAddScheme");
            this.btnAddScheme.Name = "btnAddScheme";
            this.btnAddScheme.Click += new System.EventHandler(this.btnAddScheme_Click);
            // 
            // btnEditScheme
            // 
            this.btnEditScheme.BackColor = System.Drawing.Color.Transparent;
            this.btnEditScheme.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditScheme, "btnEditScheme");
            this.btnEditScheme.Name = "btnEditScheme";
            this.btnEditScheme.Click += new System.EventHandler(this.btnEditScheme_Click);
            // 
            // ntbStartingPts
            // 
            this.ntbStartingPts.AcceptsTab = true;
            this.ntbStartingPts.AllowDecimal = false;
            this.ntbStartingPts.AllowNegative = false;
            this.ntbStartingPts.BackColor = System.Drawing.SystemColors.Window;
            this.ntbStartingPts.CultureInfo = null;
            this.ntbStartingPts.DecimalLetters = 2;
            this.ntbStartingPts.HasMinValue = true;
            resources.ApplyResources(this.ntbStartingPts, "ntbStartingPts");
            this.ntbStartingPts.MaxValue = 0D;
            this.ntbStartingPts.MinValue = 0D;
            this.ntbStartingPts.Name = "ntbStartingPts";
            this.ntbStartingPts.Value = 0D;
            this.ntbStartingPts.TextChanged += new System.EventHandler(this.ntbStartingPts_TextChanged);
            // 
            // ntbCardsQty
            // 
            this.ntbCardsQty.AcceptsTab = true;
            this.ntbCardsQty.AllowDecimal = false;
            this.ntbCardsQty.AllowNegative = false;
            this.ntbCardsQty.BackColor = System.Drawing.SystemColors.Window;
            this.ntbCardsQty.CultureInfo = null;
            this.ntbCardsQty.DecimalLetters = 0;
            this.ntbCardsQty.HasMinValue = true;
            resources.ApplyResources(this.ntbCardsQty, "ntbCardsQty");
            this.ntbCardsQty.MaxValue = 0D;
            this.ntbCardsQty.MinValue = 1D;
            this.ntbCardsQty.Name = "ntbCardsQty";
            this.ntbCardsQty.Value = 0D;
            // 
            // LoyaltyCardDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ntbCardsQty);
            this.Controls.Add(this.ntbStartingPts);
            this.Controls.Add(this.btnEditScheme);
            this.Controls.Add(this.btnAddScheme);
            this.Controls.Add(this.dcbCustomer);
            this.Controls.Add(this.laCustomer);
            this.Controls.Add(this.dcbScheme);
            this.Controls.Add(this.laScheme);
            this.Controls.Add(this.laCardType);
            this.Controls.Add(this.cmbCardType);
            this.Controls.Add(this.laStartingPts);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.laCardsQty);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "LoyaltyCardDialog";
            this.Controls.SetChildIndex(this.laCardsQty, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.laStartingPts, 0);
            this.Controls.SetChildIndex(this.cmbCardType, 0);
            this.Controls.SetChildIndex(this.laCardType, 0);
            this.Controls.SetChildIndex(this.laScheme, 0);
            this.Controls.SetChildIndex(this.dcbScheme, 0);
            this.Controls.SetChildIndex(this.laCustomer, 0);
            this.Controls.SetChildIndex(this.dcbCustomer, 0);
            this.Controls.SetChildIndex(this.btnAddScheme, 0);
            this.Controls.SetChildIndex(this.btnEditScheme, 0);
            this.Controls.SetChildIndex(this.ntbStartingPts, 0);
            this.Controls.SetChildIndex(this.ntbCardsQty, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label laCardsQty;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label laCardType;
		private System.Windows.Forms.ComboBox cmbCardType;
		private System.Windows.Forms.Label laStartingPts;
		private DualDataComboBox dcbScheme;
		private System.Windows.Forms.Label laScheme;
		private DualDataComboBox dcbCustomer;
		private System.Windows.Forms.Label laCustomer;
		private ContextButton btnAddScheme;
		private ContextButton btnEditScheme;
		private NumericTextBox ntbStartingPts;
		private NumericTextBox ntbCardsQty;
    }
}