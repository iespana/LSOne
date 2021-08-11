using LSOne.Controls;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class KitchenDisplayDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KitchenDisplayDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbStationName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblKitchenDisplayProfile = new System.Windows.Forms.Label();
            this.cmbScreenNumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbKdsFunctionalProfile = new LSOne.Controls.DualDataComboBox();
            this.cmbKdsStyleProfile = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbKdsVisualProfile = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFunctionalProfileAdd = new LSOne.Controls.ContextButton();
            this.btnStyleProfileAdd = new LSOne.Controls.ContextButton();
            this.btnVisualProfileAdd = new LSOne.Controls.ContextButton();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.lblHorizontalLocation = new System.Windows.Forms.Label();
            this.lblVertialLocation = new System.Windows.Forms.Label();
            this.cmbVerticalLocation = new System.Windows.Forms.ComboBox();
            this.cmbHorizontalLocation = new System.Windows.Forms.ComboBox();
            this.chkFullScreen = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStationType = new System.Windows.Forms.ComboBox();
            this.btnDisplayProfileAdd = new LSOne.Controls.ContextButton();
            this.cmbDisplayProfile = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            // tbStationName
            // 
            resources.ApplyResources(this.tbStationName, "tbStationName");
            this.tbStationName.Name = "tbStationName";
            this.tbStationName.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lblKitchenDisplayProfile
            // 
            resources.ApplyResources(this.lblKitchenDisplayProfile, "lblKitchenDisplayProfile");
            this.lblKitchenDisplayProfile.Name = "lblKitchenDisplayProfile";
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
            this.cmbScreenNumber.SelectedIndexChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            this.cmbKdsFunctionalProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
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
            this.cmbKdsStyleProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            this.cmbKdsVisualProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // btnFunctionalProfileAdd
            // 
            this.btnFunctionalProfileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnFunctionalProfileAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnFunctionalProfileAdd, "btnFunctionalProfileAdd");
            this.btnFunctionalProfileAdd.Name = "btnFunctionalProfileAdd";
            this.btnFunctionalProfileAdd.Click += new System.EventHandler(this.btnsFunctionalProfile_AddButtonClicked);
            // 
            // btnStyleProfileAdd
            // 
            this.btnStyleProfileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnStyleProfileAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnStyleProfileAdd, "btnStyleProfileAdd");
            this.btnStyleProfileAdd.Name = "btnStyleProfileAdd";
            this.btnStyleProfileAdd.Click += new System.EventHandler(this.btnStyleProfile_AddButtonClicked);
            // 
            // btnVisualProfileAdd
            // 
            this.btnVisualProfileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnVisualProfileAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnVisualProfileAdd, "btnVisualProfileAdd");
            this.btnVisualProfileAdd.Name = "btnVisualProfileAdd";
            this.btnVisualProfileAdd.Click += new System.EventHandler(this.btnsVisualProfile_AddButtonClicked);
            // 
            // tbID
            // 
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            this.tbID.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbStationType
            // 
            this.cmbStationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStationType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStationType, "cmbStationType");
            this.cmbStationType.Name = "cmbStationType";
            // 
            // btnDisplayProfileAdd
            // 
            this.btnDisplayProfileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnDisplayProfileAdd.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnDisplayProfileAdd, "btnDisplayProfileAdd");
            this.btnDisplayProfileAdd.Name = "btnDisplayProfileAdd";
            this.btnDisplayProfileAdd.Click += new System.EventHandler(this.btnDisplayProfileAdd_Click);
            // 
            // cmbDisplayProfile
            // 
            this.cmbDisplayProfile.AddList = null;
            this.cmbDisplayProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbDisplayProfile, "cmbDisplayProfile");
            this.cmbDisplayProfile.MaxLength = 32767;
            this.cmbDisplayProfile.Name = "cmbDisplayProfile";
            this.cmbDisplayProfile.NoChangeAllowed = false;
            this.cmbDisplayProfile.OnlyDisplayID = false;
            this.cmbDisplayProfile.RemoveList = null;
            this.cmbDisplayProfile.RowHeight = ((short)(22));
            this.cmbDisplayProfile.SecondaryData = null;
            this.cmbDisplayProfile.SelectedData = null;
            this.cmbDisplayProfile.SelectedDataID = null;
            this.cmbDisplayProfile.SelectionList = null;
            this.cmbDisplayProfile.SkipIDColumn = true;
            this.cmbDisplayProfile.RequestData += new System.EventHandler(this.cmbDisplayProfile_RequestData);
            this.cmbDisplayProfile.SelectedDataChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // KitchenDisplayDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnDisplayProfileAdd);
            this.Controls.Add(this.cmbDisplayProfile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbStationType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHorizontalLocation);
            this.Controls.Add(this.lblVertialLocation);
            this.Controls.Add(this.cmbVerticalLocation);
            this.Controls.Add(this.cmbHorizontalLocation);
            this.Controls.Add(this.chkFullScreen);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnVisualProfileAdd);
            this.Controls.Add(this.btnStyleProfileAdd);
            this.Controls.Add(this.btnFunctionalProfileAdd);
            this.Controls.Add(this.cmbKdsVisualProfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbKdsStyleProfile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbScreenNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbKdsFunctionalProfile);
            this.Controls.Add(this.lblKitchenDisplayProfile);
            this.Controls.Add(this.tbStationName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "KitchenDisplayDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbStationName, 0);
            this.Controls.SetChildIndex(this.lblKitchenDisplayProfile, 0);
            this.Controls.SetChildIndex(this.cmbKdsFunctionalProfile, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbScreenNumber, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbKdsStyleProfile, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.cmbKdsVisualProfile, 0);
            this.Controls.SetChildIndex(this.btnFunctionalProfileAdd, 0);
            this.Controls.SetChildIndex(this.btnStyleProfileAdd, 0);
            this.Controls.SetChildIndex(this.btnVisualProfileAdd, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.tbID, 0);
            this.Controls.SetChildIndex(this.chkFullScreen, 0);
            this.Controls.SetChildIndex(this.cmbHorizontalLocation, 0);
            this.Controls.SetChildIndex(this.cmbVerticalLocation, 0);
            this.Controls.SetChildIndex(this.lblVertialLocation, 0);
            this.Controls.SetChildIndex(this.lblHorizontalLocation, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbStationType, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbDisplayProfile, 0);
            this.Controls.SetChildIndex(this.btnDisplayProfileAdd, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbStationName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblKitchenDisplayProfile;
        private DualDataComboBox cmbKdsFunctionalProfile;
        private System.Windows.Forms.ComboBox cmbScreenNumber;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbKdsStyleProfile;
        private System.Windows.Forms.Label label4;
        private DualDataComboBox cmbKdsVisualProfile;
        private System.Windows.Forms.Label label5;
        private ContextButton btnFunctionalProfileAdd;
        private ContextButton btnStyleProfileAdd;
        private ContextButton btnVisualProfileAdd;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHorizontalLocation;
        private System.Windows.Forms.Label lblVertialLocation;
        private System.Windows.Forms.ComboBox cmbVerticalLocation;
        private System.Windows.Forms.ComboBox cmbHorizontalLocation;
        private System.Windows.Forms.CheckBox chkFullScreen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStationType;
        private ContextButton btnDisplayProfileAdd;
        private DualDataComboBox cmbDisplayProfile;
        private System.Windows.Forms.Label label8;
    }
}