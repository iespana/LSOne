namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class MonthlyControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonthlyControl));
            this.tbDays = new System.Windows.Forms.TextBox();
            this.rbDayOfMonth = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMonth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbWeekdayOfMonth = new System.Windows.Forms.RadioButton();
            this.cmbWeekdaySequence = new System.Windows.Forms.ComboBox();
            this.cmbWeekdayCharms = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbWeekMonth = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDays
            // 
            resources.ApplyResources(this.tbDays, "tbDays");
            this.tbDays.Name = "tbDays";
            this.tbDays.TextChanged += new System.EventHandler(this.tbDays_TextChanged);
            this.tbDays.Validating += new System.ComponentModel.CancelEventHandler(this.tbDays_Validating);
            // 
            // rbDayOfMonth
            // 
            resources.ApplyResources(this.rbDayOfMonth, "rbDayOfMonth");
            this.rbDayOfMonth.Checked = true;
            this.rbDayOfMonth.Name = "rbDayOfMonth";
            this.rbDayOfMonth.TabStop = true;
            this.rbDayOfMonth.UseVisualStyleBackColor = true;
            this.rbDayOfMonth.CheckedChanged += new System.EventHandler(this.rbDayOfMonth_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbMonth
            // 
            resources.ApplyResources(this.tbMonth, "tbMonth");
            this.tbMonth.Name = "tbMonth";
            this.tbMonth.TextChanged += new System.EventHandler(this.tbMonth_TextChanged);
            this.tbMonth.Validating += new System.ComponentModel.CancelEventHandler(this.tbMonth_Validating);
            this.tbMonth.Validated += new System.EventHandler(this.tbMonth_Validated);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // rbWeekdayOfMonth
            // 
            resources.ApplyResources(this.rbWeekdayOfMonth, "rbWeekdayOfMonth");
            this.rbWeekdayOfMonth.Name = "rbWeekdayOfMonth";
            this.rbWeekdayOfMonth.UseVisualStyleBackColor = true;
            this.rbWeekdayOfMonth.CheckedChanged += new System.EventHandler(this.rbWeekdayOfMonth_CheckedChanged);
            // 
            // cmbWeekdaySequence
            // 
            this.cmbWeekdaySequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekdaySequence.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWeekdaySequence, "cmbWeekdaySequence");
            this.cmbWeekdaySequence.Name = "cmbWeekdaySequence";
            this.cmbWeekdaySequence.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdaySequence_SelectedIndexChanged);
            // 
            // cmbWeekdayCharms
            // 
            this.cmbWeekdayCharms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekdayCharms.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWeekdayCharms, "cmbWeekdayCharms");
            this.cmbWeekdayCharms.Name = "cmbWeekdayCharms";
            this.cmbWeekdayCharms.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdayCharms_SelectedIndexChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbWeekMonth
            // 
            resources.ApplyResources(this.tbWeekMonth, "tbWeekMonth");
            this.tbWeekMonth.Name = "tbWeekMonth";
            this.tbWeekMonth.TextChanged += new System.EventHandler(this.tbWeekMonth_TextChanged);
            this.tbWeekMonth.Validating += new System.ComponentModel.CancelEventHandler(this.tbWeekMonth_Validating);
            this.tbWeekMonth.Validated += new System.EventHandler(this.tbWeekMonth_Validated);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.tbDays);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.tbMonth);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // flowLayoutPanel2
            // 
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.cmbWeekdaySequence);
            this.flowLayoutPanel2.Controls.Add(this.cmbWeekdayCharms);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.tbWeekMonth);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // MonthlyControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.rbWeekdayOfMonth);
            this.Controls.Add(this.rbDayOfMonth);
            this.Name = "MonthlyControl";
            this.Load += new System.EventHandler(this.RecurrenceDetailMonthlyControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDays;
        private System.Windows.Forms.RadioButton rbDayOfMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbWeekdayOfMonth;
        private System.Windows.Forms.ComboBox cmbWeekdaySequence;
        private System.Windows.Forms.ComboBox cmbWeekdayCharms;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbWeekMonth;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}
