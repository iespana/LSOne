namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    partial class TerminalLookupPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalLookupPanel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblInfoText = new System.Windows.Forms.Label();
            this.terminalsGrid = new System.Windows.Forms.DataGridView();
            this.rMSStoreBindingSource5 = new System.Windows.Forms.BindingSource(this.components);
            this.gbCreateTerminals = new System.Windows.Forms.GroupBox();
            this.lblCopyFrom = new System.Windows.Forms.Label();
            this.btnCreateTerminals = new System.Windows.Forms.Button();
            this.cbCreateTerminals = new System.Windows.Forms.ComboBox();
            this.storeListItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.rMSStoreBindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            this.displayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TerminalDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LSOneTerminal = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.terminalsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource5)).BeginInit();
            this.gbCreateTerminals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).BeginInit();
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
            // terminalsGrid
            // 
            resources.ApplyResources(this.terminalsGrid, "terminalsGrid");
            this.terminalsGrid.AutoGenerateColumns = false;
            this.terminalsGrid.BackgroundColor = System.Drawing.Color.White;
            this.terminalsGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.terminalsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.terminalsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.displayNameDataGridViewTextBoxColumn,
            this.TerminalDescription,
            this.LSOneTerminal});
            this.terminalsGrid.DataSource = this.rMSStoreBindingSource5;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.terminalsGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.terminalsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.terminalsGrid.GridColor = System.Drawing.Color.White;
            this.terminalsGrid.Name = "terminalsGrid";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.terminalsGrid.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.terminalsGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.terminalsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.terminalsGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.terminalsGrid_CellClick);
            this.terminalsGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.terminalsGrid_CellEndEdit);
            this.terminalsGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.terminalsGrid_DataError);
            this.terminalsGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.terminalsGrid_EditingControlShowing);
            // 
            // rMSStoreBindingSource5
            // 
            this.rMSStoreBindingSource5.DataSource = typeof(LSOne.ViewPlugins.RMSMigration.Model.RMSStore);
            // 
            // gbCreateTerminals
            // 
            resources.ApplyResources(this.gbCreateTerminals, "gbCreateTerminals");
            this.gbCreateTerminals.Controls.Add(this.lblCopyFrom);
            this.gbCreateTerminals.Controls.Add(this.btnCreateTerminals);
            this.gbCreateTerminals.Controls.Add(this.cbCreateTerminals);
            this.gbCreateTerminals.Name = "gbCreateTerminals";
            this.gbCreateTerminals.TabStop = false;
            // 
            // lblCopyFrom
            // 
            this.lblCopyFrom.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblCopyFrom, "lblCopyFrom");
            this.lblCopyFrom.Name = "lblCopyFrom";
            // 
            // btnCreateTerminals
            // 
            resources.ApplyResources(this.btnCreateTerminals, "btnCreateTerminals");
            this.btnCreateTerminals.Name = "btnCreateTerminals";
            this.btnCreateTerminals.UseVisualStyleBackColor = true;
            this.btnCreateTerminals.Click += new System.EventHandler(this.btnCreateTerminals_Click);
            // 
            // cbCreateTerminals
            // 
            this.cbCreateTerminals.FormattingEnabled = true;
            resources.ApplyResources(this.cbCreateTerminals, "cbCreateTerminals");
            this.cbCreateTerminals.Name = "cbCreateTerminals";
            this.cbCreateTerminals.SelectedIndexChanged += new System.EventHandler(this.cbCreateTerminals_SelectedIndexChanged);
            // 
            // storeListItemBindingSource
            // 
            this.storeListItemBindingSource.DataSource = typeof(LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems.StoreListItem);
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
            // displayNameDataGridViewTextBoxColumn
            // 
            this.displayNameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.displayNameDataGridViewTextBoxColumn.FillWeight = 7.614227F;
            resources.ApplyResources(this.displayNameDataGridViewTextBoxColumn, "displayNameDataGridViewTextBoxColumn");
            this.displayNameDataGridViewTextBoxColumn.Name = "displayNameDataGridViewTextBoxColumn";
            this.displayNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TerminalDescription
            // 
            this.TerminalDescription.DataPropertyName = "TerminalDescription";
            resources.ApplyResources(this.TerminalDescription, "TerminalDescription");
            this.TerminalDescription.Name = "TerminalDescription";
            this.TerminalDescription.ReadOnly = true;
            // 
            // LSOneTerminal
            // 
            this.LSOneTerminal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LSOneTerminal.DataPropertyName = "LSOneTerminal";
            this.LSOneTerminal.FillWeight = 292.3858F;
            resources.ApplyResources(this.LSOneTerminal, "LSOneTerminal");
            this.LSOneTerminal.Name = "LSOneTerminal";
            // 
            // TerminalLookupPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCreateTerminals);
            this.Controls.Add(this.terminalsGrid);
            this.Controls.Add(this.lblInfoText);
            this.Name = "TerminalLookupPanel";
            ((System.ComponentModel.ISupportInitialize)(this.terminalsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rMSStoreBindingSource5)).EndInit();
            this.gbCreateTerminals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.storeListItemBindingSource)).EndInit();
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
        private System.Windows.Forms.DataGridView terminalsGrid;
        private System.Windows.Forms.GroupBox gbCreateTerminals;
        private System.Windows.Forms.Button btnCreateTerminals;
        private System.Windows.Forms.ComboBox cbCreateTerminals;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource3;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource4;
        private System.Windows.Forms.BindingSource storeListItemBindingSource;
        private System.Windows.Forms.Label lblCopyFrom;
        private System.Windows.Forms.BindingSource rMSStoreBindingSource5;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TerminalDescription;
        private System.Windows.Forms.DataGridViewComboBoxColumn LSOneTerminal;
    }
}
