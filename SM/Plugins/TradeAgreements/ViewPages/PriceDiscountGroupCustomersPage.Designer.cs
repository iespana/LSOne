using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    partial class PriceDiscountGroupCustomersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PriceDiscountGroupCustomersPage));
            this.gpCustomers = new LSOne.Controls.GroupPanel();
            this.customerDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.btnViewCustomer = new System.Windows.Forms.Button();
            this.contextButtonsCustomers = new LSOne.Controls.ContextButtons();
            this.lblCustomersGroupHeader = new System.Windows.Forms.Label();
            this.lvCustomers = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.gpCustomers.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpCustomers
            // 
            resources.ApplyResources(this.gpCustomers, "gpCustomers");
            this.gpCustomers.Controls.Add(this.lvCustomers);
            this.gpCustomers.Controls.Add(this.customerDataScroll);
            this.gpCustomers.Controls.Add(this.btnViewCustomer);
            this.gpCustomers.Controls.Add(this.contextButtonsCustomers);
            this.gpCustomers.Controls.Add(this.lblCustomersGroupHeader);
            this.gpCustomers.Name = "gpCustomers";
            // 
            // customerDataScroll
            // 
            resources.ApplyResources(this.customerDataScroll, "customerDataScroll");
            this.customerDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.customerDataScroll.Name = "customerDataScroll";
            this.customerDataScroll.PageSize = 0;
            this.customerDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // btnViewCustomer
            // 
            resources.ApplyResources(this.btnViewCustomer, "btnViewCustomer");
            this.btnViewCustomer.Name = "btnViewCustomer";
            this.btnViewCustomer.UseVisualStyleBackColor = true;
            this.btnViewCustomer.Click += new System.EventHandler(this.btnEditCustomer_Click);
            // 
            // contextButtonsCustomers
            // 
            this.contextButtonsCustomers.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsCustomers, "contextButtonsCustomers");
            this.contextButtonsCustomers.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsCustomers.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtonsCustomers.EditButtonEnabled = false;
            this.contextButtonsCustomers.Name = "contextButtonsCustomers";
            this.contextButtonsCustomers.RemoveButtonEnabled = true;
            this.contextButtonsCustomers.AddButtonClicked += new System.EventHandler(this.btnAddCustomer_Click);
            this.contextButtonsCustomers.RemoveButtonClicked += new System.EventHandler(this.btnRemoveCustomer_Click);
            // 
            // lblCustomersGroupHeader
            // 
            resources.ApplyResources(this.lblCustomersGroupHeader, "lblCustomersGroupHeader");
            this.lblCustomersGroupHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomersGroupHeader.Name = "lblCustomersGroupHeader";
            // 
            // lvCustomers
            // 
            resources.ApplyResources(this.lvCustomers, "lvCustomers");
            this.lvCustomers.BuddyControl = null;
            this.lvCustomers.Columns.Add(this.column1);
            this.lvCustomers.Columns.Add(this.column2);
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
            this.lvCustomers.SelectionChanged += new System.EventHandler(this.lvCustomers_SelectionChanged);
            this.lvCustomers.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCustomers_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // PriceDiscountGroupCustomersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gpCustomers);
            this.Name = "PriceDiscountGroupCustomersPage";
            this.gpCustomers.ResumeLayout(false);
            this.gpCustomers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel gpCustomers;
        private System.Windows.Forms.Button btnViewCustomer;
        private ContextButtons contextButtonsCustomers;
        private System.Windows.Forms.Label lblCustomersGroupHeader;
        private DatabasePageDisplay customerDataScroll;
        private ListView lvCustomers;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
    }
}
