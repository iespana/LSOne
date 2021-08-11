using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLedger.ViewPages
{
    partial class CustomerLedgerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerLedgerPage));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCreditLimit = new System.Windows.Forms.Label();
            this.tbCreditLimit = new LSOne.Controls.NumericTextBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.tbBalance = new System.Windows.Forms.TextBox();
            this.lblTotalSales = new System.Windows.Forms.Label();
            this.tbTotalSales = new System.Windows.Forms.TextBox();
            this.lvCustomerLedger = new LSOne.Controls.ExtendedListView();
            this.clmhDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhForPayment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhStore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhTerminal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhReceiptID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddPayment = new System.Windows.Forms.Button();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.searchBar1 = new LSOne.Controls.SearchBar();
            this.btnSendViaEmail = new System.Windows.Forms.Button();
            this.groupPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.flowLayoutPanel1);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.lblCreditLimit);
            this.flowLayoutPanel1.Controls.Add(this.tbCreditLimit);
            this.flowLayoutPanel1.Controls.Add(this.lblBalance);
            this.flowLayoutPanel1.Controls.Add(this.tbBalance);
            this.flowLayoutPanel1.Controls.Add(this.lblTotalSales);
            this.flowLayoutPanel1.Controls.Add(this.tbTotalSales);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // lblCreditLimit
            // 
            resources.ApplyResources(this.lblCreditLimit, "lblCreditLimit");
            this.lblCreditLimit.BackColor = System.Drawing.Color.Transparent;
            this.lblCreditLimit.Name = "lblCreditLimit";
            // 
            // tbCreditLimit
            // 
            this.tbCreditLimit.AllowDecimal = false;
            this.tbCreditLimit.AllowNegative = false;
            this.tbCreditLimit.CultureInfo = null;
            this.tbCreditLimit.DecimalLetters = 2;
            this.tbCreditLimit.ForeColor = System.Drawing.Color.Black;
            this.tbCreditLimit.HasMinValue = false;
            resources.ApplyResources(this.tbCreditLimit, "tbCreditLimit");
            this.tbCreditLimit.MaxValue = 0D;
            this.tbCreditLimit.MinValue = 0D;
            this.tbCreditLimit.Name = "tbCreditLimit";
            this.tbCreditLimit.Value = 0D;
            // 
            // lblBalance
            // 
            resources.ApplyResources(this.lblBalance, "lblBalance");
            this.lblBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblBalance.Name = "lblBalance";
            // 
            // tbBalance
            // 
            resources.ApplyResources(this.tbBalance, "tbBalance");
            this.tbBalance.Name = "tbBalance";
            // 
            // lblTotalSales
            // 
            resources.ApplyResources(this.lblTotalSales, "lblTotalSales");
            this.lblTotalSales.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalSales.Name = "lblTotalSales";
            // 
            // tbTotalSales
            // 
            resources.ApplyResources(this.tbTotalSales, "tbTotalSales");
            this.tbTotalSales.Name = "tbTotalSales";
            // 
            // lvCustomerLedger
            // 
            resources.ApplyResources(this.lvCustomerLedger, "lvCustomerLedger");
            this.lvCustomerLedger.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmhDate,
            this.clmhType,
            this.clmhAmount,
            this.clmhForPayment,
            this.clmhStatus,
            this.clmhStore,
            this.clmhTerminal,
            this.clmhReceiptID,
            this.clmDescription});
            this.lvCustomerLedger.FullRowSelect = true;
            this.lvCustomerLedger.HideSelection = false;
            this.lvCustomerLedger.LockDrawing = false;
            this.lvCustomerLedger.MultiSelect = false;
            this.lvCustomerLedger.Name = "lvCustomerLedger";
            this.lvCustomerLedger.SortColumn = 0;
            this.lvCustomerLedger.SortedBackwards = true;
            this.lvCustomerLedger.UseCompatibleStateImageBehavior = false;
            this.lvCustomerLedger.UseEveryOtherRowColoring = true;
            this.lvCustomerLedger.View = System.Windows.Forms.View.Details;
            // 
            // clmhDate
            // 
            resources.ApplyResources(this.clmhDate, "clmhDate");
            // 
            // clmhType
            // 
            resources.ApplyResources(this.clmhType, "clmhType");
            // 
            // clmhAmount
            // 
            resources.ApplyResources(this.clmhAmount, "clmhAmount");
            // 
            // clmhForPayment
            // 
            resources.ApplyResources(this.clmhForPayment, "clmhForPayment");
            // 
            // clmhStatus
            // 
            resources.ApplyResources(this.clmhStatus, "clmhStatus");
            // 
            // clmhStore
            // 
            resources.ApplyResources(this.clmhStore, "clmhStore");
            // 
            // clmhTerminal
            // 
            resources.ApplyResources(this.clmhTerminal, "clmhTerminal");
            // 
            // clmhReceiptID
            // 
            resources.ApplyResources(this.clmhReceiptID, "clmhReceiptID");
            // 
            // clmDescription
            // 
            resources.ApplyResources(this.clmDescription, "clmDescription");
            // 
            // btnAddPayment
            // 
            resources.ApplyResources(this.btnAddPayment, "btnAddPayment");
            this.btnAddPayment.Name = "btnAddPayment";
            this.btnAddPayment.UseVisualStyleBackColor = true;
            this.btnAddPayment.Click += new System.EventHandler(this.btnAddPayment_Click);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
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
            // btnSendViaEmail
            // 
            resources.ApplyResources(this.btnSendViaEmail, "btnSendViaEmail");
            this.btnSendViaEmail.Name = "btnSendViaEmail";
            this.btnSendViaEmail.UseVisualStyleBackColor = true;
            this.btnSendViaEmail.Click += new System.EventHandler(this.btnSendViaEmail_Click);
            // 
            // CustomerLedgerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnSendViaEmail);
            this.Controls.Add(this.searchBar1);
            this.Controls.Add(this.btnAddPayment);
            this.Controls.Add(this.lvCustomerLedger);
            this.Controls.Add(this.itemDataScroll);
            this.Controls.Add(this.groupPanel1);
            this.Name = "CustomerLedgerPage";
            this.groupPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupPanel groupPanel1;
        private DatabasePageDisplay itemDataScroll;
        private ExtendedListView lvCustomerLedger;
        private System.Windows.Forms.ColumnHeader clmhDate;
        private System.Windows.Forms.ColumnHeader clmhType;
        private System.Windows.Forms.ColumnHeader clmhAmount;
        private System.Windows.Forms.ColumnHeader clmhForPayment;
        private System.Windows.Forms.ColumnHeader clmhStatus;
        private System.Windows.Forms.ColumnHeader clmhStore;
        private System.Windows.Forms.ColumnHeader clmhTerminal;
        private System.Windows.Forms.ColumnHeader clmhReceiptID;
        private System.Windows.Forms.Label lblTotalSales;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblCreditLimit;
        private System.Windows.Forms.TextBox tbTotalSales;
        private System.Windows.Forms.TextBox tbBalance;
        private NumericTextBox tbCreditLimit;
        private System.Windows.Forms.Button btnAddPayment;
        private System.Windows.Forms.ColumnHeader clmDescription;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private SearchBar searchBar1;
        private System.Windows.Forms.Button btnSendViaEmail;


	}
}
