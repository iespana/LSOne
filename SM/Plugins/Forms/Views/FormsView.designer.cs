using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Forms.Views
{
    partial class FormsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormsView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.lvForms = new LSOne.Controls.ListView();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colSystem = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.lvForms);
            this.pnlBottom.Controls.Add(this.btnExport);
            this.pnlBottom.Controls.Add(this.btnImport);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
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
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lvForms
            // 
            resources.ApplyResources(this.lvForms, "lvForms");
            this.lvForms.BuddyControl = null;
            this.lvForms.Columns.Add(this.colType);
            this.lvForms.Columns.Add(this.colDescription);
            this.lvForms.Columns.Add(this.colSystem);
            this.lvForms.ContentBackColor = System.Drawing.Color.White;
            this.lvForms.DefaultRowHeight = ((short)(22));
            this.lvForms.DimSelectionWhenDisabled = true;
            this.lvForms.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvForms.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvForms.HeaderHeight = ((short)(25));
            this.lvForms.Name = "lvForms";
            this.lvForms.OddRowColor = System.Drawing.Color.White;
            this.lvForms.RowLineColor = System.Drawing.Color.LightGray;
            this.lvForms.SecondarySortColumn = ((short)(-1));
            this.lvForms.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvForms.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvForms.SortSetting = "0:1";
            this.lvForms.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvForms_HeaderClicked);
            this.lvForms.SelectionChanged += new System.EventHandler(this.lvForms_SelectionChanged);
            this.lvForms.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvForms_RowDoubleClick);
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.MaximumWidth = ((short)(0));
            this.colType.MinimumWidth = ((short)(10));
            this.colType.SecondarySortColumn = ((short)(-1));
            this.colType.Tag = null;
            this.colType.Width = ((short)(200));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(310));
            // 
            // colSystem
            // 
            this.colSystem.AutoSize = true;
            this.colSystem.DefaultStyle = null;
            resources.ApplyResources(this.colSystem, "colSystem");
            this.colSystem.MaximumWidth = ((short)(0));
            this.colSystem.MinimumWidth = ((short)(10));
            this.colSystem.SecondarySortColumn = ((short)(-1));
            this.colSystem.Tag = null;
            this.colSystem.Width = ((short)(50));
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Name = "searchBar";
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // FormsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "FormsView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private ListView lvForms;
        private Column colType;
        private Column colDescription;
        private Column colSystem;
        private SearchBar searchBar;
    }
}
