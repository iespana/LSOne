using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class POSUserCardsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POSUserCardsView));
            this.btnsEditAddRemove = new ContextButtons();
            this.lvPOSUserCards = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvPOSUserCards);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvPOSUserCards
            // 
            resources.ApplyResources(this.lvPOSUserCards, "lvPOSUserCards");
            this.lvPOSUserCards.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvPOSUserCards.FullRowSelect = true;
            this.lvPOSUserCards.HideSelection = false;
            this.lvPOSUserCards.LockDrawing = false;
            this.lvPOSUserCards.MultiSelect = false;
            this.lvPOSUserCards.Name = "lvPOSUserCards";
            this.lvPOSUserCards.SortColumn = -1;
            this.lvPOSUserCards.SortedBackwards = false;
            this.lvPOSUserCards.UseCompatibleStateImageBehavior = false;
            this.lvPOSUserCards.UseEveryOtherRowColoring = true;
            this.lvPOSUserCards.View = System.Windows.Forms.View.Details;
            this.lvPOSUserCards.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStores_ColumnClick);
            this.lvPOSUserCards.SelectedIndexChanged += new System.EventHandler(this.lvStores_SelectedIndexChanged);
            this.lvPOSUserCards.DoubleClick += new System.EventHandler(this.lvStores_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // POSUserCardsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "POSUserCardsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvPOSUserCards;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ContextButtons btnsEditAddRemove;
    }
}
