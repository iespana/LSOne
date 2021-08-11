using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class ImageBankSelectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageBankSelectDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.lvImages = new LSOne.Controls.ListView();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colPreview = new LSOne.Controls.Columns.Column();
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Name = "panel1";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            // lvImages
            // 
            resources.ApplyResources(this.lvImages, "lvImages");
            this.lvImages.BuddyControl = null;
            this.lvImages.Columns.Add(this.colDescription);
            this.lvImages.Columns.Add(this.colPreview);
            this.lvImages.ContentBackColor = System.Drawing.Color.White;
            this.lvImages.DefaultRowHeight = ((short)(22));
            this.lvImages.DimSelectionWhenDisabled = true;
            this.lvImages.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvImages.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvImages.HeaderHeight = ((short)(25));
            this.lvImages.Name = "lvImages";
            this.lvImages.OddRowColor = System.Drawing.Color.White;
            this.lvImages.RowLineColor = System.Drawing.Color.LightGray;
            this.lvImages.SecondarySortColumn = ((short)(-1));
            this.lvImages.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvImages.SortSetting = "0:1";
            this.lvImages.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvImages_HeaderClicked);
            this.lvImages.SelectionChanged += new System.EventHandler(this.lvImages_SelectionChanged);
            this.lvImages.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvImages_RowDoubleClick);
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(300));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(300));
            // 
            // colPreview
            // 
            this.colPreview.AutoSize = true;
            this.colPreview.Clickable = false;
            this.colPreview.DefaultStyle = null;
            resources.ApplyResources(this.colPreview, "colPreview");
            this.colPreview.MaximumWidth = ((short)(0));
            this.colPreview.MinimumWidth = ((short)(200));
            this.colPreview.SecondarySortColumn = ((short)(-1));
            this.colPreview.Tag = null;
            this.colPreview.Width = ((short)(200));
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
            // ImageBankSelectDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.lvImages);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ImageBankSelectDialog";
            this.Load += new System.EventHandler(this.ImageBankSelectDialog_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.searchBar, 0);
            this.Controls.SetChildIndex(this.lvImages, 0);
            this.Controls.SetChildIndex(this.btnsContextButtons, 0);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private SearchBar searchBar;
        private ListView lvImages;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colPreview;
        private ContextButtons btnsContextButtons;
    }
}