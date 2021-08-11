using LSOne.ViewPlugins.Scheduler.Controls.Recurrence;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class JobTriggerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobTriggerPage));
            LSOne.ViewPlugins.Scheduler.Controls.Recurrence.CronSchedule cronSchedule1 = new LSOne.ViewPlugins.Scheduler.Controls.Recurrence.CronSchedule();
            this.rbManual = new System.Windows.Forms.RadioButton();
            this.rbRunOnce = new System.Windows.Forms.RadioButton();
            this.dtOnceDate = new System.Windows.Forms.DateTimePicker();
            this.rbRecurrence = new System.Windows.Forms.RadioButton();
            this.dtOnceTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.recurrenceControl = new LSOne.ViewPlugins.Scheduler.Controls.Recurrence.RecurrenceControl();
            this.SuspendLayout();
            // 
            // rbManual
            // 
            resources.ApplyResources(this.rbManual, "rbManual");
            this.rbManual.Name = "rbManual";
            this.rbManual.TabStop = true;
            this.rbManual.UseVisualStyleBackColor = true;
            this.rbManual.CheckedChanged += new System.EventHandler(this.rbManual_CheckedChanged);
            // 
            // rbRunOnce
            // 
            resources.ApplyResources(this.rbRunOnce, "rbRunOnce");
            this.rbRunOnce.Name = "rbRunOnce";
            this.rbRunOnce.TabStop = true;
            this.rbRunOnce.UseVisualStyleBackColor = true;
            this.rbRunOnce.CheckedChanged += new System.EventHandler(this.rbRunOnce_CheckedChanged);
            // 
            // dtOnceDate
            // 
            resources.ApplyResources(this.dtOnceDate, "dtOnceDate");
            this.dtOnceDate.Name = "dtOnceDate";
            this.dtOnceDate.ValueChanged += new System.EventHandler(this.dtOnceDate_ValueChanged);
            // 
            // rbRecurrence
            // 
            resources.ApplyResources(this.rbRecurrence, "rbRecurrence");
            this.rbRecurrence.Name = "rbRecurrence";
            this.rbRecurrence.TabStop = true;
            this.rbRecurrence.UseVisualStyleBackColor = true;
            this.rbRecurrence.CheckedChanged += new System.EventHandler(this.rbRecurrence_CheckedChanged);
            // 
            // dtOnceTime
            // 
            this.dtOnceTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            resources.ApplyResources(this.dtOnceTime, "dtOnceTime");
            this.dtOnceTime.Name = "dtOnceTime";
            this.dtOnceTime.ShowUpDown = true;
            this.dtOnceTime.ValueChanged += new System.EventHandler(this.dtOnceTime_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Name = "label2";
            // 
            // recurrenceControl
            // 
            resources.ApplyResources(this.recurrenceControl, "recurrenceControl");
            this.recurrenceControl.Name = "recurrenceControl";
            cronSchedule1.DaysOfMonth = "*";
            cronSchedule1.DaysOfWeek = "*";
            cronSchedule1.EndDateTime = null;
            cronSchedule1.Hours = "10";
            cronSchedule1.Minutes = "28";
            cronSchedule1.Months = "*";
            cronSchedule1.RecurrenceType = LSRetail.DD.Common.RecurrenceType.Daily;
            cronSchedule1.Seconds = "45";
            cronSchedule1.StartDateTime = new System.DateTime(2014, 10, 24, 10, 28, 45, 417);
            cronSchedule1.Years = "*";
            this.recurrenceControl.Value = cronSchedule1;
            this.recurrenceControl.ValueChanged += new System.EventHandler<System.EventArgs>(this.recurrenceControl_ValueChanged);
            this.recurrenceControl.Load += new System.EventHandler(this.recurrenceControl_Load);
            // 
            // JobTriggerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtOnceTime);
            this.Controls.Add(this.rbRecurrence);
            this.Controls.Add(this.dtOnceDate);
            this.Controls.Add(this.rbRunOnce);
            this.Controls.Add(this.rbManual);
            this.Controls.Add(this.recurrenceControl);
            this.Name = "JobTriggerPage";
            this.Load += new System.EventHandler(this.JobTriggerPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Recurrence.RecurrenceControl recurrenceControl;
        private System.Windows.Forms.RadioButton rbManual;
        private System.Windows.Forms.RadioButton rbRunOnce;
        private System.Windows.Forms.DateTimePicker dtOnceDate;
        private System.Windows.Forms.RadioButton rbRecurrence;
        private System.Windows.Forms.DateTimePicker dtOnceTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
