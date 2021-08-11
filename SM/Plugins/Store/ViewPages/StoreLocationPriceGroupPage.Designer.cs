using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class StoreLocationPriceGroupPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreLocationPriceGroupPage));
            this.label1 = new System.Windows.Forms.Label();
            this.btnsAddRemove = new ContextButtons();
            this.lvPriceGroups = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnMoveUp = new ContextButton();
            this.btnMoveDown = new ContextButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnsAddRemove
            // 
            this.btnsAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddRemove, "btnsAddRemove");
            this.btnsAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddRemove.Context = ButtonTypes.AddRemove;
            this.btnsAddRemove.EditButtonEnabled = false;
            this.btnsAddRemove.Name = "btnsAddRemove";
            this.btnsAddRemove.RemoveButtonEnabled = false;
            this.btnsAddRemove.AddButtonClicked += new System.EventHandler(this.btnsAddRemove_AddButtonClicked);
            this.btnsAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsAddRemove_RemoveButtonClicked);
            // 
            // lvPriceGroups
            // 
            resources.ApplyResources(this.lvPriceGroups, "lvPriceGroups");
            this.lvPriceGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPriceGroups.FullRowSelect = true;
            this.lvPriceGroups.LockDrawing = false;
            this.lvPriceGroups.MultiSelect = false;
            this.lvPriceGroups.Name = "lvPriceGroups";
            this.lvPriceGroups.SortColumn = -1;
            this.lvPriceGroups.SortedBackwards = false;
            this.lvPriceGroups.UseCompatibleStateImageBehavior = false;
            this.lvPriceGroups.UseEveryOtherRowColoring = true;
            this.lvPriceGroups.View = System.Windows.Forms.View.Details;
            this.lvPriceGroups.SelectedIndexChanged += new System.EventHandler(this.lvPriceGroups_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnMoveUp
            // 
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Context = ButtonType.MoveUp;
            this.btnMoveUp.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnMoveUp.MinimumSize = new System.Drawing.Size(24, 24);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Context = ButtonType.MoveDown;
            this.btnMoveDown.MaximumSize = new System.Drawing.Size(24, 24);
            this.btnMoveDown.MinimumSize = new System.Drawing.Size(24, 24);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // StoreLocationPriceGroupPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnsAddRemove);
            this.Controls.Add(this.lvPriceGroups);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "StoreLocationPriceGroupPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ExtendedListView lvPriceGroups;
        private ContextButtons btnsAddRemove;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButton btnMoveUp;
        private ContextButton btnMoveDown;
    }
}
