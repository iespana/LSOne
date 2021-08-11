using LSOne.Controls;

namespace LSOne.ViewPlugins.ExcelFiles.Views
{
    partial class ExcelImportResultsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExcelImportResultsView));
            this.lvResults = new LSOne.Controls.ExtendedListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lvFiles = new LSOne.Controls.ListView();
            this.clmFileName = new LSOne.Controls.Columns.Column();
            this.clmImportType = new LSOne.Controls.Columns.Column();
            this.clmImportProfile = new LSOne.Controls.Columns.Column();
            this.lvErrorLines = new LSOne.Controls.ListView();
            this.clmLineNumber = new LSOne.Controls.Columns.Column();
            this.clmItemId = new LSOne.Controls.Columns.Column();
            this.clmItemDescription = new LSOne.Controls.Columns.Column();
            this.clmErrorMessage = new LSOne.Controls.Columns.Column();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnExportToExcel);
            this.pnlBottom.Controls.Add(this.lvErrorLines);
            this.pnlBottom.Controls.Add(this.lvFiles);
            this.pnlBottom.Controls.Add(this.lvResults);
            // 
            // lvResults
            // 
            resources.ApplyResources(this.lvResults, "lvResults");
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvResults.FullRowSelect = true;
            this.lvResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvResults.HideSelection = false;
            this.lvResults.LockDrawing = false;
            this.lvResults.MultiSelect = false;
            this.lvResults.Name = "lvResults";
            this.lvResults.SmallImageList = this.imageList1;
            this.lvResults.SortColumn = -1;
            this.lvResults.SortedBackwards = false;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.UseEveryOtherRowColoring = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            this.lvResults.Resize += new System.EventHandler(this.lvResults_Resize);
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lvFiles
            // 
            resources.ApplyResources(this.lvFiles, "lvFiles");
            this.lvFiles.BuddyControl = null;
            this.lvFiles.Columns.Add(this.clmFileName);
            this.lvFiles.Columns.Add(this.clmImportType);
            this.lvFiles.Columns.Add(this.clmImportProfile);
            this.lvFiles.ContentBackColor = System.Drawing.Color.White;
            this.lvFiles.DefaultRowHeight = ((short)(18));
            this.lvFiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFiles.HeaderHeight = ((short)(20));
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.OddRowColor = System.Drawing.Color.White;
            this.lvFiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFiles.SecondarySortColumn = ((short)(-1));
            this.lvFiles.SortSetting = "0:1";
            this.lvFiles.SelectionChanged += new System.EventHandler(this.lvFiles_SelectionChanged);
            // 
            // clmFileName
            // 
            this.clmFileName.AutoSize = true;
            this.clmFileName.DefaultStyle = null;
            resources.ApplyResources(this.clmFileName, "clmFileName");
            this.clmFileName.InternalSort = true;
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
            this.clmImportType.InternalSort = true;
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
            this.clmImportProfile.InternalSort = true;
            this.clmImportProfile.MaximumWidth = ((short)(0));
            this.clmImportProfile.MinimumWidth = ((short)(10));
            this.clmImportProfile.SecondarySortColumn = ((short)(-1));
            this.clmImportProfile.Tag = null;
            this.clmImportProfile.Width = ((short)(200));
            // 
            // lvErrorLines
            // 
            resources.ApplyResources(this.lvErrorLines, "lvErrorLines");
            this.lvErrorLines.BuddyControl = null;
            this.lvErrorLines.Columns.Add(this.clmLineNumber);
            this.lvErrorLines.Columns.Add(this.clmItemId);
            this.lvErrorLines.Columns.Add(this.clmItemDescription);
            this.lvErrorLines.Columns.Add(this.clmErrorMessage);
            this.lvErrorLines.ContentBackColor = System.Drawing.Color.White;
            this.lvErrorLines.DefaultRowHeight = ((short)(22));
            this.lvErrorLines.DimSelectionWhenDisabled = true;
            this.lvErrorLines.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvErrorLines.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvErrorLines.HeaderHeight = ((short)(25));
            this.lvErrorLines.Name = "lvErrorLines";
            this.lvErrorLines.OddRowColor = System.Drawing.Color.White;
            this.lvErrorLines.RowLineColor = System.Drawing.Color.LightGray;
            this.lvErrorLines.SecondarySortColumn = ((short)(-1));
            this.lvErrorLines.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvErrorLines.SortSetting = "0:1";
            // 
            // clmLineNumber
            // 
            this.clmLineNumber.AutoSize = true;
            this.clmLineNumber.DefaultStyle = null;
            resources.ApplyResources(this.clmLineNumber, "clmLineNumber");
            this.clmLineNumber.InternalSort = true;
            this.clmLineNumber.MaximumWidth = ((short)(0));
            this.clmLineNumber.MinimumWidth = ((short)(10));
            this.clmLineNumber.SecondarySortColumn = ((short)(-1));
            this.clmLineNumber.Tag = null;
            this.clmLineNumber.Width = ((short)(80));
            // 
            // clmItemId
            // 
            this.clmItemId.AutoSize = true;
            this.clmItemId.DefaultStyle = null;
            resources.ApplyResources(this.clmItemId, "clmItemId");
            this.clmItemId.InternalSort = true;
            this.clmItemId.MaximumWidth = ((short)(0));
            this.clmItemId.MinimumWidth = ((short)(10));
            this.clmItemId.SecondarySortColumn = ((short)(-1));
            this.clmItemId.Tag = null;
            this.clmItemId.Width = ((short)(100));
            // 
            // clmItemDescription
            // 
            this.clmItemDescription.AutoSize = true;
            this.clmItemDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmItemDescription, "clmItemDescription");
            this.clmItemDescription.InternalSort = true;
            this.clmItemDescription.MaximumWidth = ((short)(0));
            this.clmItemDescription.MinimumWidth = ((short)(10));
            this.clmItemDescription.SecondarySortColumn = ((short)(-1));
            this.clmItemDescription.Tag = null;
            this.clmItemDescription.Width = ((short)(150));
            // 
            // clmErrorMessage
            // 
            this.clmErrorMessage.AutoSize = true;
            this.clmErrorMessage.DefaultStyle = null;
            resources.ApplyResources(this.clmErrorMessage, "clmErrorMessage");
            this.clmErrorMessage.InternalSort = true;
            this.clmErrorMessage.MaximumWidth = ((short)(0));
            this.clmErrorMessage.MinimumWidth = ((short)(10));
            this.clmErrorMessage.SecondarySortColumn = ((short)(-1));
            this.clmErrorMessage.Tag = null;
            this.clmErrorMessage.Width = ((short)(400));
            // 
            // btnExportToExcel
            // 
            resources.ApplyResources(this.btnExportToExcel, "btnExportToExcel");
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // ExcelImportResultsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ExcelImportResultsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedListView lvResults;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private ListView lvFiles;
        private Controls.Columns.Column clmFileName;
        private Controls.Columns.Column clmImportType;
        private Controls.Columns.Column clmImportProfile;
        private ListView lvErrorLines;
        private Controls.Columns.Column clmLineNumber;
        private Controls.Columns.Column clmItemId;
        private Controls.Columns.Column clmItemDescription;
        private Controls.Columns.Column clmErrorMessage;
        private System.Windows.Forms.Button btnExportToExcel;
    }
}
