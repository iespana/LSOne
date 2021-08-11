using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class ValidationPeriodDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationPeriodDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel3 = new LSOne.Controls.GroupPanel();
            this.dtpDefaultEndingDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDefaultStartingDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDefaultEndingTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDefaultStartingTime = new System.Windows.Forms.DateTimePicker();
            this.lblDefaultEndingTimeAfterMidnight = new System.Windows.Forms.Label();
            this.chkDefaultEndingTimeAfterMidnight = new System.Windows.Forms.CheckBox();
            this.lblDefaultTimeWithinBounds = new System.Windows.Forms.Label();
            this.chkDefaultTimeWithinBounds = new System.Windows.Forms.CheckBox();
            this.lblDefaultEndingTime = new System.Windows.Forms.Label();
            this.lblDefaultStartingTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbDefaultPeriod = new System.Windows.Forms.RadioButton();
            this.rbDefaultAlways = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupPanel1 = new LSOne.Controls.GroupPanel();
            this.dtpWeekdayEndingTime = new System.Windows.Forms.DateTimePicker();
            this.dtpWeekdayStartingTime = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.lblWeekdayEndingTimeAfterMidnight = new System.Windows.Forms.Label();
            this.chkWeekdayEndingTimeAfterMidnight = new System.Windows.Forms.CheckBox();
            this.lblWeekdayTimeWithinBounds = new System.Windows.Forms.Label();
            this.chkWeekdayTimeWithinBounds = new System.Windows.Forms.CheckBox();
            this.lblWeekdayEndingTime = new System.Windows.Forms.Label();
            this.lblWeekdayStartingTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbWeekdayDisabled = new System.Windows.Forms.RadioButton();
            this.rbWeekdayPeriod = new System.Windows.Forms.RadioButton();
            this.rbWeekdayNever = new System.Windows.Forms.RadioButton();
            this.rbWeekdayAlways = new System.Windows.Forms.RadioButton();
            this.cmbWeekdayDay = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupPanel2 = new LSOne.Controls.GroupPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chkSpecialSettingsWednesday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsTuesday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsSunday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsMonday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsSaturday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsFriday = new System.Windows.Forms.CheckBox();
            this.chkSpecialSettingsThursday = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel3
            // 
            this.groupPanel3.Controls.Add(this.dtpDefaultEndingDate);
            this.groupPanel3.Controls.Add(this.dtpDefaultStartingDate);
            this.groupPanel3.Controls.Add(this.dtpDefaultEndingTime);
            this.groupPanel3.Controls.Add(this.label6);
            this.groupPanel3.Controls.Add(this.dtpDefaultStartingTime);
            this.groupPanel3.Controls.Add(this.lblDefaultEndingTimeAfterMidnight);
            this.groupPanel3.Controls.Add(this.chkDefaultEndingTimeAfterMidnight);
            this.groupPanel3.Controls.Add(this.lblDefaultTimeWithinBounds);
            this.groupPanel3.Controls.Add(this.chkDefaultTimeWithinBounds);
            this.groupPanel3.Controls.Add(this.lblDefaultEndingTime);
            this.groupPanel3.Controls.Add(this.lblDefaultStartingTime);
            this.groupPanel3.Controls.Add(this.tableLayoutPanel1);
            this.groupPanel3.Controls.Add(this.label2);
            this.groupPanel3.Controls.Add(this.label3);
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.groupPanel3.Name = "groupPanel3";
            // 
            // dtpDefaultEndingDate
            // 
            this.dtpDefaultEndingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDefaultEndingDate, "dtpDefaultEndingDate");
            this.dtpDefaultEndingDate.Name = "dtpDefaultEndingDate";
            // 
            // dtpDefaultStartingDate
            // 
            this.dtpDefaultStartingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpDefaultStartingDate, "dtpDefaultStartingDate");
            this.dtpDefaultStartingDate.Name = "dtpDefaultStartingDate";
            // 
            // dtpDefaultEndingTime
            // 
            resources.ApplyResources(this.dtpDefaultEndingTime, "dtpDefaultEndingTime");
            this.dtpDefaultEndingTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDefaultEndingTime.Name = "dtpDefaultEndingTime";
            this.dtpDefaultEndingTime.ShowUpDown = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dtpDefaultStartingTime
            // 
            resources.ApplyResources(this.dtpDefaultStartingTime, "dtpDefaultStartingTime");
            this.dtpDefaultStartingTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDefaultStartingTime.Name = "dtpDefaultStartingTime";
            this.dtpDefaultStartingTime.ShowUpDown = true;
            // 
            // lblDefaultEndingTimeAfterMidnight
            // 
            this.lblDefaultEndingTimeAfterMidnight.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDefaultEndingTimeAfterMidnight, "lblDefaultEndingTimeAfterMidnight");
            this.lblDefaultEndingTimeAfterMidnight.Name = "lblDefaultEndingTimeAfterMidnight";
            // 
            // chkDefaultEndingTimeAfterMidnight
            // 
            resources.ApplyResources(this.chkDefaultEndingTimeAfterMidnight, "chkDefaultEndingTimeAfterMidnight");
            this.chkDefaultEndingTimeAfterMidnight.BackColor = System.Drawing.Color.Transparent;
            this.chkDefaultEndingTimeAfterMidnight.Name = "chkDefaultEndingTimeAfterMidnight";
            this.chkDefaultEndingTimeAfterMidnight.UseVisualStyleBackColor = false;
            this.chkDefaultEndingTimeAfterMidnight.CheckedChanged += new System.EventHandler(this.ClearError);
            // 
            // lblDefaultTimeWithinBounds
            // 
            this.lblDefaultTimeWithinBounds.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDefaultTimeWithinBounds, "lblDefaultTimeWithinBounds");
            this.lblDefaultTimeWithinBounds.Name = "lblDefaultTimeWithinBounds";
            // 
            // chkDefaultTimeWithinBounds
            // 
            resources.ApplyResources(this.chkDefaultTimeWithinBounds, "chkDefaultTimeWithinBounds");
            this.chkDefaultTimeWithinBounds.BackColor = System.Drawing.Color.Transparent;
            this.chkDefaultTimeWithinBounds.Checked = true;
            this.chkDefaultTimeWithinBounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefaultTimeWithinBounds.Name = "chkDefaultTimeWithinBounds";
            this.chkDefaultTimeWithinBounds.UseVisualStyleBackColor = false;
            this.chkDefaultTimeWithinBounds.CheckedChanged += new System.EventHandler(this.ClearError);
            // 
            // lblDefaultEndingTime
            // 
            this.lblDefaultEndingTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDefaultEndingTime, "lblDefaultEndingTime");
            this.lblDefaultEndingTime.Name = "lblDefaultEndingTime";
            // 
            // lblDefaultStartingTime
            // 
            this.lblDefaultStartingTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblDefaultStartingTime, "lblDefaultStartingTime");
            this.lblDefaultStartingTime.Name = "lblDefaultStartingTime";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.rbDefaultPeriod, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbDefaultAlways, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // rbDefaultPeriod
            // 
            resources.ApplyResources(this.rbDefaultPeriod, "rbDefaultPeriod");
            this.rbDefaultPeriod.Name = "rbDefaultPeriod";
            this.rbDefaultPeriod.UseVisualStyleBackColor = true;
            this.rbDefaultPeriod.CheckedChanged += new System.EventHandler(this.rbDefault_CheckChanged);
            // 
            // rbDefaultAlways
            // 
            resources.ApplyResources(this.rbDefaultAlways, "rbDefaultAlways");
            this.rbDefaultAlways.Checked = true;
            this.rbDefaultAlways.Name = "rbDefaultAlways";
            this.rbDefaultAlways.TabStop = true;
            this.rbDefaultAlways.UseVisualStyleBackColor = true;
            this.rbDefaultAlways.CheckedChanged += new System.EventHandler(this.rbDefault_CheckChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label13.Name = "label13";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label1.Name = "label1";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckOkBtnEnabled);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label11.Name = "label11";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.dtpWeekdayEndingTime);
            this.groupPanel1.Controls.Add(this.dtpWeekdayStartingTime);
            this.groupPanel1.Controls.Add(this.label7);
            this.groupPanel1.Controls.Add(this.lblWeekdayEndingTimeAfterMidnight);
            this.groupPanel1.Controls.Add(this.chkWeekdayEndingTimeAfterMidnight);
            this.groupPanel1.Controls.Add(this.lblWeekdayTimeWithinBounds);
            this.groupPanel1.Controls.Add(this.chkWeekdayTimeWithinBounds);
            this.groupPanel1.Controls.Add(this.lblWeekdayEndingTime);
            this.groupPanel1.Controls.Add(this.lblWeekdayStartingTime);
            this.groupPanel1.Controls.Add(this.tableLayoutPanel2);
            this.groupPanel1.Controls.Add(this.cmbWeekdayDay);
            this.groupPanel1.Controls.Add(this.label17);
            resources.ApplyResources(this.groupPanel1, "groupPanel1");
            this.groupPanel1.Name = "groupPanel1";
            // 
            // dtpWeekdayEndingTime
            // 
            resources.ApplyResources(this.dtpWeekdayEndingTime, "dtpWeekdayEndingTime");
            this.dtpWeekdayEndingTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWeekdayEndingTime.Name = "dtpWeekdayEndingTime";
            this.dtpWeekdayEndingTime.ShowUpDown = true;
            this.dtpWeekdayEndingTime.ValueChanged += new System.EventHandler(this.dtpWeekdayTime_ValueChanged);
            // 
            // dtpWeekdayStartingTime
            // 
            resources.ApplyResources(this.dtpWeekdayStartingTime, "dtpWeekdayStartingTime");
            this.dtpWeekdayStartingTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWeekdayStartingTime.Name = "dtpWeekdayStartingTime";
            this.dtpWeekdayStartingTime.ShowUpDown = true;
            this.dtpWeekdayStartingTime.ValueChanged += new System.EventHandler(this.dtpWeekdayTime_ValueChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lblWeekdayEndingTimeAfterMidnight
            // 
            this.lblWeekdayEndingTimeAfterMidnight.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblWeekdayEndingTimeAfterMidnight, "lblWeekdayEndingTimeAfterMidnight");
            this.lblWeekdayEndingTimeAfterMidnight.Name = "lblWeekdayEndingTimeAfterMidnight";
            // 
            // chkWeekdayEndingTimeAfterMidnight
            // 
            resources.ApplyResources(this.chkWeekdayEndingTimeAfterMidnight, "chkWeekdayEndingTimeAfterMidnight");
            this.chkWeekdayEndingTimeAfterMidnight.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekdayEndingTimeAfterMidnight.Name = "chkWeekdayEndingTimeAfterMidnight";
            this.chkWeekdayEndingTimeAfterMidnight.UseVisualStyleBackColor = false;
            this.chkWeekdayEndingTimeAfterMidnight.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // lblWeekdayTimeWithinBounds
            // 
            this.lblWeekdayTimeWithinBounds.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblWeekdayTimeWithinBounds, "lblWeekdayTimeWithinBounds");
            this.lblWeekdayTimeWithinBounds.Name = "lblWeekdayTimeWithinBounds";
            // 
            // chkWeekdayTimeWithinBounds
            // 
            resources.ApplyResources(this.chkWeekdayTimeWithinBounds, "chkWeekdayTimeWithinBounds");
            this.chkWeekdayTimeWithinBounds.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekdayTimeWithinBounds.Checked = true;
            this.chkWeekdayTimeWithinBounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeekdayTimeWithinBounds.Name = "chkWeekdayTimeWithinBounds";
            this.chkWeekdayTimeWithinBounds.UseVisualStyleBackColor = false;
            this.chkWeekdayTimeWithinBounds.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // lblWeekdayEndingTime
            // 
            this.lblWeekdayEndingTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblWeekdayEndingTime, "lblWeekdayEndingTime");
            this.lblWeekdayEndingTime.Name = "lblWeekdayEndingTime";
            // 
            // lblWeekdayStartingTime
            // 
            this.lblWeekdayStartingTime.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblWeekdayStartingTime, "lblWeekdayStartingTime");
            this.lblWeekdayStartingTime.Name = "lblWeekdayStartingTime";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.rbWeekdayDisabled, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbWeekdayPeriod, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbWeekdayNever, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbWeekdayAlways, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // rbWeekdayDisabled
            // 
            resources.ApplyResources(this.rbWeekdayDisabled, "rbWeekdayDisabled");
            this.rbWeekdayDisabled.Checked = true;
            this.rbWeekdayDisabled.Name = "rbWeekdayDisabled";
            this.rbWeekdayDisabled.TabStop = true;
            this.rbWeekdayDisabled.UseVisualStyleBackColor = true;
            this.rbWeekdayDisabled.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // rbWeekdayPeriod
            // 
            resources.ApplyResources(this.rbWeekdayPeriod, "rbWeekdayPeriod");
            this.rbWeekdayPeriod.Name = "rbWeekdayPeriod";
            this.rbWeekdayPeriod.UseVisualStyleBackColor = true;
            this.rbWeekdayPeriod.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // rbWeekdayNever
            // 
            resources.ApplyResources(this.rbWeekdayNever, "rbWeekdayNever");
            this.rbWeekdayNever.Name = "rbWeekdayNever";
            this.rbWeekdayNever.UseVisualStyleBackColor = true;
            this.rbWeekdayNever.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // rbWeekdayAlways
            // 
            resources.ApplyResources(this.rbWeekdayAlways, "rbWeekdayAlways");
            this.rbWeekdayAlways.Name = "rbWeekdayAlways";
            this.rbWeekdayAlways.UseVisualStyleBackColor = true;
            this.rbWeekdayAlways.CheckedChanged += new System.EventHandler(this.rbWeekday_CheckChanged);
            // 
            // cmbWeekdayDay
            // 
            this.cmbWeekdayDay.FormattingEnabled = true;
            this.cmbWeekdayDay.Items.AddRange(new object[] {
            resources.GetString("cmbWeekdayDay.Items"),
            resources.GetString("cmbWeekdayDay.Items1"),
            resources.GetString("cmbWeekdayDay.Items2"),
            resources.GetString("cmbWeekdayDay.Items3"),
            resources.GetString("cmbWeekdayDay.Items4"),
            resources.GetString("cmbWeekdayDay.Items5"),
            resources.GetString("cmbWeekdayDay.Items6")});
            resources.ApplyResources(this.cmbWeekdayDay, "cmbWeekdayDay");
            this.cmbWeekdayDay.Name = "cmbWeekdayDay";
            this.cmbWeekdayDay.SelectedIndexChanged += new System.EventHandler(this.cmbWeekdayDay_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // groupPanel2
            // 
            this.groupPanel2.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.groupPanel2, "groupPanel2");
            this.groupPanel2.Name = "groupPanel2";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsWednesday, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsTuesday, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsSunday, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsMonday, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsSaturday, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsFriday, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.chkSpecialSettingsThursday, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // chkSpecialSettingsWednesday
            // 
            resources.ApplyResources(this.chkSpecialSettingsWednesday, "chkSpecialSettingsWednesday");
            this.chkSpecialSettingsWednesday.Name = "chkSpecialSettingsWednesday";
            this.chkSpecialSettingsWednesday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsTuesday
            // 
            resources.ApplyResources(this.chkSpecialSettingsTuesday, "chkSpecialSettingsTuesday");
            this.chkSpecialSettingsTuesday.Name = "chkSpecialSettingsTuesday";
            this.chkSpecialSettingsTuesday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsSunday
            // 
            resources.ApplyResources(this.chkSpecialSettingsSunday, "chkSpecialSettingsSunday");
            this.chkSpecialSettingsSunday.Name = "chkSpecialSettingsSunday";
            this.chkSpecialSettingsSunday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsMonday
            // 
            resources.ApplyResources(this.chkSpecialSettingsMonday, "chkSpecialSettingsMonday");
            this.chkSpecialSettingsMonday.Name = "chkSpecialSettingsMonday";
            this.chkSpecialSettingsMonday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsSaturday
            // 
            resources.ApplyResources(this.chkSpecialSettingsSaturday, "chkSpecialSettingsSaturday");
            this.chkSpecialSettingsSaturday.Name = "chkSpecialSettingsSaturday";
            this.chkSpecialSettingsSaturday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsFriday
            // 
            resources.ApplyResources(this.chkSpecialSettingsFriday, "chkSpecialSettingsFriday");
            this.chkSpecialSettingsFriday.Name = "chkSpecialSettingsFriday";
            this.chkSpecialSettingsFriday.UseVisualStyleBackColor = true;
            // 
            // chkSpecialSettingsThursday
            // 
            resources.ApplyResources(this.chkSpecialSettingsThursday, "chkSpecialSettingsThursday");
            this.chkSpecialSettingsThursday.Name = "chkSpecialSettingsThursday";
            this.chkSpecialSettingsThursday.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label20.Name = "label20";
            // 
            // ValidationPeriodDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.groupPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "ValidationPeriodDialog";
            this.Controls.SetChildIndex(this.groupPanel3, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.groupPanel2, 0);
            this.Controls.SetChildIndex(this.groupPanel1, 0);
            this.Controls.SetChildIndex(this.label20, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label13;
        private GroupPanel groupPanel3;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbDefaultPeriod;
        private System.Windows.Forms.RadioButton rbDefaultAlways;
        private System.Windows.Forms.Label lblDefaultEndingTime;
        private System.Windows.Forms.Label lblDefaultStartingTime;
        private System.Windows.Forms.Label label11;
        private GroupPanel groupPanel1;
        private System.Windows.Forms.Label lblWeekdayEndingTimeAfterMidnight;
        private System.Windows.Forms.CheckBox chkWeekdayEndingTimeAfterMidnight;
        private System.Windows.Forms.Label lblWeekdayTimeWithinBounds;
        private System.Windows.Forms.CheckBox chkWeekdayTimeWithinBounds;
        private System.Windows.Forms.Label lblWeekdayEndingTime;
        private System.Windows.Forms.Label lblWeekdayStartingTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbWeekdayNever;
        private System.Windows.Forms.RadioButton rbWeekdayPeriod;
        private System.Windows.Forms.RadioButton rbWeekdayAlways;
        private System.Windows.Forms.ComboBox cmbWeekdayDay;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblDefaultEndingTimeAfterMidnight;
        private System.Windows.Forms.CheckBox chkDefaultEndingTimeAfterMidnight;
        private System.Windows.Forms.Label lblDefaultTimeWithinBounds;
        private System.Windows.Forms.CheckBox chkDefaultTimeWithinBounds;
        private System.Windows.Forms.Label label20;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.CheckBox chkSpecialSettingsTuesday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsWednesday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsFriday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsSunday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsSaturday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsThursday;
        private System.Windows.Forms.CheckBox chkSpecialSettingsMonday;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpWeekdayStartingTime;
        private System.Windows.Forms.DateTimePicker dtpWeekdayEndingTime;
        private System.Windows.Forms.DateTimePicker dtpDefaultEndingTime;
        private System.Windows.Forms.DateTimePicker dtpDefaultStartingTime;
        private System.Windows.Forms.DateTimePicker dtpDefaultEndingDate;
        private System.Windows.Forms.DateTimePicker dtpDefaultStartingDate;
        private System.Windows.Forms.RadioButton rbWeekdayDisabled;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
