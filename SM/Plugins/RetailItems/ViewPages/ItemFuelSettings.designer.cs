using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
    partial class ItemFuelSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemFuelSettings));
            this.groupBox4 = new LSOne.Controls.DoubleBufferGroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbGradeID = new System.Windows.Forms.TextBox();
            this.chkFuelItem = new LSOne.Controls.DoubleBufferedCheckbox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.tbGradeID);
            this.groupBox4.Controls.Add(this.chkFuelItem);
            this.groupBox4.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tbGradeID
            // 
            resources.ApplyResources(this.tbGradeID, "tbGradeID");
            this.tbGradeID.Name = "tbGradeID";
            // 
            // chkFuelItem
            // 
            resources.ApplyResources(this.chkFuelItem, "chkFuelItem");
            this.chkFuelItem.Name = "chkFuelItem";
            this.chkFuelItem.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // ItemFuelSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox4);
            this.DoubleBuffered = true;
            this.Name = "ItemFuelSettings";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferGroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbGradeID;
        private DoubleBufferedCheckbox chkFuelItem;
        private System.Windows.Forms.Label label11;

        //private System.Windows.Forms.ComboBox cmbValPeriod;


    }
}
