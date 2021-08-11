using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class CardTypeView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardTypeView));
			this.label2 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lvNumberSeries = new LSOne.Controls.ExtendedListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnsContextButtons = new LSOne.Controls.ContextButtons();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.btnsContextButtons);
			this.pnlBottom.Controls.Add(this.lvNumberSeries);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.cmbType);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
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
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cmbType
			// 
			this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3"),
            resources.GetString("cmbType.Items4"),
            resources.GetString("cmbType.Items5"),
            resources.GetString("cmbType.Items6"),
            resources.GetString("cmbType.Items7")});
			resources.ApplyResources(this.cmbType, "cmbType");
			this.cmbType.Name = "cmbType";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// lvNumberSeries
			// 
			resources.ApplyResources(this.lvNumberSeries, "lvNumberSeries");
			this.lvNumberSeries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
			this.lvNumberSeries.FullRowSelect = true;
			this.lvNumberSeries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvNumberSeries.HideSelection = false;
			this.lvNumberSeries.LockDrawing = false;
			this.lvNumberSeries.MultiSelect = false;
			this.lvNumberSeries.Name = "lvNumberSeries";
			this.lvNumberSeries.SortColumn = -1;
			this.lvNumberSeries.SortedBackwards = false;
			this.lvNumberSeries.UseCompatibleStateImageBehavior = false;
			this.lvNumberSeries.UseEveryOtherRowColoring = true;
			this.lvNumberSeries.View = System.Windows.Forms.View.Details;
			this.lvNumberSeries.SelectedIndexChanged += new System.EventHandler(this.lvNumberSeries_SelectedIndexChanged);
			this.lvNumberSeries.DoubleClick += new System.EventHandler(this.lvNumberSeries_DoubleClick);
			// 
			// columnHeader1
			// 
			resources.ApplyResources(this.columnHeader1, "columnHeader1");
			// 
			// columnHeader3
			// 
			resources.ApplyResources(this.columnHeader3, "columnHeader3");
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
			// CardTypeView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "CardTypeView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label4;
        private ExtendedListView lvNumberSeries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private ContextButtons btnsContextButtons;

    }
}
