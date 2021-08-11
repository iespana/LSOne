using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    partial class InventoryTemplateView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryTemplateView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbStore = new LSOne.Controls.DualDataComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControlLeft();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.checkBox1);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.cmbStore);
			this.pnlBottom.Controls.Add(this.label1);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// tbDescription
			// 
			resources.ApplyResources(this.tbDescription, "tbDescription");
			this.tbDescription.Name = "tbDescription";
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
			this.tbID.TabStop = false;
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// checkBox1
			// 
			resources.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.BackColor = System.Drawing.Color.Transparent;
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = false;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// cmbStore
			// 
			this.cmbStore.AddList = null;
			this.cmbStore.AllowKeyboardSelection = false;
			this.cmbStore.EnableTextBox = true;
			resources.ApplyResources(this.cmbStore, "cmbStore");
			this.cmbStore.MaxLength = 32767;
			this.cmbStore.Name = "cmbStore";
			this.cmbStore.NoChangeAllowed = false;
			this.cmbStore.OnlyDisplayID = false;
			this.cmbStore.RemoveList = null;
			this.cmbStore.RowHeight = ((short)(22));
			this.cmbStore.SecondaryData = null;
			this.cmbStore.SelectedData = null;
			this.cmbStore.SelectedDataID = null;
			this.cmbStore.SelectionList = null;
			this.cmbStore.ShowDropDownOnTyping = true;
			this.cmbStore.SkipIDColumn = true;
			this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
			this.cmbStore.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbStore_DropDown);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// InventoryTemplateView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 110;
			this.Name = "InventoryTemplateView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label label1;
        private TabControlLeft tabSheetTabs;
    }
}
