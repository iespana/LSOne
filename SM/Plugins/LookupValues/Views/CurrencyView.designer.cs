using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class CurrencyView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyView));
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvCurrencies = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.btnEditCompanyCurrency = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEditCompanyCurrency);
            this.pnlBottom.Controls.Add(this.lvCurrencies);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.groupPanel1);
            // 
            // groupPanel1
            // 
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Controls.Add(this.tabSheetTabs);
            this.groupPanel1.Controls.Add(this.lblNoSelection);
            this.groupPanel1.Name = "groupPanel1";
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvCurrencies
            // 
            resources.ApplyResources(this.lvCurrencies, "lvCurrencies");
            this.lvCurrencies.BuddyControl = null;
            this.lvCurrencies.Columns.Add(this.column1);
            this.lvCurrencies.Columns.Add(this.column2);
            this.lvCurrencies.Columns.Add(this.column3);
            this.lvCurrencies.ContentBackColor = System.Drawing.Color.White;
            this.lvCurrencies.DefaultRowHeight = ((short)(22));
            this.lvCurrencies.DimSelectionWhenDisabled = true;
            this.lvCurrencies.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCurrencies.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCurrencies.HeaderHeight = ((short)(25));
            this.lvCurrencies.Name = "lvCurrencies";
            this.lvCurrencies.OddRowColor = System.Drawing.Color.White;
            this.lvCurrencies.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCurrencies.SecondarySortColumn = ((short)(-1));
            this.lvCurrencies.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCurrencies.SortSetting = "0:1";
            this.lvCurrencies.SelectionChanged += new System.EventHandler(this.lvCurrencies_SelectionChanged);
            this.lvCurrencies.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvCurrencies_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Tag = null;
            this.column1.Width = ((short)(100));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.Tag = null;
            this.column2.Width = ((short)(100));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Tag = null;
            this.column3.Width = ((short)(100));
            // 
            // btnEditCompanyCurrency
            // 
            resources.ApplyResources(this.btnEditCompanyCurrency, "btnEditCompanyCurrency");
            this.btnEditCompanyCurrency.Name = "btnEditCompanyCurrency";
            this.btnEditCompanyCurrency.UseVisualStyleBackColor = true;
            this.btnEditCompanyCurrency.Click += new System.EventHandler(this.btnEditCompanyCurrency_Click);
            // 
            // CurrencyView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CurrencyView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblNoSelection;
        private TabControl tabSheetTabs;
        private ContextButtons btnsContextButtons;
        private LSOne.Controls.ListView lvCurrencies;
        private LSOne.Controls.Columns.Column column1;
        private LSOne.Controls.Columns.Column column2;
        private LSOne.Controls.Columns.Column column3;
        private System.Windows.Forms.Button btnEditCompanyCurrency;

    }
}
