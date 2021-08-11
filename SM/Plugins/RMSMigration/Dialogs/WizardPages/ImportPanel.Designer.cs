namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    partial class ImportPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportPanel));
            this.migrationItemsGrid = new System.Windows.Forms.DataGridView();
            this.btnImport = new System.Windows.Forms.Button();
            this.storeListItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblBarCodeSetup = new System.Windows.Forms.Label();
            this.cbBarCodeSetup = new System.Windows.Forms.ComboBox();
            this.lblImagePath = new System.Windows.Forms.Label();
            this.tbImageFolderPath = new System.Windows.Forms.TextBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.shouldImportDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.itemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressStatusDisplayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rMSMigrationItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource5 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.migrationItemsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSMigrationItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource4)).BeginInit();
            this.SuspendLayout();
            // 
            // migrationItemsGrid
            // 
            this.migrationItemsGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Transparent;
            this.migrationItemsGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.migrationItemsGrid.AutoGenerateColumns = false;
            this.migrationItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.migrationItemsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shouldImportDataGridViewCheckBoxColumn,
            this.itemNameDataGridViewTextBoxColumn,
            this.progressStatusDisplayDataGridViewTextBoxColumn});
            this.migrationItemsGrid.DataSource = this.rMSMigrationItemBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.migrationItemsGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.migrationItemsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            resources.ApplyResources(this.migrationItemsGrid, "migrationItemsGrid");
            this.migrationItemsGrid.Name = "migrationItemsGrid";
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Transparent;
            this.migrationItemsGrid.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.migrationItemsGrid.RowTemplate.Height = 46;
            this.migrationItemsGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.migrationItemsGrid_CellClick);
            this.migrationItemsGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.migrationItemsGrid_CellFormatting);
            this.migrationItemsGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.migrationItemsGrid_CurrentCellDirtyStateChanged);
            this.migrationItemsGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.migrationItemsGrid_DataError);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // storeListItemBindingSource
            // 
            this.storeListItemBindingSource.DataSource = typeof(LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems.StoreListItem);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Step = 1;
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.Name = "lblInfo";
            // 
            // lblBarCodeSetup
            // 
            this.lblBarCodeSetup.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblBarCodeSetup, "lblBarCodeSetup");
            this.lblBarCodeSetup.Name = "lblBarCodeSetup";
            // 
            // cbBarCodeSetup
            // 
            this.cbBarCodeSetup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBarCodeSetup.FormattingEnabled = true;
            resources.ApplyResources(this.cbBarCodeSetup, "cbBarCodeSetup");
            this.cbBarCodeSetup.Name = "cbBarCodeSetup";
            this.cbBarCodeSetup.SelectedIndexChanged += new System.EventHandler(this.cbBarCodeSetup_SelectedIndexChanged);
            // 
            // lblImagePath
            // 
            this.lblImagePath.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblImagePath, "lblImagePath");
            this.lblImagePath.Name = "lblImagePath";
            // 
            // tbImageFolderPath
            // 
            resources.ApplyResources(this.tbImageFolderPath, "tbImageFolderPath");
            this.tbImageFolderPath.Name = "tbImageFolderPath";
            // 
            // btnSelectFolder
            // 
            resources.ApplyResources(this.btnSelectFolder, "btnSelectFolder");
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.selectFolder_Click);
            // 
            // shouldImportDataGridViewCheckBoxColumn
            // 
            this.shouldImportDataGridViewCheckBoxColumn.DataPropertyName = "ShouldImport";
            resources.ApplyResources(this.shouldImportDataGridViewCheckBoxColumn, "shouldImportDataGridViewCheckBoxColumn");
            this.shouldImportDataGridViewCheckBoxColumn.Name = "shouldImportDataGridViewCheckBoxColumn";
            // 
            // itemNameDataGridViewTextBoxColumn
            // 
            this.itemNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName";
            resources.ApplyResources(this.itemNameDataGridViewTextBoxColumn, "itemNameDataGridViewTextBoxColumn");
            this.itemNameDataGridViewTextBoxColumn.Name = "itemNameDataGridViewTextBoxColumn";
            this.itemNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // progressStatusDisplayDataGridViewTextBoxColumn
            // 
            this.progressStatusDisplayDataGridViewTextBoxColumn.DataPropertyName = "ProgressStatusDisplay";
            resources.ApplyResources(this.progressStatusDisplayDataGridViewTextBoxColumn, "progressStatusDisplayDataGridViewTextBoxColumn");
            this.progressStatusDisplayDataGridViewTextBoxColumn.Name = "progressStatusDisplayDataGridViewTextBoxColumn";
            this.progressStatusDisplayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rMSMigrationItemBindingSource
            // 
            this.rMSMigrationItemBindingSource.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSMigrationItem);
            // 
            // rMSStoreBindingSource5
            // 
            this.rMSStoreBindingSource5.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // rMSStoreBindingSource
            // 
            this.rMSStoreBindingSource.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // rMSStoreBindingSource1
            // 
            this.rMSStoreBindingSource1.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // rMSStoreBindingSource2
            // 
            this.rMSStoreBindingSource2.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // rMSStoreBindingSource3
            // 
            this.rMSStoreBindingSource3.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // rMSStoreBindingSource4
            // 
            this.rMSStoreBindingSource4.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // ImportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.tbImageFolderPath);
            this.Controls.Add(this.lblImagePath);
            this.Controls.Add(this.lblBarCodeSetup);
            this.Controls.Add(this.cbBarCodeSetup);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.migrationItemsGrid);
            this.Name = "ImportPanel";
            ((System.ComponentModel.ISupportInitialize)(this.migrationItemsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSMigrationItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource rMSStoreBindingSource;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource1;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource2;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource3;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource4;
        private System.Windows.Forms.BindingSource storeListItemBindingSource;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource5;
        private System.Windows.Forms.DataGridView migrationItemsGrid;
        private System.Windows.Forms.BindingSource rMSMigrationItemBindingSource;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblBarCodeSetup;
        private System.Windows.Forms.ComboBox cbBarCodeSetup;
        private System.Windows.Forms.DataGridViewCheckBoxColumn shouldImportDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn progressStatusDisplayDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblImagePath;
        private System.Windows.Forms.TextBox tbImageFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
    }
}
