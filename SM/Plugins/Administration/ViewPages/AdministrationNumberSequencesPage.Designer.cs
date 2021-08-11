using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationNumberSequencesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationNumberSequencesPage));
            this.btnsEditAddDelete = new LSOne.Controls.ContextButtons();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lvNumberSequences = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.column7 = new LSOne.Controls.Columns.Column();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.SuspendLayout();
            // 
            // btnsEditAddDelete
            // 
            this.btnsEditAddDelete.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddDelete, "btnsEditAddDelete");
            this.btnsEditAddDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddDelete.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddDelete.EditButtonEnabled = false;
            this.btnsEditAddDelete.Name = "btnsEditAddDelete";
            this.btnsEditAddDelete.RemoveButtonEnabled = false;
            this.btnsEditAddDelete.EditButtonClicked += new System.EventHandler(this.btnsEditAddDelete_EditButtonClicked);
            this.btnsEditAddDelete.AddButtonClicked += new System.EventHandler(this.btnsEditAddDelete_AddButtonClicked);
            this.btnsEditAddDelete.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddDelete_RemoveButtonClicked);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "lock-small.png");
            // 
            // lvNumberSequences
            // 
            resources.ApplyResources(this.lvNumberSequences, "lvNumberSequences");
            this.lvNumberSequences.BuddyControl = null;
            this.lvNumberSequences.Columns.Add(this.column1);
            this.lvNumberSequences.Columns.Add(this.column2);
            this.lvNumberSequences.Columns.Add(this.column3);
            this.lvNumberSequences.Columns.Add(this.column4);
            this.lvNumberSequences.Columns.Add(this.column5);
            this.lvNumberSequences.Columns.Add(this.column6);
            this.lvNumberSequences.Columns.Add(this.column7);
            this.lvNumberSequences.ContentBackColor = System.Drawing.Color.White;
            this.lvNumberSequences.DefaultRowHeight = ((short)(22));
            this.lvNumberSequences.DimSelectionWhenDisabled = true;
            this.lvNumberSequences.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvNumberSequences.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvNumberSequences.HeaderHeight = ((short)(25));
            this.lvNumberSequences.HorizontalScrollbar = true;
            this.lvNumberSequences.Name = "lvNumberSequences";
            this.lvNumberSequences.OddRowColor = System.Drawing.Color.White;
            this.lvNumberSequences.RowLineColor = System.Drawing.Color.LightGray;
            this.lvNumberSequences.SecondarySortColumn = ((short)(-1));
            this.lvNumberSequences.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvNumberSequences.SortSetting = "0:1";
            this.lvNumberSequences.SelectionChanged += new System.EventHandler(this.lvNumberSequences_SelectionChanged);
            this.lvNumberSequences.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvNumberSequences_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.InternalSort = true;
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.InternalSort = true;
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // column7
            // 
            this.column7.AutoSize = true;
            this.column7.DefaultStyle = null;
            resources.ApplyResources(this.column7, "column7");
            this.column7.InternalSort = true;
            this.column7.MaximumWidth = ((short)(0));
            this.column7.MinimumWidth = ((short)(10));
            this.column7.SecondarySortColumn = ((short)(-1));
            this.column7.Tag = null;
            this.column7.Width = ((short)(50));
            // 
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.EnableCheckbox = false;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            // 
            // AdministrationNumberSequencesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.lvNumberSequences);
            this.Controls.Add(this.btnsEditAddDelete);
            this.Name = "AdministrationNumberSequencesPage";
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditAddDelete;
        private System.Windows.Forms.ImageList imageList1;
        private ListView lvNumberSequences;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
        private Controls.Columns.Column column6;
        private Controls.Columns.Column column7;
        private Controls.SearchBar searchBar;
    }
}
