using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    partial class AggregateGroupItemDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AggregateGroupItemDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbConnection = new LSOne.Controls.DualDataComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblConnection = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblEditPreview = new System.Windows.Forms.Label();
            this.lvlEditPreview = new LSOne.Controls.ListView();
            this.clmType = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmChange = new LSOne.Controls.Columns.Column();
            this.clmRemoveBtn = new LSOne.Controls.Columns.Column();
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
            // cmbConnection
            // 
            this.cmbConnection.AddList = null;
            this.cmbConnection.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbConnection, "cmbConnection");
            this.cmbConnection.MaxLength = 32767;
            this.cmbConnection.Name = "cmbConnection";
            this.cmbConnection.NoChangeAllowed = false;
            this.cmbConnection.OnlyDisplayID = false;
            this.cmbConnection.RemoveList = null;
            this.cmbConnection.RowHeight = ((short)(22));
            this.cmbConnection.SecondaryData = null;
            this.cmbConnection.SelectedData = null;
            this.cmbConnection.SelectedDataID = null;
            this.cmbConnection.SelectionList = null;
            this.cmbConnection.SkipIDColumn = false;
            this.cmbConnection.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbConnection_DropDown);
            this.cmbConnection.SelectedDataChanged += new System.EventHandler(this.cmbConnection_SelectedDataChanged);
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2")});
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // lblConnection
            // 
            resources.ApplyResources(this.lblConnection, "lblConnection");
            this.lblConnection.Name = "lblConnection";
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // lblEditPreview
            // 
            resources.ApplyResources(this.lblEditPreview, "lblEditPreview");
            this.lblEditPreview.BackColor = System.Drawing.Color.Transparent;
            this.lblEditPreview.Name = "lblEditPreview";
            // 
            // lvlEditPreview
            // 
            this.lvlEditPreview.BorderColor = System.Drawing.Color.DarkGray;
            this.lvlEditPreview.BuddyControl = null;
            this.lvlEditPreview.Columns.Add(this.clmType);
            this.lvlEditPreview.Columns.Add(this.clmDescription);
            this.lvlEditPreview.Columns.Add(this.clmChange);
            this.lvlEditPreview.Columns.Add(this.clmRemoveBtn);
            this.lvlEditPreview.ContentBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.DefaultRowHeight = ((short)(22));
            this.lvlEditPreview.DimSelectionWhenDisabled = true;
            this.lvlEditPreview.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvlEditPreview.HeaderBackColor = System.Drawing.Color.White;
            this.lvlEditPreview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvlEditPreview.HeaderHeight = ((short)(25));
            resources.ApplyResources(this.lvlEditPreview, "lvlEditPreview");
            this.lvlEditPreview.Name = "lvlEditPreview";
            this.lvlEditPreview.OddRowColor = System.Drawing.Color.White;
            this.lvlEditPreview.RowLineColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SecondarySortColumn = ((short)(-1));
            this.lvlEditPreview.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvlEditPreview.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvlEditPreview.SortSetting = "0:1";
            this.lvlEditPreview.VerticalScrollbarValue = 0;
            this.lvlEditPreview.VerticalScrollbarYOffset = 0;
            this.lvlEditPreview.CellAction += new LSOne.Controls.CellActionDelegate(this.lvlEditPreview_CellAction);
            // 
            // clmType
            // 
            this.clmType.AutoSize = true;
            this.clmType.Clickable = false;
            this.clmType.DefaultStyle = null;
            resources.ApplyResources(this.clmType, "clmType");
            this.clmType.MaximumWidth = ((short)(0));
            this.clmType.MinimumWidth = ((short)(10));
            this.clmType.RelativeSize = 0;
            this.clmType.SecondarySortColumn = ((short)(-1));
            this.clmType.Sizable = false;
            this.clmType.Tag = null;
            this.clmType.Width = ((short)(50));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.Clickable = false;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.RelativeSize = 0;
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // clmChange
            // 
            this.clmChange.AutoSize = true;
            this.clmChange.Clickable = false;
            this.clmChange.DefaultStyle = null;
            resources.ApplyResources(this.clmChange, "clmChange");
            this.clmChange.MaximumWidth = ((short)(0));
            this.clmChange.MinimumWidth = ((short)(10));
            this.clmChange.RelativeSize = 0;
            this.clmChange.SecondarySortColumn = ((short)(-1));
            this.clmChange.Tag = null;
            this.clmChange.Width = ((short)(50));
            // 
            // clmRemoveBtn
            // 
            this.clmRemoveBtn.AutoSize = true;
            this.clmRemoveBtn.Clickable = false;
            this.clmRemoveBtn.DefaultStyle = null;
            resources.ApplyResources(this.clmRemoveBtn, "clmRemoveBtn");
            this.clmRemoveBtn.MaximumWidth = ((short)(0));
            this.clmRemoveBtn.MinimumWidth = ((short)(10));
            this.clmRemoveBtn.RelativeSize = 10;
            this.clmRemoveBtn.SecondarySortColumn = ((short)(-1));
            this.clmRemoveBtn.Tag = null;
            this.clmRemoveBtn.Width = ((short)(50));
            // 
            // AggregateGroupItemDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lvlEditPreview);
            this.Controls.Add(this.lblEditPreview);
            this.Controls.Add(this.cmbConnection);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "AggregateGroupItemDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblType, 0);
            this.Controls.SetChildIndex(this.lblConnection, 0);
            this.Controls.SetChildIndex(this.cmbType, 0);
            this.Controls.SetChildIndex(this.cmbConnection, 0);
            this.Controls.SetChildIndex(this.lblEditPreview, 0);
            this.Controls.SetChildIndex(this.lvlEditPreview, 0);
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
        private DualDataComboBox cmbConnection;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblEditPreview;
        private ListView lvlEditPreview;
        private Column clmType;
        private Column clmDescription;
        private Column clmChange;
        private Column clmRemoveBtn;

    }
}