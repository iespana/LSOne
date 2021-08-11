using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    partial class ItemSalesPricePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSalesPricePage));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnViewCustomer = new System.Windows.Forms.Button();
            this.btnViewGroup = new System.Windows.Forms.Button();
            this.lvItemAgreements = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.column8 = new LSOne.Controls.Columns.Column();
            this.column9 = new LSOne.Controls.Columns.Column();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.btnViewCustomer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnViewGroup, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // btnViewCustomer
            // 
            resources.ApplyResources(this.btnViewCustomer, "btnViewCustomer");
            this.btnViewCustomer.Name = "btnViewCustomer";
            this.btnViewCustomer.UseVisualStyleBackColor = true;
            this.btnViewCustomer.Click += new System.EventHandler(this.btnViewCustomer_Click);
            // 
            // btnViewGroup
            // 
            resources.ApplyResources(this.btnViewGroup, "btnViewGroup");
            this.btnViewGroup.Name = "btnViewGroup";
            this.btnViewGroup.UseVisualStyleBackColor = true;
            this.btnViewGroup.Click += new System.EventHandler(this.btnViewGroup_Click);
            // 
            // lvItemAgreements
            // 
            resources.ApplyResources(this.lvItemAgreements, "lvItemAgreements");
            this.lvItemAgreements.BuddyControl = null;
            this.lvItemAgreements.Columns.Add(this.column1);
            this.lvItemAgreements.Columns.Add(this.column2);
            this.lvItemAgreements.Columns.Add(this.column3);
            this.lvItemAgreements.Columns.Add(this.column4);
            this.lvItemAgreements.Columns.Add(this.column5);
            this.lvItemAgreements.Columns.Add(this.column6);
            this.lvItemAgreements.Columns.Add(this.column7);
            this.lvItemAgreements.Columns.Add(this.column8);
            this.lvItemAgreements.Columns.Add(this.column9);
            this.lvItemAgreements.ContentBackColor = System.Drawing.Color.White;
            this.lvItemAgreements.DefaultRowHeight = ((short)(22));
            this.lvItemAgreements.DimSelectionWhenDisabled = true;
            this.lvItemAgreements.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItemAgreements.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItemAgreements.HeaderHeight = ((short)(25));
            this.lvItemAgreements.HorizontalScrollbar = true;
            this.lvItemAgreements.Name = "lvItemAgreements";
            this.lvItemAgreements.OddRowColor = System.Drawing.Color.White;
            this.lvItemAgreements.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItemAgreements.SecondarySortColumn = ((short)(-1));
            this.lvItemAgreements.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItemAgreements.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItemAgreements.SortSetting = "0:1";
            this.lvItemAgreements.SelectionChanged += new System.EventHandler(this.lvItemAgreements_SelectionChanged);
            this.lvItemAgreements.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItemAgreements_CellAction);
            this.lvItemAgreements.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItemAgreements_RowDoubleClick);
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
            this.column3.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.NoTextWhenSmall = true;
            this.column3.Tag = null;
            this.column3.Width = ((short)(60));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.Tag = null;
            this.column4.Width = ((short)(40));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.Tag = null;
            this.column5.Width = ((short)(60));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.Tag = null;
            this.column6.Width = ((short)(60));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.Tag = null;
            this.column7.Width = ((short)(60));
            // 
            // column8
            // 
            this.column8.AutoSize = true;
            this.column8.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column8.DefaultStyle = null;
            resources.ApplyResources(this.column8, "column8");
            this.column8.InternalSort = true;
            this.column8.MaximumWidth = ((short)(0));
            this.column8.MinimumWidth = ((short)(10));
            this.column8.Tag = null;
            this.column8.Width = ((short)(60));
            // 
            // column9
            // 
            this.column9.AutoSize = true;
            this.column9.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.column9.DefaultStyle = null;
            resources.ApplyResources(this.column9, "column9");
            this.column9.InternalSort = true;
            this.column9.MaximumWidth = ((short)(0));
            this.column9.MinimumWidth = ((short)(10));
            this.column9.Tag = null;
            this.column9.Width = ((short)(60));
            // 
            // ItemSalesPricePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvItemAgreements);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "ItemSalesPricePage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnViewCustomer;
        private System.Windows.Forms.Button btnViewGroup;
        private ListView lvItemAgreements;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.Columns.Column column8;
        private Controls.Columns.Column column9;
    }
}
