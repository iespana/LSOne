namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class RecurrenceControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecurrenceControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSeperator = new System.Windows.Forms.Label();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.rbYearly = new System.Windows.Forms.RadioButton();
            this.rbMonthly = new System.Windows.Forms.RadioButton();
            this.rbWeekly = new System.Windows.Forms.RadioButton();
            this.rbDaily = new System.Windows.Forms.RadioButton();
            this.rbHourly = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.rangeControl = new LSOne.ViewPlugins.Scheduler.Controls.Recurrence.RangeControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lblSeperator);
            this.groupBox1.Controls.Add(this.pnlDetails);
            this.groupBox1.Controls.Add(this.rbYearly);
            this.groupBox1.Controls.Add(this.rbMonthly);
            this.groupBox1.Controls.Add(this.rbWeekly);
            this.groupBox1.Controls.Add(this.rbDaily);
            this.groupBox1.Controls.Add(this.rbHourly);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // lblSeperator
            // 
            resources.ApplyResources(this.lblSeperator, "lblSeperator");
            this.lblSeperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeperator.Name = "lblSeperator";
            // 
            // pnlDetails
            // 
            resources.ApplyResources(this.pnlDetails, "pnlDetails");
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDetails_Paint);
            // 
            // rbYearly
            // 
            resources.ApplyResources(this.rbYearly, "rbYearly");
            this.rbYearly.Name = "rbYearly";
            this.rbYearly.TabStop = true;
            this.rbYearly.UseVisualStyleBackColor = true;
            this.rbYearly.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // rbMonthly
            // 
            resources.ApplyResources(this.rbMonthly, "rbMonthly");
            this.rbMonthly.Name = "rbMonthly";
            this.rbMonthly.TabStop = true;
            this.rbMonthly.UseVisualStyleBackColor = true;
            this.rbMonthly.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // rbWeekly
            // 
            resources.ApplyResources(this.rbWeekly, "rbWeekly");
            this.rbWeekly.Name = "rbWeekly";
            this.rbWeekly.TabStop = true;
            this.rbWeekly.UseVisualStyleBackColor = true;
            this.rbWeekly.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // rbDaily
            // 
            resources.ApplyResources(this.rbDaily, "rbDaily");
            this.rbDaily.Name = "rbDaily";
            this.rbDaily.TabStop = true;
            this.rbDaily.UseVisualStyleBackColor = true;
            this.rbDaily.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // rbHourly
            // 
            resources.ApplyResources(this.rbHourly, "rbHourly");
            this.rbHourly.Name = "rbHourly";
            this.rbHourly.TabStop = true;
            this.rbHourly.UseVisualStyleBackColor = true;
            this.rbHourly.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.rangeControl);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // rangeControl
            // 
            resources.ApplyResources(this.rangeControl, "rangeControl");
            this.rangeControl.Name = "rangeControl";
            this.rangeControl.SettingChanged += new System.EventHandler<System.EventArgs>(this.DetailControlSettingChanged);
            // 
            // RecurrenceControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "RecurrenceControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblSeperator;
        private System.Windows.Forms.RadioButton rbYearly;
        private System.Windows.Forms.RadioButton rbMonthly;
        private System.Windows.Forms.RadioButton rbWeekly;
        private System.Windows.Forms.RadioButton rbDaily;
        private System.Windows.Forms.RadioButton rbHourly;
        private System.Windows.Forms.GroupBox groupBox2;
        private RangeControl rangeControl;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
