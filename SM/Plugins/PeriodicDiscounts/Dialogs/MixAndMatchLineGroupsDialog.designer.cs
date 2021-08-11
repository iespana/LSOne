using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    partial class MixAndMatchLineGroupsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MixAndMatchLineGroupsDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnsContextButtons = new LSOne.Controls.ContextButtons();
            this.lvGroups = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Name = "panel2";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
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
            // lvGroups
            // 
            resources.ApplyResources(this.lvGroups, "lvGroups");
            this.lvGroups.BuddyControl = null;
            this.lvGroups.Columns.Add(this.column1);
            this.lvGroups.Columns.Add(this.column2);
            this.lvGroups.Columns.Add(this.column3);
            this.lvGroups.Columns.Add(this.column4);
            this.lvGroups.ContentBackColor = System.Drawing.Color.White;
            this.lvGroups.DefaultRowHeight = ((short)(25));
            this.lvGroups.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvGroups.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvGroups.HeaderHeight = ((short)(25));
            this.lvGroups.Name = "lvGroups";
            this.lvGroups.OddRowColor = System.Drawing.Color.White;
            this.lvGroups.RowLineColor = System.Drawing.Color.LightGray;
            this.lvGroups.SecondarySortColumn = ((short)(-1));
            this.lvGroups.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvGroups.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvGroups.SortSetting = "-1:1";
            this.lvGroups.SelectionChanged += new System.EventHandler(this.lvGroups_SelectionChanged);
            this.lvGroups.CellAction += new LSOne.Controls.CellActionDelegate(this.lvGroups_CellAction);
            this.lvGroups.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvGroups_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.RelativeSize = 50;
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(150));
            // 
            // column2
            // 
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.RelativeSize = 50;
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Sizable = false;
            this.column2.Tag = null;
            this.column2.Width = ((short)(150));
            // 
            // column3
            // 
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.RelativeSize = 0;
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Sizable = false;
            this.column3.Tag = null;
            this.column3.Width = ((short)(100));
            // 
            // column4
            // 
            this.column4.Clickable = false;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.RelativeSize = 0;
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Sizable = false;
            this.column4.Tag = null;
            this.column4.Width = ((short)(20));
            // 
            // MixAndMatchLineGroupsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lvGroups);
            this.Controls.Add(this.btnsContextButtons);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "MixAndMatchLineGroupsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.btnsContextButtons, 0);
            this.Controls.SetChildIndex(this.lvGroups, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButtons btnsContextButtons;
        private ListView lvGroups;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
    }
}