using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemDiscountsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDiscountsPage));
            this.btnEditSalesMultilineDiscount = new LSOne.Controls.ContextButton();
            this.btnEditSalesLineDiscount = new LSOne.Controls.ContextButton();
            this.label29 = new System.Windows.Forms.Label();
            this.chkSalesTotalDiscount = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label27 = new System.Windows.Forms.Label();
            this.cmbSalesMultilineDiscount = new LSOne.Controls.DualDataComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbSalesLineDiscount = new LSOne.Controls.DualDataComboBox();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.SuspendLayout();
            // 
            // btnEditSalesMultilineDiscount
            // 
            this.btnEditSalesMultilineDiscount.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSalesMultilineDiscount, "btnEditSalesMultilineDiscount");
            this.btnEditSalesMultilineDiscount.Name = "btnEditSalesMultilineDiscount";
            this.btnEditSalesMultilineDiscount.Click += new System.EventHandler(this.btnEditSalesMultilineDiscount_Click);
            // 
            // btnEditSalesLineDiscount
            // 
            this.btnEditSalesLineDiscount.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditSalesLineDiscount, "btnEditSalesLineDiscount");
            this.btnEditSalesLineDiscount.Name = "btnEditSalesLineDiscount";
            this.btnEditSalesLineDiscount.Click += new System.EventHandler(this.btnEditSalesLineDiscount_Click);
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // chkSalesTotalDiscount
            // 
            resources.ApplyResources(this.chkSalesTotalDiscount, "chkSalesTotalDiscount");
            this.chkSalesTotalDiscount.Name = "chkSalesTotalDiscount";
            this.chkSalesTotalDiscount.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // cmbSalesMultilineDiscount
            // 
            this.cmbSalesMultilineDiscount.AddList = null;
            this.cmbSalesMultilineDiscount.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesMultilineDiscount, "cmbSalesMultilineDiscount");
            this.cmbSalesMultilineDiscount.MaxLength = 32767;
            this.cmbSalesMultilineDiscount.Name = "cmbSalesMultilineDiscount";
            this.cmbSalesMultilineDiscount.NoChangeAllowed = false;
            this.cmbSalesMultilineDiscount.OnlyDisplayID = false;
            this.cmbSalesMultilineDiscount.RemoveList = null;
            this.cmbSalesMultilineDiscount.RowHeight = ((short)(22));
            this.cmbSalesMultilineDiscount.SecondaryData = null;
            this.cmbSalesMultilineDiscount.SelectedData = null;
            this.cmbSalesMultilineDiscount.SelectedDataID = null;
            this.cmbSalesMultilineDiscount.SelectionList = null;
            this.cmbSalesMultilineDiscount.SkipIDColumn = true;
            this.cmbSalesMultilineDiscount.RequestData += new System.EventHandler(this.cmbSalesMultilineDiscount_RequestData);
            this.cmbSalesMultilineDiscount.SelectedDataChanged += new System.EventHandler(this.cmbSalesMultilineDiscount_SelectedDataChanged);
            this.cmbSalesMultilineDiscount.RequestClear += new System.EventHandler(this.cmbSalesMultilineDiscount_RequestClear);
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // cmbSalesLineDiscount
            // 
            this.cmbSalesLineDiscount.AddList = null;
            this.cmbSalesLineDiscount.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesLineDiscount, "cmbSalesLineDiscount");
            this.cmbSalesLineDiscount.MaxLength = 32767;
            this.cmbSalesLineDiscount.Name = "cmbSalesLineDiscount";
            this.cmbSalesLineDiscount.NoChangeAllowed = false;
            this.cmbSalesLineDiscount.OnlyDisplayID = false;
            this.cmbSalesLineDiscount.RemoveList = null;
            this.cmbSalesLineDiscount.RowHeight = ((short)(22));
            this.cmbSalesLineDiscount.SecondaryData = null;
            this.cmbSalesLineDiscount.SelectedData = null;
            this.cmbSalesLineDiscount.SelectedDataID = null;
            this.cmbSalesLineDiscount.SelectionList = null;
            this.cmbSalesLineDiscount.SkipIDColumn = true;
            this.cmbSalesLineDiscount.RequestData += new System.EventHandler(this.cmbSalesLineDiscount_RequestData);
            this.cmbSalesLineDiscount.SelectedDataChanged += new System.EventHandler(this.cmbSalesLineDiscount_SelectedDataChanged);
            this.cmbSalesLineDiscount.RequestClear += new System.EventHandler(this.cmbSalesLineDiscount_RequestClear);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // ItemDiscountsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabSheetTabs);
            this.Controls.Add(this.btnEditSalesMultilineDiscount);
            this.Controls.Add(this.btnEditSalesLineDiscount);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.chkSalesTotalDiscount);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.cmbSalesMultilineDiscount);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.cmbSalesLineDiscount);
            this.DoubleBuffered = true;
            this.Name = "ItemDiscountsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextButton btnEditSalesMultilineDiscount;
        private ContextButton btnEditSalesLineDiscount;
        private System.Windows.Forms.Label label29;
        private DoubleBufferedCheckbox chkSalesTotalDiscount;
        private System.Windows.Forms.Label label27;
        private DualDataComboBox cmbSalesMultilineDiscount;
        private System.Windows.Forms.Label label26;
        private DualDataComboBox cmbSalesLineDiscount;
        private TabControl tabSheetTabs;
    }
}
