namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class VisualProfileGeneralPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualProfileGeneralPage));
            this.cmbColumns = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRows = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbChitRefresh = new LSOne.Controls.NumericTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbChitSize = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbColumns
            // 
            this.cmbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColumns.FormattingEnabled = true;
            this.cmbColumns.Items.AddRange(new object[] {
            resources.GetString("cmbColumns.Items"),
            resources.GetString("cmbColumns.Items1"),
            resources.GetString("cmbColumns.Items2"),
            resources.GetString("cmbColumns.Items3"),
            resources.GetString("cmbColumns.Items4"),
            resources.GetString("cmbColumns.Items5"),
            resources.GetString("cmbColumns.Items6"),
            resources.GetString("cmbColumns.Items7"),
            resources.GetString("cmbColumns.Items8"),
            resources.GetString("cmbColumns.Items9")});
            resources.ApplyResources(this.cmbColumns, "cmbColumns");
            this.cmbColumns.Name = "cmbColumns";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbRows
            // 
            this.cmbRows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRows.FormattingEnabled = true;
            this.cmbRows.Items.AddRange(new object[] {
            resources.GetString("cmbRows.Items"),
            resources.GetString("cmbRows.Items1"),
            resources.GetString("cmbRows.Items2"),
            resources.GetString("cmbRows.Items3"),
            resources.GetString("cmbRows.Items4"),
            resources.GetString("cmbRows.Items5"),
            resources.GetString("cmbRows.Items6"),
            resources.GetString("cmbRows.Items7"),
            resources.GetString("cmbRows.Items8"),
            resources.GetString("cmbRows.Items9")});
            resources.ApplyResources(this.cmbRows, "cmbRows");
            this.cmbRows.Name = "cmbRows";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbChitRefresh
            // 
            this.tbChitRefresh.AllowDecimal = false;
            this.tbChitRefresh.AllowNegative = false;
            this.tbChitRefresh.CultureInfo = null;
            this.tbChitRefresh.DecimalLetters = 0;
            this.tbChitRefresh.ForeColor = System.Drawing.Color.Black;
            this.tbChitRefresh.HasMinValue = false;
            resources.ApplyResources(this.tbChitRefresh, "tbChitRefresh");
            this.tbChitRefresh.MaxValue = 100D;
            this.tbChitRefresh.MinValue = 1D;
            this.tbChitRefresh.Name = "tbChitRefresh";
            this.tbChitRefresh.Value = 1D;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbChitSize
            // 
            this.cmbChitSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChitSize.FormattingEnabled = true;
            this.cmbChitSize.Items.AddRange(new object[] {
            resources.GetString("cmbChitSize.Items"),
            resources.GetString("cmbChitSize.Items1")});
            resources.ApplyResources(this.cmbChitSize, "cmbChitSize");
            this.cmbChitSize.Name = "cmbChitSize";
            // 
            // VisualProfileGeneralPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbChitSize);
            this.Controls.Add(this.tbChitRefresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbRows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbColumns);
            this.Controls.Add(this.label4);
            this.Name = "VisualProfileGeneralPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbColumns;
        private System.Windows.Forms.ComboBox cmbRows;
        private System.Windows.Forms.Label label1;
        private LSOne.Controls.NumericTextBox tbChitRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbChitSize;
    }
}
