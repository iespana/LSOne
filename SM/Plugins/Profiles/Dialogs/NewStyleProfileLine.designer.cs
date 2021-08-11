using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    partial class NewStyleProfileLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewStyleProfileLine));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.contextEdit = new ContextButton();
            this.contextAdd = new ContextButton();
            this.menuEdit = new ContextButton();
            this.menuAdd = new ContextButton();
            this.styleEdit = new ContextButton();
            this.styleAdd = new ContextButton();
            this.cmbStyle = new DualDataComboBox();
            this.cmbMenu = new DualDataComboBox();
            this.cmbContext = new DualDataComboBox();
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // contextEdit
            // 
            this.contextEdit.Context = ButtonType.Edit;
            resources.ApplyResources(this.contextEdit, "contextEdit");
            this.contextEdit.MaximumSize = new System.Drawing.Size(24, 24);
            this.contextEdit.MinimumSize = new System.Drawing.Size(23, 23);
            this.contextEdit.Name = "contextEdit";
            this.contextEdit.Click += new System.EventHandler(this.contextEdit_Click);
            // 
            // contextAdd
            // 
            this.contextAdd.Context = ButtonType.Add;
            resources.ApplyResources(this.contextAdd, "contextAdd");
            this.contextAdd.MaximumSize = new System.Drawing.Size(24, 24);
            this.contextAdd.MinimumSize = new System.Drawing.Size(23, 23);
            this.contextAdd.Name = "contextAdd";
            this.contextAdd.Click += new System.EventHandler(this.contextAdd_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.Context = ButtonType.Edit;
            resources.ApplyResources(this.menuEdit, "menuEdit");
            this.menuEdit.MaximumSize = new System.Drawing.Size(24, 24);
            this.menuEdit.MinimumSize = new System.Drawing.Size(23, 23);
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // menuAdd
            // 
            this.menuAdd.Context = ButtonType.Add;
            resources.ApplyResources(this.menuAdd, "menuAdd");
            this.menuAdd.MaximumSize = new System.Drawing.Size(24, 24);
            this.menuAdd.MinimumSize = new System.Drawing.Size(23, 23);
            this.menuAdd.Name = "menuAdd";
            this.menuAdd.Click += new System.EventHandler(this.menuAdd_Click);
            // 
            // styleEdit
            // 
            this.styleEdit.Context = ButtonType.Edit;
            resources.ApplyResources(this.styleEdit, "styleEdit");
            this.styleEdit.MaximumSize = new System.Drawing.Size(24, 24);
            this.styleEdit.MinimumSize = new System.Drawing.Size(23, 23);
            this.styleEdit.Name = "styleEdit";
            this.styleEdit.Click += new System.EventHandler(this.styleEdit_Click);
            // 
            // styleAdd
            // 
            this.styleAdd.Context = ButtonType.Add;
            resources.ApplyResources(this.styleAdd, "styleAdd");
            this.styleAdd.MaximumSize = new System.Drawing.Size(24, 24);
            this.styleAdd.MinimumSize = new System.Drawing.Size(23, 23);
            this.styleAdd.Name = "styleAdd";
            this.styleAdd.Click += new System.EventHandler(this.styleAdd_Click);
            // 
            // cmbStyle
            // 
            this.cmbStyle.EnableTextBox = true;
            resources.ApplyResources(this.cmbStyle, "cmbStyle");
            this.cmbStyle.MaxLength = 32767;
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.SelectedData = null;
            this.cmbStyle.SkipIDColumn = true;
            this.cmbStyle.RequestData += new System.EventHandler(this.cmbStyle_RequestData);
            this.cmbStyle.SelectedDataChanged += new System.EventHandler(this.cmbStyle_SelectedDataChanged);
            // 
            // cmbMenu
            // 
            resources.ApplyResources(this.cmbMenu, "cmbMenu");
            this.cmbMenu.MaxLength = 32767;
            this.cmbMenu.Name = "cmbMenu";
            this.cmbMenu.SelectedData = null;
            this.cmbMenu.SkipIDColumn = true;
            this.cmbMenu.RequestData += new System.EventHandler(this.cmbMenu_RequestData);
            this.cmbMenu.SelectedDataChanged += new System.EventHandler(this.cmbMenu_SelectedDataChanged);
            // 
            // cmbContext
            // 
            resources.ApplyResources(this.cmbContext, "cmbContext");
            this.cmbContext.MaxLength = 32767;
            this.cmbContext.Name = "cmbContext";
            this.cmbContext.SelectedData = null;
            this.cmbContext.SkipIDColumn = true;
            this.cmbContext.RequestData += new System.EventHandler(this.cmbContext_RequestData);
            this.cmbContext.SelectedDataChanged += new System.EventHandler(this.cmbContext_SelectedDataChanged);
            // 
            // NewContextStyle
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cmbStyle);
            this.Controls.Add(this.cmbMenu);
            this.Controls.Add(this.cmbContext);
            this.Controls.Add(this.styleAdd);
            this.Controls.Add(this.styleEdit);
            this.Controls.Add(this.menuAdd);
            this.Controls.Add(this.menuEdit);
            this.Controls.Add(this.contextAdd);
            this.Controls.Add(this.contextEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "NewContextStyle";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.contextEdit, 0);
            this.Controls.SetChildIndex(this.contextAdd, 0);
            this.Controls.SetChildIndex(this.menuEdit, 0);
            this.Controls.SetChildIndex(this.menuAdd, 0);
            this.Controls.SetChildIndex(this.styleEdit, 0);
            this.Controls.SetChildIndex(this.styleAdd, 0);
            this.Controls.SetChildIndex(this.cmbContext, 0);
            this.Controls.SetChildIndex(this.cmbMenu, 0);
            this.Controls.SetChildIndex(this.cmbStyle, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButton styleAdd;
        private ContextButton styleEdit;
        private ContextButton menuAdd;
        private ContextButton menuEdit;
        private ContextButton contextAdd;
        private ContextButton contextEdit;
        private System.Windows.Forms.Label label1;
        private DualDataComboBox cmbContext;
        private DualDataComboBox cmbMenu;
        private DualDataComboBox cmbStyle;

        public System.Windows.Forms.PaintEventHandler panel2_Paint { get; set; }
    }
}