namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class HourlyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HourlyControl));
            this.label1 = new System.Windows.Forms.Label();
            this.tbHours = new System.Windows.Forms.TextBox();
            this.rbEveryHour = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMinutes = new System.Windows.Forms.TextBox();
            this.rbEveryMinute = new System.Windows.Forms.RadioButton();
            this.lblHoursFrom = new System.Windows.Forms.Label();
            this.cmbHoursFrom = new System.Windows.Forms.ComboBox();
            this.lblTo = new System.Windows.Forms.Label();
            this.cmbHoursTo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbHours
            // 
            resources.ApplyResources(this.tbHours, "tbHours");
            this.tbHours.Name = "tbHours";
            this.tbHours.TextChanged += new System.EventHandler(this.tbHours_TextChanged);
            this.tbHours.Validating += new System.ComponentModel.CancelEventHandler(this.tbHours_Validating);
            // 
            // rbEveryHour
            // 
            resources.ApplyResources(this.rbEveryHour, "rbEveryHour");
            this.rbEveryHour.Checked = true;
            this.rbEveryHour.Name = "rbEveryHour";
            this.rbEveryHour.TabStop = true;
            this.rbEveryHour.UseVisualStyleBackColor = true;
            this.rbEveryHour.CheckedChanged += new System.EventHandler(this.rbEveryHour_CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbMinutes
            // 
            resources.ApplyResources(this.tbMinutes, "tbMinutes");
            this.tbMinutes.Name = "tbMinutes";
            this.tbMinutes.TextChanged += new System.EventHandler(this.tbMinutes_TextChanged);
            this.tbMinutes.Validating += new System.ComponentModel.CancelEventHandler(this.tbMinutes_Validating);
            // 
            // rbEveryMinute
            // 
            resources.ApplyResources(this.rbEveryMinute, "rbEveryMinute");
            this.rbEveryMinute.Name = "rbEveryMinute";
            this.rbEveryMinute.UseVisualStyleBackColor = true;
            this.rbEveryMinute.CheckedChanged += new System.EventHandler(this.rbEveryMinute_CheckedChanged);
            // 
            // lblHoursFrom
            // 
            resources.ApplyResources(this.lblHoursFrom, "lblHoursFrom");
            this.lblHoursFrom.Name = "lblHoursFrom";
            // 
            // cmbHoursFrom
            // 
            this.cmbHoursFrom.FormattingEnabled = true;
            this.cmbHoursFrom.Items.AddRange(new object[] {
            resources.GetString("cmbHoursFrom.Items"),
            resources.GetString("cmbHoursFrom.Items1"),
            resources.GetString("cmbHoursFrom.Items2"),
            resources.GetString("cmbHoursFrom.Items3"),
            resources.GetString("cmbHoursFrom.Items4"),
            resources.GetString("cmbHoursFrom.Items5"),
            resources.GetString("cmbHoursFrom.Items6"),
            resources.GetString("cmbHoursFrom.Items7"),
            resources.GetString("cmbHoursFrom.Items8"),
            resources.GetString("cmbHoursFrom.Items9"),
            resources.GetString("cmbHoursFrom.Items10"),
            resources.GetString("cmbHoursFrom.Items11"),
            resources.GetString("cmbHoursFrom.Items12"),
            resources.GetString("cmbHoursFrom.Items13"),
            resources.GetString("cmbHoursFrom.Items14"),
            resources.GetString("cmbHoursFrom.Items15"),
            resources.GetString("cmbHoursFrom.Items16"),
            resources.GetString("cmbHoursFrom.Items17"),
            resources.GetString("cmbHoursFrom.Items18"),
            resources.GetString("cmbHoursFrom.Items19"),
            resources.GetString("cmbHoursFrom.Items20"),
            resources.GetString("cmbHoursFrom.Items21"),
            resources.GetString("cmbHoursFrom.Items22"),
            resources.GetString("cmbHoursFrom.Items23")});
            resources.ApplyResources(this.cmbHoursFrom, "cmbHoursFrom");
            this.cmbHoursFrom.Name = "cmbHoursFrom";
            this.cmbHoursFrom.SelectedIndexChanged += new System.EventHandler(this.cmbHoursFrom_SelectedIndexChanged);
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // cmbHoursTo
            // 
            this.cmbHoursTo.FormattingEnabled = true;
            this.cmbHoursTo.Items.AddRange(new object[] {
            resources.GetString("cmbHoursTo.Items"),
            resources.GetString("cmbHoursTo.Items1"),
            resources.GetString("cmbHoursTo.Items2"),
            resources.GetString("cmbHoursTo.Items3"),
            resources.GetString("cmbHoursTo.Items4"),
            resources.GetString("cmbHoursTo.Items5"),
            resources.GetString("cmbHoursTo.Items6"),
            resources.GetString("cmbHoursTo.Items7"),
            resources.GetString("cmbHoursTo.Items8"),
            resources.GetString("cmbHoursTo.Items9"),
            resources.GetString("cmbHoursTo.Items10"),
            resources.GetString("cmbHoursTo.Items11"),
            resources.GetString("cmbHoursTo.Items12"),
            resources.GetString("cmbHoursTo.Items13"),
            resources.GetString("cmbHoursTo.Items14"),
            resources.GetString("cmbHoursTo.Items15"),
            resources.GetString("cmbHoursTo.Items16"),
            resources.GetString("cmbHoursTo.Items17"),
            resources.GetString("cmbHoursTo.Items18"),
            resources.GetString("cmbHoursTo.Items19"),
            resources.GetString("cmbHoursTo.Items20"),
            resources.GetString("cmbHoursTo.Items21"),
            resources.GetString("cmbHoursTo.Items22"),
            resources.GetString("cmbHoursTo.Items23")});
            resources.ApplyResources(this.cmbHoursTo, "cmbHoursTo");
            this.cmbHoursTo.Name = "cmbHoursTo";
            this.cmbHoursTo.SelectedIndexChanged += new System.EventHandler(this.cmbHoursTo_SelectedIndexChanged);
            // 
            // HourlyControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbHoursTo);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.cmbHoursFrom);
            this.Controls.Add(this.lblHoursFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbMinutes);
            this.Controls.Add(this.rbEveryMinute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbHours);
            this.Controls.Add(this.rbEveryHour);
            this.Name = "HourlyControl";
            this.Load += new System.EventHandler(this.RecurrenceDetailHourlyControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHours;
        private System.Windows.Forms.RadioButton rbEveryHour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMinutes;
        private System.Windows.Forms.RadioButton rbEveryMinute;
        private System.Windows.Forms.Label lblHoursFrom;
        private System.Windows.Forms.ComboBox cmbHoursFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.ComboBox cmbHoursTo;
    }
}
