namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    partial class ReadDesignDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadDesignDialog));
            this.rbTablesAndFields = new System.Windows.Forms.RadioButton();
            this.rbTables = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbData = new System.Windows.Forms.GroupBox();
            this.gbDesign = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.rbUpdateExisting = new System.Windows.Forms.RadioButton();
            this.rbCreateNew = new System.Windows.Forms.RadioButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            this.gbData.SuspendLayout();
            this.gbDesign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // rbTablesAndFields
            // 
            resources.ApplyResources(this.rbTablesAndFields, "rbTablesAndFields");
            this.rbTablesAndFields.Name = "rbTablesAndFields";
            this.rbTablesAndFields.UseVisualStyleBackColor = true;
            // 
            // rbTables
            // 
            resources.ApplyResources(this.rbTables, "rbTables");
            this.rbTables.Checked = true;
            this.rbTables.Name = "rbTables";
            this.rbTables.TabStop = true;
            this.rbTables.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
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
            // 
            // gbData
            // 
            this.gbData.Controls.Add(this.rbTablesAndFields);
            this.gbData.Controls.Add(this.rbTables);
            resources.ApplyResources(this.gbData, "gbData");
            this.gbData.Name = "gbData";
            this.gbData.TabStop = false;
            // 
            // gbDesign
            // 
            this.gbDesign.Controls.Add(this.label1);
            this.gbDesign.Controls.Add(this.tbDescription);
            this.gbDesign.Controls.Add(this.rbUpdateExisting);
            this.gbDesign.Controls.Add(this.rbCreateNew);
            resources.ApplyResources(this.gbDesign, "gbDesign");
            this.gbDesign.Name = "gbDesign";
            this.gbDesign.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            this.tbDescription.Validating += new System.ComponentModel.CancelEventHandler(this.tbDescription_Validating);
            // 
            // rbUpdateExisting
            // 
            resources.ApplyResources(this.rbUpdateExisting, "rbUpdateExisting");
            this.rbUpdateExisting.Checked = true;
            this.rbUpdateExisting.Name = "rbUpdateExisting";
            this.rbUpdateExisting.TabStop = true;
            this.rbUpdateExisting.UseVisualStyleBackColor = true;
            // 
            // rbCreateNew
            // 
            resources.ApplyResources(this.rbCreateNew, "rbCreateNew");
            this.rbCreateNew.Name = "rbCreateNew";
            this.rbCreateNew.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ReadDesignDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.gbDesign);
            this.Controls.Add(this.gbData);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "ReadDesignDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.gbData, 0);
            this.Controls.SetChildIndex(this.gbDesign, 0);
            this.panel2.ResumeLayout(false);
            this.gbData.ResumeLayout(false);
            this.gbData.PerformLayout();
            this.gbDesign.ResumeLayout(false);
            this.gbDesign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rbTablesAndFields;
        private System.Windows.Forms.RadioButton rbTables;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.GroupBox gbDesign;
        private System.Windows.Forms.RadioButton rbUpdateExisting;
        private System.Windows.Forms.RadioButton rbCreateNew;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}