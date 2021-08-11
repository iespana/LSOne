using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Store.Views
{
    partial class CompanyInfoView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyInfoView));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addressField = new LSOne.Controls.AddressControl();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.cmbCurrency = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCurrenciesEdit = new LSOne.Controls.ContextButton();
            this.tbRegistrationNumber = new System.Windows.Forms.TextBox();
            this.lblRegistrationNumber = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tbRegistrationNumber);
            this.pnlBottom.Controls.Add(this.lblRegistrationNumber);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.cmbCurrency);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.btnCurrenciesEdit);
            this.pnlBottom.Controls.Add(this.addressField);
            this.pnlBottom.Controls.Add(this.label3);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // addressField
            // 
            this.addressField.BackColor = System.Drawing.Color.Transparent;
            this.addressField.DataModel = null;
            resources.ApplyResources(this.addressField, "addressField");
            this.addressField.Name = "addressField";
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
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
            this.cmbCurrency.SkipIDColumn = true;
            this.cmbCurrency.RequestData += new System.EventHandler(this.cmbCurrency_RequestData);
            this.cmbCurrency.SelectedDataChanged += new System.EventHandler(this.cmbCurrency_SelectedDataChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnCurrenciesEdit
            // 
            this.btnCurrenciesEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnCurrenciesEdit.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnCurrenciesEdit, "btnCurrenciesEdit");
            this.btnCurrenciesEdit.Name = "btnCurrenciesEdit";
            this.btnCurrenciesEdit.Click += new System.EventHandler(this.btnCurrenciesEdit_Click);
            // 
            // tbRegistrationNumber
            // 
            resources.ApplyResources(this.tbRegistrationNumber, "tbRegistrationNumber");
            this.tbRegistrationNumber.Name = "tbRegistrationNumber";
            // 
            // lblRegistrationNumber
            // 
            this.lblRegistrationNumber.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblRegistrationNumber, "lblRegistrationNumber");
            this.lblRegistrationNumber.Name = "lblRegistrationNumber";
            // 
            // CompanyInfoView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 50;
            this.Name = "CompanyInfoView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private AddressControl addressField;
        private TabControl tabSheetTabs;
        private DualDataComboBox cmbCurrency;
        private System.Windows.Forms.Label label1;
        private ContextButton btnCurrenciesEdit;
        private System.Windows.Forms.TextBox tbRegistrationNumber;
        private System.Windows.Forms.Label lblRegistrationNumber;
    }
}
