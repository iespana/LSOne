using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class SalesTypesView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesTypesView));
            this.lvSalesTypes = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsEditAddRemove = new ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvSalesTypes);
            // 
            // lvSalesTypes
            // 
            resources.ApplyResources(this.lvSalesTypes, "lvSalesTypes");
            this.lvSalesTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvSalesTypes.FullRowSelect = true;
            this.lvSalesTypes.HideSelection = false;
            this.lvSalesTypes.LockDrawing = false;
            this.lvSalesTypes.MultiSelect = false;
            this.lvSalesTypes.Name = "lvSalesTypes";
            this.lvSalesTypes.SortColumn = -1;
            this.lvSalesTypes.SortedBackwards = false;
            this.lvSalesTypes.UseCompatibleStateImageBehavior = false;
            this.lvSalesTypes.UseEveryOtherRowColoring = true;
            this.lvSalesTypes.View = System.Windows.Forms.View.Details;
            this.lvSalesTypes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSalesTypes_ColumnClick);
            this.lvSalesTypes.SelectedIndexChanged += new System.EventHandler(this.lvSalesTypes_SelectedIndexChanged);
            this.lvSalesTypes.DoubleClick += new System.EventHandler(this.lvSalesTypes_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
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
            // SalesTypesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SalesTypesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvSalesTypes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private ContextButtons btnsEditAddRemove;

    }
}