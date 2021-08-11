using LSOne.Controls;
using LSOne.Utilities.ColorPalette;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Terminals.Views
{
    partial class TerminalView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalView));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbVisualProfile = new LSOne.Controls.DualDataComboBox();
            this.cmbHardwareProfile = new LSOne.Controls.DualDataComboBox();
            this.cmbStore = new LSOne.Controls.DualDataComboBox();
            this.tabSheetTabs = new LSOne.ViewCore.Controls.TabControl();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTouchLayout = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbFunctionalProfile = new LSOne.Controls.DualDataComboBox();
            this.btnEditStore = new LSOne.Controls.ContextButton();
            this.btnEditTouchButtons = new LSOne.Controls.ContextButton();
            this.btnEditFunctionalProfile = new LSOne.Controls.ContextButton();
            this.btnEditVisualProfile = new LSOne.Controls.ContextButton();
            this.btnEditHardwareProfiles = new LSOne.Controls.ContextButton();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEditHardwareProfiles);
            this.pnlBottom.Controls.Add(this.btnEditVisualProfile);
            this.pnlBottom.Controls.Add(this.btnEditFunctionalProfile);
            this.pnlBottom.Controls.Add(this.btnEditTouchButtons);
            this.pnlBottom.Controls.Add(this.btnEditStore);
            this.pnlBottom.Controls.Add(this.cmbFunctionalProfile);
            this.pnlBottom.Controls.Add(this.label7);
            this.pnlBottom.Controls.Add(this.cmbTouchLayout);
            this.pnlBottom.Controls.Add(this.label4);
            this.pnlBottom.Controls.Add(this.tabSheetTabs);
            this.pnlBottom.Controls.Add(this.label6);
            this.pnlBottom.Controls.Add(this.cmbVisualProfile);
            this.pnlBottom.Controls.Add(this.cmbHardwareProfile);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.cmbStore);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.tbID);
            this.pnlBottom.Controls.Add(this.label2);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tbID
            // 
            this.tbID.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbID, "tbID");
            this.tbID.Name = "tbID";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbVisualProfile
            // 
            this.cmbVisualProfile.AddList = null;
            this.cmbVisualProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbVisualProfile, "cmbVisualProfile");
            this.cmbVisualProfile.MaxLength = 32767;
            this.cmbVisualProfile.Name = "cmbVisualProfile";
            this.cmbVisualProfile.NoChangeAllowed = false;
            this.cmbVisualProfile.OnlyDisplayID = false;
            this.cmbVisualProfile.RemoveList = null;
            this.cmbVisualProfile.RowHeight = ((short)(22));
            this.cmbVisualProfile.SecondaryData = null;
            this.cmbVisualProfile.SelectedData = null;
            this.cmbVisualProfile.SelectedDataID = null;
            this.cmbVisualProfile.SelectionList = null;
            this.cmbVisualProfile.SkipIDColumn = true;
            this.cmbVisualProfile.RequestData += new System.EventHandler(this.cmbVisualProfile_RequestData);
            // 
            // cmbHardwareProfile
            // 
            this.cmbHardwareProfile.AddList = null;
            this.cmbHardwareProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbHardwareProfile, "cmbHardwareProfile");
            this.cmbHardwareProfile.MaxLength = 32767;
            this.cmbHardwareProfile.Name = "cmbHardwareProfile";
            this.cmbHardwareProfile.NoChangeAllowed = false;
            this.cmbHardwareProfile.OnlyDisplayID = false;
            this.cmbHardwareProfile.RemoveList = null;
            this.cmbHardwareProfile.RowHeight = ((short)(22));
            this.cmbHardwareProfile.SecondaryData = null;
            this.cmbHardwareProfile.SelectedData = null;
            this.cmbHardwareProfile.SelectedDataID = null;
            this.cmbHardwareProfile.SelectionList = null;
            this.cmbHardwareProfile.SkipIDColumn = true;
            this.cmbHardwareProfile.RequestData += new System.EventHandler(this.cmbHardwareProfile_RequestData);
            this.cmbHardwareProfile.SelectedDataChanged += new System.EventHandler(this.cmbHardwareProfile_SelectedDataChanged);
            // 
            // cmbStore
            // 
            this.cmbStore.AddList = null;
            this.cmbStore.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbStore, "cmbStore");
            this.cmbStore.MaxLength = 32767;
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.NoChangeAllowed = false;
            this.cmbStore.OnlyDisplayID = false;
            this.cmbStore.RemoveList = null;
            this.cmbStore.RowHeight = ((short)(22));
            this.cmbStore.SecondaryData = null;
            this.cmbStore.SelectedData = null;
            this.cmbStore.SelectedDataID = null;
            this.cmbStore.SelectionList = null;
            this.cmbStore.SkipIDColumn = true;
            this.cmbStore.RequestData += new System.EventHandler(this.cmbStore_RequestData);
            // 
            // tabSheetTabs
            // 
            resources.ApplyResources(this.tabSheetTabs, "tabSheetTabs");
            this.tabSheetTabs.Name = "tabSheetTabs";
            this.tabSheetTabs.TabStop = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbTouchLayout
            // 
            this.cmbTouchLayout.AddList = null;
            this.cmbTouchLayout.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbTouchLayout, "cmbTouchLayout");
            this.cmbTouchLayout.MaxLength = 32767;
            this.cmbTouchLayout.Name = "cmbTouchLayout";
            this.cmbTouchLayout.NoChangeAllowed = false;
            this.cmbTouchLayout.OnlyDisplayID = false;
            this.cmbTouchLayout.RemoveList = null;
            this.cmbTouchLayout.RowHeight = ((short)(22));
            this.cmbTouchLayout.SecondaryData = null;
            this.cmbTouchLayout.SelectedData = null;
            this.cmbTouchLayout.SelectedDataID = null;
            this.cmbTouchLayout.SelectionList = null;
            this.cmbTouchLayout.SkipIDColumn = true;
            this.cmbTouchLayout.RequestData += new System.EventHandler(this.cmbTouchLayout_RequestData);
            this.cmbTouchLayout.RequestClear += new System.EventHandler(this.cmbTouchLayout_RequestClear);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbFunctionalProfile
            // 
            this.cmbFunctionalProfile.AddList = null;
            this.cmbFunctionalProfile.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbFunctionalProfile, "cmbFunctionalProfile");
            this.cmbFunctionalProfile.MaxLength = 32767;
            this.cmbFunctionalProfile.Name = "cmbFunctionalProfile";
            this.cmbFunctionalProfile.NoChangeAllowed = false;
            this.cmbFunctionalProfile.OnlyDisplayID = false;
            this.cmbFunctionalProfile.RemoveList = null;
            this.cmbFunctionalProfile.RowHeight = ((short)(22));
            this.cmbFunctionalProfile.SecondaryData = null;
            this.cmbFunctionalProfile.SelectedData = null;
            this.cmbFunctionalProfile.SelectedDataID = null;
            this.cmbFunctionalProfile.SelectionList = null;
            this.cmbFunctionalProfile.SkipIDColumn = true;
            this.cmbFunctionalProfile.RequestData += new System.EventHandler(this.cmbFunctionalProfile_RequestData);
            this.cmbFunctionalProfile.RequestClear += new System.EventHandler(this.cmbFunctionalProfile_RequestClear);
            // 
            // btnEditStore
            // 
            this.btnEditStore.BackColor = System.Drawing.Color.Transparent;
            this.btnEditStore.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditStore, "btnEditStore");
            this.btnEditStore.Name = "btnEditStore";
            this.btnEditStore.Click += new System.EventHandler(this.btnEditStore_Click);
            // 
            // btnEditTouchButtons
            // 
            this.btnEditTouchButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnEditTouchButtons.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditTouchButtons, "btnEditTouchButtons");
            this.btnEditTouchButtons.Name = "btnEditTouchButtons";
            this.btnEditTouchButtons.Click += new System.EventHandler(this.btnEditTouchButtons_Click);
            // 
            // btnEditFunctionalProfile
            // 
            this.btnEditFunctionalProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnEditFunctionalProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditFunctionalProfile, "btnEditFunctionalProfile");
            this.btnEditFunctionalProfile.Name = "btnEditFunctionalProfile";
            this.btnEditFunctionalProfile.Click += new System.EventHandler(this.btnEditFunctionalProfile_Click);
            // 
            // btnEditVisualProfile
            // 
            this.btnEditVisualProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnEditVisualProfile.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditVisualProfile, "btnEditVisualProfile");
            this.btnEditVisualProfile.Name = "btnEditVisualProfile";
            this.btnEditVisualProfile.Click += new System.EventHandler(this.btnEditVisualProfile_Click);
            // 
            // btnEditHardwareProfiles
            // 
            this.btnEditHardwareProfiles.BackColor = System.Drawing.Color.Transparent;
            this.btnEditHardwareProfiles.Context = LSOne.Controls.ButtonType.Edit;
            resources.ApplyResources(this.btnEditHardwareProfiles, "btnEditHardwareProfiles");
            this.btnEditHardwareProfiles.Name = "btnEditHardwareProfiles";
            this.btnEditHardwareProfiles.Click += new System.EventHandler(this.btnEditHardwareProfiles_Click);
            // 
            // TerminalView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 70;
            this.Name = "TerminalView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbStore;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbVisualProfile;
        private DualDataComboBox cmbHardwareProfile;
        private System.Windows.Forms.Label label5;
        private TabControl tabSheetTabs;
        private DualDataComboBox cmbTouchLayout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbFunctionalProfile;
        private ContextButton btnEditHardwareProfiles;
        private ContextButton btnEditVisualProfile;
        private ContextButton btnEditFunctionalProfile;
        private ContextButton btnEditTouchButtons;
        private ContextButton btnEditStore;

    }
}
