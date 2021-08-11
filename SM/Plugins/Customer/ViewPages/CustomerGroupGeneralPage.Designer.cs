using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    partial class CustomerGroupGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupGeneralPage));
            this.btnAddCategory = new LSOne.Controls.ContextButton();
            this.label1 = new System.Windows.Forms.Label();
            this.chkExclusive = new System.Windows.Forms.CheckBox();
            this.cmbCategories = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddCategory, "btnAddCategory");
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkExclusive
            // 
            resources.ApplyResources(this.chkExclusive, "chkExclusive");
            this.chkExclusive.Name = "chkExclusive";
            this.chkExclusive.UseVisualStyleBackColor = true;
            // 
            // cmbCategories
            // 
            this.cmbCategories.AddList = null;
            this.cmbCategories.AllowKeyboardSelection = false;
            this.cmbCategories.EnableTextBox = true;
            resources.ApplyResources(this.cmbCategories, "cmbCategories");
            this.cmbCategories.MaxLength = 32767;
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.OnlyDisplayID = false;
            this.cmbCategories.RemoveList = null;
            this.cmbCategories.RowHeight = ((short)(22));
            this.cmbCategories.SecondaryData = null;
            this.cmbCategories.SelectedData = null;
            this.cmbCategories.SelectedDataID = null;
            this.cmbCategories.SelectionList = null;
            this.cmbCategories.ShowDropDownOnTyping = true;
            this.cmbCategories.SkipIDColumn = true;
            this.cmbCategories.RequestData += new System.EventHandler(this.cmbCategories_RequestData);
            this.cmbCategories.RequestClear += new System.EventHandler(this.cmbCategories_RequestClear);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // CustomerGroupGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkExclusive);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.label7);
            this.Name = "CustomerGroupGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButton btnAddCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkExclusive;
        private DualDataComboBox cmbCategories;
        private System.Windows.Forms.Label label7;
    }
}
