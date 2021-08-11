namespace LSOne.ViewPlugins.RMSMigration.Dialogs
{
    partial class ItemLogForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemLogForm));
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.errorMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importLogItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importLogItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            this.dataGridViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLog.AutoGenerateColumns = false;
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.errorMessageDataGridViewTextBoxColumn});
            this.dataGridViewLog.DataSource = this.importLogItemBindingSource;
            this.dataGridViewLog.Location = new System.Drawing.Point(36, 41);
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.Size = new System.Drawing.Size(614, 443);
            this.dataGridViewLog.TabIndex = 0;
            // 
            // errorMessageDataGridViewTextBoxColumn
            // 
            this.errorMessageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.errorMessageDataGridViewTextBoxColumn.DataPropertyName = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.HeaderText = "Log message";
            this.errorMessageDataGridViewTextBoxColumn.Name = "errorMessageDataGridViewTextBoxColumn";
            this.errorMessageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // importLogItemBindingSource
            // 
            this.importLogItemBindingSource.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.Import.ImportLogItem);
            // 
            // ItemLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 518);
            this.Controls.Add(this.dataGridViewLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ItemLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importLogItemBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.BindingSource importLogItemBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessageDataGridViewTextBoxColumn;
    }
}