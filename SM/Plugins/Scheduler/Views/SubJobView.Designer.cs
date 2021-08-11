using LSOne.Controls;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class SubJobView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubJobView));
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRepliactionMethod = new System.Windows.Forms.ComboBox();
            this.cmbWhatToDo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkIncludeFlowFields = new System.Windows.Forms.CheckBox();
            this.tabControl = new LSOne.ViewCore.Controls.TabControl();
            this.label9 = new System.Windows.Forms.Label();
            this.tbStoredProcName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbTableNameTo = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.cmbTableName = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.chkMoveActions = new System.Windows.Forms.CheckBox();
            this.tbFilterCodeFilter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.tbFilterCodeFilter);
            this.pnlBottom.Controls.Add(this.label5);
            this.pnlBottom.Controls.Add(this.tbStoredProcName);
            this.pnlBottom.Controls.Add(this.label2);
            this.pnlBottom.Controls.Add(this.chkMoveActions);
            this.pnlBottom.Controls.Add(this.label10);
            this.pnlBottom.Controls.Add(this.cmbTableNameTo);
            this.pnlBottom.Controls.Add(this.label9);
            this.pnlBottom.Controls.Add(this.cmbWhatToDo);
            this.pnlBottom.Controls.Add(this.label8);
            this.pnlBottom.Controls.Add(this.label7);
            this.pnlBottom.Controls.Add(this.chkEnabled);
            this.pnlBottom.Controls.Add(this.chkIncludeFlowFields);
            this.pnlBottom.Controls.Add(this.tbDescription);
            this.pnlBottom.Controls.Add(this.label6);
            this.pnlBottom.Controls.Add(this.tabControl);
            this.pnlBottom.Controls.Add(this.label1);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.cmbTableName);
            this.pnlBottom.Controls.Add(this.cmbRepliactionMethod);
            this.pnlBottom.Controls.Add(this.label4);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Validating += new System.ComponentModel.CancelEventHandler(this.tbDescription_Validating);
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
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbRepliactionMethod
            // 
            this.cmbRepliactionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRepliactionMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbRepliactionMethod, "cmbRepliactionMethod");
            this.cmbRepliactionMethod.Name = "cmbRepliactionMethod";
            this.cmbRepliactionMethod.SelectedIndexChanged += new System.EventHandler(this.cmbRepliactionMethod_SelectedIndexChanged);
            // 
            // cmbWhatToDo
            // 
            this.cmbWhatToDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWhatToDo.FormattingEnabled = true;
            resources.ApplyResources(this.cmbWhatToDo, "cmbWhatToDo");
            this.cmbWhatToDo.Name = "cmbWhatToDo";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // chkEnabled
            // 
            resources.ApplyResources(this.chkEnabled, "chkEnabled");
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // chkIncludeFlowFields
            // 
            resources.ApplyResources(this.chkIncludeFlowFields, "chkIncludeFlowFields");
            this.chkIncludeFlowFields.Name = "chkIncludeFlowFields";
            this.chkIncludeFlowFields.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.TabStop = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbStoredProcName
            // 
            resources.ApplyResources(this.tbStoredProcName, "tbStoredProcName");
            this.tbStoredProcName.Name = "tbStoredProcName";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cmbTableNameTo
            // 
            this.cmbTableNameTo.AddList = null;
            this.cmbTableNameTo.EnableTextBox = true;
            resources.ApplyResources(this.cmbTableNameTo, "cmbTableNameTo");
            this.cmbTableNameTo.MaxLength = 250;
            this.cmbTableNameTo.Name = "cmbTableNameTo";
            this.cmbTableNameTo.NoChangeAllowed = false;
            this.cmbTableNameTo.RemoveList = null;
            this.cmbTableNameTo.RowHeight = ((short)(22));
            this.cmbTableNameTo.SecondaryData = null;
            this.cmbTableNameTo.SelectedData = null;
            this.cmbTableNameTo.SelectionList = null;
            this.cmbTableNameTo.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTableNameTo_DropDown);
            this.cmbTableNameTo.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbTableNameTo_FormatData);
            // 
            // cmbTableName
            // 
            this.cmbTableName.AddList = null;
            resources.ApplyResources(this.cmbTableName, "cmbTableName");
            this.cmbTableName.MaxLength = 32767;
            this.cmbTableName.Name = "cmbTableName";
            this.cmbTableName.NoChangeAllowed = false;
            this.cmbTableName.RemoveList = null;
            this.cmbTableName.RowHeight = ((short)(22));
            this.cmbTableName.SecondaryData = null;
            this.cmbTableName.SelectedData = null;
            this.cmbTableName.SelectionList = null;
            this.cmbTableName.DropDown += new LSOne.Controls.DropDownEventHandler(this.cmbTableName_DropDown);
            this.cmbTableName.FormatData += new LSOne.Controls.DropDownFormatDataHandler(this.cmbTableName_FormatData);
            this.cmbTableName.SelectedDataChanged += new System.EventHandler(this.cmbTableName_SelectedDataChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // chkMoveActions
            // 
            resources.ApplyResources(this.chkMoveActions, "chkMoveActions");
            this.chkMoveActions.Name = "chkMoveActions";
            this.chkMoveActions.UseVisualStyleBackColor = true;
            // 
            // tbFilterCodeFilter
            // 
            resources.ApplyResources(this.tbFilterCodeFilter, "tbFilterCodeFilter");
            this.tbFilterCodeFilter.Name = "tbFilterCodeFilter";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // SubJobView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.GrayHeaderHeight = 46;
            this.Name = "SubJobView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkIncludeFlowFields;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.ComboBox cmbWhatToDo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbRepliactionMethod;
        private System.Windows.Forms.Label label4;
        private TabControl tabControl;
        private System.Windows.Forms.Label label9;
        private DropDownFormComboBox cmbTableNameTo;
        private DropDownFormComboBox cmbTableName;
        private System.Windows.Forms.TextBox tbStoredProcName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkMoveActions;
        private System.Windows.Forms.TextBox tbFilterCodeFilter;
        private System.Windows.Forms.Label label5;
    }
}
