using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MixAndMatchLineMultiEditDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MixAndMatchLineMultiEditDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbRelation = new LSOne.Controls.DualDataComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblRelation = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lvlEditPreview = new LSOne.Controls.ListView();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.lblEditPreview = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLineGroups = new LSOne.Controls.DualDataComboBox();
            this.clmVariant = new LSOne.Controls.Columns.Column();
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbRelation
            // 
            this.cmbRelation.AddList = null;
            this.cmbRelation.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbRelation, "cmbRelation");
            this.cmbRelation.MaxLength = 32767;
            this.cmbRelation.Name = "cmbRelation";
            this.cmbRelation.NoChangeAllowed = false;
            this.cmbRelation.OnlyDisplayID = false;
            this.cmbRelation.RemoveList = null;
            this.cmbRelation.RowHeight = ((short)(22));
            this.cmbRelation.SecondaryData = null;
            this.cmbRelation.SelectedData = null;
            this.cmbRelation.SelectedDataID = null;
            this.cmbRelation.SelectionList = null;
            this.cmbRelation.SkipIDColumn = false;
            this.cmbRelation.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbRelation_DropDown);
            this.cmbRelation.SelectedDataChanged += new System.EventHandler(this.cmbConnection_SelectedDataChanged);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2"),
            resources.GetString("cmbType.Items3")});
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // lblRelation
            // 
            resources.ApplyResources(this.lblRelation, "lblRelation");
            this.lblRelation.Name = "lblRelation";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lvlEditPreview
            // 
            this.lvlEditPreview.BuddyControl = null;
            this.lvlEditPreview.Columns.Add(this.column5);
            this.lvlEditPreview.Columns.Add(this.column1);
            this.lvlEditPreview.Columns.Add(this.column2);
            this.lvlEditPreview.Columns.Add(this.clmVariant);
            this.lvlEditPreview.Columns.Add(this.column3);
            this.lvlEditPreview.Columns.Add(this.column4);
            this.lvlEditPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.DefaultRowHeight = ((short)(22));
            this.lvlEditPreview.DimSelectionWhenDisabled = true;
            this.lvlEditPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvlEditPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlEditPreview.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvlEditPreview, "lvlEditPreview");
            this.lvlEditPreview.Name = "lvlEditPreview";
            this.lvlEditPreview.OddRowColor = System.Drawing.Color.White;
            this.lvlEditPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SecondarySortColumn = ((short)(-1));
            this.lvlEditPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvlEditPreview.SortSetting = "-1:1";
            this.lvlEditPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvlEditPreview_CellAction);
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.RelativeSize = 0;
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 0;
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.RelativeSize = 0;
            this.column2.Tag = null;
            this.column2.Width = ((short)(50));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.RelativeSize = 0;
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.Clickable = false;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.RelativeSize = 10;
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // lblEditPreview
            // 
            resources.ApplyResources(this.lblEditPreview, "lblEditPreview");
            this.lblEditPreview.BackColor = System.Drawing.Color.Transparent;
            this.lblEditPreview.Name = "lblEditPreview";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbLineGroups
            // 
            this.cmbLineGroups.AddList = null;
            this.cmbLineGroups.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbLineGroups, "cmbLineGroups");
            this.cmbLineGroups.MaxLength = 32767;
            this.cmbLineGroups.Name = "cmbLineGroups";
            this.cmbLineGroups.NoChangeAllowed = false;
            this.cmbLineGroups.OnlyDisplayID = false;
            this.cmbLineGroups.RemoveList = null;
            this.cmbLineGroups.RowHeight = ((short)(22));
            this.cmbLineGroups.SecondaryData = null;
            this.cmbLineGroups.SelectedData = null;
            this.cmbLineGroups.SelectedDataID = null;
            this.cmbLineGroups.SelectionList = null;
            this.cmbLineGroups.SkipIDColumn = true;
            this.cmbLineGroups.RequestData += new System.EventHandler(this.cmbLineGroups_RequestData);
            this.cmbLineGroups.SelectedDataChanged += new System.EventHandler(this.cmbLineGroups_SelectedDataChanged);
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.Clickable = false;
            this.clmVariant.DefaultStyle = null;
            this.clmVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(5));
            this.clmVariant.NoTextWhenSmall = true;
            this.clmVariant.Sizable = false;
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(50));
            // 
            // MixAndMatchLineMultiEditDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbLineGroups);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblEditPreview);
            this.Controls.Add(this.lvlEditPreview);
            this.Controls.Add(this.cmbRelation);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblRelation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "MixAndMatchLineMultiEditDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblRelation, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbRelation, 0);
            this.Controls.SetChildIndex(this.lvlEditPreview, 0);
            this.Controls.SetChildIndex(this.lblEditPreview, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.cmbLineGroups, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DualDataComboBox cmbRelation;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblRelation;
        private System.Windows.Forms.Label label3;
        private ListView lvlEditPreview;
        private System.Windows.Forms.Label lblEditPreview;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbLineGroups;
        private Column column5;
        private Column clmVariant;
    }
}