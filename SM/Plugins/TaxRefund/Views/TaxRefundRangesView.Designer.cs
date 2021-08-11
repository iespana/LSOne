namespace LSOne.ViewPlugins.TaxRefund.Views
{
    partial class TaxRefundRangesView
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
            this.lvTaxRefundRanges = new LSOne.Controls.ListView();
            this.colValueFrom = new LSOne.Controls.Columns.Column();
            this.colValueTo = new LSOne.Controls.Columns.Column();
            this.colTaxRefund = new LSOne.Controls.Columns.Column();
            this.colTaxRefundPct = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvTaxRefundRanges);
            // 
            // lvTaxRefundRanges
            // 
            this.lvTaxRefundRanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTaxRefundRanges.BuddyControl = null;
            this.lvTaxRefundRanges.Columns.Add(this.colValueFrom);
            this.lvTaxRefundRanges.Columns.Add(this.colValueTo);
            this.lvTaxRefundRanges.Columns.Add(this.colTaxRefund);
            this.lvTaxRefundRanges.Columns.Add(this.colTaxRefundPct);
            this.lvTaxRefundRanges.ContentBackColor = System.Drawing.Color.White;
            this.lvTaxRefundRanges.DefaultRowHeight = ((short)(22));
            this.lvTaxRefundRanges.EvenRowColor = System.Drawing.Color.White;
            this.lvTaxRefundRanges.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTaxRefundRanges.HeaderHeight = ((short)(25));
            this.lvTaxRefundRanges.Location = new System.Drawing.Point(18, 5);
            this.lvTaxRefundRanges.Name = "lvTaxRefundRanges";
            this.lvTaxRefundRanges.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTaxRefundRanges.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTaxRefundRanges.SecondarySortColumn = ((short)(-1));
            this.lvTaxRefundRanges.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvTaxRefundRanges.Size = new System.Drawing.Size(508, 479);
            this.lvTaxRefundRanges.SortSetting = "0:1";
            this.lvTaxRefundRanges.TabIndex = 1;
            this.lvTaxRefundRanges.SelectionChanged += new System.EventHandler(this.OnListSelectionChanged);
            this.lvTaxRefundRanges.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.OnListRowDoubleClick);
            // 
            // colValueFrom
            // 
            this.colValueFrom.AutoSize = true;
            this.colValueFrom.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colValueFrom.DefaultStyle = null;
            this.colValueFrom.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colValueFrom.HeaderText = "From";
            this.colValueFrom.InternalSort = true;
            this.colValueFrom.MaximumWidth = ((short)(0));
            this.colValueFrom.MinimumWidth = ((short)(150));
            this.colValueFrom.Tag = null;
            this.colValueFrom.Width = ((short)(150));
            // 
            // colValueTo
            // 
            this.colValueTo.AutoSize = true;
            this.colValueTo.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colValueTo.DefaultStyle = null;
            this.colValueTo.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colValueTo.HeaderText = "To";
            this.colValueTo.InternalSort = true;
            this.colValueTo.MaximumWidth = ((short)(0));
            this.colValueTo.MinimumWidth = ((short)(150));
            this.colValueTo.Tag = null;
            this.colValueTo.Width = ((short)(150));
            // 
            // colTaxRefund
            // 
            this.colTaxRefund.AutoSize = true;
            this.colTaxRefund.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colTaxRefund.DefaultStyle = null;
            this.colTaxRefund.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colTaxRefund.HeaderText = "Tax refund";
            this.colTaxRefund.InternalSort = true;
            this.colTaxRefund.MaximumWidth = ((short)(0));
            this.colTaxRefund.MinimumWidth = ((short)(150));
            this.colTaxRefund.Tag = null;
            this.colTaxRefund.Width = ((short)(150));
            // 
            // colTaxRefundPct
            // 
            this.colTaxRefundPct.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colTaxRefundPct.DefaultStyle = null;
            this.colTaxRefundPct.HeaderHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.colTaxRefundPct.HeaderText = "Tax refund %";
            this.colTaxRefundPct.InternalSort = true;
            this.colTaxRefundPct.MaximumWidth = ((short)(0));
            this.colTaxRefundPct.MinimumWidth = ((short)(10));
            this.colTaxRefundPct.Tag = null;
            this.colTaxRefundPct.Width = ((short)(100));
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            this.btnsContextButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Location = new System.Drawing.Point(442, 490);
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.Size = new System.Drawing.Size(84, 24);
            this.btnsContextButtons.TabIndex = 2;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // TaxRefundRangesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TaxRefundRangesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvTaxRefundRanges;
        private Controls.ContextButtons btnsContextButtons;
        private Controls.Columns.Column colValueFrom;
        private Controls.Columns.Column colValueTo;
        private Controls.Columns.Column colTaxRefund;
        private Controls.Columns.Column colTaxRefundPct;
    }
}
