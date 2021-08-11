
using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class EFTMappingsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EFTMappingsView));
            this.lvMappings = new LSOne.Controls.ExtendedListView();
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScheme = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTenderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCardType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.colLookupOrder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvMappings);
            // 
            // lvMappings
            // 
            resources.ApplyResources(this.lvMappings, "lvMappings");
            this.lvMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colScheme,
            this.colTenderType,
            this.colCardType,
            this.colEnabled,
            this.colLookupOrder});
            this.lvMappings.FullRowSelect = true;
            this.lvMappings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMappings.HideSelection = false;
            this.lvMappings.LockDrawing = false;
            this.lvMappings.MultiSelect = false;
            this.lvMappings.Name = "lvMappings";
            this.lvMappings.SortColumn = -1;
            this.lvMappings.SortedBackwards = false;
            this.lvMappings.UseCompatibleStateImageBehavior = false;
            this.lvMappings.UseEveryOtherRowColoring = true;
            this.lvMappings.View = System.Windows.Forms.View.Details;
            this.lvMappings.SelectedIndexChanged += new System.EventHandler(this.lvMappings_SelectedIndexChanged);
            this.lvMappings.DoubleClick += new System.EventHandler(this.lvMapping_DoubleClick);
            // 
            // colID
            // 
            resources.ApplyResources(this.colID, "colID");
            // 
            // colScheme
            // 
            resources.ApplyResources(this.colScheme, "colScheme");
            // 
            // colTenderType
            // 
            resources.ApplyResources(this.colTenderType, "colTenderType");
            // 
            // colCardType
            // 
            resources.ApplyResources(this.colCardType, "colCardType");
            // 
            // colEnabled
            // 
            resources.ApplyResources(this.colEnabled, "colEnabled");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // colLookupOrder
            // 
            resources.ApplyResources(this.colLookupOrder, "colLookupOrder");
            // 
            // EFTMappingsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EFTMappingsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvMappings;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colScheme;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.ColumnHeader colTenderType;
        private System.Windows.Forms.ColumnHeader colEnabled;
        private System.Windows.Forms.ColumnHeader colCardType;
        private System.Windows.Forms.ColumnHeader colLookupOrder;

    }
}
