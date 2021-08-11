using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    partial class StoreTerminalsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTerminalsPage));
            this.lvTerminals = new ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnContextButtons = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvTerminals
            // 
            resources.ApplyResources(this.lvTerminals, "lvTerminals");
            this.lvTerminals.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvTerminals.FullRowSelect = true;
            this.lvTerminals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTerminals.HideSelection = false;
            this.lvTerminals.LockDrawing = false;
            this.lvTerminals.MultiSelect = false;
            this.lvTerminals.Name = "lvTerminals";
            this.lvTerminals.SortColumn = -1;
            this.lvTerminals.SortedBackwards = false;
            this.lvTerminals.UseCompatibleStateImageBehavior = false;
            this.lvTerminals.UseEveryOtherRowColoring = true;
            this.lvTerminals.View = System.Windows.Forms.View.Details;
            this.lvTerminals.SelectedIndexChanged += new System.EventHandler(this.lvTerminals_SelectedIndexChanged);
            this.lvTerminals.DoubleClick += new System.EventHandler(this.lvTerminals_DoubleClick);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // btnContextButtons
            // 
            this.btnContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnContextButtons, "btnContextButtons");
            this.btnContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnContextButtons.EditButtonEnabled = true;
            this.btnContextButtons.Name = "btnContextButtons";
            this.btnContextButtons.RemoveButtonEnabled = true;
            this.btnContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // StoreTerminalsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnContextButtons);
            this.Controls.Add(this.lvTerminals);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "StoreTerminalsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvTerminals;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnContextButtons;
    }
}
