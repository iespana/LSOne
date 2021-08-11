using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Columns;
using ListView = LSOne.Controls.ListView;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    partial class SelectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionDialog));
            this.tbSearch = new LSOne.Controls.ShadeTextBoxTouch();
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.lvSelection = new LSOne.Controls.ListView();
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.touchScrollButtonPanel1 = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbSearch.MaxLength = 32767;
            this.tbSearch.Name = "tbSearch";
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard1.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 100;
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(777));
            // 
            // lvSelection
            // 
            resources.ApplyResources(this.lvSelection, "lvSelection");
            this.lvSelection.ApplyVisualStyles = false;
            this.lvSelection.BackColor = System.Drawing.Color.White;
            this.lvSelection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.BuddyControl = null;
            this.lvSelection.Columns.Add(this.column1);
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
            this.lvSelection.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvSelection_RowDoubleClick);
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // touchScrollButtonPanel1
            // 
            resources.ApplyResources(this.touchScrollButtonPanel1, "touchScrollButtonPanel1");
            this.touchScrollButtonPanel1.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel1.ButtonHeight = 50;
            this.touchScrollButtonPanel1.Name = "touchScrollButtonPanel1";
            this.touchScrollButtonPanel1.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel1_Click);
            // 
            // SelectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.touchScrollButtonPanel1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.touchKeyboard1);
            this.Controls.Add(this.lvSelection);
            this.Name = "SelectionDialog";
            this.ResumeLayout(false);

        }

        #endregion
        private LSOne.Controls.ShadeTextBoxTouch tbSearch;
        private TouchKeyboard touchKeyboard1;
        private Column column1;
        private ListView lvSelection;
        private TouchDialogBanner touchDialogBanner1;
        private TouchScrollButtonPanel touchScrollButtonPanel1;
    }
}