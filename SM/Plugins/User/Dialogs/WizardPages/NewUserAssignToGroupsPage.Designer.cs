using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.User.Dialogs.WizardPages
{
    partial class NewUserAssignToGroupsPage
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewUserAssignToGroupsPage));
			this.label1 = new System.Windows.Forms.Label();
			this.lblSecurityGroups = new System.Windows.Forms.Label();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.lvSecurityGroups = new LSOne.Controls.ListView();
			this.column1 = new LSOne.Controls.Columns.Column();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbCopyUser = new LSOne.Controls.DualDataComboBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.cmbUserProfile = new LSOne.Controls.DualDataComboBox();
			this.lblUserProfile = new System.Windows.Forms.Label();
			this.btnAddUserProfile = new LSOne.Controls.ContextButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Name = "label1";
			// 
			// lblSecurityGroups
			// 
			this.lblSecurityGroups.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblSecurityGroups, "lblSecurityGroups");
			this.lblSecurityGroups.Name = "lblSecurityGroups";
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this.imageList1, "imageList1");
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// lvSecurityGroups
			// 
			resources.ApplyResources(this.lvSecurityGroups, "lvSecurityGroups");
			this.lvSecurityGroups.AutoSelectOnFocus = true;
			this.lvSecurityGroups.BorderColor = System.Drawing.Color.DarkGray;
			this.lvSecurityGroups.BuddyControl = null;
			this.lvSecurityGroups.Columns.Add(this.column1);
			this.lvSecurityGroups.ContentBackColor = System.Drawing.Color.White;
			this.lvSecurityGroups.DefaultRowHeight = ((short)(22));
			this.lvSecurityGroups.DimSelectionWhenDisabled = true;
			this.lvSecurityGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
			this.lvSecurityGroups.HeaderBackColor = System.Drawing.Color.White;
			this.lvSecurityGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvSecurityGroups.HeaderHeight = ((short)(1));
			this.lvSecurityGroups.Name = "lvSecurityGroups";
			this.lvSecurityGroups.OddRowColor = System.Drawing.Color.White;
			this.lvSecurityGroups.RowLineColor = System.Drawing.Color.LightGray;
			this.lvSecurityGroups.SecondarySortColumn = ((short)(-1));
			this.lvSecurityGroups.SelectedRowColor = System.Drawing.Color.LightGray;
			this.lvSecurityGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
			this.lvSecurityGroups.SortSetting = "0:1";
			this.lvSecurityGroups.VerticalScrollbarValue = 0;
			this.lvSecurityGroups.VerticalScrollbarYOffset = 0;
			this.lvSecurityGroups.CellAction += new LSOne.Controls.CellActionDelegate(this.lvSecurityGroups_CellAction);
			// 
			// column1
			// 
			this.column1.AutoSize = true;
			this.column1.Clickable = false;
			this.column1.DefaultStyle = null;
			resources.ApplyResources(this.column1, "column1");
			this.column1.MaximumWidth = ((short)(0));
			this.column1.MinimumWidth = ((short)(5));
			this.column1.SecondarySortColumn = ((short)(-1));
			this.column1.Sizable = false;
			this.column1.Tag = null;
			this.column1.Width = ((short)(50));
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// cmbCopyUser
			// 
			this.cmbCopyUser.AddList = null;
			this.cmbCopyUser.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbCopyUser, "cmbCopyUser");
			this.cmbCopyUser.MaxLength = 32767;
			this.cmbCopyUser.Name = "cmbCopyUser";
			this.cmbCopyUser.NoChangeAllowed = false;
			this.cmbCopyUser.OnlyDisplayID = false;
			this.cmbCopyUser.RemoveList = null;
			this.cmbCopyUser.RowHeight = ((short)(22));
			this.cmbCopyUser.SecondaryData = null;
			this.cmbCopyUser.SelectedData = null;
			this.cmbCopyUser.SelectedDataID = null;
			this.cmbCopyUser.SelectionList = null;
			this.cmbCopyUser.SkipIDColumn = false;
			this.cmbCopyUser.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbCopyUser_DropDown);
			this.cmbCopyUser.RequestData += new System.EventHandler(this.cmbCopyUser_RequestData);
			this.cmbCopyUser.SelectedDataChanged += new System.EventHandler(this.cmbCopyUser_SelectedDataChanged);
			this.cmbCopyUser.RequestClear += new System.EventHandler(this.cmbCopyUser_RequestClear);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// cmbUserProfile
			// 
			this.cmbUserProfile.AddList = null;
			this.cmbUserProfile.AllowKeyboardSelection = false;
			resources.ApplyResources(this.cmbUserProfile, "cmbUserProfile");
			this.cmbUserProfile.MaxLength = 32767;
			this.cmbUserProfile.Name = "cmbUserProfile";
			this.cmbUserProfile.NoChangeAllowed = false;
			this.cmbUserProfile.OnlyDisplayID = false;
			this.cmbUserProfile.RemoveList = null;
			this.cmbUserProfile.RowHeight = ((short)(22));
			this.cmbUserProfile.SecondaryData = null;
			this.cmbUserProfile.SelectedData = null;
			this.cmbUserProfile.SelectedDataID = null;
			this.cmbUserProfile.SelectionList = null;
			this.cmbUserProfile.SkipIDColumn = true;
			this.cmbUserProfile.RequestData += new System.EventHandler(this.cmbUserProfile_RequestData);
			this.cmbUserProfile.SelectedDataChanged += new System.EventHandler(this.cmbUserProfile_SelectedDataChanged);
			// 
			// lblUserProfile
			// 
			this.lblUserProfile.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.lblUserProfile, "lblUserProfile");
			this.lblUserProfile.Name = "lblUserProfile";
			// 
			// btnAddUserProfile
			// 
			this.btnAddUserProfile.BackColor = System.Drawing.Color.Transparent;
			this.btnAddUserProfile.Context = LSOne.Controls.ButtonType.Add;
			resources.ApplyResources(this.btnAddUserProfile, "btnAddUserProfile");
			this.btnAddUserProfile.Name = "btnAddUserProfile";
			this.btnAddUserProfile.Click += new System.EventHandler(this.btnAddUserProfile_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// NewUserAssignToGroupsPage
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnAddUserProfile);
			this.Controls.Add(this.cmbUserProfile);
			this.Controls.Add(this.lblUserProfile);
			this.Controls.Add(this.cmbCopyUser);
			this.Controls.Add(this.lvSecurityGroups);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblSecurityGroups);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Name = "NewUserAssignToGroupsPage";
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSecurityGroups;
        private System.Windows.Forms.ImageList imageList1;
        private ListView lvSecurityGroups;
        private Column column1;
        private System.Windows.Forms.Label label3;
        private DualDataComboBox cmbCopyUser;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private DualDataComboBox cmbUserProfile;
        private System.Windows.Forms.Label lblUserProfile;
        private ContextButton btnAddUserProfile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}
