using LSOne.Controls;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    partial class NewTouchButtonDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTouchButtonDialog));
            this.cmbCopyFrom = new LSOne.Controls.DualDataComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbButtonGrid5Menu = new LSOne.Controls.DualDataComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbButtonGrid4Menu = new LSOne.Controls.DualDataComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbButtonGrid3Menu = new LSOne.Controls.DualDataComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbButtonGrid2Menu = new LSOne.Controls.DualDataComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbButtonGrid1Menu = new LSOne.Controls.DualDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd1 = new LSOne.Controls.ContextButton();
            this.btnAdd2 = new LSOne.Controls.ContextButton();
            this.btnAdd3 = new LSOne.Controls.ContextButton();
            this.btnAdd4 = new LSOne.Controls.ContextButton();
            this.btnAdd5 = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbCopyFrom
            // 
            this.cmbCopyFrom.AddList = null;
            this.cmbCopyFrom.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbCopyFrom, "cmbCopyFrom");
            this.cmbCopyFrom.MaxLength = 32767;
            this.cmbCopyFrom.Name = "cmbCopyFrom";
            this.cmbCopyFrom.NoChangeAllowed = false;
            this.cmbCopyFrom.OnlyDisplayID = false;
            this.cmbCopyFrom.RemoveList = null;
            this.cmbCopyFrom.RowHeight = ((short)(22));
            this.cmbCopyFrom.SecondaryData = null;
            this.cmbCopyFrom.SelectedData = null;
            this.cmbCopyFrom.SelectedDataID = null;
            this.cmbCopyFrom.SelectionList = null;
            this.cmbCopyFrom.SkipIDColumn = true;
            this.cmbCopyFrom.RequestData += new System.EventHandler(this.cmbCopyFrom_RequestData);
            this.cmbCopyFrom.SelectedDataChanged += new System.EventHandler(this.cmbCopyFrom_SelectedDataChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAdd5);
            this.groupBox1.Controls.Add(this.btnAdd4);
            this.groupBox1.Controls.Add(this.btnAdd3);
            this.groupBox1.Controls.Add(this.btnAdd2);
            this.groupBox1.Controls.Add(this.btnAdd1);
            this.groupBox1.Controls.Add(this.cmbButtonGrid5Menu);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbButtonGrid4Menu);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbButtonGrid3Menu);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbButtonGrid2Menu);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbButtonGrid1Menu);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmbButtonGrid5Menu
            // 
            this.cmbButtonGrid5Menu.AddList = null;
            this.cmbButtonGrid5Menu.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtonGrid5Menu, "cmbButtonGrid5Menu");
            this.cmbButtonGrid5Menu.MaxLength = 32767;
            this.cmbButtonGrid5Menu.Name = "cmbButtonGrid5Menu";
            this.cmbButtonGrid5Menu.NoChangeAllowed = false;
            this.cmbButtonGrid5Menu.OnlyDisplayID = false;
            this.cmbButtonGrid5Menu.RemoveList = null;
            this.cmbButtonGrid5Menu.RowHeight = ((short)(22));
            this.cmbButtonGrid5Menu.SecondaryData = null;
            this.cmbButtonGrid5Menu.SelectedData = null;
            this.cmbButtonGrid5Menu.SelectedDataID = null;
            this.cmbButtonGrid5Menu.SelectionList = null;
            this.cmbButtonGrid5Menu.SkipIDColumn = true;
            this.cmbButtonGrid5Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid5Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbButtonGrid4Menu
            // 
            this.cmbButtonGrid4Menu.AddList = null;
            this.cmbButtonGrid4Menu.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtonGrid4Menu, "cmbButtonGrid4Menu");
            this.cmbButtonGrid4Menu.MaxLength = 32767;
            this.cmbButtonGrid4Menu.Name = "cmbButtonGrid4Menu";
            this.cmbButtonGrid4Menu.NoChangeAllowed = false;
            this.cmbButtonGrid4Menu.OnlyDisplayID = false;
            this.cmbButtonGrid4Menu.RemoveList = null;
            this.cmbButtonGrid4Menu.RowHeight = ((short)(22));
            this.cmbButtonGrid4Menu.SecondaryData = null;
            this.cmbButtonGrid4Menu.SelectedData = null;
            this.cmbButtonGrid4Menu.SelectedDataID = null;
            this.cmbButtonGrid4Menu.SelectionList = null;
            this.cmbButtonGrid4Menu.SkipIDColumn = true;
            this.cmbButtonGrid4Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid4Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmbButtonGrid3Menu
            // 
            this.cmbButtonGrid3Menu.AddList = null;
            this.cmbButtonGrid3Menu.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtonGrid3Menu, "cmbButtonGrid3Menu");
            this.cmbButtonGrid3Menu.MaxLength = 32767;
            this.cmbButtonGrid3Menu.Name = "cmbButtonGrid3Menu";
            this.cmbButtonGrid3Menu.NoChangeAllowed = false;
            this.cmbButtonGrid3Menu.OnlyDisplayID = false;
            this.cmbButtonGrid3Menu.RemoveList = null;
            this.cmbButtonGrid3Menu.RowHeight = ((short)(22));
            this.cmbButtonGrid3Menu.SecondaryData = null;
            this.cmbButtonGrid3Menu.SelectedData = null;
            this.cmbButtonGrid3Menu.SelectedDataID = null;
            this.cmbButtonGrid3Menu.SelectionList = null;
            this.cmbButtonGrid3Menu.SkipIDColumn = true;
            this.cmbButtonGrid3Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid3Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbButtonGrid2Menu
            // 
            this.cmbButtonGrid2Menu.AddList = null;
            this.cmbButtonGrid2Menu.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtonGrid2Menu, "cmbButtonGrid2Menu");
            this.cmbButtonGrid2Menu.MaxLength = 32767;
            this.cmbButtonGrid2Menu.Name = "cmbButtonGrid2Menu";
            this.cmbButtonGrid2Menu.NoChangeAllowed = false;
            this.cmbButtonGrid2Menu.OnlyDisplayID = false;
            this.cmbButtonGrid2Menu.RemoveList = null;
            this.cmbButtonGrid2Menu.RowHeight = ((short)(22));
            this.cmbButtonGrid2Menu.SecondaryData = null;
            this.cmbButtonGrid2Menu.SelectedData = null;
            this.cmbButtonGrid2Menu.SelectedDataID = null;
            this.cmbButtonGrid2Menu.SelectionList = null;
            this.cmbButtonGrid2Menu.SkipIDColumn = true;
            this.cmbButtonGrid2Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid2Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbButtonGrid1Menu
            // 
            this.cmbButtonGrid1Menu.AddList = null;
            this.cmbButtonGrid1Menu.AllowKeyboardSelection = false;
            resources.ApplyResources(this.cmbButtonGrid1Menu, "cmbButtonGrid1Menu");
            this.cmbButtonGrid1Menu.MaxLength = 32767;
            this.cmbButtonGrid1Menu.Name = "cmbButtonGrid1Menu";
            this.cmbButtonGrid1Menu.NoChangeAllowed = false;
            this.cmbButtonGrid1Menu.OnlyDisplayID = false;
            this.cmbButtonGrid1Menu.RemoveList = null;
            this.cmbButtonGrid1Menu.RowHeight = ((short)(22));
            this.cmbButtonGrid1Menu.SecondaryData = null;
            this.cmbButtonGrid1Menu.SelectedData = null;
            this.cmbButtonGrid1Menu.SelectedDataID = null;
            this.cmbButtonGrid1Menu.SelectionList = null;
            this.cmbButtonGrid1Menu.SkipIDColumn = true;
            this.cmbButtonGrid1Menu.RequestData += new System.EventHandler(this.cmbButtonGridMenu_RequestData);
            this.cmbButtonGrid1Menu.RequestClear += new System.EventHandler(this.cmbButtonGridMenu_RequestClear);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnAdd1
            // 
            this.btnAdd1.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd1, "btnAdd1");
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAdd2
            // 
            this.btnAdd2.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd2, "btnAdd2");
            this.btnAdd2.Name = "btnAdd2";
            this.btnAdd2.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAdd3
            // 
            this.btnAdd3.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd3, "btnAdd3");
            this.btnAdd3.Name = "btnAdd3";
            this.btnAdd3.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAdd4
            // 
            this.btnAdd4.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd4, "btnAdd4");
            this.btnAdd4.Name = "btnAdd4";
            this.btnAdd4.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // btnAdd5
            // 
            this.btnAdd5.Context = LSOne.Controls.ButtonType.Add;
            resources.ApplyResources(this.btnAdd5, "btnAdd5");
            this.btnAdd5.Name = "btnAdd5";
            this.btnAdd5.Click += new System.EventHandler(this.btnAddButtonGridMenu_Click);
            // 
            // NewTouchButtonDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cmbCopyFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewTouchButtonDialog";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cmbCopyFrom, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DualDataComboBox cmbCopyFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DualDataComboBox cmbButtonGrid5Menu;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox cmbButtonGrid4Menu;
        private System.Windows.Forms.Label label6;
        private DualDataComboBox cmbButtonGrid3Menu;
        private System.Windows.Forms.Label label5;
        private DualDataComboBox cmbButtonGrid2Menu;
        private System.Windows.Forms.Label label2;
        private DualDataComboBox cmbButtonGrid1Menu;
        private System.Windows.Forms.Label label1;
        private ContextButton btnAdd5;
        private ContextButton btnAdd4;
        private ContextButton btnAdd3;
        private ContextButton btnAdd2;
        private ContextButton btnAdd1;
    }
}