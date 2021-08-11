using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    partial class SpecialGroupDiscountsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecialGroupDiscountsPage));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvValues = new LSOne.Controls.ExtendedListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnShowOffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader9,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvValues.FullRowSelect = true;
            this.lvValues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvValues.HideSelection = false;
            this.lvValues.LockDrawing = false;
            this.lvValues.MultiSelect = false;
            this.lvValues.Name = "lvValues";
            this.lvValues.SortColumn = 0;
            this.lvValues.SortedBackwards = false;
            this.lvValues.UseCompatibleStateImageBehavior = false;
            this.lvValues.UseEveryOtherRowColoring = true;
            this.lvValues.View = System.Windows.Forms.View.Details;
            this.lvValues.SelectedIndexChanged += new System.EventHandler(this.lvValues_SelectedIndexChanged);
            this.lvValues.DoubleClick += new System.EventHandler(this.lvValues_DoubleClick);
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader6
            // 
            resources.ApplyResources(this.columnHeader6, "columnHeader6");
            // 
            // columnHeader9
            // 
            resources.ApplyResources(this.columnHeader9, "columnHeader9");
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
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // btnShowOffer
            // 
            resources.ApplyResources(this.btnShowOffer, "btnShowOffer");
            this.btnShowOffer.Name = "btnShowOffer";
            this.btnShowOffer.UseVisualStyleBackColor = true;
            this.btnShowOffer.Click += new System.EventHandler(this.ShowOffer);
            // 
            // SpecialGroupDiscountsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnShowOffer);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvValues);
            this.Controls.Add(this.lblNoSelection);
            this.Name = "SpecialGroupDiscountsPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private ExtendedListView lvValues;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label lblNoSelection;
        private System.Windows.Forms.Button btnShowOffer;
    }
}
