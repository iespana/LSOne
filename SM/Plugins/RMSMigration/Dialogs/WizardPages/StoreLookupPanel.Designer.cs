namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    partial class StoreLookupPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreLookupPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblInfoText = new System.Windows.Forms.Label();
            this.storesGrid = new System.Windows.Forms.DataGridView();
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LSOneStore = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.storeListItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gbCreateStores = new System.Windows.Forms.GroupBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.btnCreateStores = new System.Windows.Forms.Button();
            this.cbCreateStores = new System.Windows.Forms.ComboBox();
            this.rMSStoreBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.storesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).BeginInit();
            this.gbCreateStores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource4)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfoText
            // 
            resources.ApplyResources(this.lblInfoText, "lblInfoText");
            this.lblInfoText.BackColor = System.Drawing.Color.Transparent;
            this.lblInfoText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInfoText.Name = "lblInfoText";
            // 
            // storesGrid
            // 
            resources.ApplyResources(this.storesGrid, "storesGrid");
            this.storesGrid.AutoGenerateColumns = false;
            this.storesGrid.BackgroundColor = System.Drawing.Color.White;
            this.storesGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.storesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.storesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.displayNameDataGridViewTextBoxColumn,
            this.LSOneStore});
            this.storesGrid.DataSource = this.storeListItemBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.storesGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.storesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.storesGrid.GridColor = System.Drawing.Color.White;
            this.storesGrid.Name = "storesGrid";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.storesGrid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.storesGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.storesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.storesGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.storesGrid_CellClick);
            this.storesGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.storesGrid_CellEndEdit);
            this.storesGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.storesGrid_DataError);
            this.storesGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.storesGrid_EditingControlShowing);
            // 
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.FillWeight = 7.614227F;
            resources.ApplyResources(this.displayNameDataGridViewTextBoxColumn, "displayNameDataGridViewTextBoxColumn");
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // LSOneStore
            // 
            this.LSOneStore.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LSOneStore.DataPropertyName = "LSOneStore";
            this.LSOneStore.FillWeight = 292.3858F;
            resources.ApplyResources(this.LSOneStore, "LSOneStore");
            this.LSOneStore.Name = "LSOneStore";
            // 
            // storeListItemBindingSource
            // 
            this.storeListItemBindingSource.DataSource = typeof(LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems.StoreListItem);
            // 
            // gbCreateStores
            // 
            resources.ApplyResources(this.gbCreateStores, "gbCreateStores");
            this.gbCreateStores.Controls.Add(this.lblCopyFrom);
            this.gbCreateStores.Controls.Add(this.btnCreateStores);
            this.gbCreateStores.Controls.Add(this.cbCreateStores);
            this.gbCreateStores.Name = "gbCreateStores";
            this.gbCreateStores.TabStop = false;
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // btnCreateStores
            // 
            resources.ApplyResources(this.btnCreateStores, "btnCreateStores");
            this.btnCreateStores.Name = "btnCreateStores";
            this.btnCreateStores.UseVisualStyleBackColor = true;
            this.btnCreateStores.Click += new System.EventHandler(this.btnCreateStores_Click);
            // 
            // cbCreateStores
            // 
            this.cbCreateStores.FormattingEnabled = true;
            resources.ApplyResources(this.cbCreateStores, "cbCreateStores");
            this.cbCreateStores.Name = "cbCreateStores";
            this.cbCreateStores.SelectedIndexChanged += new System.EventHandler(this.cbCreateStores_SelectedIndexChanged);
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
            // StoreLookupPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCreateStores);
            this.Controls.Add(this.storesGrid);
            this.Controls.Add(this.lblInfoText);
            this.Name = "StoreLookupPanel";
            ((System.ComponentModel.ISupportInitialize)(this.storesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).EndInit();
            this.gbCreateStores.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblInfoText;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource1;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource2;
        private System.Windows.Forms.DataGridView storesGrid;
        private System.Windows.Forms.DataGridViewComboBoxColumn LSOneStore;
        private System.Windows.Forms.GroupBox gbCreateStores;
        private System.Windows.Forms.Button btnCreateStores;
        private System.Windows.Forms.ComboBox cbCreateStores;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource3;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource4;
        private System.Windows.Forms.BindingSource storeListItemBindingSource;
        private System.Windows.Forms.Label lblCopyFrom;
    }
}
