using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Inventory.Views
{
    partial class InventoryInTransitView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryInTransitView));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvInventoryInTransfer = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.btnViewInventoryTransfer = new System.Windows.Forms.Button();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnViewInventoryTransfer);
            this.pnlBottom.Controls.Add(this.lvInventoryInTransfer);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lvInventoryInTransfer
            // 
            resources.ApplyResources(this.lvInventoryInTransfer, "lvInventoryInTransfer");
            this.lvInventoryInTransfer.BuddyControl = null;
            this.lvInventoryInTransfer.Columns.Add(this.column1);
            this.lvInventoryInTransfer.Columns.Add(this.column6);
            this.lvInventoryInTransfer.Columns.Add(this.column4);
            this.lvInventoryInTransfer.Columns.Add(this.column2);
            this.lvInventoryInTransfer.Columns.Add(this.column3);
            this.lvInventoryInTransfer.Columns.Add(this.column5);
            this.lvInventoryInTransfer.ContentBackColor = System.Drawing.Color.White;
            this.lvInventoryInTransfer.DefaultRowHeight = ((short)(22));
            this.lvInventoryInTransfer.DimSelectionWhenDisabled = true;
            this.lvInventoryInTransfer.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvInventoryInTransfer.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvInventoryInTransfer.HeaderHeight = ((short)(25));
            this.lvInventoryInTransfer.Name = "lvInventoryInTransfer";
            this.lvInventoryInTransfer.OddRowColor = System.Drawing.Color.White;
            this.lvInventoryInTransfer.RowLineColor = System.Drawing.Color.LightGray;
            this.lvInventoryInTransfer.SecondarySortColumn = ((short)(-1));
            this.lvInventoryInTransfer.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvInventoryInTransfer.SortSetting = "0:1";
            this.lvInventoryInTransfer.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvInventoryInTransfer_HeaderClicked);
            this.lvInventoryInTransfer.SelectionChanged += new System.EventHandler(this.lvInventoryInTransfer_SelectionChanged);
            this.lvInventoryInTransfer.DoubleClick += new System.EventHandler(this.lvInventoryInTransfer_DoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // btnViewInventoryTransfer
            // 
            resources.ApplyResources(this.btnViewInventoryTransfer, "btnViewInventoryTransfer");
            this.btnViewInventoryTransfer.Name = "btnViewInventoryTransfer";
            this.btnViewInventoryTransfer.UseVisualStyleBackColor = true;
            this.btnViewInventoryTransfer.Click += new System.EventHandler(this.btnViewInventoryTransfer_Click);
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // InventoryInTransitView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InventoryInTransitView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ListView lvInventoryInTransfer;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private System.Windows.Forms.Button btnViewInventoryTransfer;
        private Column column6;
        //private System.Windows.Forms.ComboBox comboBox1;

    }
}
