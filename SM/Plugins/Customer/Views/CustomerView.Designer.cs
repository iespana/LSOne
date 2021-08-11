using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Customer.Views
{
    partial class CustomerView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerView));
			this.label2 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.fullNameControl = new LSOne.Controls.FullNameControl();
			this.tbDisplayName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.linkFields1 = new LSOne.Controls.LinkFields();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.linkFields1);
			this.pnlBottom.Controls.Add(this.tbDisplayName);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.fullNameControl);
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
			// fullNameControl
			// 
			this.fullNameControl.Alias = "";
			resources.ApplyResources(this.fullNameControl, "fullNameControl");
			this.fullNameControl.BackColor = System.Drawing.Color.Transparent;
			this.fullNameControl.FirstName = "";
			this.fullNameControl.LastName = "";
			this.fullNameControl.MiddleName = "";
			this.fullNameControl.Name = "fullNameControl";
			this.fullNameControl.Prefix = "";
			this.fullNameControl.ShowAlias = false;
			this.fullNameControl.Suffix = "";
			this.fullNameControl.ValueChanged += new System.EventHandler(this.fullNameControl_ValueChanged);
			// 
			// tbDisplayName
			// 
			this.tbDisplayName.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.tbDisplayName, "tbDisplayName");
			this.tbDisplayName.Name = "tbDisplayName";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// linkFields1
			// 
			this.linkFields1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.linkFields1, "linkFields1");
			this.linkFields1.Name = "linkFields1";
			this.linkFields1.TabStop = false;
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			this.tabSheetTabs.SelectedTabChanged += new System.EventHandler(this.tabSheetTabs_SelectedTabChanged);
			// 
			// CustomerView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 100;
			this.Name = "CustomerView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private FullNameControl fullNameControl;
        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label label1;
        private LinkFields linkFields1;
        private TabControlLeft tabSheetTabs;

    }
}
