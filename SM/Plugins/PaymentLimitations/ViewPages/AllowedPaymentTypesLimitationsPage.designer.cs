using LSOne.Controls;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    partial class AllowedPaymentTypesLimitationsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllowedPaymentTypesLimitationsPage));
            this.lvLimitations = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.chkForceRefundToThisPaymentType = new System.Windows.Forms.CheckBox();
            this.lblForceRefundToThisPaymentType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvLimitations
            // 
            resources.ApplyResources(this.lvLimitations, "lvLimitations");
            this.lvLimitations.BuddyControl = null;
            this.lvLimitations.Columns.Add(this.column1);
            this.lvLimitations.Columns.Add(this.column2);
            this.lvLimitations.Columns.Add(this.column3);
            this.lvLimitations.ContentBackColor = System.Drawing.Color.White;
            this.lvLimitations.DefaultRowHeight = ((short)(18));
            this.lvLimitations.DimSelectionWhenDisabled = true;
            this.lvLimitations.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLimitations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLimitations.HeaderHeight = ((short)(25));
            this.lvLimitations.HorizontalScrollbar = true;
            this.lvLimitations.Name = "lvLimitations";
            this.lvLimitations.OddRowColor = System.Drawing.Color.White;
            this.lvLimitations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLimitations.SecondarySortColumn = ((short)(-1));
            this.lvLimitations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLimitations.SortSetting = "0:1";
            this.lvLimitations.CellAction += new LSOne.Controls.CellActionDelegate(this.LvLimitations_CellAction);
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
            // chkForceRefundToThisPaymentType
            // 
            resources.ApplyResources(this.chkForceRefundToThisPaymentType, "chkForceRefundToThisPaymentType");
            this.chkForceRefundToThisPaymentType.Name = "chkForceRefundToThisPaymentType";
            this.chkForceRefundToThisPaymentType.UseVisualStyleBackColor = true;
            // 
            // lblForceRefundToThisPaymentType
            // 
            this.lblForceRefundToThisPaymentType.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblForceRefundToThisPaymentType, "lblForceRefundToThisPaymentType");
            this.lblForceRefundToThisPaymentType.Name = "lblForceRefundToThisPaymentType";
            // 
            // AllowedPaymentTypesLimitationsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkForceRefundToThisPaymentType);
            this.Controls.Add(this.lblForceRefundToThisPaymentType);
            this.Controls.Add(this.lvLimitations);
            this.DoubleBuffered = true;
            this.Name = "AllowedPaymentTypesLimitationsPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ListView lvLimitations;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private System.Windows.Forms.CheckBox chkForceRefundToThisPaymentType;
        private System.Windows.Forms.Label lblForceRefundToThisPaymentType;
    }
}
