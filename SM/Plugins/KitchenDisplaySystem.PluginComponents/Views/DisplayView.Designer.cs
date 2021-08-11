using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    partial class DisplayView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisplayView));
			this.tbStationName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
			this.cmbStationType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.tbStationLetter = new System.Windows.Forms.TextBox();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.tbStationLetter);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.cmbStationType);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.tbStationName);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			// 
			// tbStationName
			// 
			resources.ApplyResources(this.tbStationName, "tbStationName");
			this.tbStationName.Name = "tbStationName";
			// 
			// label3
			// 
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
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// cmbStationType
			// 
			this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbStationType.FormattingEnabled = true;
			resources.ApplyResources(this.cmbStationType, "cmbStationType");
			this.cmbStationType.Name = "cmbStationType";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// tbStationLetter
			// 
			resources.ApplyResources(this.tbStationLetter, "tbStationLetter");
			this.tbStationLetter.Name = "tbStationLetter";
			// 
			// DisplayView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "DisplayView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbStationName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private TabControlLeft tabSheetTabs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStationType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStationLetter;
    }
}
