using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    partial class ButtonMenuView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonMenuView));
			this.tbID = new System.Windows.Forms.TextBox();
			this.lblID = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblMainMenu = new System.Windows.Forms.Label();
			this.chkMainMenu = new System.Windows.Forms.CheckBox();
			this.lblColumns = new System.Windows.Forms.Label();
			this.ntColumns = new LSOne.Controls.NumericTextBox();
			this.ntRows = new LSOne.Controls.NumericTextBox();
			this.lblRows = new System.Windows.Forms.Label();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.ntRows);
			this.pnlBottom.Controls.Add(this.lblRows);
			this.pnlBottom.Controls.Add(this.ntColumns);
			this.pnlBottom.Controls.Add(this.lblColumns);
			this.pnlBottom.Controls.Add(this.chkMainMenu);
			this.pnlBottom.Controls.Add(this.lblMainMenu);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.lblDescription);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.lblID);
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
			// lblID
			// 
			this.lblID.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblID, "lblID");
			this.lblID.Name = "lblID";
			// 
			// tbDescription
			// 
			this.tbDescription.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
			// 
			// lblDescription
			// 
			this.lblDescription.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblDescription, "lblDescription");
			this.lblDescription.Name = "lblDescription";
			// 
			// lblMainMenu
			// 
			this.lblMainMenu.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblMainMenu, "lblMainMenu");
			this.lblMainMenu.Name = "lblMainMenu";
			// 
			// chkMainMenu
			// 
			resources.ApplyResources(this.chkMainMenu, "chkMainMenu");
			this.chkMainMenu.Name = "chkMainMenu";
			this.chkMainMenu.UseVisualStyleBackColor = true;
			// 
			// lblColumns
			// 
			this.lblColumns.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblColumns, "lblColumns");
			this.lblColumns.Name = "lblColumns";
			// 
			// ntColumns
			// 
			this.ntColumns.AllowDecimal = false;
			this.ntColumns.AllowNegative = false;
			this.ntColumns.CultureInfo = null;
			this.ntColumns.DecimalLetters = 2;
			this.ntColumns.ForeColor = System.Drawing.Color.Black;
			this.ntColumns.HasMinValue = false;
			resources.ApplyResources(this.ntColumns, "ntColumns");
			this.ntColumns.MaxValue = 99999999D;
			this.ntColumns.MinValue = 0D;
			this.ntColumns.Name = "ntColumns";
			this.ntColumns.Value = 0D;
			// 
			// ntRows
			// 
			this.ntRows.AllowDecimal = false;
			this.ntRows.AllowNegative = false;
			this.ntRows.CultureInfo = null;
			this.ntRows.DecimalLetters = 2;
			this.ntRows.ForeColor = System.Drawing.Color.Black;
			this.ntRows.HasMinValue = false;
			resources.ApplyResources(this.ntRows, "ntRows");
			this.ntRows.MaxValue = 99999999D;
			this.ntRows.MinValue = 0D;
			this.ntRows.Name = "ntRows";
			this.ntRows.Value = 0D;
			// 
			// lblRows
			// 
			this.lblRows.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblRows, "lblRows");
			this.lblRows.Name = "lblRows";
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// ButtonMenuView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "ButtonMenuView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblMainMenu;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private LSOne.Controls.NumericTextBox ntRows;
        private System.Windows.Forms.Label lblRows;
        private LSOne.Controls.NumericTextBox ntColumns;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.CheckBox chkMainMenu;
        private ViewCore.Controls.TabControlLeft tabSheetTabs;
    }
}
