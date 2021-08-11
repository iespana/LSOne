using LSOne.Controls;

namespace LSOne.ViewPlugins.SerialNumbers.Views
{
    partial class SerialNumbersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialNumbersView));
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvSerialNumbers = new LSOne.Controls.ListView();
            this.searchBarSerialNumbers = new LSOne.Controls.SearchBar();
            this.itemDataScroll = new LSOne.Controls.DatabasePageDisplay();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.itemDataScroll);
            this.pnlBottom.Controls.Add(this.searchBarSerialNumbers);
            this.pnlBottom.Controls.Add(this.lvSerialNumbers);
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
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
            // lvSerialNumbers
            // 
            resources.ApplyResources(this.lvSerialNumbers, "lvSerialNumbers");
            this.lvSerialNumbers.BuddyControl = null;
            this.lvSerialNumbers.ContentBackColor = System.Drawing.Color.White;
            this.lvSerialNumbers.DefaultRowHeight = ((short)(22));
            this.lvSerialNumbers.DimSelectionWhenDisabled = true;
            this.lvSerialNumbers.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvSerialNumbers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSerialNumbers.HeaderHeight = ((short)(25));
            this.lvSerialNumbers.HorizontalScrollbar = true;
            this.lvSerialNumbers.Name = "lvSerialNumbers";
            this.lvSerialNumbers.OddRowColor = System.Drawing.Color.White;
            this.lvSerialNumbers.RowLineColor = System.Drawing.Color.LightGray;
            this.lvSerialNumbers.SecondarySortColumn = ((short)(-1));
            this.lvSerialNumbers.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvSerialNumbers.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSerialNumbers.SortSetting = "-1:1";
            this.lvSerialNumbers.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvSerialNumbers_HeaderClicked);
            this.lvSerialNumbers.SelectionChanged += new System.EventHandler(this.lvSerialNumbers_SelectedIndexChanged);
            this.lvSerialNumbers.DoubleClick += new System.EventHandler(this.lvSerialNumbers_DoubleClick);
            // 
            // searchBarSerialNumbers
            // 
            resources.ApplyResources(this.searchBarSerialNumbers, "searchBarSerialNumbers");
            this.searchBarSerialNumbers.BackColor = System.Drawing.Color.Transparent;
            this.searchBarSerialNumbers.BuddyControl = null;
            this.searchBarSerialNumbers.Name = "searchBarSerialNumbers";
            this.searchBarSerialNumbers.SearchOptionEnabled = true;
            this.searchBarSerialNumbers.SetupConditions += new System.EventHandler(this.searchBarSerialNumbers_SetupConditions);
            this.searchBarSerialNumbers.SearchClicked += new System.EventHandler(this.searchBarSerialNumbers_SearchClicked);
            this.searchBarSerialNumbers.SaveAsDefault += new System.EventHandler(this.searchBarSerialNumbers_SaveAsDefault);
            this.searchBarSerialNumbers.LoadDefault += new System.EventHandler(this.searchBarSerialNumbers_LoadDefault);
            this.searchBarSerialNumbers.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBarSerialNumbers_UnknownControlAdd);
            this.searchBarSerialNumbers.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBarSerialNumbers_UnknownControlRemove);
            this.searchBarSerialNumbers.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBarSerialNumbers_UnknownControlHasSelection);
            this.searchBarSerialNumbers.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBarSerialNumbers_UnknownControlGetSelection);
            this.searchBarSerialNumbers.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBarSerialNumbers_UnknownControlSetSelection);
            // 
            // itemDataScroll
            // 
            resources.ApplyResources(this.itemDataScroll, "itemDataScroll");
            this.itemDataScroll.BackColor = System.Drawing.Color.Transparent;
            this.itemDataScroll.Name = "itemDataScroll";
            this.itemDataScroll.PageSize = 0;
            this.itemDataScroll.PageChanged += new System.EventHandler(this.OnPageScrollPageChanged);
            // 
            // SerialNumbersView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SerialNumbersView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsEditAddRemove;
        private ListView lvSerialNumbers;
        private SearchBar searchBarSerialNumbers;
        private DatabasePageDisplay itemDataScroll;
    }
}
