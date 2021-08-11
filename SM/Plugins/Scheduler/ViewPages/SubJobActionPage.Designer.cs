using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    partial class SubJobActionPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobActionPage));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ntbActionCounterInterval = new LSOne.Controls.NumericTextBox();
            this.cmbActionTable = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvActions = new LSOne.Controls.ListView();
            this.colActionId = new LSOne.Controls.Columns.Column();
            this.colAction = new LSOne.Controls.Columns.Column();
            this.colObject = new LSOne.Controls.Columns.Column();
            this.colParameters = new LSOne.Controls.Columns.Column();
            this.colDateCreated = new LSOne.Controls.Columns.Column();
            this.btnLoadActions = new System.Windows.Forms.Button();
            this.btnRemoveAction = new LSOne.Controls.ContextButton();
            this.chkAlwaysExecute = new System.Windows.Forms.CheckBox();
            this.lblAlwaysExecute = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ntbActionCounterInterval
            // 
            this.ntbActionCounterInterval.AllowDecimal = false;
            this.ntbActionCounterInterval.AllowNegative = false;
            this.ntbActionCounterInterval.CultureInfo = null;
            this.ntbActionCounterInterval.DecimalLetters = 0;
            this.ntbActionCounterInterval.ForeColor = System.Drawing.Color.Black;
            this.ntbActionCounterInterval.HasMinValue = false;
            resources.ApplyResources(this.ntbActionCounterInterval, "ntbActionCounterInterval");
            this.ntbActionCounterInterval.MaxValue = 0D;
            this.ntbActionCounterInterval.MinValue = 0D;
            this.ntbActionCounterInterval.Name = "ntbActionCounterInterval";
            this.ntbActionCounterInterval.Value = 0D;
            // 
            // cmbActionTable
            // 
            this.cmbActionTable.AddList = null;
            resources.ApplyResources(this.cmbActionTable, "cmbActionTable");
            this.cmbActionTable.MaxLength = 32767;
            this.cmbActionTable.Name = "cmbActionTable";
            this.cmbActionTable.NoChangeAllowed = false;
            this.cmbActionTable.RemoveList = null;
            this.cmbActionTable.RowHeight = ((short)(22));
            this.cmbActionTable.SecondaryData = null;
            this.cmbActionTable.SelectedData = null;
            this.cmbActionTable.SelectionList = null;
            this.cmbActionTable.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbActionTable_DropDown);
            this.cmbActionTable.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbActionTable_FormatData);
            this.cmbActionTable.SelectedDataChanged += new System.EventHandler(this.cmbActionTable_SelectedDataChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // lvActions
            // 
            resources.ApplyResources(this.lvActions, "lvActions");
            this.lvActions.BorderColor = System.Drawing.Color.DarkGray;
            this.lvActions.BuddyControl = null;
            this.lvActions.Columns.Add(this.colActionId);
            this.lvActions.Columns.Add(this.colAction);
            this.lvActions.Columns.Add(this.colObject);
            this.lvActions.Columns.Add(this.colParameters);
            this.lvActions.Columns.Add(this.colDateCreated);
            this.lvActions.ContentBackColor = System.Drawing.Color.White;
            this.lvActions.DefaultRowHeight = ((short)(18));
            this.lvActions.EvenRowColor = System.Drawing.Color.White;
            this.lvActions.HeaderBackColor = System.Drawing.Color.White;
            this.lvActions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvActions.HeaderHeight = ((short)(20));
            this.lvActions.Name = "lvActions";
            this.lvActions.OddRowColor = System.Drawing.Color.White;
            this.lvActions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvActions.SecondarySortColumn = ((short)(-1));
            this.lvActions.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvActions.SortSetting = "0:1";
            this.lvActions.VerticalScrollbarValue = 0;
            this.lvActions.VerticalScrollbarYOffset = 0;
            // 
            // colActionId
            // 
            this.colActionId.AutoSize = true;
            this.colActionId.DefaultStyle = null;
            resources.ApplyResources(this.colActionId, "colActionId");
            this.colActionId.MaximumWidth = ((short)(0));
            this.colActionId.MinimumWidth = ((short)(10));
            this.colActionId.SecondarySortColumn = ((short)(-1));
            this.colActionId.Tag = null;
            this.colActionId.Width = ((short)(50));
            // 
            // colAction
            // 
            this.colAction.AutoSize = true;
            this.colAction.DefaultStyle = null;
            resources.ApplyResources(this.colAction, "colAction");
            this.colAction.MaximumWidth = ((short)(0));
            this.colAction.MinimumWidth = ((short)(10));
            this.colAction.SecondarySortColumn = ((short)(-1));
            this.colAction.Tag = null;
            this.colAction.Width = ((short)(50));
            // 
            // colObject
            // 
            this.colObject.AutoSize = true;
            this.colObject.DefaultStyle = null;
            resources.ApplyResources(this.colObject, "colObject");
            this.colObject.MaximumWidth = ((short)(0));
            this.colObject.MinimumWidth = ((short)(10));
            this.colObject.SecondarySortColumn = ((short)(-1));
            this.colObject.Tag = null;
            this.colObject.Width = ((short)(50));
            // 
            // colParameters
            // 
            this.colParameters.AutoSize = true;
            this.colParameters.DefaultStyle = null;
            resources.ApplyResources(this.colParameters, "colParameters");
            this.colParameters.MaximumWidth = ((short)(0));
            this.colParameters.MinimumWidth = ((short)(10));
            this.colParameters.SecondarySortColumn = ((short)(-1));
            this.colParameters.Tag = null;
            this.colParameters.Width = ((short)(50));
            // 
            // colDateCreated
            // 
            this.colDateCreated.AutoSize = true;
            this.colDateCreated.DefaultStyle = null;
            resources.ApplyResources(this.colDateCreated, "colDateCreated");
            this.colDateCreated.MaximumWidth = ((short)(0));
            this.colDateCreated.MinimumWidth = ((short)(10));
            this.colDateCreated.SecondarySortColumn = ((short)(-1));
            this.colDateCreated.Tag = null;
            this.colDateCreated.Width = ((short)(50));
            // 
            // btnLoadActions
            // 
            resources.ApplyResources(this.btnLoadActions, "btnLoadActions");
            this.btnLoadActions.Name = "btnLoadActions";
            this.btnLoadActions.UseVisualStyleBackColor = true;
            this.btnLoadActions.Click += new System.EventHandler(this.btnLoadActions_Click);
            // 
            // btnRemoveAction
            // 
            resources.ApplyResources(this.btnRemoveAction, "btnRemoveAction");
            this.btnRemoveAction.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveAction.Context = LSOne.Controls.ButtonType.Remove;
            this.btnRemoveAction.Name = "btnRemoveAction";
            this.btnRemoveAction.Click += new System.EventHandler(this.btnRemoveAction_Click);
            // 
            // chkAlwaysExecute
            // 
            resources.ApplyResources(this.chkAlwaysExecute, "chkAlwaysExecute");
            this.chkAlwaysExecute.Name = "chkAlwaysExecute";
            this.chkAlwaysExecute.UseVisualStyleBackColor = true;
            this.chkAlwaysExecute.CheckedChanged += new System.EventHandler(this.chkAlwaysExecute_CheckedChanged);
            // 
            // lblAlwaysExecute
            // 
            this.lblAlwaysExecute.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblAlwaysExecute, "lblAlwaysExecute");
            this.lblAlwaysExecute.Name = "lblAlwaysExecute";
            // 
            // SubJobActionPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblAlwaysExecute);
            this.Controls.Add(this.chkAlwaysExecute);
            this.Controls.Add(this.btnRemoveAction);
            this.Controls.Add(this.btnLoadActions);
            this.Controls.Add(this.lvActions);
            this.Controls.Add(this.ntbActionCounterInterval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbActionTable);
            this.Name = "SubJobActionPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private NumericTextBox ntbActionCounterInterval;
        private DropDownFormComboBox cmbActionTable;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private ListView lvActions;
        private LSOne.Controls.Columns.Column colAction;
        private LSOne.Controls.Columns.Column colObject;
        private LSOne.Controls.Columns.Column colParameters;
        private LSOne.Controls.Columns.Column colDateCreated;
        private LSOne.Controls.Columns.Column colActionId;
        private System.Windows.Forms.Button btnLoadActions;
        private ContextButton btnRemoveAction;
        private System.Windows.Forms.CheckBox chkAlwaysExecute;
        private System.Windows.Forms.Label lblAlwaysExecute;
    }
}
