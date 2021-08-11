using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class DatabaseDesignPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseDesignPage));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbCodePage = new System.Windows.Forms.TextBox();
            this.grpHeader = new LSOne.Controls.GroupPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lvLocations = new LSOne.Controls.ListView();
            this.colName = new LSOne.Controls.Columns.Column();
            this.colExCode = new LSOne.Controls.Columns.Column();
            this.contextButtonsMembers = new LSOne.Controls.ContextButtons();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.grpHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // tbCodePage
            // 
            resources.ApplyResources(this.tbCodePage, "tbCodePage");
            this.tbCodePage.Name = "tbCodePage";
            this.tbCodePage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCodePage_KeyPress);
            this.tbCodePage.Validating += new System.ComponentModel.CancelEventHandler(this.tbCodePage_Validating);
            // 
            // grpHeader
            // 
            this.grpHeader.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.grpHeader, "grpHeader");
            this.grpHeader.Name = "grpHeader";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // lvLocations
            // 
            resources.ApplyResources(this.lvLocations, "lvLocations");
            this.lvLocations.BuddyControl = null;
            this.lvLocations.Columns.Add(this.colName);
            this.lvLocations.Columns.Add(this.colExCode);
            this.lvLocations.ContentBackColor = System.Drawing.Color.White;
            this.lvLocations.DefaultRowHeight = ((short)(22));
            this.lvLocations.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLocations.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLocations.HeaderHeight = ((short)(25));
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.OddRowColor = System.Drawing.Color.White;
            this.lvLocations.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLocations.SecondarySortColumn = ((short)(-1));
            this.lvLocations.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvLocations.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLocations.SortSetting = "0:1";
            // 
            // colName
            // 
            this.colName.AutoSize = true;
            this.colName.Clickable = false;
            this.colName.DefaultStyle = null;
            resources.ApplyResources(this.colName, "colName");
            this.colName.MaximumWidth = ((short)(0));
            this.colName.MinimumWidth = ((short)(10));
            this.colName.Sizable = false;
            this.colName.Tag = null;
            this.colName.Width = ((short)(300));
            // 
            // colExCode
            // 
            this.colExCode.AutoSize = true;
            this.colExCode.Clickable = false;
            this.colExCode.DefaultStyle = null;
            resources.ApplyResources(this.colExCode, "colExCode");
            this.colExCode.MaximumWidth = ((short)(0));
            this.colExCode.MinimumWidth = ((short)(10));
            this.colExCode.Sizable = false;
            this.colExCode.Tag = null;
            this.colExCode.Width = ((short)(85));
            // 
            // contextButtonsMembers
            // 
            this.contextButtonsMembers.AddButtonEnabled = true;
            resources.ApplyResources(this.contextButtonsMembers, "contextButtonsMembers");
            this.contextButtonsMembers.BackColor = System.Drawing.Color.Transparent;
            this.contextButtonsMembers.Context = LSOne.Controls.ButtonTypes.AddRemove;
            this.contextButtonsMembers.EditButtonEnabled = false;
            this.contextButtonsMembers.Name = "contextButtonsMembers";
            this.contextButtonsMembers.RemoveButtonEnabled = true;
            this.contextButtonsMembers.AddButtonClicked += new System.EventHandler(this.contextButtonsMembers_AddButtonClicked);
            this.contextButtonsMembers.RemoveButtonClicked += new System.EventHandler(this.contextButtonsMembers_RemoveButtonClicked);
            // 
            // DatabaseDesignPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.contextButtonsMembers);
            this.Controls.Add(this.lvLocations);
            this.Controls.Add(this.tbCodePage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grpHeader);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseDesignPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.TextBox tbCodePage;
        private GroupPanel grpHeader;
        private System.Windows.Forms.Label lblTitle;
        private ListView lvLocations;
        private LSOne.Controls.Columns.Column colName;
        private LSOne.Controls.Columns.Column colExCode;
        private ContextButtons contextButtonsMembers;
    }
}
