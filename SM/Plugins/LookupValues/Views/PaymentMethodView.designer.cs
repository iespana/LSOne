using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    partial class PaymentMethodView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentMethodView));
			this.label2 = new System.Windows.Forms.Label();
			this.tbID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbDescription = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDefaultFunction = new System.Windows.Forms.ComboBox();
			this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
			this.label4 = new System.Windows.Forms.Label();
			this.chkLocalCurrency = new System.Windows.Forms.CheckBox();
			this.pnlBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.chkLocalCurrency);
			this.pnlBottom.Controls.Add(this.label4);
			this.pnlBottom.Controls.Add(this.tabSheetTabs);
			this.pnlBottom.Controls.Add(this.cmbDefaultFunction);
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
			// cmbDefaultFunction
			// 
			this.cmbDefaultFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDefaultFunction.FormattingEnabled = true;
			this.cmbDefaultFunction.Items.AddRange(new object[] {
            resources.GetString("cmbDefaultFunction.Items"),
            resources.GetString("cmbDefaultFunction.Items1"),
            resources.GetString("cmbDefaultFunction.Items2"),
            resources.GetString("cmbDefaultFunction.Items3"),
            resources.GetString("cmbDefaultFunction.Items4"),
            resources.GetString("cmbDefaultFunction.Items5")});
			resources.ApplyResources(this.cmbDefaultFunction, "cmbDefaultFunction");
			this.cmbDefaultFunction.Name = "cmbDefaultFunction";
			// 
			// tabSheetTabs
			// 
			resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
			this.tabSheetTabs.Name = "tabSheetTabs";
			this.tabSheetTabs.TabStop = true;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// chkLocalCurrency
			// 
			resources.ApplyResources(this.chkLocalCurrency, "chkLocalCurrency");
			this.chkLocalCurrency.Name = "chkLocalCurrency";
			this.chkLocalCurrency.UseVisualStyleBackColor = true;
			// 
			// PaymentMethodView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 100;
			this.Name = "PaymentMethodView";
			this.pnlBottom.ResumeLayout(false);
			this.pnlBottom.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDefaultFunction;
        private TabControl tabSheetTabs;
        private System.Windows.Forms.CheckBox chkLocalCurrency;
        private System.Windows.Forms.Label label4;
    }
}
