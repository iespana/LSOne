using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.ViewPages
{
    partial class CurrencyExchangeRatePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyExchangeRatePage));
            this.lvValues = new LSOne.Controls.ExtendedListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnsValueButtons = new LSOne.Controls.ContextButtons();
            this.SuspendLayout();
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvValues.FullRowSelect = true;
            this.lvValues.HideSelection = false;
            this.lvValues.LockDrawing = false;
            this.lvValues.MultiSelect = false;
            this.lvValues.Name = "lvValues";
            this.lvValues.SortColumn = -1;
            this.lvValues.SortedBackwards = false;
            this.lvValues.UseCompatibleStateImageBehavior = false;
            this.lvValues.UseEveryOtherRowColoring = true;
            this.lvValues.View = System.Windows.Forms.View.Details;
            this.lvValues.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvValues_ColumnClick);
            this.lvValues.SelectedIndexChanged += new System.EventHandler(this.lvValues_SelectedIndexChanged);
            this.lvValues.DoubleClick += new System.EventHandler(this.lvValues_DoubleClick);
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // btnsValueButtons
            // 
            this.btnsValueButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsValueButtons, "btnsValueButtons");
            this.btnsValueButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsValueButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsValueButtons.EditButtonEnabled = true;
            this.btnsValueButtons.Name = "btnsValueButtons";
            this.btnsValueButtons.RemoveButtonEnabled = true;
            this.btnsValueButtons.EditButtonClicked += new System.EventHandler(this.btnEditValue_Click);
            this.btnsValueButtons.AddButtonClicked += new System.EventHandler(this.btnAddValue_Click);
            this.btnsValueButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemoveValue_Click);
            // 
            // CurrencyExchangeRatePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnsValueButtons);
            this.Controls.Add(this.lvValues);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CurrencyExchangeRatePage";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvValues;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private ContextButtons btnsValueButtons;

    }
}
