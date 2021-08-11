namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    partial class WeeklyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeeklyControl));
            this.label2 = new System.Windows.Forms.Label();
            this.cbWeekday0 = new System.Windows.Forms.CheckBox();
            this.cbWeekday1 = new System.Windows.Forms.CheckBox();
            this.cbWeekday2 = new System.Windows.Forms.CheckBox();
            this.cbWeekday3 = new System.Windows.Forms.CheckBox();
            this.cbWeekday6 = new System.Windows.Forms.CheckBox();
            this.cbWeekday5 = new System.Windows.Forms.CheckBox();
            this.cbWeekday4 = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbWeekday0
            // 
            resources.ApplyResources(this.cbWeekday0, "cbWeekday0");
            this.cbWeekday0.Name = "cbWeekday0";
            this.cbWeekday0.UseVisualStyleBackColor = true;
            // 
            // cbWeekday1
            // 
            resources.ApplyResources(this.cbWeekday1, "cbWeekday1");
            this.cbWeekday1.Name = "cbWeekday1";
            this.cbWeekday1.UseVisualStyleBackColor = true;
            // 
            // cbWeekday2
            // 
            resources.ApplyResources(this.cbWeekday2, "cbWeekday2");
            this.cbWeekday2.Name = "cbWeekday2";
            this.cbWeekday2.UseVisualStyleBackColor = true;
            // 
            // cbWeekday3
            // 
            resources.ApplyResources(this.cbWeekday3, "cbWeekday3");
            this.cbWeekday3.Name = "cbWeekday3";
            this.cbWeekday3.UseVisualStyleBackColor = true;
            // 
            // cbWeekday6
            // 
            resources.ApplyResources(this.cbWeekday6, "cbWeekday6");
            this.cbWeekday6.Name = "cbWeekday6";
            this.cbWeekday6.UseVisualStyleBackColor = true;
            // 
            // cbWeekday5
            // 
            resources.ApplyResources(this.cbWeekday5, "cbWeekday5");
            this.cbWeekday5.Name = "cbWeekday5";
            this.cbWeekday5.UseVisualStyleBackColor = true;
            // 
            // cbWeekday4
            // 
            resources.ApplyResources(this.cbWeekday4, "cbWeekday4");
            this.cbWeekday4.Name = "cbWeekday4";
            this.cbWeekday4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday0, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbWeekday3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // WeeklyControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label2);
            this.Name = "WeeklyControl";
            this.Load += new System.EventHandler(this.RecurrenceDetailWeeklyControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RecurrenceDetailWeeklyControl_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbWeekday0;
        private System.Windows.Forms.CheckBox cbWeekday1;
        private System.Windows.Forms.CheckBox cbWeekday2;
        private System.Windows.Forms.CheckBox cbWeekday3;
        private System.Windows.Forms.CheckBox cbWeekday6;
        private System.Windows.Forms.CheckBox cbWeekday5;
        private System.Windows.Forms.CheckBox cbWeekday4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
