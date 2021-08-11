using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class SalesPersonDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesPersonDialog));
            this.banner = new LSOne.Controls.TouchDialogBanner();
            this.touchKeyboard = new LSOne.Controls.TouchKeyboard();
            this.lvSelection = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colNameOnReceipt = new LSOne.Controls.Columns.Column();
            this.navigationBtns = new LSOne.Controls.TouchScrollButtonPanel();
            this.tbSearch = new LSOne.Controls.MSRTextBoxTouch(this.components);
            this.SuspendLayout();
            // 
            // banner
            // 
            this.banner.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.banner, "banner");
            this.banner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.banner.Name = "banner";
            this.banner.TabStop = false;
            // 
            // touchKeyboard
            // 
            resources.ApplyResources(this.touchKeyboard, "touchKeyboard");
            this.touchKeyboard.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard.BuddyControl = null;
            this.touchKeyboard.KeystrokeMode = true;
            this.touchKeyboard.Name = "touchKeyboard";
            this.touchKeyboard.TabStop = false;
            this.touchKeyboard.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // lvSelection
            // 
            resources.ApplyResources(this.lvSelection, "lvSelection");
            this.lvSelection.ApplyVisualStyles = false;
            this.lvSelection.BackColor = System.Drawing.Color.White;
            this.lvSelection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.BuddyControl = null;
            this.lvSelection.Columns.Add(this.colID);
            this.lvSelection.Columns.Add(this.colName);
            this.lvSelection.Columns.Add(this.colNameOnReceipt);
            this.lvSelection.ContentBackColor = System.Drawing.Color.White;
            this.lvSelection.DefaultRowHeight = ((short)(50));
            this.lvSelection.EvenRowColor = System.Drawing.Color.White;
            this.lvSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvSelection.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvSelection.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvSelection.HeaderHeight = ((short)(30));
            this.lvSelection.HideHorizontalScrollbarWhenDisabled = true;
            this.lvSelection.HideVerticalScrollbarWhenDisabled = true;
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.OddRowColor = System.Drawing.Color.White;
            this.lvSelection.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.RowLines = true;
            this.lvSelection.SecondarySortColumn = ((short)(-1));
            this.lvSelection.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvSelection.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSelection.SortSetting = "0:1";
            this.lvSelection.TouchScroll = true;
            this.lvSelection.UseFocusRectangle = false;
            this.lvSelection.VerticalScrollbarValue = 0;
            this.lvSelection.VerticalScrollbarYOffset = 0;
            this.lvSelection.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvSelection.SelectionChanged += new System.EventHandler(this.lvSelection_SelectionChanged);
            this.lvSelection.DoubleClick += new System.EventHandler(this.lvSelection_DoubleClick);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(100));
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(20));
            this.colName.RelativeSize = 40;
            this.colName.SecondarySortColumn = ((short)(-1));
            this.colName.Tag = null;
            this.colName.Width = ((short)(100));
            // 
            // colNameOnReceipt
            // 
            this.colNameOnReceipt.AutoSize = true;
            this.colNameOnReceipt.DefaultStyle = null;
            resources.ApplyResources(this.colNameOnReceipt, "colNameOnReceipt");
            this.colNameOnReceipt.MaximumWidth = ((short)(0));
            this.colNameOnReceipt.MinimumWidth = ((short)(10));
            this.colNameOnReceipt.RelativeSize = 40;
            this.colNameOnReceipt.SecondarySortColumn = ((short)(-1));
            this.colNameOnReceipt.Tag = null;
            this.colNameOnReceipt.Width = ((short)(100));
            // 
            // navigationBtns
            // 
            resources.ApplyResources(this.navigationBtns, "navigationBtns");
            this.navigationBtns.BackColor = System.Drawing.Color.White;
            this.navigationBtns.ButtonHeight = 50;
            this.navigationBtns.Name = "navigationBtns";
            this.navigationBtns.Click += new LSOne.Controls.ScrollButtonEventHandler(this.navigationBtns_Click);
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.EndCharacter = null;
            this.tbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbSearch.LastTrack = null;
            this.tbSearch.ManualEntryOfTrack = true;
            this.tbSearch.MaxLength = 32767;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.NumericOnly = false;
            this.tbSearch.Seperator = null;
            this.tbSearch.StartCharacter = null;
            this.tbSearch.TrackSeperation = LSOne.Controls.TrackSeperation.None;
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // SalesPersonDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.navigationBtns);
            this.Controls.Add(this.touchKeyboard);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.banner);
            this.Name = "SalesPersonDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner banner;
        private TouchKeyboard touchKeyboard;
        private ListView lvSelection;
        private Column colID;
        private Column colName;
        private TouchScrollButtonPanel navigationBtns;
        private Column colNameOnReceipt;
        private MSRTextBoxTouch tbSearch;
    }
}