using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    partial class PurchaseWorksheetsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseWorksheetsView));
            this.lvPurchaseWorksheets = new ListView();
            this.column1 = new Column();
            this.column3 = new Column();
            this.column5 = new Column();
            this.btnEdit = new ContextButton();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEdit);
            this.pnlBottom.Controls.Add(this.lvPurchaseWorksheets);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // lvPurchaseWorksheets
            // 
            resources.ApplyResources(this.lvPurchaseWorksheets, "lvPurchaseWorksheets");
            this.lvPurchaseWorksheets.BuddyControl = null;
            this.lvPurchaseWorksheets.Columns.Add(this.column1);
            this.lvPurchaseWorksheets.Columns.Add(this.column3);
            this.lvPurchaseWorksheets.Columns.Add(this.column5);
            this.lvPurchaseWorksheets.ContentBackColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheets.DefaultRowHeight = ((short)(22));
            this.lvPurchaseWorksheets.DimSelectionWhenDisabled = true;
            this.lvPurchaseWorksheets.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPurchaseWorksheets.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvPurchaseWorksheets.HeaderHeight = ((short)(25));
            this.lvPurchaseWorksheets.Name = "lvPurchaseWorksheets";
            this.lvPurchaseWorksheets.OddRowColor = System.Drawing.Color.White;
            this.lvPurchaseWorksheets.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPurchaseWorksheets.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvPurchaseWorksheets.SortSetting = "0:1";
            this.lvPurchaseWorksheets.SelectionChanged += new System.EventHandler(this.lvPurchaseWorksheets_SelectionChanged);
            this.lvPurchaseWorksheets.RowDoubleClick += new RowClickDelegate(this.lvPurchaseWorksheets_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 0;
            this.column1.Sizable = true;
            this.column1.Tag = null;
            this.column1.Width = ((short)(100));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Sizable = true;
            this.column3.Tag = null;
            this.column3.Width = ((short)(100));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.Sizable = true;
            this.column5.Tag = null;
            this.column5.Width = ((short)(100));
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = ButtonType.Edit;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // PurchaseWorksheetsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PurchaseWorksheetsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvPurchaseWorksheets;
        private Column column1;
        private Column column3;
        private Column column5;
        private ContextButton btnEdit;

    }
}
