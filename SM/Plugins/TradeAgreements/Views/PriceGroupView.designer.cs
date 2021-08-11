using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.Views
{
    partial class PriceGroupView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PriceGroupView));
            this.btnsGroup = new ContextButtons();
            this.tabSheetTabs = new TabControl();
            this.groupPanelNoSelection = new GroupPanel();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.lvPriceGroups = new ListView();
            this.colID = new Column();
            this.colDescription = new Column();
            this.colPriceInTax = new Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.lvPriceGroups);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
            this.pnlBottom.Controls.Add(this.btnsGroup);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
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
            // lvPriceGroups
            // 
            resources.ApplyResources(this.lvPriceGroups, "lvPriceGroups");
            this.lvPriceGroups.BuddyControl = null;
            this.lvPriceGroups.Columns.Add(this.colID);
            this.lvPriceGroups.Columns.Add(this.colDescription);
            this.lvPriceGroups.Columns.Add(this.colPriceInTax);
            this.lvPriceGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvPriceGroups.DefaultRowHeight = ((short)(22));
            this.lvPriceGroups.DimSelectionWhenDisabled = true;
            this.lvPriceGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPriceGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPriceGroups.HeaderHeight = ((short)(25));
            this.lvPriceGroups.Name = "lvPriceGroups";
            this.lvPriceGroups.OddRowColor = System.Drawing.Color.White;
            this.lvPriceGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPriceGroups.SelectionModel = ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvPriceGroups.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvPriceGroups.SortSetting = "0:1";
            this.lvPriceGroups.HeaderClicked += new HeaderDelegate(this.lvPriceGroups_HeaderClicked);
            this.lvPriceGroups.SelectionChanged += new System.EventHandler(this.lvPriceGroups_SelectionChanged);
            this.lvPriceGroups.RowDoubleClick += new RowClickDelegate(this.lvPriceGroups_RowDoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.Clickable = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.Sizable = false;
            this.colID.Tag = null;
            this.colID.Width = ((short)(100));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.Clickable = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.Sizable = false;
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(200));
            // 
            // colPriceInTax
            // 
            this.colPriceInTax.AutoSize = true;
            this.colPriceInTax.Clickable = true;
            this.colPriceInTax.DefaultStyle = null;
            resources.ApplyResources(this.colPriceInTax, "colPriceInTax");
            this.colPriceInTax.MaximumWidth = ((short)(0));
            this.colPriceInTax.MinimumWidth = ((short)(10));
            this.colPriceInTax.Sizable = false;
            this.colPriceInTax.Tag = null;
            this.colPriceInTax.Width = ((short)(150));
            // 
            // PriceGroupView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PriceGroupView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //private Controls.DatabasePageDisplay storeDataScroll;
        private ContextButtons btnsGroup;
        private TabControl tabSheetTabs;
        private GroupPanel groupPanelNoSelection;
        private System.Windows.Forms.Label lblNoSelection;
        private ListView lvPriceGroups;
        private Column colID;
        private Column colDescription;
        private Column colPriceInTax;

    }
}
