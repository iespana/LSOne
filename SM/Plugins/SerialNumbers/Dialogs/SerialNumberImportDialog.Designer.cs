namespace LSOne.ViewPlugins.SerialNumbers.Dialogs
{
    partial class SerialNumberImportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialNumberImportDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lvImportFiles = new LSOne.Controls.ListView();
            this.clmFileName = new LSOne.Controls.Columns.Column();
            this.clmImportType = new LSOne.Controls.Columns.Column();
            this.clmImportProfile = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            this.clmImportType.Width = ((short)(200));
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
            this.clmImportProfile.Width = ((short)(200));
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
            // SerialNumberImportDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvImportFiles);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.KeyPreview = true;
            this.Name = "SerialNumberImportDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvImportFiles, 0);
            this.Controls.SetChildIndex(this.btnsContextButtons, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private Controls.ListView lvImportFiles;
        private Controls.ContextButtons btnsContextButtons;
        private Controls.Columns.Column clmFileName;
        private Controls.Columns.Column clmImportType;
        private Controls.Columns.Column clmImportProfile;
    }
}