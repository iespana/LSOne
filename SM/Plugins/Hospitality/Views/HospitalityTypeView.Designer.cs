using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class HospitalityTypeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityTypeView));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRestaurantID = new System.Windows.Forms.TextBox();
            this.cmbSalesType = new DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabSheetTabs = new TabControl();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.cmbSalesType);
            this.pnlBottom.Controls.Add(this.label4);
            this.pnlBottom.Controls.Add(this.tbRestaurantID);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.tbDescription);
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
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbRestaurantID
            // 
            resources.ApplyResources(this.tbRestaurantID, "tbRestaurantID");
            this.tbRestaurantID.Name = "tbRestaurantID";
            this.tbRestaurantID.ReadOnly = true;
            this.tbRestaurantID.BackColor = ColorPalette.BackgroundColor;
            this.tbRestaurantID.ForeColor = ColorPalette.DisabledTextColor;
            // 
            // cmbSalesType
            // 
            this.cmbSalesType.AddList = null;
            this.cmbSalesType.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbSalesType, "cmbSalesType");
            this.cmbSalesType.MaxLength = 32767;
            this.cmbSalesType.Name = "cmbSalesType";
            this.cmbSalesType.OnlyDisplayID = false;
            this.cmbSalesType.RemoveList = null;
            this.cmbSalesType.RowHeight = ((short)(22));
            this.cmbSalesType.SelectedData = null;
            this.cmbSalesType.SelectedDataID = null;
            this.cmbSalesType.SelectionList = null;
            this.cmbSalesType.SkipIDColumn = true;
            this.cmbSalesType.RequestData += new System.EventHandler(this.cmbSalesType_RequestData);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // HospitalityTypeView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 100;
            this.Name = "HospitalityTypeView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRestaurantID;
        private DualDataComboBox cmbSalesType;
        private System.Windows.Forms.Label label4;
        private TabControl tabSheetTabs;
    }
}
