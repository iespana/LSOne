using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class StyleProfileView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleProfileView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.tbExtraInfo = new System.Windows.Forms.TextBox();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lvProfileSettings = new LSOne.Controls.ListView();
			this.colContext = new LSOne.Controls.Columns.Column();
			this.colMenu = new LSOne.Controls.Columns.Column();
			this.colStyle = new LSOne.Controls.Columns.Column();
			this.colSystemStyle = new LSOne.Controls.Columns.Column();
			this.btnsContextButtons = new LSOne.Controls.ContextButtons();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.btnsContextButtons);
			this.pnlBottom.Controls.Add(this.lvProfileSettings);
			this.pnlBottom.Controls.Add(this.txtDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label1);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// tbExtraInfo
			// 
			resources.ApplyResources(this.tbExtraInfo, "tbExtraInfo");
			this.tbExtraInfo.Name = "tbExtraInfo";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			resources.ApplyResources(this.dateTimePicker1, "dateTimePicker1");
			this.dateTimePicker1.Name = "dateTimePicker1";
			// 
			// txtDescription
			// 
			resources.ApplyResources(this.txtDescription, "txtDescription");
			this.txtDescription.Name = "txtDescription";
			// 
			// label3
			// 
			this.label3.AllowDrop = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbID
			// 
			this.tbID.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbID, "tbID");
			this.tbID.Name = "tbID";
			this.tbID.ReadOnly = true;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lvProfileSettings
			// 
			resources.ApplyResources(this.lvProfileSettings, "lvProfileSettings");
			this.lvProfileSettings.BorderColor = System.Drawing.Color.DarkGray;
			this.lvProfileSettings.BuddyControl = null;
			this.lvProfileSettings.Columns.Add(this.colContext);
			this.lvProfileSettings.Columns.Add(this.colMenu);
			this.lvProfileSettings.Columns.Add(this.colStyle);
			this.lvProfileSettings.Columns.Add(this.colSystemStyle);
			this.lvProfileSettings.ContentBackColor = System.Drawing.Color.White;
			this.lvProfileSettings.DefaultRowHeight = ((short)(22));
			this.lvProfileSettings.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
			this.lvProfileSettings.HeaderBackColor = System.Drawing.Color.White;
			this.lvProfileSettings.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvProfileSettings.HeaderHeight = ((short)(25));
			this.lvProfileSettings.Name = "lvProfileSettings";
			this.lvProfileSettings.OddRowColor = System.Drawing.Color.White;
			this.lvProfileSettings.RowLineColor = System.Drawing.Color.LightGray;
			this.lvProfileSettings.SecondarySortColumn = ((short)(-1));
			this.lvProfileSettings.SelectedRowColor = System.Drawing.Color.LightGray;
			this.lvProfileSettings.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
			this.lvProfileSettings.SortSetting = "-1:1";
			this.lvProfileSettings.VerticalScrollbarValue = 0;
			this.lvProfileSettings.VerticalScrollbarYOffset = 0;
			this.lvProfileSettings.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvProfileSettings_HeaderClicked);
			this.lvProfileSettings.SelectionChanged += new System.EventHandler(this.lvProfileSettings_SelectionChanged);
			this.lvProfileSettings.RowClick += new LSOne.Controls.RowClickDelegate(this.lvProfileSettings_RowClick);
			this.lvProfileSettings.DoubleClick += new System.EventHandler(this.btnsContextButtons_EditButtonClicked);
			// 
			// colContext
			// 
			this.colContext.DefaultStyle = null;
			resources.ApplyResources(this.colContext, "colContext");
			this.colContext.MaximumWidth = ((short)(0));
			this.colContext.MinimumWidth = ((short)(30));
			this.colContext.SecondarySortColumn = ((short)(-1));
			this.colContext.Tag = null;
			this.colContext.Width = ((short)(200));
			// 
			// colMenu
			// 
			this.colMenu.DefaultStyle = null;
			resources.ApplyResources(this.colMenu, "colMenu");
			this.colMenu.MaximumWidth = ((short)(0));
			this.colMenu.MinimumWidth = ((short)(10));
			this.colMenu.SecondarySortColumn = ((short)(-1));
			this.colMenu.Tag = null;
			this.colMenu.Width = ((short)(200));
			// 
			// colStyle
			// 
			this.colStyle.DefaultStyle = null;
			resources.ApplyResources(this.colStyle, "colStyle");
			this.colStyle.MaximumWidth = ((short)(0));
			this.colStyle.MinimumWidth = ((short)(10));
			this.colStyle.SecondarySortColumn = ((short)(-1));
			this.colStyle.Tag = null;
			this.colStyle.Width = ((short)(200));
			// 
			// colSystemStyle
			// 
			this.colSystemStyle.DefaultStyle = null;
			resources.ApplyResources(this.colSystemStyle, "colSystemStyle");
			this.colSystemStyle.MaximumWidth = ((short)(0));
			this.colSystemStyle.MinimumWidth = ((short)(10));
			this.colSystemStyle.SecondarySortColumn = ((short)(-1));
			this.colSystemStyle.Tag = null;
			this.colSystemStyle.Width = ((short)(75));
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
			// StyleProfileView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "StyleProfileView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbExtraInfo;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private ListView lvProfileSettings;
        private Column colContext;
        private Column colMenu;
        private Column colStyle;
        private Column colSystemStyle;
        private ContextButtons btnsContextButtons;


    }
}
