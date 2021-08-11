using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Profiles.Views
{
    partial class FormProfilesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProfilesView));
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvFormProfiles = new LSOne.Controls.ListView();
            this.colProfile = new LSOne.Controls.Columns.Column();
            this.sysProfile = new LSOne.Controls.Columns.Column();
            this.groupPanelNoSelection = new LSOne.Controls.GroupPanel();
            this.lblFormProfileHeader = new System.Windows.Forms.Label();
            this.btnsLineContextButtons = new LSOne.Controls.ContextButtons();
            this.lvProfileLines = new LSOne.Controls.ListView();
            this.colFormType = new LSOne.Controls.Columns.Column();
            this.colLayout = new LSOne.Controls.Columns.Column();
            this.colAsSlip = new LSOne.Controls.Columns.Column();
            this.colPrintBehavior = new LSOne.Controls.Columns.Column();
            this.colSystem = new LSOne.Controls.Columns.Column();
            this.lblNoSelection = new System.Windows.Forms.Label();
            this.colCopies = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.groupPanelNoSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            this.pnlBottom.Controls.Add(this.lvFormProfiles);
            this.pnlBottom.Controls.Add(this.groupPanelNoSelection);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvFormProfiles
            // 
            resources.ApplyResources(this.lvFormProfiles, "lvFormProfiles");
            this.lvFormProfiles.BuddyControl = null;
            this.lvFormProfiles.Columns.Add(this.colProfile);
            this.lvFormProfiles.Columns.Add(this.sysProfile);
            this.lvFormProfiles.ContentBackColor = System.Drawing.Color.White;
            this.lvFormProfiles.DefaultRowHeight = ((short)(18));
            this.lvFormProfiles.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFormProfiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFormProfiles.HeaderHeight = ((short)(20));
            this.lvFormProfiles.Name = "lvFormProfiles";
            this.lvFormProfiles.OddRowColor = System.Drawing.Color.White;
            this.lvFormProfiles.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFormProfiles.SecondarySortColumn = ((short)(-1));
            this.lvFormProfiles.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvFormProfiles.SortSetting = "0:1";
            this.lvFormProfiles.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvFormProfiles_HeaderClicked);
            this.lvFormProfiles.SelectionChanged += new System.EventHandler(this.lvFormProfiles_SelectionChanged);
            this.lvFormProfiles.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvFormProfiles_RowDoubleClick);
            // 
            // colProfile
            // 
            this.colProfile.AutoSize = true;
            this.colProfile.DefaultStyle = null;
            resources.ApplyResources(this.colProfile, "colProfile");
            this.colProfile.MaximumWidth = ((short)(0));
            this.colProfile.MinimumWidth = ((short)(10));
            this.colProfile.SecondarySortColumn = ((short)(-1));
            this.colProfile.Tag = null;
            this.colProfile.Width = ((short)(510));
            // 
            // sysProfile
            // 
            this.sysProfile.AutoSize = true;
            this.sysProfile.DefaultStyle = null;
            resources.ApplyResources(this.sysProfile, "sysProfile");
            this.sysProfile.MaximumWidth = ((short)(0));
            this.sysProfile.MinimumWidth = ((short)(10));
            this.sysProfile.SecondarySortColumn = ((short)(-1));
            this.sysProfile.Tag = null;
            this.sysProfile.Width = ((short)(50));
            // 
            // groupPanelNoSelection
            // 
            resources.ApplyResources(this.groupPanelNoSelection, "groupPanelNoSelection");
            this.groupPanelNoSelection.Controls.Add(this.lblFormProfileHeader);
            this.groupPanelNoSelection.Controls.Add(this.btnsLineContextButtons);
            this.groupPanelNoSelection.Controls.Add(this.lvProfileLines);
            this.groupPanelNoSelection.Controls.Add(this.lblNoSelection);
            this.groupPanelNoSelection.Name = "groupPanelNoSelection";
            // 
            // lblFormProfileHeader
            // 
            resources.ApplyResources(this.lblFormProfileHeader, "lblFormProfileHeader");
            this.lblFormProfileHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblFormProfileHeader.Name = "lblFormProfileHeader";
            // 
            // btnsLineContextButtons
            // 
            this.btnsLineContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsLineContextButtons, "btnsLineContextButtons");
            this.btnsLineContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsLineContextButtons.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsLineContextButtons.EditButtonEnabled = false;
            this.btnsLineContextButtons.Name = "btnsLineContextButtons";
            this.btnsLineContextButtons.RemoveButtonEnabled = false;
            this.btnsLineContextButtons.EditButtonClicked += new System.EventHandler(this.btnsLineContextButtons_EditButtonClicked);
            this.btnsLineContextButtons.AddButtonClicked += new System.EventHandler(this.btnsLineContextButtons_AddButtonClicked);
            this.btnsLineContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnsLineContextButtons_RemoveButtonClicked);
            // 
            // lvProfileLines
            // 
            resources.ApplyResources(this.lvProfileLines, "lvProfileLines");
            this.lvProfileLines.BuddyControl = null;
            this.lvProfileLines.Columns.Add(this.colFormType);
            this.lvProfileLines.Columns.Add(this.colLayout);
            this.lvProfileLines.Columns.Add(this.colAsSlip);
            this.lvProfileLines.Columns.Add(this.colPrintBehavior);
            this.lvProfileLines.Columns.Add(this.colCopies);
            this.lvProfileLines.Columns.Add(this.colSystem);
            this.lvProfileLines.ContentBackColor = System.Drawing.Color.White;
            this.lvProfileLines.DefaultRowHeight = ((short)(18));
            this.lvProfileLines.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvProfileLines.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvProfileLines.HeaderHeight = ((short)(20));
            this.lvProfileLines.Name = "lvProfileLines";
            this.lvProfileLines.OddRowColor = System.Drawing.Color.White;
            this.lvProfileLines.RowLineColor = System.Drawing.Color.LightGray;
            this.lvProfileLines.SecondarySortColumn = ((short)(-1));
            this.lvProfileLines.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvProfileLines.SortSetting = "0:1";
            this.lvProfileLines.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvProfileLines_HeaderClicked);
            this.lvProfileLines.SelectionChanged += new System.EventHandler(this.lvProfileLines_SelectionChanged);
            this.lvProfileLines.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvProfileLines_RowDoubleClick);
            // 
            // colFormType
            // 
            this.colFormType.AutoSize = true;
            this.colFormType.DefaultStyle = null;
            resources.ApplyResources(this.colFormType, "colFormType");
            this.colFormType.MaximumWidth = ((short)(0));
            this.colFormType.MinimumWidth = ((short)(10));
            this.colFormType.SecondarySortColumn = ((short)(-1));
            this.colFormType.Tag = null;
            this.colFormType.Width = ((short)(150));
            // 
            // colLayout
            // 
            this.colLayout.AutoSize = true;
            this.colLayout.DefaultStyle = null;
            resources.ApplyResources(this.colLayout, "colLayout");
            this.colLayout.MaximumWidth = ((short)(0));
            this.colLayout.MinimumWidth = ((short)(10));
            this.colLayout.SecondarySortColumn = ((short)(-1));
            this.colLayout.Tag = null;
            this.colLayout.Width = ((short)(150));
            // 
            // colAsSlip
            // 
            this.colAsSlip.AutoSize = true;
            this.colAsSlip.DefaultStyle = null;
            resources.ApplyResources(this.colAsSlip, "colAsSlip");
            this.colAsSlip.MaximumWidth = ((short)(0));
            this.colAsSlip.MinimumWidth = ((short)(10));
            this.colAsSlip.SecondarySortColumn = ((short)(-1));
            this.colAsSlip.Tag = null;
            this.colAsSlip.Width = ((short)(50));
            // 
            // colPrintBehavior
            // 
            this.colPrintBehavior.AutoSize = true;
            this.colPrintBehavior.DefaultStyle = null;
            resources.ApplyResources(this.colPrintBehavior, "colPrintBehavior");
            this.colPrintBehavior.MaximumWidth = ((short)(0));
            this.colPrintBehavior.MinimumWidth = ((short)(10));
            this.colPrintBehavior.SecondarySortColumn = ((short)(-1));
            this.colPrintBehavior.Tag = null;
            this.colPrintBehavior.Width = ((short)(100));
            // 
            // colSystem
            // 
            this.colSystem.AutoSize = true;
            this.colSystem.DefaultStyle = null;
            resources.ApplyResources(this.colSystem, "colSystem");
            this.colSystem.MaximumWidth = ((short)(0));
            this.colSystem.MinimumWidth = ((short)(10));
            this.colSystem.SecondarySortColumn = ((short)(-1));
            this.colSystem.Tag = null;
            this.colSystem.Width = ((short)(50));
            // 
            // lblNoSelection
            // 
            resources.ApplyResources(this.lblNoSelection, "lblNoSelection");
            this.lblNoSelection.BackColor = System.Drawing.Color.Transparent;
            this.lblNoSelection.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNoSelection.Name = "lblNoSelection";
            // 
            // colCopies
            // 
            this.colCopies.AutoSize = true;
            this.colCopies.DefaultStyle = null;
            resources.ApplyResources(this.colCopies, "colCopies");
            this.colCopies.MaximumWidth = ((short)(0));
            this.colCopies.MinimumWidth = ((short)(10));
            this.colCopies.SecondarySortColumn = ((short)(-1));
            this.colCopies.Tag = null;
            this.colCopies.Width = ((short)(50));
            // 
            // FormProfilesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "FormProfilesView";
            this.pnlBottom.ResumeLayout(false);
            this.groupPanelNoSelection.ResumeLayout(false);
            this.groupPanelNoSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private ListView lvFormProfiles;
        private Column colProfile;
        private GroupPanel groupPanelNoSelection;
        private ListView lvProfileLines;
        private System.Windows.Forms.Label lblNoSelection;
        private Column colFormType;
        private Column colLayout;
        private Column colAsSlip;
        private Column colPrintBehavior;
        private ContextButtons btnsLineContextButtons;
        private System.Windows.Forms.Label lblFormProfileHeader;
        private Column sysProfile;
        private Column colSystem;
        private Column colCopies;
    }
}
