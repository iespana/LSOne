using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.Views
{
    partial class CustomersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersView));
            this.customerDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvCustomers = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colCashCustomer = new LSOne.Controls.Columns.Column();
            this.colSalesTaxGroup = new LSOne.Controls.Columns.Column();
            this.colPriceGroup = new LSOne.Controls.Columns.Column();
            this.colLineDiscountGroup = new LSOne.Controls.Columns.Column();
            this.colTotalDiscountGroup = new LSOne.Controls.Columns.Column();
            this.colCreditLimit = new LSOne.Controls.Columns.Column();
            this.colBlockingStatus = new LSOne.Controls.Columns.Column();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.colTaxExempt = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar1);
            this.pnlBottom.Controls.Add(this.lvCustomers);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.customerDataScroll);
            // 
            // customerDataScroll
            // 
            resources.ApplyResources(this.customerDataScroll, "customerDataScroll");
            this.customerDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.customerDataScroll.Name = "customerDataScroll";
            this.customerDataScroll.PageSize = 0;
            this.customerDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvCustomers
            // 
            resources.ApplyResources(this.lvCustomers, "lvCustomers");
            this.lvCustomers.BuddyControl = null;
            this.lvCustomers.Columns.Add(this.colID);
            this.lvCustomers.Columns.Add(this.colName);
            this.lvCustomers.Columns.Add(this.colCashCustomer);
            this.lvCustomers.Columns.Add(this.colTaxExempt);
            this.lvCustomers.Columns.Add(this.colSalesTaxGroup);
            this.lvCustomers.Columns.Add(this.colPriceGroup);
            this.lvCustomers.Columns.Add(this.colLineDiscountGroup);
            this.lvCustomers.Columns.Add(this.colTotalDiscountGroup);
            this.lvCustomers.Columns.Add(this.colCreditLimit);
            this.lvCustomers.Columns.Add(this.colBlockingStatus);
            this.lvCustomers.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomers.DefaultRowHeight = ((short)(22));
            this.lvCustomers.DimSelectionWhenDisabled = true;
            this.lvCustomers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCustomers.HeaderHeight = ((short)(25));
            this.lvCustomers.HorizontalScrollbar = true;
            this.lvCustomers.Name = "lvCustomers";
            this.lvCustomers.OddRowColor = System.Drawing.Color.White;
            this.lvCustomers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCustomers.SecondarySortColumn = ((short)(-1));
            this.lvCustomers.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCustomers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomers.SortSetting = "0:1";
            this.lvCustomers.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvCustomers_HeaderClicked);
            this.lvCustomers.SelectionChanged += new System.EventHandler(this.lvCustomers_SelectionChanged);
            this.lvCustomers.DoubleClick += new System.EventHandler(this.lvCustomers_DoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Tag = null;
            this.colName.Width = ((short)(50));
            // 
            // colCashCustomer
            // 
            this.colCashCustomer.AutoSize = true;
            this.colCashCustomer.DefaultStyle = null;
            resources.ApplyResources(this.colCashCustomer, "colCashCustomer");
            this.colCashCustomer.MaximumWidth = ((short)(0));
            this.colCashCustomer.MinimumWidth = ((short)(10));
            this.colCashCustomer.SecondarySortColumn = ((short)(-1));
            this.colCashCustomer.Tag = null;
            this.colCashCustomer.Width = ((short)(50));
            // 
            // colSalesTaxGroup
            // 
            this.colSalesTaxGroup.AutoSize = true;
            this.colSalesTaxGroup.Clickable = false;
            this.colSalesTaxGroup.DefaultStyle = null;
            resources.ApplyResources(this.colSalesTaxGroup, "colSalesTaxGroup");
            this.colSalesTaxGroup.MaximumWidth = ((short)(0));
            this.colSalesTaxGroup.MinimumWidth = ((short)(10));
            this.colSalesTaxGroup.SecondarySortColumn = ((short)(-1));
            this.colSalesTaxGroup.Tag = null;
            this.colSalesTaxGroup.Width = ((short)(50));
            // 
            // colPriceGroup
            // 
            this.colPriceGroup.AutoSize = true;
            this.colPriceGroup.Clickable = false;
            this.colPriceGroup.DefaultStyle = null;
            resources.ApplyResources(this.colPriceGroup, "colPriceGroup");
            this.colPriceGroup.MaximumWidth = ((short)(0));
            this.colPriceGroup.MinimumWidth = ((short)(10));
            this.colPriceGroup.SecondarySortColumn = ((short)(-1));
            this.colPriceGroup.Tag = null;
            this.colPriceGroup.Width = ((short)(50));
            // 
            // colLineDiscountGroup
            // 
            this.colLineDiscountGroup.AutoSize = true;
            this.colLineDiscountGroup.Clickable = false;
            this.colLineDiscountGroup.DefaultStyle = null;
            resources.ApplyResources(this.colLineDiscountGroup, "colLineDiscountGroup");
            this.colLineDiscountGroup.MaximumWidth = ((short)(0));
            this.colLineDiscountGroup.MinimumWidth = ((short)(10));
            this.colLineDiscountGroup.SecondarySortColumn = ((short)(-1));
            this.colLineDiscountGroup.Tag = null;
            this.colLineDiscountGroup.Width = ((short)(50));
            // 
            // colTotalDiscountGroup
            // 
            this.colTotalDiscountGroup.AutoSize = true;
            this.colTotalDiscountGroup.Clickable = false;
            this.colTotalDiscountGroup.DefaultStyle = null;
            resources.ApplyResources(this.colTotalDiscountGroup, "colTotalDiscountGroup");
            this.colTotalDiscountGroup.MaximumWidth = ((short)(0));
            this.colTotalDiscountGroup.MinimumWidth = ((short)(10));
            this.colTotalDiscountGroup.SecondarySortColumn = ((short)(-1));
            this.colTotalDiscountGroup.Tag = null;
            this.colTotalDiscountGroup.Width = ((short)(50));
            // 
            // colCreditLimit
            // 
            this.colCreditLimit.AutoSize = true;
            this.colCreditLimit.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colCreditLimit.DefaultStyle = null;
            resources.ApplyResources(this.colCreditLimit, "colCreditLimit");
            this.colCreditLimit.MaximumWidth = ((short)(0));
            this.colCreditLimit.MinimumWidth = ((short)(10));
            this.colCreditLimit.SecondarySortColumn = ((short)(-1));
            this.colCreditLimit.Tag = null;
            this.colCreditLimit.Width = ((short)(50));
            // 
            // colBlockingStatus
            // 
            this.colBlockingStatus.AutoSize = true;
            this.colBlockingStatus.DefaultStyle = null;
            resources.ApplyResources(this.colBlockingStatus, "colBlockingStatus");
            this.colBlockingStatus.MaximumWidth = ((short)(0));
            this.colBlockingStatus.MinimumWidth = ((short)(10));
            this.colBlockingStatus.SecondarySortColumn = ((short)(-1));
            this.colBlockingStatus.Tag = null;
            this.colBlockingStatus.Width = ((short)(50));
            // 
            // searchBar1
            // 
            resources.ApplyResources(this.searchBar1, "searchBar1");
            this.searchBar1.BackColor = System.Drawing.Color.Transparent;
            this.searchBar1.BuddyControl = null;
            this.searchBar1.Name = "searchBar1";
            this.searchBar1.SearchOptionEnabled = true;
            this.searchBar1.SetupConditions += new System.EventHandler(this.searchBar1_SetupConditions);
            this.searchBar1.SearchClicked += new System.EventHandler(this.searchBar1_SearchClicked);
            this.searchBar1.SaveAsDefault += new System.EventHandler(this.searchBar1_SaveAsDefault);
            this.searchBar1.LoadDefault += new System.EventHandler(this.searchBar1_LoadDefault);
            this.searchBar1.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar1_UnknownControlAdd);
            this.searchBar1.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar1_UnknownControlRemove);
            this.searchBar1.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar1_UnknownControlHasSelection);
            this.searchBar1.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar1_UnknownControlGetSelection);
            this.searchBar1.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar1_UnknownControlSetSelection);
            // 
            // colTaxExempt
            // 
            this.colTaxExempt.AutoSize = true;
            this.colTaxExempt.Clickable = false;
            this.colTaxExempt.DefaultStyle = null;
            resources.ApplyResources(this.colTaxExempt, "colTaxExempt");
            this.colTaxExempt.MaximumWidth = ((short)(0));
            this.colTaxExempt.MinimumWidth = ((short)(10));
            this.colTaxExempt.SecondarySortColumn = ((short)(-1));
            this.colTaxExempt.Tag = null;
            this.colTaxExempt.Width = ((short)(50));
            // 
            // CustomersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CustomersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DatabasePageDisplay customerDataScroll;
        private ContextButtons btnsEditAddRemove;
        private ListView lvCustomers;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colName;
        private LSOne.Controls.SearchBar searchBar1;
        private Controls.Columns.Column colSalesTaxGroup;
        private Controls.Columns.Column colCashCustomer;
        private Controls.Columns.Column colPriceGroup;
        private Controls.Columns.Column colLineDiscountGroup;
        private Controls.Columns.Column colTotalDiscountGroup;
        private Controls.Columns.Column colCreditLimit;
        private Controls.Columns.Column colBlockingStatus;
        private Controls.Columns.Column colTaxExempt;
    }
}
