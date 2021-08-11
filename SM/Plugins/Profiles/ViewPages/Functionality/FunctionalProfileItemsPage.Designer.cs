using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    partial class FunctionalProfileItemsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionalProfileItemsPage));
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSetDefaultImage = new System.Windows.Forms.Button();
            this.picDefaultImage = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbItemLineAggregate = new System.Windows.Forms.ComboBox();
            this.lblItemLineAggregate = new System.Windows.Forms.Label();
            this.chkAllowImagesInItemLookup = new System.Windows.Forms.CheckBox();
            this.chkRememberItemSearchMode = new System.Windows.Forms.CheckBox();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.chkSalesLineAggregate = new System.Windows.Forms.CheckBox();
            this.chkShowPrices = new System.Windows.Forms.CheckBox();
            this.chkDisplayVoidedItems = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultImage)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSetDefaultImage
            // 
            resources.ApplyResources(this.btnSetDefaultImage, "btnSetDefaultImage");
            this.btnSetDefaultImage.Name = "btnSetDefaultImage";
            this.btnSetDefaultImage.UseVisualStyleBackColor = true;
            this.btnSetDefaultImage.Click += new System.EventHandler(this.btnSetDefaultImage_Click);
            // 
            // picDefaultImage
            // 
            this.picDefaultImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.picDefaultImage, "picDefaultImage");
            this.picDefaultImage.Name = "picDefaultImage";
            this.picDefaultImage.TabStop = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbItemLineAggregate
            // 
            this.cmbItemLineAggregate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemLineAggregate.FormattingEnabled = true;
            this.cmbItemLineAggregate.Items.AddRange(new object[] {
            resources.GetString("cmbItemLineAggregate.Items"),
            resources.GetString("cmbItemLineAggregate.Items1")});
            resources.ApplyResources(this.cmbItemLineAggregate, "cmbItemLineAggregate");
            this.cmbItemLineAggregate.Name = "cmbItemLineAggregate";
            // 
            // lblItemLineAggregate
            // 
            this.lblItemLineAggregate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItemLineAggregate, "lblItemLineAggregate");
            this.lblItemLineAggregate.Name = "lblItemLineAggregate";
            // 
            // chkAllowImagesInItemLookup
            // 
            resources.ApplyResources(this.chkAllowImagesInItemLookup, "chkAllowImagesInItemLookup");
            this.chkAllowImagesInItemLookup.BackColor = System.Drawing.Color.Transparent;
            this.chkAllowImagesInItemLookup.Name = "chkAllowImagesInItemLookup";
            this.chkAllowImagesInItemLookup.UseVisualStyleBackColor = false;
            this.chkAllowImagesInItemLookup.CheckedChanged += new System.EventHandler(this.chkAllowImagesInItemLookup_CheckedChanged);
            // 
            // chkRememberItemSearchMode
            // 
            resources.ApplyResources(this.chkRememberItemSearchMode, "chkRememberItemSearchMode");
            this.chkRememberItemSearchMode.BackColor = System.Drawing.Color.Transparent;
            this.chkRememberItemSearchMode.Name = "chkRememberItemSearchMode";
            this.chkRememberItemSearchMode.UseVisualStyleBackColor = false;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.chkSalesLineAggregate);
            this.groupPanel1.Controls.Add(this.chkShowPrices);
            this.groupPanel1.Controls.Add(this.chkDisplayVoidedItems);
            this.groupPanel1.Controls.Add(this.chkRememberItemSearchMode);
            this.groupPanel1.Controls.Add(this.chkAllowImagesInItemLookup);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // chkSalesLineAggregate
            // 
            resources.ApplyResources(this.chkSalesLineAggregate, "chkSalesLineAggregate");
            this.chkSalesLineAggregate.BackColor = System.Drawing.Color.Transparent;
            this.chkSalesLineAggregate.Name = "chkSalesLineAggregate";
            this.chkSalesLineAggregate.UseVisualStyleBackColor = false;
            // 
            // chkShowPrices
            // 
            resources.ApplyResources(this.chkShowPrices, "chkShowPrices");
            this.chkShowPrices.BackColor = System.Drawing.Color.Transparent;
            this.chkShowPrices.Name = "chkShowPrices";
            this.chkShowPrices.UseVisualStyleBackColor = false;
            // 
            // chkDisplayVoidedItems
            // 
            resources.ApplyResources(this.chkDisplayVoidedItems, "chkDisplayVoidedItems");
            this.chkDisplayVoidedItems.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayVoidedItems.Name = "chkDisplayVoidedItems";
            this.chkDisplayVoidedItems.UseVisualStyleBackColor = false;
            // 
            // FunctionalProfileItemsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cmbItemLineAggregate);
            this.Controls.Add(this.lblItemLineAggregate);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSetDefaultImage);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.picDefaultImage);
            this.Controls.Add(this.label5);
            this.Name = "FunctionalProfileItemsPage";
            ((System.ComponentModel.ISupportInitialize)(this.picDefaultImage)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAllowImagesInItemLookup;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSetDefaultImage;
        private System.Windows.Forms.PictureBox picDefaultImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkRememberItemSearchMode;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.CheckBox chkDisplayVoidedItems;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkShowPrices;
        private System.Windows.Forms.ComboBox cmbItemLineAggregate;
        private System.Windows.Forms.Label lblItemLineAggregate;
        private System.Windows.Forms.CheckBox chkSalesLineAggregate;
    }
}
