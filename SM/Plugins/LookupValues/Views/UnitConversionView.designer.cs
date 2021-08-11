using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class UnitConversionView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnitConversionView));
            this.lvItems = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupPanel1 = new GroupPanel();
            this.lblItem = new System.Windows.Forms.Label();
            this.cmbItem = new DualDataComboBox();
            this.cmbShowConversionsFor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnsContextButtons = new ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            this.pnlBottom.Controls.Add(this.lvItems);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader7,
            this.columnHeader2});
            this.lvItems.FullRowSelect = true;
            this.lvItems.HideSelection = false;
            this.lvItems.LockDrawing = false;
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.SortColumn = 1;
            this.lvItems.SortedBackwards = false;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.UseEveryOtherRowColoring = true;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroups_ColumnClick);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvGroups_SelectedIndexChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvGroups_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader7
            // 
            resources.ApplyResources(this.columnHeader7, "columnHeader7");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.lblItem);
            this.groupPanel1.Controls.Add(this.cmbItem);
            this.groupPanel1.Controls.Add(this.cmbShowConversionsFor);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // lblItem
            // 
            this.lblItem.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblItem, "lblItem");
            this.lblItem.Name = "lblItem";
            // 
            // cmbItem
            // 
            resources.ApplyResources(this.cmbItem, "cmbItem");
            this.cmbItem.EnableTextBox = true;
            this.cmbItem.MaxLength = 32767;
            this.cmbItem.Name = "cmbItem";
            this.cmbItem.ReceiveKeyboardEvents = true;
            this.cmbItem.SelectedData = null;
            this.cmbItem.ShowDropDownOnTyping = true;
            this.cmbItem.SkipIDColumn = false;
            this.cmbItem.DropDown += new DropDownEventHandler(this.cmbItem_DropDown);
            this.cmbItem.SelectedDataChanged += new System.EventHandler(this.cmbItem_SelectedDataChanged);
            this.cmbItem.TextChanged += new System.EventHandler(this.cmbItem_TextChanged);
            this.cmbItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbItem_KeyPress);
            // 
            // cmbShowConversionsFor
            // 
            this.cmbShowConversionsFor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShowConversionsFor.FormattingEnabled = true;
            this.cmbShowConversionsFor.Items.AddRange(new object[] {
            resources.GetString("cmbShowConversionsFor.Items"),
            resources.GetString("cmbShowConversionsFor.Items1")});
            resources.ApplyResources(this.cmbShowConversionsFor, "cmbShowConversionsFor");
            this.cmbShowConversionsFor.Name = "cmbShowConversionsFor";
            this.cmbShowConversionsFor.SelectedIndexChanged += new System.EventHandler(this.cmbShowConversionsFor_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // UnitConversionView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 90;
            this.Name = "UnitConversionView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.ComboBox cmbShowConversionsFor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblItem;
        private DualDataComboBox cmbItem;
        private ContextButtons btnsContextButtons;

    }
}
