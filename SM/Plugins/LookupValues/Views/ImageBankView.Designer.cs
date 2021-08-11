namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class ImageBankView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageBankView));
            this.lvImageBank = new LSOne.Controls.ListView();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colImage = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvImageBank);
            this.pnlBottom.Controls.Add(this.searchBar);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            // 
            // lvImageBank
            // 
            resources.ApplyResources(this.lvImageBank, "lvImageBank");
            this.lvImageBank.BuddyControl = null;
            this.lvImageBank.Columns.Add(this.colDescription);
            this.lvImageBank.Columns.Add(this.colType);
            this.lvImageBank.Columns.Add(this.colImage);
            this.lvImageBank.ContentBackColor = System.Drawing.Color.White;
            this.lvImageBank.DefaultRowHeight = ((short)(22));
            this.lvImageBank.DimSelectionWhenDisabled = true;
            this.lvImageBank.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvImageBank.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvImageBank.HeaderHeight = ((short)(25));
            this.lvImageBank.HorizontalScrollbar = true;
            this.lvImageBank.Name = "lvImageBank";
            this.lvImageBank.OddRowColor = System.Drawing.Color.White;
            this.lvImageBank.RowLineColor = System.Drawing.Color.LightGray;
            this.lvImageBank.SecondarySortColumn = ((short)(-1));
            this.lvImageBank.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvImageBank.SortSetting = "0:1";
            this.lvImageBank.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvImageBank_HeaderClicked);
            this.lvImageBank.SelectionChanged += new System.EventHandler(this.lvImageBank_SelectionChanged);
            this.lvImageBank.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvImageBank_RowDoubleClick);
            this.lvImageBank.ClientSizeChanged += new System.EventHandler(this.lvImageBank_ClientSizeChanged);
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(250));
            this.colDescription.RelativeSize = 100;
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(250));
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.MaximumWidth = ((short)(150));
            this.colType.MinimumWidth = ((short)(150));
            this.colType.RelativeSize = 0;
            this.colType.SecondarySortColumn = ((short)(-1));
            this.colType.Tag = null;
            this.colType.Width = ((short)(150));
            // 
            // colImage
            // 
            this.colImage.AutoSize = true;
            this.colImage.Clickable = false;
            this.colImage.DefaultStyle = null;
            resources.ApplyResources(this.colImage, "colImage");
            this.colImage.MaximumWidth = ((short)(200));
            this.colImage.MinimumWidth = ((short)(200));
            this.colImage.RelativeSize = 0;
            this.colImage.SecondarySortColumn = ((short)(-1));
            this.colImage.Tag = null;
            this.colImage.Width = ((short)(200));
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
            // searchBar
            // 
            resources.ApplyResources(this.searchBar, "searchBar");
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.DefaultNumberOfSections = 2;
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.SearchOptionChanged += new System.EventHandler(this.searchBar_SearchOptionChanged);
            // 
            // ImageBankView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ImageBankView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ContextButtons btnsContextButtons;
        private Controls.SearchBar searchBar;
        private Controls.ListView lvImageBank;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colType;
        private Controls.Columns.Column colImage;
    }
}
