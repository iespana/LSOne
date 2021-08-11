using LSOne.Controls;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    partial class CustomerGroupCustomersPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupCustomersPage));
            this.btnsContextButtonsCustomers = new LSOne.Controls.ContextButtons();
            this.customerDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvCustomerList = new LSOne.Controls.ListView();
            this.colCustomer = new LSOne.Controls.Columns.Column();
            this.btnViewCustomer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsContextButtonsCustomers
            // 
            this.btnsContextButtonsCustomers.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtonsCustomers, "btnsContextButtonsCustomers");
            this.btnsContextButtonsCustomers.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsCustomers.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.btnsContextButtonsCustomers.EditButtonEnabled = false;
            this.btnsContextButtonsCustomers.Name = "btnsContextButtonsCustomers";
            this.btnsContextButtonsCustomers.RemoveButtonEnabled = true;
            this.btnsContextButtonsCustomers.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtonsCustomers.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // customerDataScroll
            // 
            resources.ApplyResources(this.customerDataScroll, "customerDataScroll");
            this.customerDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.customerDataScroll.Name = "customerDataScroll";
            this.customerDataScroll.PageSize = 0;
            this.customerDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvCustomerList
            // 
            resources.ApplyResources(this.lvCustomerList, "lvCustomerList");
            this.lvCustomerList.BuddyControl = null;
            this.lvCustomerList.Columns.Add(this.colCustomer);
            this.lvCustomerList.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomerList.DefaultRowHeight = ((short)(22));
            this.lvCustomerList.DimSelectionWhenDisabled = true;
            this.lvCustomerList.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomerList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCustomerList.HeaderHeight = ((short)(25));
            this.lvCustomerList.Name = "lvCustomerList";
            this.lvCustomerList.OddRowColor = System.Drawing.Color.White;
            this.lvCustomerList.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCustomerList.SecondarySortColumn = ((short)(-1));
            this.lvCustomerList.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCustomerList.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomerList.SortSetting = "0:1";
            this.lvCustomerList.SelectionChanged += new System.EventHandler(this.lvCustomers_SelectedIndexChanged);
            this.lvCustomerList.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCustomerList_RowDoubleClick);
            // 
            // colCustomer
            // 
            this.colCustomer.AutoSize = true;
            this.colCustomer.DefaultStyle = null;
            resources.ApplyResources(this.colCustomer, "colCustomer");
            this.colCustomer.InternalSort = true;
            this.colCustomer.MaximumWidth = ((short)(0));
            this.colCustomer.MinimumWidth = ((short)(100));
            this.colCustomer.SecondarySortColumn = ((short)(-1));
            this.colCustomer.Tag = null;
            this.colCustomer.Width = ((short)(100));
            // 
            // btnViewCustomer
            // 
            resources.ApplyResources(this.btnViewCustomer, "btnViewCustomer");
            this.btnViewCustomer.Name = "btnViewCustomer";
            this.btnViewCustomer.UseVisualStyleBackColor = true;
            this.btnViewCustomer.Click += new System.EventHandler(this.ViewCustomer_Click);
            // 
            // CustomerGroupCustomersPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnViewCustomer);
            this.Controls.Add(this.lvCustomerList);
            this.Controls.Add(this.btnsContextButtonsCustomers);
            this.Controls.Add(this.customerDataScroll);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "CustomerGroupCustomersPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtonsCustomers;
        private DatabasePageDisplay customerDataScroll;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvCustomerList;
        private Controls.Columns.Column colCustomer;
        private System.Windows.Forms.Button btnViewCustomer;
    }
}
