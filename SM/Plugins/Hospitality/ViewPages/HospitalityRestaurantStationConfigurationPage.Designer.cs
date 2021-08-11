using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    partial class HospitalityRestaurantStationConfigurationPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityRestaurantStationConfigurationPage));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCompressBOMReceipt = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbExcludeFromCompression = new DualDataComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbCompressBOMReceipt
            // 
            this.cmbCompressBOMReceipt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompressBOMReceipt.FormattingEnabled = true;
            this.cmbCompressBOMReceipt.Items.AddRange(new object[] {
            resources.GetString("cmbCompressBOMReceipt.Items"),
            resources.GetString("cmbCompressBOMReceipt.Items1")});
            resources.ApplyResources(this.cmbCompressBOMReceipt, "cmbCompressBOMReceipt");
            this.cmbCompressBOMReceipt.Name = "cmbCompressBOMReceipt";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbExcludeFromCompression
            // 
            this.cmbExcludeFromCompression.EnableTextBox = true;
            resources.ApplyResources(this.cmbExcludeFromCompression, "cmbExcludeFromCompression");
            this.cmbExcludeFromCompression.MaxLength = 32767;
            this.cmbExcludeFromCompression.Name = "cmbExcludeFromCompression";
            this.cmbExcludeFromCompression.SelectedData = null;
            this.cmbExcludeFromCompression.ShowDropDownOnTyping = true;
            this.cmbExcludeFromCompression.SkipIDColumn = true;
            this.cmbExcludeFromCompression.DropDown += new DropDownEventHandler(this.cmbExcludeFromCompression_DropDown);
            this.cmbExcludeFromCompression.RequestClear += new System.EventHandler(this.cmbExcludeFromCompression_RequestClear);
            // 
            // HospitalityRestaurantStationConfigurationPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.cmbExcludeFromCompression);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCompressBOMReceipt);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "HospitalityRestaurantStationConfigurationPage";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCompressBOMReceipt;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbExcludeFromCompression;
    }
}
