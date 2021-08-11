using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemSpecialGroupsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSpecialGroupsPage));
            this.lvSpecialGroups = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.btnAdd = new LSOne.Controls.ContextButton();
            this.SuspendLayout();
            // 
            // lvSpecialGroups
            // 
            resources.ApplyResources(this.lvSpecialGroups, "lvSpecialGroups");
            this.lvSpecialGroups.BuddyControl = null;
            this.lvSpecialGroups.Columns.Add(this.column1);
            this.lvSpecialGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvSpecialGroups.DefaultRowHeight = ((short)(22));
            this.lvSpecialGroups.DimSelectionWhenDisabled = true;
            this.lvSpecialGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSpecialGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSpecialGroups.HeaderHeight = ((short)(25));
            this.lvSpecialGroups.Name = "lvSpecialGroups";
            this.lvSpecialGroups.OddRowColor = System.Drawing.Color.White;
            this.lvSpecialGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSpecialGroups.SecondarySortColumn = ((short)(-1));
            this.lvSpecialGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSpecialGroups.SortSetting = "0:1";
            this.lvSpecialGroups.CellAction += new LSOne.Controls.CellActionDelegate(this.lvSpecialGroups_CellAction);
            // 
            // column1
            // 
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 100;
            this.column1.Tag = null;
            this.column1.Width = ((short)(300));
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Context = LSOne.Controls.ButtonType.Add;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ItemSpecialGroupsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvSpecialGroups);
            this.DoubleBuffered = true;
            this.Name = "ItemSpecialGroupsPage";
            this.ResumeLayout(false);

        }

        #endregion
        private ListView lvSpecialGroups;
        private Controls.Columns.Column column1;
        private LSOne.Controls.ContextButton btnAdd;
    }
}
