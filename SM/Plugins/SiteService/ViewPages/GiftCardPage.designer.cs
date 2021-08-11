using LSOne.Controls;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
    partial class GiftCardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GiftCardPage));
            this.ntbMaximumGiftCardAmount = new LSOne.Controls.NumericTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRefillableGiftcard = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUseGiftcard = new System.Windows.Forms.CheckBox();
            this.lblUseGiftCard = new System.Windows.Forms.Label();
            this.lblGiftCardOption = new System.Windows.Forms.Label();
            this.cmbGiftCardOption = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ntbMaximumGiftCardAmount
            // 
            this.ntbMaximumGiftCardAmount.AllowDecimal = false;
            this.ntbMaximumGiftCardAmount.AllowNegative = false;
            this.ntbMaximumGiftCardAmount.CultureInfo = null;
            this.ntbMaximumGiftCardAmount.DecimalLetters = 2;
            this.ntbMaximumGiftCardAmount.HasMinValue = false;
            resources.ApplyResources(this.ntbMaximumGiftCardAmount, "ntbMaximumGiftCardAmount");
            this.ntbMaximumGiftCardAmount.MaxValue = 0D;
            this.ntbMaximumGiftCardAmount.MinValue = 0D;
            this.ntbMaximumGiftCardAmount.Name = "ntbMaximumGiftCardAmount";
            this.ntbMaximumGiftCardAmount.Value = 0D;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbRefillableGiftcard
            // 
            this.cmbRefillableGiftcard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRefillableGiftcard.FormattingEnabled = true;
            this.cmbRefillableGiftcard.Items.AddRange(new object[] {
            resources.GetString("cmbRefillableGiftcard.Items"),
            resources.GetString("cmbRefillableGiftcard.Items1")});
            resources.ApplyResources(this.cmbRefillableGiftcard, "cmbRefillableGiftcard");
            this.cmbRefillableGiftcard.Name = "cmbRefillableGiftcard";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkUseGiftcard
            // 
            resources.ApplyResources(this.chkUseGiftcard, "chkUseGiftcard");
            this.chkUseGiftcard.Name = "chkUseGiftcard";
            this.chkUseGiftcard.UseVisualStyleBackColor = true;
            this.chkUseGiftcard.CheckedChanged += new System.EventHandler(this.chkUseGiftcard_CheckedChanged);
            // 
            // lblUseGiftCard
            // 
            resources.ApplyResources(this.lblUseGiftCard, "lblUseGiftCard");
            this.lblUseGiftCard.Name = "lblUseGiftCard";
            // 
            // lblGiftCardOption
            // 
            resources.ApplyResources(this.lblGiftCardOption, "lblGiftCardOption");
            this.lblGiftCardOption.Name = "lblGiftCardOption";
            // 
            // cmbGiftCardOption
            // 
            this.cmbGiftCardOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGiftCardOption.FormattingEnabled = true;
            this.cmbGiftCardOption.Items.AddRange(new object[] {
            resources.GetString("cmbGiftCardOption.Items"),
            resources.GetString("cmbGiftCardOption.Items1"),
            resources.GetString("cmbGiftCardOption.Items2")});
            resources.ApplyResources(this.cmbGiftCardOption, "cmbGiftCardOption");
            this.cmbGiftCardOption.Name = "cmbGiftCardOption";
            // 
            // GiftCardPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ntbMaximumGiftCardAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbRefillableGiftcard);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkUseGiftcard);
            this.Controls.Add(this.lblUseGiftCard);
            this.Controls.Add(this.lblGiftCardOption);
            this.Controls.Add(this.cmbGiftCardOption);
            this.DoubleBuffered = true;
            this.Name = "GiftCardPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericTextBox ntbMaximumGiftCardAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRefillableGiftcard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUseGiftcard;
        private System.Windows.Forms.Label lblUseGiftCard;
        private System.Windows.Forms.Label lblGiftCardOption;
        private System.Windows.Forms.ComboBox cmbGiftCardOption;



    }
}
