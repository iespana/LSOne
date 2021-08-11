using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.Views
{
    partial class AccessTokensView
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
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvIntegrationFrameworkTokens = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvIntegrationFrameworkTokens);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            this.btnsContextButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Location = new System.Drawing.Point(440, 481);
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.Size = new System.Drawing.Size(84, 24);
            this.btnsContextButtons.TabIndex = 1;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvIntegrationFrameworkTokens
            // 
            this.lvIntegrationFrameworkTokens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvIntegrationFrameworkTokens.BuddyControl = null;
            this.lvIntegrationFrameworkTokens.Columns.Add(this.column1);
            this.lvIntegrationFrameworkTokens.Columns.Add(this.column2);
            this.lvIntegrationFrameworkTokens.Columns.Add(this.column3);
            this.lvIntegrationFrameworkTokens.Columns.Add(this.column4);
            this.lvIntegrationFrameworkTokens.Columns.Add(this.column5);
            this.lvIntegrationFrameworkTokens.ContentBackColor = System.Drawing.Color.White;
            this.lvIntegrationFrameworkTokens.DefaultRowHeight = ((short)(22));
            this.lvIntegrationFrameworkTokens.DimSelectionWhenDisabled = true;
            this.lvIntegrationFrameworkTokens.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvIntegrationFrameworkTokens.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvIntegrationFrameworkTokens.HeaderHeight = ((short)(25));
            this.lvIntegrationFrameworkTokens.HorizontalScrollbar = true;
            this.lvIntegrationFrameworkTokens.Location = new System.Drawing.Point(18, 17);
            this.lvIntegrationFrameworkTokens.Name = "lvIntegrationFrameworkTokens";
            this.lvIntegrationFrameworkTokens.OddRowColor = System.Drawing.Color.White;
            this.lvIntegrationFrameworkTokens.RowLineColor = System.Drawing.Color.LightGray;
            this.lvIntegrationFrameworkTokens.SecondarySortColumn = ((short)(-1));
            this.lvIntegrationFrameworkTokens.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvIntegrationFrameworkTokens.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvIntegrationFrameworkTokens.Size = new System.Drawing.Size(506, 458);
            this.lvIntegrationFrameworkTokens.SortSetting = "0:1";
            this.lvIntegrationFrameworkTokens.TabIndex = 0;
            this.lvIntegrationFrameworkTokens.SelectionChanged += new System.EventHandler(this.lvTransactionServiceProfiles_SelectionChanged);
            this.lvIntegrationFrameworkTokens.DoubleClick += new System.EventHandler(this.lvTransactionServiceProfiles_DoubleClick_1);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            this.column1.HeaderText = "Description";
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            this.column2.HeaderText = "Sender";
            this.column2.InternalSort = true;
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
            this.column3.HeaderText = "User";
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            this.column4.HeaderText = "Store";
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            this.column5.HeaderText = "Status";
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // AccessTokensView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AccessTokensView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsContextButtons;
        private ListView lvIntegrationFrameworkTokens;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
    }
}
