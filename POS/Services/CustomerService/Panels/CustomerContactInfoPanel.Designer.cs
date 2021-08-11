using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Panels
{
    partial class CustomerContactInfoPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerContactInfoPanel));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtDateOfBirth = new System.Windows.Forms.DateTimePicker();
            this.chkIsCashCustomer = new LSOne.Controls.TouchCheckBox();
            this.lblUserIsDisabled = new System.Windows.Forms.Label();
            this.lblSalesTaxGroup = new System.Windows.Forms.Label();
            this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.Icon = ((System.Drawing.Image)(resources.GetObject("touchDialogBanner1.Icon")));
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbGender, "cmbGender");
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            resources.GetString("cmbGender.Items"),
            resources.GetString("cmbGender.Items1"),
            resources.GetString("cmbGender.Items2")});
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.SelectedIndexChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // dtDateOfBirth
            // 
            resources.ApplyResources(this.dtDateOfBirth, "dtDateOfBirth");
            this.dtDateOfBirth.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dtDateOfBirth.Name = "dtDateOfBirth";
            this.dtDateOfBirth.ShowCheckBox = true;
            this.dtDateOfBirth.ValueChanged += new System.EventHandler(this.ClearErrorProvider);
            // 
            // chkIsCashCustomer
            // 
            this.chkIsCashCustomer.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.chkIsCashCustomer, "chkIsCashCustomer");
            this.chkIsCashCustomer.Name = "chkIsCashCustomer";
            // 
            // lblUserIsDisabled
            // 
            this.lblUserIsDisabled.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblUserIsDisabled, "lblUserIsDisabled");
            this.lblUserIsDisabled.Name = "lblUserIsDisabled";
            // 
            // lblSalesTaxGroup
            // 
            resources.ApplyResources(this.lblSalesTaxGroup, "lblSalesTaxGroup");
            this.lblSalesTaxGroup.Name = "lblSalesTaxGroup";
            // 
            // cmbSalesTaxGroup
            // 
            this.cmbSalesTaxGroup.AddList = null;
            this.cmbSalesTaxGroup.AllowKeyboardSelection = false;
            this.cmbSalesTaxGroup.EnableTextBox = true;
            resources.ApplyResources(this.cmbSalesTaxGroup, "cmbSalesTaxGroup");
            this.cmbSalesTaxGroup.MaxLength = 32767;
            this.cmbSalesTaxGroup.Name = "cmbSalesTaxGroup";
            this.cmbSalesTaxGroup.NoChangeAllowed = false;
            this.cmbSalesTaxGroup.OnlyDisplayID = false;
            this.cmbSalesTaxGroup.RemoveList = null;
            this.cmbSalesTaxGroup.RowHeight = ((short)(22));
            this.cmbSalesTaxGroup.SecondaryData = null;
            this.cmbSalesTaxGroup.SelectedData = null;
            this.cmbSalesTaxGroup.SelectedDataID = null;
            this.cmbSalesTaxGroup.SelectionList = null;
            this.cmbSalesTaxGroup.ShowDropDownOnTyping = true;
            this.cmbSalesTaxGroup.SkipIDColumn = true;
            this.cmbSalesTaxGroup.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbSalesTaxGroup_DropDown);
            // 
            // CustomerContactInfoPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbSalesTaxGroup);
            this.Controls.Add(this.lblSalesTaxGroup);
            this.Controls.Add(this.chkIsCashCustomer);
            this.Controls.Add(this.lblUserIsDisabled);
            this.Controls.Add(this.dtDateOfBirth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.touchDialogBanner1);
            this.DoubleBuffered = true;
            this.Name = "CustomerContactInfoPanel";
            this.Load += new System.EventHandler(this.CustomerContactInfoPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private TouchDialogBanner touchDialogBanner1;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtDateOfBirth;
        private TouchCheckBox chkIsCashCustomer;
        private System.Windows.Forms.Label lblUserIsDisabled;
        private System.Windows.Forms.Label lblSalesTaxGroup;
        private DualDataComboBox cmbSalesTaxGroup;
    }
}
