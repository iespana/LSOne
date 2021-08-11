using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationBlankOperationsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationBlankOperationsPage));
            this.lvBlankOperations = new ExtendedListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvBlankOperations
            // 
            resources.ApplyResources(this.lvBlankOperations, "lvBlankOperations");
            this.lvBlankOperations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4});
            this.lvBlankOperations.FullRowSelect = true;
            this.lvBlankOperations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvBlankOperations.HideSelection = false;
            this.lvBlankOperations.LockDrawing = false;
            this.lvBlankOperations.MultiSelect = false;
            this.lvBlankOperations.Name = "lvBlankOperations";
            this.lvBlankOperations.SortColumn = -1;
            this.lvBlankOperations.SortedBackwards = false;
            this.lvBlankOperations.UseCompatibleStateImageBehavior = false;
            this.lvBlankOperations.UseEveryOtherRowColoring = true;
            this.lvBlankOperations.View = System.Windows.Forms.View.Details;
            this.lvBlankOperations.SelectedIndexChanged += new System.EventHandler(this.lvBlankOperations_SelectedIndexChanged);
            this.lvBlankOperations.DoubleClick += new System.EventHandler(this.lvBlankOperations_DoubleClick);
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
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
            // AdministrationBlankOperationsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvBlankOperations);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "AdministrationBlankOperationsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvBlankOperations;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
