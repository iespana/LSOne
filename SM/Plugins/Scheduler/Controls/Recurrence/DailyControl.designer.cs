namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class DailyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyControl));
            this.rbEveryDay = new System.Windows.Forms.RadioButton();
            this.tbDays = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbEveryWeekday = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // rbEveryDay
            // 
            resources.ApplyResources(this.rbEveryDay, "rbEveryDay");
            this.rbEveryDay.Checked = true;
            this.errorProvider.SetError(this.rbEveryDay, resources.GetString("rbEveryDay.Error"));
            this.errorProvider.SetIconAlignment(this.rbEveryDay, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rbEveryDay.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.rbEveryDay, ((int)(resources.GetObject("rbEveryDay.IconPadding"))));
            this.rbEveryDay.Name = "rbEveryDay";
            this.rbEveryDay.TabStop = true;
            this.rbEveryDay.UseVisualStyleBackColor = true;
            this.rbEveryDay.CheckedChanged += new System.EventHandler(this.rbEveryDay_CheckedChanged);
            // 
            // tbDays
            // 
            resources.ApplyResources(this.tbDays, "tbDays");
            this.errorProvider.SetError(this.tbDays, resources.GetString("tbDays.Error"));
            this.errorProvider.SetIconAlignment(this.tbDays, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tbDays.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.tbDays, ((int)(resources.GetObject("tbDays.IconPadding"))));
            this.tbDays.Name = "tbDays";
            this.tbDays.TextChanged += new System.EventHandler(this.tbDays_TextChanged);
            this.tbDays.Validating += new System.ComponentModel.CancelEventHandler(this.tbDays_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // rbEveryWeekday
            // 
            resources.ApplyResources(this.rbEveryWeekday, "rbEveryWeekday");
            this.errorProvider.SetError(this.rbEveryWeekday, resources.GetString("rbEveryWeekday.Error"));
            this.errorProvider.SetIconAlignment(this.rbEveryWeekday, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rbEveryWeekday.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.rbEveryWeekday, ((int)(resources.GetObject("rbEveryWeekday.IconPadding"))));
            this.rbEveryWeekday.Name = "rbEveryWeekday";
            this.rbEveryWeekday.UseVisualStyleBackColor = true;
            this.rbEveryWeekday.CheckedChanged += new System.EventHandler(this.rbEveryWeekday_CheckedChanged);
            // 
            // DailyControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbEveryWeekday);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDays);
            this.Controls.Add(this.rbEveryDay);
            this.errorProvider.SetError(this, resources.GetString("$this.Error"));
            this.errorProvider.SetIconAlignment(this, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("$this.IconAlignment"))));
            this.errorProvider.SetIconPadding(this, ((int)(resources.GetObject("$this.IconPadding"))));
            this.Name = "DailyControl";
            this.Load += new System.EventHandler(this.RecurrenceDetailDailyControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbEveryDay;
        private System.Windows.Forms.TextBox tbDays;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbEveryWeekday;
    }
}
