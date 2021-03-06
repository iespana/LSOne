using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Views
{
    partial class CustomerDiscountGroupView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerDiscountGroupView));
            this.lblItem = new System.Windows.Forms.Label();
            this.cmbShowType = new System.Windows.Forms.ComboBox();
            this.btnsGroup = new ContextButtons();
            this.lvItems = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabSheetTabs = new TabControl();
            this.groupPanelNoSelection = new GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
            this.pnlBottom.Controls.Add(this.btnsGroup);
            this.pnlBottom.Controls.Add(this.cmbShowType);
            this.pnlBottom.Controls.Add(this.lblItem);
            this.pnlBottom.Controls.Add(this.lvItems);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lblItem
            // 
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            // 
            // cmbShowType
            // 
            this.cmbShowType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShowType.FormattingEnabled = true;
            this.cmbShowType.Items.AddRange(new object[] {
            resources.GetString("cmbShowType.Items"),
            resources.GetString("cmbShowType.Items1"),
            resources.GetString("cmbShowType.Items2")});
            resources.ApplyResources(this.cmbShowType, "cmbShowType");
            this.cmbShowType.Name = "cmbShowType";
            this.cmbShowType.SelectedIndexChanged += new System.EventHandler(this.cmbShowType_SelectedIndexChanged);
            // 
            // btnsGroup
            // 
            this.btnsGroup.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsGroup, "btnsGroup");
            this.btnsGroup.BackColor = System.Drawing.Color.Transparent;
            this.btnsGroup.Context = ButtonTypes.EditAddRemove;
            this.btnsGroup.EditButtonEnabled = true;
            this.btnsGroup.Name = "btnsGroup";
            this.btnsGroup.RemoveButtonEnabled = true;
            this.btnsGroup.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsGroup.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsGroup.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvItems.FullRowSelect = true;
            this.lvItems.HideSelection = false;
            this.lvItems.LockDrawing = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.SortColumn = -1;
            this.lvItems.SortedBackwards = false;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.UseEveryOtherRowColoring = true;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItems_ColumnClick);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
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
            // PriceDiscGroupView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 50;
            this.Name = "PriceDiscGroupView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.ComboBox cmbShowType;
        //private Controls.DatabasePageDisplay storeDataScroll;
        private ContextButtons btnsGroup;
        private TabControl tabSheetTabs;
        private GroupPanel groupPanelNoSelection;
        private System.Windows.Forms.Label lblNoSelection;

    }
}
