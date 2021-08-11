using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerGroupsPage
    {
        #region Component Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupsPage));
            this.lvCustomerGroup = new LSOne.Controls.ListView();
            this.colDefault = new LSOne.Controls.Columns.Column();
            this.colGroup = new LSOne.Controls.Columns.Column();
            this.colCategory = new LSOne.Controls.Columns.Column();
            this.colExclusive = new LSOne.Controls.Columns.Column();
            this.colUsesLimit = new LSOne.Controls.Columns.Column();
            this.colPurchaseLimit = new LSOne.Controls.Columns.Column();
            this.colPeriod = new LSOne.Controls.Columns.Column();
            this.btnsContextButtonsCustomers = new LSOne.Controls.ContextButtons();
            this.btnSetGroupAsDefault = new System.Windows.Forms.Button();
            this.btnViewCustomerGroup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvCustomerGroup
            // 
            resources.ApplyResources(this.lvCustomerGroup, "lvCustomerGroup");
            this.lvCustomerGroup.BuddyControl = null;
            this.lvCustomerGroup.Columns.Add(this.colDefault);
            this.lvCustomerGroup.Columns.Add(this.colGroup);
            this.lvCustomerGroup.Columns.Add(this.colCategory);
            this.lvCustomerGroup.Columns.Add(this.colExclusive);
            this.lvCustomerGroup.Columns.Add(this.colUsesLimit);
            this.lvCustomerGroup.Columns.Add(this.colPurchaseLimit);
            this.lvCustomerGroup.Columns.Add(this.colPeriod);
            this.lvCustomerGroup.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomerGroup.DefaultRowHeight = ((short)(22));
            this.lvCustomerGroup.DimSelectionWhenDisabled = true;
            this.lvCustomerGroup.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomerGroup.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCustomerGroup.HeaderHeight = ((short)(25));
            this.lvCustomerGroup.Name = "lvCustomerGroup";
            this.lvCustomerGroup.OddRowColor = System.Drawing.Color.White;
            this.lvCustomerGroup.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCustomerGroup.SecondarySortColumn = ((short)(-1));
            this.lvCustomerGroup.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCustomerGroup.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomerGroup.SortSetting = "1:1";
            this.lvCustomerGroup.SelectionChanged += new System.EventHandler(this.lvCustomerGroup_SelectionChanged);
            // 
            // colDefault
            // 
            this.colDefault.AutoSize = true;
            this.colDefault.Clickable = false;
            this.colDefault.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Center;
            this.colDefault.DefaultStyle = null;
            resources.ApplyResources(this.colDefault, "colDefault");
            this.colDefault.MaximumWidth = ((short)(0));
            this.colDefault.MinimumWidth = ((short)(10));
            this.colDefault.SecondarySortColumn = ((short)(-1));
            this.colDefault.Tag = null;
            this.colDefault.Width = ((short)(50));
            // 
            // colGroup
            // 
            this.colGroup.AutoSize = true;
            this.colGroup.DefaultStyle = null;
            resources.ApplyResources(this.colGroup, "colGroup");
            this.colGroup.InternalSort = true;
            this.colGroup.MaximumWidth = ((short)(0));
            this.colGroup.MinimumWidth = ((short)(20));
            this.colGroup.SecondarySortColumn = ((short)(-1));
            this.colGroup.Tag = null;
            this.colGroup.Width = ((short)(50));
            // 
            // colCategory
            // 
            this.colCategory.AutoSize = true;
            this.colCategory.DefaultStyle = null;
            resources.ApplyResources(this.colCategory, "colCategory");
            this.colCategory.InternalSort = true;
            this.colCategory.MaximumWidth = ((short)(0));
            this.colCategory.MinimumWidth = ((short)(20));
            this.colCategory.SecondarySortColumn = ((short)(-1));
            this.colCategory.Tag = null;
            this.colCategory.Width = ((short)(50));
            // 
            // colExclusive
            // 
            this.colExclusive.AutoSize = true;
            this.colExclusive.Clickable = false;
            this.colExclusive.DefaultStyle = null;
            resources.ApplyResources(this.colExclusive, "colExclusive");
            this.colExclusive.MaximumWidth = ((short)(0));
            this.colExclusive.MinimumWidth = ((short)(20));
            this.colExclusive.SecondarySortColumn = ((short)(-1));
            this.colExclusive.Tag = null;
            this.colExclusive.Width = ((short)(50));
            // 
            // colUsesLimit
            // 
            this.colUsesLimit.AutoSize = true;
            this.colUsesLimit.Clickable = false;
            this.colUsesLimit.DefaultStyle = null;
            resources.ApplyResources(this.colUsesLimit, "colUsesLimit");
            this.colUsesLimit.MaximumWidth = ((short)(0));
            this.colUsesLimit.MinimumWidth = ((short)(10));
            this.colUsesLimit.SecondarySortColumn = ((short)(-1));
            this.colUsesLimit.Tag = null;
            this.colUsesLimit.Width = ((short)(50));
            // 
            // colPurchaseLimit
            // 
            this.colPurchaseLimit.AutoSize = true;
            this.colPurchaseLimit.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colPurchaseLimit.DefaultStyle = null;
            resources.ApplyResources(this.colPurchaseLimit, "colPurchaseLimit");
            this.colPurchaseLimit.InternalSort = true;
            this.colPurchaseLimit.MaximumWidth = ((short)(0));
            this.colPurchaseLimit.MinimumWidth = ((short)(20));
            this.colPurchaseLimit.SecondarySortColumn = ((short)(-1));
            this.colPurchaseLimit.Tag = null;
            this.colPurchaseLimit.Width = ((short)(50));
            // 
            // colPeriod
            // 
            this.colPeriod.AutoSize = true;
            this.colPeriod.DefaultStyle = null;
            resources.ApplyResources(this.colPeriod, "colPeriod");
            this.colPeriod.InternalSort = true;
            this.colPeriod.MaximumWidth = ((short)(0));
            this.colPeriod.MinimumWidth = ((short)(10));
            this.colPeriod.SecondarySortColumn = ((short)(-1));
            this.colPeriod.Tag = null;
            this.colPeriod.Width = ((short)(50));
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
            this.btnsContextButtonsCustomers.RemoveButtonClicked += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSetGroupAsDefault
            // 
            resources.ApplyResources(this.btnSetGroupAsDefault, "btnSetGroupAsDefault");
            this.btnSetGroupAsDefault.Name = "btnSetGroupAsDefault";
            this.btnSetGroupAsDefault.UseVisualStyleBackColor = true;
            this.btnSetGroupAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // btnViewCustomerGroup
            // 
            resources.ApplyResources(this.btnViewCustomerGroup, "btnViewCustomerGroup");
            this.btnViewCustomerGroup.Name = "btnViewCustomerGroup";
            this.btnViewCustomerGroup.UseVisualStyleBackColor = true;
            this.btnViewCustomerGroup.Click += new System.EventHandler(this.btnViewCustomerGroup_Click);
            // 
            // CustomerGroupsPage
            // 
            this.Controls.Add(this.btnViewCustomerGroup);
            this.Controls.Add(this.btnSetGroupAsDefault);
            this.Controls.Add(this.btnsContextButtonsCustomers);
            this.Controls.Add(this.lvCustomerGroup);
            resources.ApplyResources(this, "$this");
            this.Name = "CustomerGroupsPage";
            this.ResumeLayout(false);

        }
        #endregion
        private Controls.ListView lvCustomerGroup;
        private Controls.ContextButtons btnsContextButtonsCustomers;
        private Controls.Columns.Column colDefault;
        private Controls.Columns.Column colGroup;
        private Controls.Columns.Column colCategory;
        private Controls.Columns.Column colExclusive;
        private Controls.Columns.Column colPurchaseLimit;
        private Button btnSetGroupAsDefault;
        private Controls.Columns.Column colUsesLimit;
        private Controls.Columns.Column colPeriod;
        private Button btnViewCustomerGroup;
    }
}
