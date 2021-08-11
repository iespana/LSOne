using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    partial class KitchenDisplaySettingsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenDisplaySettingsPage));
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbScreenNumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVerticalLocation = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBumpbarOperation = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkBumpbar = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsAggregateProfile = new LSOne.Controls.ContextButtons();
            this.cmbKdsAggregateProfile = new LSOne.Controls.DualDataComboBox();
            this.lblBumpedPrevious = new System.Windows.Forms.Label();
            this.chkBumpedPrevoius = new System.Windows.Forms.CheckBox();
            this.lblTransferStation = new System.Windows.Forms.Label();
            this.cmbTransferStation = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMaxRecallFiles = new System.Windows.Forms.TextBox();
            this.cmbKdsFunctionalProfile = new LSOne.Controls.DualDataComboBox();
            this.btnsVisualProfile = new LSOne.Controls.ContextButtons();
            this.btnsFunctionalProfile = new LSOne.Controls.ContextButtons();
            this.cmbKdsVisualProfile = new LSOne.Controls.DualDataComboBox();
            this.btnStyleProfile = new LSOne.Controls.ContextButtons();
            this.cmbKdsStyleProfile = new LSOne.Controls.DualDataComboBox();
            this.lblKitchenDisplayProfile = new System.Windows.Forms.Label();
            this.lblNextStation = new System.Windows.Forms.Label();
            this.lblDisplayMode = new System.Windows.Forms.Label();
            this.lblHorizontalLocation = new System.Windows.Forms.Label();
            this.lblVertialLocation = new System.Windows.Forms.Label();
            this.cmbNextStation = new LSOne.Controls.DualDataComboBox();
            this.cmbHorizontalLocation = new System.Windows.Forms.ComboBox();
            this.chkFullScreen = new System.Windows.Forms.CheckBox();
            this.lnkIdentify = new System.Windows.Forms.LinkLabel();
            this.btnsDisplayProfile = new LSOne.Controls.ContextButtons();
            this.cmbKdsDisplayProfile = new LSOne.Controls.DualDataComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbScreenNumber
            // 
            this.cmbScreenNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScreenNumber.FormattingEnabled = true;
            this.cmbScreenNumber.Items.AddRange(new object[] {
            resources.GetString("cmbScreenNumber.Items"),
            resources.GetString("cmbScreenNumber.Items1"),
            resources.GetString("cmbScreenNumber.Items2"),
            resources.GetString("cmbScreenNumber.Items3"),
            resources.GetString("cmbScreenNumber.Items4"),
            resources.GetString("cmbScreenNumber.Items5"),
            resources.GetString("cmbScreenNumber.Items6"),
            resources.GetString("cmbScreenNumber.Items7"),
            resources.GetString("cmbScreenNumber.Items8"),
            resources.GetString("cmbScreenNumber.Items9")});
            resources.ApplyResources(this.cmbScreenNumber, "cmbScreenNumber");
            this.cmbScreenNumber.Name = "cmbScreenNumber";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbVerticalLocation
            // 
            this.cmbVerticalLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVerticalLocation.FormattingEnabled = true;
            this.cmbVerticalLocation.Items.AddRange(new object[] {
            resources.GetString("cmbVerticalLocation.Items"),
            resources.GetString("cmbVerticalLocation.Items1"),
            resources.GetString("cmbVerticalLocation.Items2")});
            resources.ApplyResources(this.cmbVerticalLocation, "cmbVerticalLocation");
            this.cmbVerticalLocation.Name = "cmbVerticalLocation";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.cmbBumpbarOperation, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.chkBumpbar, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnsAggregateProfile, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbKdsAggregateProfile, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblBumpedPrevious, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.chkBumpedPrevoius, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblTransferStation, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.cmbTransferStation, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.tbMaxRecallFiles, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.cmbKdsFunctionalProfile, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnsVisualProfile, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnsFunctionalProfile, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbKdsVisualProfile, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnStyleProfile, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbKdsStyleProfile, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblKitchenDisplayProfile, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNextStation, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblDisplayMode, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblHorizontalLocation, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lblVertialLocation, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.cmbNextStation, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbScreenNumber, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbVerticalLocation, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.cmbHorizontalLocation, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.chkFullScreen, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.lnkIdentify, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnsDisplayProfile, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbKdsDisplayProfile, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmbBumpbarOperation
            // 
            this.cmbBumpbarOperation.AddList = null;
            this.cmbBumpbarOperation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbBumpbarOperation, "cmbBumpbarOperation");
            this.cmbBumpbarOperation.MaxLength = 32767;
            this.cmbBumpbarOperation.Name = "cmbBumpbarOperation";
            this.cmbBumpbarOperation.NoChangeAllowed = false;
            this.cmbBumpbarOperation.OnlyDisplayID = false;
            this.cmbBumpbarOperation.RemoveList = null;
            this.cmbBumpbarOperation.RowHeight = ((short)(22));
            this.cmbBumpbarOperation.SecondaryData = null;
            this.cmbBumpbarOperation.SelectedData = null;
            this.cmbBumpbarOperation.SelectedDataID = null;
            this.cmbBumpbarOperation.SelectionList = null;
            this.cmbBumpbarOperation.SkipIDColumn = true;
            this.cmbBumpbarOperation.RequestData += new System.EventHandler(this.cmbBumpbarOperation_RequestData);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // chkBumpbar
            // 
            resources.ApplyResources(this.chkBumpbar, "chkBumpbar");
            this.chkBumpbar.Name = "chkBumpbar";
            this.chkBumpbar.UseVisualStyleBackColor = true;
            this.chkBumpbar.CheckedChanged += new System.EventHandler(this.chkBumpbar_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btnsAggregateProfile
            // 
            this.btnsAggregateProfile.AddButtonEnabled = true;
            this.btnsAggregateProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnsAggregateProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsAggregateProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsAggregateProfile, "btnsAggregateProfile");
            this.btnsAggregateProfile.Name = "btnsAggregateProfile";
            this.btnsAggregateProfile.RemoveButtonEnabled = false;
            this.btnsAggregateProfile.EditButtonClicked += new System.EventHandler(this.btnsAggregateProfile_EditButtonClicked);
            this.btnsAggregateProfile.AddButtonClicked += new System.EventHandler(this.btnsAggregateProfile_AddButtonClicked);
            // 
            // cmbKdsAggregateProfile
            // 
            this.cmbKdsAggregateProfile.AddList = null;
            this.cmbKdsAggregateProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKdsAggregateProfile, "cmbKdsAggregateProfile");
            this.cmbKdsAggregateProfile.MaxLength = 32767;
            this.cmbKdsAggregateProfile.Name = "cmbKdsAggregateProfile";
            this.cmbKdsAggregateProfile.NoChangeAllowed = false;
            this.cmbKdsAggregateProfile.OnlyDisplayID = false;
            this.cmbKdsAggregateProfile.RemoveList = null;
            this.cmbKdsAggregateProfile.RowHeight = ((short)(22));
            this.cmbKdsAggregateProfile.SecondaryData = null;
            this.cmbKdsAggregateProfile.SelectedData = null;
            this.cmbKdsAggregateProfile.SelectedDataID = null;
            this.cmbKdsAggregateProfile.SelectionList = null;
            this.cmbKdsAggregateProfile.SkipIDColumn = true;
            this.cmbKdsAggregateProfile.RequestData += new System.EventHandler(this.cmbKdsAggregateProfile_RequestData);
            // 
            // lblBumpedPrevious
            // 
            resources.ApplyResources(this.lblBumpedPrevious, "lblBumpedPrevious");
            this.lblBumpedPrevious.Name = "lblBumpedPrevious";
            // 
            // chkBumpedPrevoius
            // 
            resources.ApplyResources(this.chkBumpedPrevoius, "chkBumpedPrevoius");
            this.chkBumpedPrevoius.Name = "chkBumpedPrevoius";
            this.chkBumpedPrevoius.UseVisualStyleBackColor = true;
            // 
            // lblTransferStation
            // 
            resources.ApplyResources(this.lblTransferStation, "lblTransferStation");
            this.lblTransferStation.Name = "lblTransferStation";
            // 
            // cmbTransferStation
            // 
            this.cmbTransferStation.AddList = null;
            this.cmbTransferStation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTransferStation, "cmbTransferStation");
            this.cmbTransferStation.MaxLength = 32767;
            this.cmbTransferStation.Name = "cmbTransferStation";
            this.cmbTransferStation.NoChangeAllowed = false;
            this.cmbTransferStation.OnlyDisplayID = false;
            this.cmbTransferStation.RemoveList = null;
            this.cmbTransferStation.RowHeight = ((short)(22));
            this.cmbTransferStation.SecondaryData = null;
            this.cmbTransferStation.SelectedData = null;
            this.cmbTransferStation.SelectedDataID = null;
            this.cmbTransferStation.SelectionList = null;
            this.cmbTransferStation.SkipIDColumn = true;
            this.cmbTransferStation.RequestData += new System.EventHandler(this.cmbTransferStation_RequestData);
            this.cmbTransferStation.RequestClear += new System.EventHandler(this.cmbTransferStation_RequestClear);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbMaxRecallFiles
            // 
            resources.ApplyResources(this.tbMaxRecallFiles, "tbMaxRecallFiles");
            this.tbMaxRecallFiles.Name = "tbMaxRecallFiles";
            // 
            // cmbKdsFunctionalProfile
            // 
            this.cmbKdsFunctionalProfile.AddList = null;
            this.cmbKdsFunctionalProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKdsFunctionalProfile, "cmbKdsFunctionalProfile");
            this.cmbKdsFunctionalProfile.MaxLength = 32767;
            this.cmbKdsFunctionalProfile.Name = "cmbKdsFunctionalProfile";
            this.cmbKdsFunctionalProfile.NoChangeAllowed = false;
            this.cmbKdsFunctionalProfile.OnlyDisplayID = false;
            this.cmbKdsFunctionalProfile.RemoveList = null;
            this.cmbKdsFunctionalProfile.RowHeight = ((short)(22));
            this.cmbKdsFunctionalProfile.SecondaryData = null;
            this.cmbKdsFunctionalProfile.SelectedData = null;
            this.cmbKdsFunctionalProfile.SelectedDataID = null;
            this.cmbKdsFunctionalProfile.SelectionList = null;
            this.cmbKdsFunctionalProfile.SkipIDColumn = true;
            this.cmbKdsFunctionalProfile.RequestData += new System.EventHandler(this.cmbKdsFunctionalProfile_RequestData);
            this.cmbKdsFunctionalProfile.SelectedDataChanged += new System.EventHandler(this.cmbKdsFunctionalProfile_SelectedDataChanged);
            // 
            // btnsVisualProfile
            // 
            this.btnsVisualProfile.AddButtonEnabled = true;
            this.btnsVisualProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnsVisualProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsVisualProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsVisualProfile, "btnsVisualProfile");
            this.btnsVisualProfile.Name = "btnsVisualProfile";
            this.btnsVisualProfile.RemoveButtonEnabled = false;
            this.btnsVisualProfile.EditButtonClicked += new System.EventHandler(this.btnsVisualProfile_EditButtonClicked);
            this.btnsVisualProfile.AddButtonClicked += new System.EventHandler(this.btnsVisualProfile_AddButtonClicked);
            // 
            // btnsFunctionalProfile
            // 
            this.btnsFunctionalProfile.AddButtonEnabled = true;
            this.btnsFunctionalProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnsFunctionalProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsFunctionalProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsFunctionalProfile, "btnsFunctionalProfile");
            this.btnsFunctionalProfile.Name = "btnsFunctionalProfile";
            this.btnsFunctionalProfile.RemoveButtonEnabled = false;
            this.btnsFunctionalProfile.EditButtonClicked += new System.EventHandler(this.btnsFunctionalProfile_EditButtonClicked);
            this.btnsFunctionalProfile.AddButtonClicked += new System.EventHandler(this.btnsFunctionalProfile_AddButtonClicked);
            // 
            // cmbKdsVisualProfile
            // 
            this.cmbKdsVisualProfile.AddList = null;
            this.cmbKdsVisualProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKdsVisualProfile, "cmbKdsVisualProfile");
            this.cmbKdsVisualProfile.MaxLength = 32767;
            this.cmbKdsVisualProfile.Name = "cmbKdsVisualProfile";
            this.cmbKdsVisualProfile.NoChangeAllowed = false;
            this.cmbKdsVisualProfile.OnlyDisplayID = false;
            this.cmbKdsVisualProfile.RemoveList = null;
            this.cmbKdsVisualProfile.RowHeight = ((short)(22));
            this.cmbKdsVisualProfile.SecondaryData = null;
            this.cmbKdsVisualProfile.SelectedData = null;
            this.cmbKdsVisualProfile.SelectedDataID = null;
            this.cmbKdsVisualProfile.SelectionList = null;
            this.cmbKdsVisualProfile.SkipIDColumn = true;
            this.cmbKdsVisualProfile.RequestData += new System.EventHandler(this.cmbKdsVisualProfile_RequestData);
            // 
            // btnStyleProfile
            // 
            this.btnStyleProfile.AddButtonEnabled = true;
            this.btnStyleProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnStyleProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnStyleProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnStyleProfile, "btnStyleProfile");
            this.btnStyleProfile.Name = "btnStyleProfile";
            this.btnStyleProfile.RemoveButtonEnabled = false;
            this.btnStyleProfile.EditButtonClicked += new System.EventHandler(this.btnStyleProfile_EditButtonClicked);
            this.btnStyleProfile.AddButtonClicked += new System.EventHandler(this.btnStyleProfile_AddButtonClicked);
            // 
            // cmbKdsStyleProfile
            // 
            this.cmbKdsStyleProfile.AddList = null;
            this.cmbKdsStyleProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKdsStyleProfile, "cmbKdsStyleProfile");
            this.cmbKdsStyleProfile.MaxLength = 32767;
            this.cmbKdsStyleProfile.Name = "cmbKdsStyleProfile";
            this.cmbKdsStyleProfile.NoChangeAllowed = false;
            this.cmbKdsStyleProfile.OnlyDisplayID = false;
            this.cmbKdsStyleProfile.RemoveList = null;
            this.cmbKdsStyleProfile.RowHeight = ((short)(22));
            this.cmbKdsStyleProfile.SecondaryData = null;
            this.cmbKdsStyleProfile.SelectedData = null;
            this.cmbKdsStyleProfile.SelectedDataID = null;
            this.cmbKdsStyleProfile.SelectionList = null;
            this.cmbKdsStyleProfile.SkipIDColumn = true;
            this.cmbKdsStyleProfile.RequestData += new System.EventHandler(this.cmbKdsStyleProfile_RequestData);
            // 
            // lblKitchenDisplayProfile
            // 
            resources.ApplyResources(this.lblKitchenDisplayProfile, "lblKitchenDisplayProfile");
            this.lblKitchenDisplayProfile.Name = "lblKitchenDisplayProfile";
            // 
            // lblNextStation
            // 
            resources.ApplyResources(this.lblNextStation, "lblNextStation");
            this.lblNextStation.Name = "lblNextStation";
            // 
            // lblDisplayMode
            // 
            resources.ApplyResources(this.lblDisplayMode, "lblDisplayMode");
            this.lblDisplayMode.Name = "lblDisplayMode";
            // 
            // lblHorizontalLocation
            // 
            resources.ApplyResources(this.lblHorizontalLocation, "lblHorizontalLocation");
            this.lblHorizontalLocation.Name = "lblHorizontalLocation";
            // 
            // lblVertialLocation
            // 
            resources.ApplyResources(this.lblVertialLocation, "lblVertialLocation");
            this.lblVertialLocation.Name = "lblVertialLocation";
            // 
            // cmbNextStation
            // 
            this.cmbNextStation.AddList = null;
            this.cmbNextStation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbNextStation, "cmbNextStation");
            this.cmbNextStation.MaxLength = 32767;
            this.cmbNextStation.Name = "cmbNextStation";
            this.cmbNextStation.NoChangeAllowed = false;
            this.cmbNextStation.OnlyDisplayID = false;
            this.cmbNextStation.RemoveList = null;
            this.cmbNextStation.RowHeight = ((short)(22));
            this.cmbNextStation.SecondaryData = null;
            this.cmbNextStation.SelectedData = null;
            this.cmbNextStation.SelectedDataID = null;
            this.cmbNextStation.SelectionList = null;
            this.cmbNextStation.SkipIDColumn = true;
            this.cmbNextStation.RequestData += new System.EventHandler(this.cmbNextStation_RequestData);
            this.cmbNextStation.RequestClear += new System.EventHandler(this.cmbNextStation_RequestClear);
            // 
            // cmbHorizontalLocation
            // 
            this.cmbHorizontalLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHorizontalLocation.FormattingEnabled = true;
            this.cmbHorizontalLocation.Items.AddRange(new object[] {
            resources.GetString("cmbHorizontalLocation.Items"),
            resources.GetString("cmbHorizontalLocation.Items1"),
            resources.GetString("cmbHorizontalLocation.Items2")});
            resources.ApplyResources(this.cmbHorizontalLocation, "cmbHorizontalLocation");
            this.cmbHorizontalLocation.Name = "cmbHorizontalLocation";
            // 
            // chkFullScreen
            // 
            resources.ApplyResources(this.chkFullScreen, "chkFullScreen");
            this.chkFullScreen.Name = "chkFullScreen";
            this.chkFullScreen.UseVisualStyleBackColor = true;
            this.chkFullScreen.CheckedChanged += new System.EventHandler(this.chkFullScreen_CheckedChanged);
            // 
            // lnkIdentify
            // 
            resources.ApplyResources(this.lnkIdentify, "lnkIdentify");
            this.lnkIdentify.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkIdentify.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(113)))), ((int)(((byte)(156)))));
            this.lnkIdentify.Name = "lnkIdentify";
            this.lnkIdentify.TabStop = true;
            this.lnkIdentify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIdentify_LinkClicked);
            // 
            // btnsDisplayProfile
            // 
            this.btnsDisplayProfile.AddButtonEnabled = true;
            this.btnsDisplayProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnsDisplayProfile.Context = LSOne.Controls.ButtonTypes.EditAdd;
            this.btnsDisplayProfile.EditButtonEnabled = true;
            resources.ApplyResources(this.btnsDisplayProfile, "btnsDisplayProfile");
            this.btnsDisplayProfile.Name = "btnsDisplayProfile";
            this.btnsDisplayProfile.RemoveButtonEnabled = false;
            this.btnsDisplayProfile.EditButtonClicked += new System.EventHandler(this.btnsDisplayProfile_EditButtonClicked);
            this.btnsDisplayProfile.AddButtonClicked += new System.EventHandler(this.btnsDisplayProfile_AddButtonClicked);
            // 
            // cmbKdsDisplayProfile
            // 
            this.cmbKdsDisplayProfile.AddList = null;
            this.cmbKdsDisplayProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbKdsDisplayProfile, "cmbKdsDisplayProfile");
            this.cmbKdsDisplayProfile.MaxLength = 32767;
            this.cmbKdsDisplayProfile.Name = "cmbKdsDisplayProfile";
            this.cmbKdsDisplayProfile.NoChangeAllowed = false;
            this.cmbKdsDisplayProfile.OnlyDisplayID = false;
            this.cmbKdsDisplayProfile.RemoveList = null;
            this.cmbKdsDisplayProfile.RowHeight = ((short)(22));
            this.cmbKdsDisplayProfile.SecondaryData = null;
            this.cmbKdsDisplayProfile.SelectedData = null;
            this.cmbKdsDisplayProfile.SelectedDataID = null;
            this.cmbKdsDisplayProfile.SelectionList = null;
            this.cmbKdsDisplayProfile.SkipIDColumn = true;
            this.cmbKdsDisplayProfile.RequestData += new System.EventHandler(this.cmbKdsDisplayProfile_RequestData);
            // 
            // KitchenDisplaySettingsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "KitchenDisplaySettingsPage";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DualDataComboBox cmbKdsVisualProfile;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbKdsStyleProfile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbScreenNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbVerticalLocation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDisplayMode;
        private System.Windows.Forms.Label lblNextStation;
        private DualDataComboBox cmbNextStation;
        private System.Windows.Forms.LinkLabel lnkIdentify;
        private ContextButtons btnsVisualProfile;
        private ContextButtons btnStyleProfile;
        private DualDataComboBox cmbKdsFunctionalProfile;
        private ContextButtons btnsFunctionalProfile;
        private System.Windows.Forms.Label lblKitchenDisplayProfile;
        private System.Windows.Forms.Label lblHorizontalLocation;
        private System.Windows.Forms.Label lblVertialLocation;
        private System.Windows.Forms.ComboBox cmbHorizontalLocation;
        private System.Windows.Forms.CheckBox chkFullScreen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMaxRecallFiles;
        private ContextButtons btnsDisplayProfile;
        private DualDataComboBox cmbKdsDisplayProfile;
        private System.Windows.Forms.Label lblTransferStation;
        private DualDataComboBox cmbTransferStation;
        private System.Windows.Forms.Label lblBumpedPrevious;
        private System.Windows.Forms.CheckBox chkBumpedPrevoius;
        private System.Windows.Forms.Label label3;
        private ContextButtons btnsAggregateProfile;
        private DualDataComboBox cmbKdsAggregateProfile;
        private System.Windows.Forms.Label label8;
        private DualDataComboBox cmbBumpbarOperation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkBumpbar;
    }
}
