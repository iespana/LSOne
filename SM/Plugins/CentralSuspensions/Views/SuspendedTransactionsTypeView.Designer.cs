using LSOne.Controls;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
    partial class SuspendedTransactionsTypeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuspendedTransactionsTypeView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvSuspenedTransactionTypes = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvSuspenedTransactionTypes);
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvSuspenedTransactionTypes
            // 
            resources.ApplyResources(this.lvSuspenedTransactionTypes, "lvSuspenedTransactionTypes");
            this.lvSuspenedTransactionTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvSuspenedTransactionTypes.FullRowSelect = true;
            this.lvSuspenedTransactionTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvSuspenedTransactionTypes.HideSelection = false;
            this.lvSuspenedTransactionTypes.LockDrawing = false;
            this.lvSuspenedTransactionTypes.Name = "lvSuspenedTransactionTypes";
            this.lvSuspenedTransactionTypes.SortColumn = -1;
            this.lvSuspenedTransactionTypes.SortedBackwards = false;
            this.lvSuspenedTransactionTypes.UseCompatibleStateImageBehavior = false;
            this.lvSuspenedTransactionTypes.UseEveryOtherRowColoring = true;
            this.lvSuspenedTransactionTypes.View = System.Windows.Forms.View.Details;
            this.lvSuspenedTransactionTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGiftCards_ColumnClick);
            this.lvSuspenedTransactionTypes.SelectedIndexChanged += new System.EventHandler(this.lvSuspenedTransactionTypes_SelectedIndexChanged);
            this.lvSuspenedTransactionTypes.DoubleClick += new System.EventHandler(this.lvSuspenedTransactionTypes_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // SuspendedTransactionsTypeView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SuspendedTransactionsTypeView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddRemove;
        private ExtendedListView lvSuspenedTransactionTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
