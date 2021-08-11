namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    partial class ImportSCJFromFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportSCJFromFile));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvImportFiles = new LSOne.Controls.ListView();
            this.clmFileName = new LSOne.Controls.Columns.Column();
            this.clmImportType = new LSOne.Controls.Columns.Column();
            this.clmImportProfile = new LSOne.Controls.Columns.Column();
            this.cmbStockCountingJournal = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.stockCountingJournalLabel = new System.Windows.Forms.Label();
            this.btnAddStockCountingJournal = new LSOne.Controls.ContextButton();
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnsContextButtons_AddButtonClicked);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsContextButtons_RemoveButtonClicked);
            // 
            // lvImportFiles
            // 
            resources.ApplyResources(this.lvImportFiles, "lvImportFiles");
            this.lvImportFiles.BuddyControl = null;
            this.lvImportFiles.Columns.Add(this.clmFileName);
            this.lvImportFiles.Columns.Add(this.clmImportType);
            this.lvImportFiles.Columns.Add(this.clmImportProfile);
            this.lvImportFiles.ContentBackColor = System.Drawing.Color.White;
            this.lvImportFiles.DefaultRowHeight = ((short)(18));
            this.lvImportFiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvImportFiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvImportFiles.HeaderHeight = ((short)(20));
            this.lvImportFiles.Name = "lvImportFiles";
            this.lvImportFiles.OddRowColor = System.Drawing.Color.White;
            this.lvImportFiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvImportFiles.SecondarySortColumn = ((short)(-1));
            this.lvImportFiles.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvImportFiles.SortSetting = "0:1";
            this.lvImportFiles.SelectionChanged += new System.EventHandler(this.lvImportFiles_SelectionChanged);
            this.lvImportFiles.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvImportFiles_RowDoubleClick);
            // 
            // clmFileName
            // 
            this.clmFileName.AutoSize = true;
            this.clmFileName.DefaultStyle = null;
            resources.ApplyResources(this.clmFileName, "clmFileName");
            this.clmFileName.MaximumWidth = ((short)(0));
            this.clmFileName.MinimumWidth = ((short)(10));
            this.clmFileName.SecondarySortColumn = ((short)(-1));
            this.clmFileName.Tag = null;
            this.clmFileName.Width = ((short)(200));
            // 
            // clmImportType
            // 
            this.clmImportType.AutoSize = true;
            this.clmImportType.DefaultStyle = null;
            resources.ApplyResources(this.clmImportType, "clmImportType");
            this.clmImportType.MaximumWidth = ((short)(0));
            this.clmImportType.MinimumWidth = ((short)(10));
            this.clmImportType.SecondarySortColumn = ((short)(-1));
            this.clmImportType.Tag = null;
            this.clmImportType.Width = ((short)(150));
            // 
            // clmImportProfile
            // 
            this.clmImportProfile.AutoSize = true;
            this.clmImportProfile.DefaultStyle = null;
            resources.ApplyResources(this.clmImportProfile, "clmImportProfile");
            this.clmImportProfile.MaximumWidth = ((short)(0));
            this.clmImportProfile.MinimumWidth = ((short)(10));
            this.clmImportProfile.SecondarySortColumn = ((short)(-1));
            this.clmImportProfile.Tag = null;
            this.clmImportProfile.Width = ((short)(150));
            // 
            // cmbStockCountingJournal
            // 
            this.cmbStockCountingJournal.AddList = null;
            this.cmbStockCountingJournal.EnableTextBox = true;
            resources.ApplyResources(this.cmbStockCountingJournal, "cmbStockCountingJournal");
            this.cmbStockCountingJournal.MaxLength = 32767;
            this.cmbStockCountingJournal.Name = "cmbStockCountingJournal";
            this.cmbStockCountingJournal.NoChangeAllowed = false;
            this.cmbStockCountingJournal.RemoveList = null;
            this.cmbStockCountingJournal.RowHeight = ((short)(22));
            this.cmbStockCountingJournal.SecondaryData = null;
            this.cmbStockCountingJournal.SelectedData = null;
            this.cmbStockCountingJournal.SelectionList = null;
            this.cmbStockCountingJournal.ShowDropDownOnTyping = true;
            this.cmbStockCountingJournal.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStockCountingJournal_DropDown);
            this.cmbStockCountingJournal.SelectedDataChanged += new System.EventHandler(this.cmbStockCountingJournal_SelectedDataChanged);
            this.cmbStockCountingJournal.RequestClear += new System.EventHandler(this.cmbStockCountingJournal_RequestClear);
            // 
            // stockCountingJournalLabel
            // 
            this.stockCountingJournalLabel.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.stockCountingJournalLabel, "stockCountingJournalLabel");
            this.stockCountingJournalLabel.Name = "stockCountingJournalLabel";
            // 
            // btnAddStockCountingJournal
            // 
            this.btnAddStockCountingJournal.BackColor = System.Drawing.Color.Transparent;
            this.btnAddStockCountingJournal.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAddStockCountingJournal, "btnAddStockCountingJournal");
            this.btnAddStockCountingJournal.Name = "btnAddStockCountingJournal";
            this.btnAddStockCountingJournal.Click += new System.EventHandler(this.btnAddStockCountingJournal_Click);
            // 
            // ImportSCJFromFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.btnAddStockCountingJournal);
            this.Controls.Add(this.stockCountingJournalLabel);
            this.Controls.Add(this.cmbStockCountingJournal);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvImportFiles);
            this.Name = "ImportSCJFromFile";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButtons btnsContextButtons;
        private Controls.ListView lvImportFiles;
        private Controls.Columns.Column clmFileName;
        private Controls.Columns.Column clmImportType;
        private Controls.Columns.Column clmImportProfile;
        private Controls.DropDownFormComboBox cmbStockCountingJournal;
        private System.Windows.Forms.Label stockCountingJournalLabel;
        private Controls.ContextButton btnAddStockCountingJournal;
    }
}
