namespace LSOne.ViewPlugins.Store.Views
{
    partial class RegionsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegionsView));
            this.lvRegions = new LSOne.Controls.ListView();
            this.clmRegionID = new LSOne.Controls.Columns.Column();
            this.clmRegionDescription = new LSOne.Controls.Columns.Column();
            this.clmID = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.grpStores = new System.Windows.Forms.GroupBox();
            this.contextBtnsStores = new LSOne.Controls.ContextButtons();
            this.lvStores = new LSOne.Controls.ListView();
            this.pnlBottom.SuspendLayout();
            this.grpStores.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.grpStores);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvRegions);
            // 
            // lvRegions
            // 
            resources.ApplyResources(this.lvRegions, "lvRegions");
            this.lvRegions.BuddyControl = null;
            this.lvRegions.Columns.Add(this.clmRegionID);
            this.lvRegions.Columns.Add(this.clmRegionDescription);
            this.lvRegions.ContentBackColor = System.Drawing.Color.White;
            this.lvRegions.DefaultRowHeight = ((short)(18));
            this.lvRegions.DimSelectionWhenDisabled = true;
            this.lvRegions.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvRegions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvRegions.HeaderHeight = ((short)(25));
            this.lvRegions.Name = "lvRegions";
            this.lvRegions.OddRowColor = System.Drawing.Color.White;
            this.lvRegions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvRegions.SecondarySortColumn = ((short)(-1));
            this.lvRegions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvRegions.SortSetting = "0:1";
            this.lvRegions.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvRegions_HeaderClicked);
            this.lvRegions.SelectionChanged += new System.EventHandler(this.lvRegions_SelectionChanged);
            // 
            // clmRegionID
            // 
            this.clmRegionID.AutoSize = true;
            this.clmRegionID.DefaultStyle = null;
            resources.ApplyResources(this.clmRegionID, "clmRegionID");
            this.clmRegionID.MaximumWidth = ((short)(0));
            this.clmRegionID.MinimumWidth = ((short)(10));
            this.clmRegionID.SecondarySortColumn = ((short)(-1));
            this.clmRegionID.Tag = null;
            this.clmRegionID.Width = ((short)(50));
            // 
            // clmRegionDescription
            // 
            this.clmRegionDescription.AutoSize = true;
            this.clmRegionDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmRegionDescription, "clmRegionDescription");
            this.clmRegionDescription.MaximumWidth = ((short)(0));
            this.clmRegionDescription.MinimumWidth = ((short)(10));
            this.clmRegionDescription.RelativeSize = 80;
            this.clmRegionDescription.SecondarySortColumn = ((short)(-1));
            this.clmRegionDescription.Tag = null;
            this.clmRegionDescription.Width = ((short)(50));
            // 
            // clmID
            // 
            this.clmID.AutoSize = true;
            this.clmID.DefaultStyle = null;
            resources.ApplyResources(this.clmID, "clmID");
            this.clmID.MaximumWidth = ((short)(0));
            this.clmID.MinimumWidth = ((short)(10));
            this.clmID.SecondarySortColumn = ((short)(-1));
            this.clmID.Tag = null;
            this.clmID.Width = ((short)(50));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.RelativeSize = 80;
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = false;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // grpStores
            // 
            resources.ApplyResources(this.grpStores, "grpStores");
            this.grpStores.Controls.Add(this.contextBtnsStores);
            this.grpStores.Controls.Add(this.lvStores);
            this.grpStores.Name = "grpStores";
            this.grpStores.TabStop = false;
            // 
            // contextBtnsStores
            // 
            this.contextBtnsStores.AddButtonEnabled = true;
            resources.ApplyResources(this.contextBtnsStores, "contextBtnsStores");
            this.contextBtnsStores.BackColor = System.Drawing.Color.Transparent;
            this.contextBtnsStores.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextBtnsStores.EditButtonEnabled = false;
            this.contextBtnsStores.Name = "contextBtnsStores";
            this.contextBtnsStores.RemoveButtonEnabled = true;
            this.contextBtnsStores.AddButtonClicked += new System.EventHandler(this.contextBtnsStores_AddButtonClicked);
            this.contextBtnsStores.RemoveButtonClicked += new System.EventHandler(this.contextBtnsStores_RemoveButtonClicked);
            // 
            // lvStores
            // 
            resources.ApplyResources(this.lvStores, "lvStores");
            this.lvStores.BuddyControl = null;
            this.lvStores.Columns.Add(this.clmID);
            this.lvStores.Columns.Add(this.clmDescription);
            this.lvStores.ContentBackColor = System.Drawing.Color.White;
            this.lvStores.DefaultRowHeight = ((short)(18));
            this.lvStores.DimSelectionWhenDisabled = true;
            this.lvStores.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvStores.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvStores.HeaderHeight = ((short)(25));
            this.lvStores.Name = "lvStores";
            this.lvStores.OddRowColor = System.Drawing.Color.White;
            this.lvStores.RowLineColor = System.Drawing.Color.LightGray;
            this.lvStores.SecondarySortColumn = ((short)(-1));
            this.lvStores.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvStores.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvStores.SortSetting = "0:1";
            this.lvStores.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvStores_HeaderClicked);
            this.lvStores.SelectionChanged += new System.EventHandler(this.lvStores_SelectionChanged);
            // 
            // RegionsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "RegionsView";
            this.pnlBottom.ResumeLayout(false);
            this.grpStores.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ListView lvRegions;
        private Controls.ContextButtons btnsEditAddRemove;
        private Controls.Columns.Column clmID;
        private Controls.Columns.Column clmDescription;
        private System.Windows.Forms.GroupBox grpStores;
        private Controls.ContextButtons contextBtnsStores;
        private Controls.ListView lvStores;
        private Controls.Columns.Column clmRegionID;
        private Controls.Columns.Column clmRegionDescription;
    }
}
