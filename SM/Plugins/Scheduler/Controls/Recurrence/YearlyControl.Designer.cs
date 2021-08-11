namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class YearlyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YearlyControl));
            this.rbOnDayOfMonth = new System.Windows.Forms.RadioButton();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.tbDay = new System.Windows.Forms.TextBox();
            this.cmbWeekdayCharms = new System.Windows.Forms.ComboBox();
            this.cmbWeekdaySequence = new System.Windows.Forms.ComboBox();
            this.rbWeekdayOfMonth = new System.Windows.Forms.RadioButton();
            this.cmbWeekdayMonth = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // rbOnDayOfMonth
            // 
            resources.ApplyResources(this.rbOnDayOfMonth, "rbOnDayOfMonth");
            this.rbOnDayOfMonth.Name = "rbOnDayOfMonth";
            this.rbOnDayOfMonth.TabStop = true;
            this.rbOnDayOfMonth.UseVisualStyleBackColor = true;
            this.rbOnDayOfMonth.CheckedChanged += new System.EventHandler(this.rbOnDayOfMonth_CheckedChanged);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            resources.ApplyResources(this.cmbMonth, "cmbMonth");
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
            // 
            // tbDay
            // 
            resources.ApplyResources(this.tbDay, "tbDay");
            this.tbDay.Name = "tbDay";
            this.tbDay.TextChanged += new System.EventHandler(this.tbDay_TextChanged);
            this.tbDay.Validating += new System.ComponentModel.CancelEventHandler(this.tbDay_Validating);
            // 
            // cmbWeekdayCharms
            // 
            this.cmbWeekdayCharms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekdayCharms.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWeekdayCharms, "cmbWeekdayCharms");
            this.cmbWeekdayCharms.Name = "cmbWeekdayCharms";
            this.cmbWeekdayCharms.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdayCharms_SelectedIndexChanged);
            // 
            // cmbWeekdaySequence
            // 
            this.cmbWeekdaySequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekdaySequence.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWeekdaySequence, "cmbWeekdaySequence");
            this.cmbWeekdaySequence.Name = "cmbWeekdaySequence";
            this.cmbWeekdaySequence.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdaySequence_SelectedIndexChanged);
            // 
            // rbWeekdayOfMonth
            // 
            resources.ApplyResources(this.rbWeekdayOfMonth, "rbWeekdayOfMonth");
            this.rbWeekdayOfMonth.Name = "rbWeekdayOfMonth";
            this.rbWeekdayOfMonth.UseVisualStyleBackColor = true;
            this.rbWeekdayOfMonth.CheckedChanged += new System.EventHandler(this.rbWeekdayOfMonth_CheckedChanged);
            // 
            // cmbWeekdayMonth
            // 
            this.cmbWeekdayMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWeekdayMonth.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWeekdayMonth, "cmbWeekdayMonth");
            this.cmbWeekdayMonth.Name = "cmbWeekdayMonth";
            this.cmbWeekdayMonth.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdayMonth_SelectedIndexChanged);
            // 
            // YearlyControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbWeekdayMonth);
            this.Controls.Add(this.cmbWeekdayCharms);
            this.Controls.Add(this.cmbWeekdaySequence);
            this.Controls.Add(this.rbWeekdayOfMonth);
            this.Controls.Add(this.tbDay);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.rbOnDayOfMonth);
            this.Name = "YearlyControl";
            this.Load += new System.EventHandler(this.RecurrenceDetailYearlyControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbOnDayOfMonth;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.TextBox tbDay;
        private System.Windows.Forms.ComboBox cmbWeekdayCharms;
        private System.Windows.Forms.ComboBox cmbWeekdaySequence;
        private System.Windows.Forms.RadioButton rbWeekdayOfMonth;
        private System.Windows.Forms.ComboBox cmbWeekdayMonth;
    }
}
