using LSOne.Controls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    partial class SalesTypeView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesTypeView));
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.cmbPriceGroup = new LSOne.Controls.DualDataComboBox();
			this.cmbSalesTaxGroup = new LSOne.Controls.DualDataComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.cmbSalesTaxGroup);
			this.pnlBottom.Controls.Add(this.label5);
			this.pnlBottom.Controls.Add(this.label12);
			this.pnlBottom.Controls.Add(this.cmbPriceGroup);
			this.pnlBottom.Controls.Add(this.tbDescription);
			this.pnlBottom.Controls.Add(this.label3);
			this.pnlBottom.Controls.Add(this.tbID);
			this.pnlBottom.Controls.Add(this.label2);
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
			this.tbID.BackColor = ColorPalette.BackgroundColor;
			this.tbID.ForeColor = ColorPalette.DisabledTextColor;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// cmbPriceGroup
			// 
			this.cmbPriceGroup.AddList = null;
			this.cmbPriceGroup.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbPriceGroup, "cmbPriceGroup");
			this.cmbPriceGroup.MaxLength = 32767;
			this.cmbPriceGroup.Name = "cmbPriceGroup";
			this.cmbPriceGroup.NoChangeAllowed = false;
			this.cmbPriceGroup.OnlyDisplayID = false;
			this.cmbPriceGroup.RemoveList = null;
			this.cmbPriceGroup.RowHeight = ((short)(22));
			this.cmbPriceGroup.SecondaryData = null;
			this.cmbPriceGroup.SelectedData = null;
			this.cmbPriceGroup.SelectedDataID = null;
			this.cmbPriceGroup.SelectionList = null;
			this.cmbPriceGroup.SkipIDColumn = true;
			this.cmbPriceGroup.RequestData += new System.EventHandler(this.cmbPriceGroup_RequestData);
			this.cmbPriceGroup.RequestClear += new System.EventHandler(this.ClearData);
			// 
			// cmbSalesTaxGroup
			// 
			this.cmbSalesTaxGroup.AddList = null;
			this.cmbSalesTaxGroup.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbSalesTaxGroup, "cmbSalesTaxGroup");
			this.cmbSalesTaxGroup.MaxLength = 32767;
			this.cmbSalesTaxGroup.Name = "cmbSalesTaxGroup";
			this.cmbSalesTaxGroup.NoChangeAllowed = false;
			this.cmbSalesTaxGroup.OnlyDisplayID = false;
			this.cmbSalesTaxGroup.RemoveList = null;
			this.cmbSalesTaxGroup.RowHeight = ((short)(22));
			this.cmbSalesTaxGroup.SecondaryData = null;
			this.cmbSalesTaxGroup.SelectedData = null;
			this.cmbSalesTaxGroup.SelectedDataID = null;
			this.cmbSalesTaxGroup.SelectionList = null;
			this.cmbSalesTaxGroup.SkipIDColumn = true;
			this.cmbSalesTaxGroup.RequestData += new System.EventHandler(this.cmbSalesTaxGroup_RequestData);
			this.cmbSalesTaxGroup.RequestClear += new System.EventHandler(this.ClearData);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// SalesTypeView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 72;
			this.Name = "SalesTypeView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private DualDataComboBox cmbPriceGroup;
        private DualDataComboBox cmbSalesTaxGroup;
        private System.Windows.Forms.Label label5;
    }
}
