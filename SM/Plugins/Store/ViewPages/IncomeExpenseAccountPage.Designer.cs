using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    partial class IncomeExpenseAccountPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncomeExpenseAccountPage));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIncomeAccount = new System.Windows.Forms.ComboBox();
            this.lvIncomeExpense = new LSOne.Controls.ExtendedListView();
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbIncomeAccount
            // 
            this.cmbIncomeAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIncomeAccount.FormattingEnabled = true;
            this.cmbIncomeAccount.Items.AddRange(new object[] {
            resources.GetString("cmbIncomeAccount.Items"),
            resources.GetString("cmbIncomeAccount.Items1"),
            resources.GetString("cmbIncomeAccount.Items2")});
            resources.ApplyResources(this.cmbIncomeAccount, "cmbIncomeAccount");
            this.cmbIncomeAccount.Name = "cmbIncomeAccount";
            this.cmbIncomeAccount.SelectedIndexChanged += new System.EventHandler(this.cmbIncomeAccount_SelectedIndexChanged);
            // 
            // lvIncomeExpense
            // 
            resources.ApplyResources(this.lvIncomeExpense, "lvIncomeExpense");
            this.lvIncomeExpense.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader1,
            this.columnHeader8});
            this.lvIncomeExpense.FullRowSelect = true;
            this.lvIncomeExpense.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvIncomeExpense.HideSelection = false;
            this.lvIncomeExpense.LockDrawing = false;
            this.lvIncomeExpense.MultiSelect = false;
            this.lvIncomeExpense.Name = "lvIncomeExpense";
            this.lvIncomeExpense.SortColumn = -1;
            this.lvIncomeExpense.SortedBackwards = false;
            this.lvIncomeExpense.UseCompatibleStateImageBehavior = false;
            this.lvIncomeExpense.UseEveryOtherRowColoring = true;
            this.lvIncomeExpense.View = System.Windows.Forms.View.Details;
            this.lvIncomeExpense.SelectedIndexChanged += new System.EventHandler(this.lvIncomeExpense_SelectedIndexChanged);
            this.lvIncomeExpense.DoubleClick += new System.EventHandler(this.lvIncomeExpense_DoubleClick);
            // 
            // columnHeader13
            // 
            resources.ApplyResources(this.columnHeader13, "columnHeader13");
            // 
            // columnHeader14
            // 
            resources.ApplyResources(this.columnHeader14, "columnHeader14");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader8
            // 
            resources.ApplyResources(this.columnHeader8, "columnHeader8");
            // 
            // IncomeExpenseAccountPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbIncomeAccount);
            this.Controls.Add(this.lvIncomeExpense);
            this.Controls.Add(this.btnsContextButtons);
            this.DoubleBuffered = true;
            this.Name = "IncomeExpenseAccountPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbIncomeAccount;
        private ExtendedListView lvIncomeExpense;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}
