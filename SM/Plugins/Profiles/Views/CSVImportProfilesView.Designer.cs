using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class CSVImportProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSVImportProfilesView));
            this.lvCSVImportProfiles = new LSOne.Controls.ListView();
            this.clmId = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmImportType = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.btnSetProfileAsDefault = new System.Windows.Forms.Button();
            this.clmHasHeaders = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnSetProfileAsDefault);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvCSVImportProfiles);
            // 
            // lvCSVImportProfiles
            // 
            resources.ApplyResources(this.lvCSVImportProfiles, "lvCSVImportProfiles");
            this.lvCSVImportProfiles.BuddyControl = null;
            this.lvCSVImportProfiles.Columns.Add(this.clmId);
            this.lvCSVImportProfiles.Columns.Add(this.clmDescription);
            this.lvCSVImportProfiles.Columns.Add(this.clmImportType);
            this.lvCSVImportProfiles.Columns.Add(this.clmHasHeaders);
            this.lvCSVImportProfiles.ContentBackColor = System.Drawing.Color.White;
            this.lvCSVImportProfiles.DefaultRowHeight = ((short)(22));
            this.lvCSVImportProfiles.DimSelectionWhenDisabled = true;
            this.lvCSVImportProfiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCSVImportProfiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCSVImportProfiles.HeaderHeight = ((short)(25));
            this.lvCSVImportProfiles.HorizontalScrollbar = true;
            this.lvCSVImportProfiles.Name = "lvCSVImportProfiles";
            this.lvCSVImportProfiles.OddRowColor = System.Drawing.Color.White;
            this.lvCSVImportProfiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCSVImportProfiles.SecondarySortColumn = ((short)(-1));
            this.lvCSVImportProfiles.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCSVImportProfiles.SortSetting = "0:1";
            this.lvCSVImportProfiles.SelectionChanged += new System.EventHandler(this.lvCSVImportProfiles_SelectionChanged);
            this.lvCSVImportProfiles.DoubleClick += new System.EventHandler(this.lvCSVImportProfiles_DoubleClick);
            // 
            // clmId
            // 
            this.clmId.AutoSize = true;
            this.clmId.DefaultStyle = null;
            resources.ApplyResources(this.clmId, "clmId");
            this.clmId.InternalSort = true;
            this.clmId.MaximumWidth = ((short)(0));
            this.clmId.MinimumWidth = ((short)(10));
            this.clmId.SecondarySortColumn = ((short)(1));
            this.clmId.Tag = null;
            this.clmId.Width = ((short)(150));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(2));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(150));
            // 
            // clmImportType
            // 
            this.clmImportType.AutoSize = true;
            this.clmImportType.DefaultStyle = null;
            resources.ApplyResources(this.clmImportType, "clmImportType");
            this.clmImportType.InternalSort = true;
            this.clmImportType.MaximumWidth = ((short)(0));
            this.clmImportType.MinimumWidth = ((short)(10));
            this.clmImportType.SecondarySortColumn = ((short)(1));
            this.clmImportType.Tag = null;
            this.clmImportType.Width = ((short)(150));
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
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSetProfileAsDefault
            // 
            resources.ApplyResources(this.btnSetProfileAsDefault, "btnSetProfileAsDefault");
            this.btnSetProfileAsDefault.Name = "btnSetProfileAsDefault";
            this.btnSetProfileAsDefault.UseVisualStyleBackColor = true;
            this.btnSetProfileAsDefault.Click += new System.EventHandler(this.btnSetProfileAsDefault_Click);
            // 
            // clmHasHeaders
            // 
            this.clmHasHeaders.AutoSize = true;
            this.clmHasHeaders.DefaultStyle = null;
            resources.ApplyResources(this.clmHasHeaders, "clmHasHeaders");
            this.clmHasHeaders.InternalSort = true;
            this.clmHasHeaders.MaximumWidth = ((short)(0));
            this.clmHasHeaders.MinimumWidth = ((short)(10));
            this.clmHasHeaders.SecondarySortColumn = ((short)(-1));
            this.clmHasHeaders.Tag = null;
            this.clmHasHeaders.Width = ((short)(150));
            // 
            // CSVImportProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CSVImportProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvCSVImportProfiles;
        private Column clmId;
        private Column clmDescription;
        private Column clmImportType;
        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.Button btnSetProfileAsDefault;
        private Column clmHasHeaders;
    }
}
