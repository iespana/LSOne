using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.LabelPrinting.Views
{
    partial class LabelTemplateView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelTemplateView));
			this.lblName = new System.Windows.Forms.Label();
			this.lblID = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.tbID = new System.Windows.Forms.TextBox();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
			this.lblContext = new System.Windows.Forms.Label();
			this.cmbContext = new System.Windows.Forms.ComboBox();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.cmbContext);
			this.pnlBottom.Controls.Add(this.lblContext);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.lblName);
			this.pnlBottom.Controls.Add(this.lblID);
			this.pnlBottom.Controls.Add(this.tbName);
			this.pnlBottom.Controls.Add(this.tbID);
			// 
			// lblName
			// 
			this.lblName.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			// 
			// lblID
			// 
			this.lblID.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblID, "lblID");
			this.lblID.Name = "lblID";
			// 
			// tbName
			// 
			resources.ApplyResources(this.tbName, "tbName");
			this.tbName.Name = "tbName";
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
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// lblContext
			// 
			this.lblContext.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblContext, "lblContext");
			this.lblContext.Name = "lblContext";
			// 
			// cmbContext
			// 
			this.cmbContext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbContext.FormattingEnabled = true;
			resources.ApplyResources(this.cmbContext, "cmbContext");
			this.cmbContext.Name = "cmbContext";
			// 
			// LabelTemplateView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 100;
			this.Name = "LabelTemplateView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbID;
        private TabControl tabSheetTabs;
        private System.Windows.Forms.Label lblContext;
        private System.Windows.Forms.ComboBox cmbContext;

    }
}
