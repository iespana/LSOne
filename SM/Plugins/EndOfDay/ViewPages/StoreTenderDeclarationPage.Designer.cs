using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    partial class StoreTenderDeclarationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreTenderDeclarationPage));
            this.lvTenderDeclarations = new ExtendedListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsContextButtons = new ContextButtons();
            this.SuspendLayout();
            // 
            // lvTenderDeclarations
            // 
            resources.ApplyResources(this.lvTenderDeclarations, "lvTenderDeclarations");
            this.lvTenderDeclarations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader1,
            this.columnHeader2});
            this.lvTenderDeclarations.FullRowSelect = true;
            this.lvTenderDeclarations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTenderDeclarations.HideSelection = false;
            this.lvTenderDeclarations.LockDrawing = false;
            this.lvTenderDeclarations.MultiSelect = false;
            this.lvTenderDeclarations.Name = "lvTenderDeclarations";
            this.lvTenderDeclarations.SortColumn = -1;
            this.lvTenderDeclarations.SortedBackwards = false;
            this.lvTenderDeclarations.UseCompatibleStateImageBehavior = false;
            this.lvTenderDeclarations.UseEveryOtherRowColoring = true;
            this.lvTenderDeclarations.View = System.Windows.Forms.View.Details;
            this.lvTenderDeclarations.SelectedIndexChanged += new System.EventHandler(this.lvTenderDeclarations_SelectedIndexChanged);
            this.lvTenderDeclarations.DoubleClick += new System.EventHandler(this.lvTenderDeclarations_DoubleClick);
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
            // StoreTenderDeclarationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvTenderDeclarations);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "StoreTenderDeclarationPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvTenderDeclarations;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
