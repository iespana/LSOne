using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CustomerGroupsView
    {
        #region Component Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerGroupsView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.groupPanelNoSelection = new LSOne.Controls.GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvCustomerGroups = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colCategory = new LSOne.Controls.Columns.Column();
            this.btnSetGroupAsDefault = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSetGroupAsDefault);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvCustomerGroups);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // groupPanelNoSelection
            // 
            resources.ApplyResources(this.groupPanelNoSelection, "groupPanelNoSelection");
            this.groupPanelNoSelection.Controls.Add(this.lblNoSelection);
            this.groupPanelNoSelection.Name = "groupPanelNoSelection";
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // lvCustomerGroups
            // 
            resources.ApplyResources(this.lvCustomerGroups, "lvCustomerGroups");
            this.lvCustomerGroups.BuddyControl = null;
            this.lvCustomerGroups.Columns.Add(this.colID);
            this.lvCustomerGroups.Columns.Add(this.colDescription);
            this.lvCustomerGroups.Columns.Add(this.colCategory);
            this.lvCustomerGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvCustomerGroups.DefaultRowHeight = ((short)(22));
            this.lvCustomerGroups.DimSelectionWhenDisabled = true;
            this.lvCustomerGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCustomerGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCustomerGroups.HeaderHeight = ((short)(25));
            this.lvCustomerGroups.Name = "lvCustomerGroups";
            this.lvCustomerGroups.OddRowColor = System.Drawing.Color.White;
            this.lvCustomerGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCustomerGroups.RowLines = true;
            this.lvCustomerGroups.SecondarySortColumn = ((short)(-1));
            this.lvCustomerGroups.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvCustomerGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCustomerGroups.SortSetting = "1:1";
            this.lvCustomerGroups.SelectionChanged += new System.EventHandler(this.lvCustomerGroups_SelectionChanged);
            this.lvCustomerGroups.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCustomerGroups_RowDoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.InternalSort = true;
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(100));
            this.colID.Tag = null;
            this.colID.Width = ((short)(50));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(100));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colCategory
            // 
            this.colCategory.AutoSize = true;
            this.colCategory.DefaultStyle = null;
            resources.ApplyResources(this.colCategory, "colCategory");
            this.colCategory.InternalSort = true;
            this.colCategory.MaximumWidth = ((short)(0));
            this.colCategory.MinimumWidth = ((short)(100));
            this.colCategory.Tag = null;
            this.colCategory.Width = ((short)(50));
            // 
            // btnSetGroupAsDefault
            // 
            resources.ApplyResources(this.btnSetGroupAsDefault, "btnSetGroupAsDefault");
            this.btnSetGroupAsDefault.Name = "btnSetGroupAsDefault";
            this.btnSetGroupAsDefault.UseVisualStyleBackColor = true;
            this.btnSetGroupAsDefault.Click += new System.EventHandler(this.btnSetGroupAsDefault_Click);
            // 
            // CustomerGroupsView
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "CustomerGroupsView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ListView lvCustomerGroups;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colCategory;
        private Controls.ContextButtons btnsEditAddRemove;
        private ViewCore.Controls.TabControl tabSheetTabs;
        private Controls.GroupPanel groupPanelNoSelection;
        private Label lblNoSelection;
        private Button btnSetGroupAsDefault;
        private Controls.Columns.Column colID;
    }
}
