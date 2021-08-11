namespace LSOne.Controls.Dialogs
{
    partial class DatePickerDialog
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
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.btnOK = new LSOne.Controls.TouchButton();
            this.btnCancel = new LSOne.Controls.TouchButton();
            this.calendarControl1 = new LSOne.Controls.CalendarControl();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            this.touchDialogBanner1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.BannerText = "Select date";
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Location = new System.Drawing.Point(0, 0);
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.Size = new System.Drawing.Size(400, 50);
            this.touchDialogBanner1.TabIndex = 0;
            this.touchDialogBanner1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(189)))), ((int)(((byte)(139)))));
            this.btnOK.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.OK;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(11, 479);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(189, 50);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(83)))), ((int)(((byte)(81)))));
            this.btnCancel.ButtonType = LSOne.Controls.SupportClasses.TouchButtonType.Cancel;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(200, 479);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(189, 50);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // calendarControl1
            // 
            this.calendarControl1.BackColor = System.Drawing.Color.White;
            this.calendarControl1.BeforeTodayEndColor = System.Drawing.Color.Empty;
            this.calendarControl1.BeforeTodayStartColor = System.Drawing.Color.Empty;
            this.calendarControl1.CancelEventAction = false;
            this.calendarControl1.ClickableOutOfBounds = true;
            this.calendarControl1.DrawFrame = true;
            this.calendarControl1.DrawOutOfBounds = true;
            this.calendarControl1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.calendarControl1.FridayEnabled = true;
            this.calendarControl1.FridaySpecial = false;
            this.calendarControl1.FromTodayEndColor = System.Drawing.Color.Empty;
            this.calendarControl1.FromTodayStartColor = System.Drawing.Color.Empty;
            this.calendarControl1.Location = new System.Drawing.Point(11, 60);
            this.calendarControl1.MarkStyle = LSOne.Controls.MarkedDaysStyle.RedUnderMark;
            this.calendarControl1.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.calendarControl1.MaximumSize = new System.Drawing.Size(378, 414);
            this.calendarControl1.MinDate = new System.DateTime(((long)(0)));
            this.calendarControl1.MinimumSize = new System.Drawing.Size(378, 414);
            this.calendarControl1.MondayEnabled = true;
            this.calendarControl1.MondaySpecial = false;
            this.calendarControl1.Name = "calendarControl1";
            this.calendarControl1.SaturdayEnabled = true;
            this.calendarControl1.SaturdaySpecial = false;
            this.calendarControl1.SelectedDate = new System.DateTime(((long)(0)));
            this.calendarControl1.Size = new System.Drawing.Size(378, 414);
            this.calendarControl1.SundayEnabled = true;
            this.calendarControl1.SundaySpecial = false;
            this.calendarControl1.TabIndex = 1;
            this.calendarControl1.ThursdayEnabled = true;
            this.calendarControl1.ThursdaySpecial = false;
            this.calendarControl1.TodayEndColor = System.Drawing.Color.Empty;
            this.calendarControl1.TodayStartColor = System.Drawing.Color.Empty;
            this.calendarControl1.TuesdayEnabled = true;
            this.calendarControl1.TuesdaySpecial = false;
            this.calendarControl1.UseBackColor = false;
            this.calendarControl1.UseBeforeTodayColoring = false;
            this.calendarControl1.UseFromTodayColoring = false;
            this.calendarControl1.UseMaxDate = false;
            this.calendarControl1.UseMinDate = false;
            this.calendarControl1.UseTodayColoring = false;
            this.calendarControl1.WednesdayEnabled = true;
            this.calendarControl1.WednesdaySpecial = false;
            // 
            // DatePickerDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 540);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.calendarControl1);
            this.Controls.Add(this.touchDialogBanner1);
            this.Name = "DatePickerDialog";
            this.Text = "DatePickerDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner touchDialogBanner1;
        private CalendarControl calendarControl1;
        private TouchButton btnOK;
        private TouchButton btnCancel;
    }
}