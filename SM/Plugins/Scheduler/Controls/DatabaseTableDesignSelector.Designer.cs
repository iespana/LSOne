namespace LSOne.ViewPlugins.Scheduler.Controls
{
    partial class DatabaseTableDesignSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseTableDesignSelector));
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.cmbDatabaseDesigns = new System.Windows.Forms.ComboBox();
            this.cmbTableDesigns = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.cmbDatabaseDesigns);
            this.gbMain.Controls.Add(this.cmbTableDesigns);
            this.gbMain.Controls.Add(this.label1);
            this.gbMain.Controls.Add(this.label4);
            resources.ApplyResources(this.gbMain, "gbMain");
            this.gbMain.Name = "gbMain";
            this.gbMain.TabStop = false;
            // 
            // cmbDatabaseDesigns
            // 
            resources.ApplyResources(this.cmbDatabaseDesigns, "cmbDatabaseDesigns");
            this.cmbDatabaseDesigns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseDesigns.FormattingEnabled = true;
            this.cmbDatabaseDesigns.Name = "cmbDatabaseDesigns";
            this.cmbDatabaseDesigns.SelectedValueChanged += new System.EventHandler(this.cmbDatabaseDesigns_SelectedValueChanged);
            // 
            // cmbTableDesigns
            // 
            resources.ApplyResources(this.cmbTableDesigns, "cmbTableDesigns");
            this.cmbTableDesigns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableDesigns.FormattingEnabled = true;
            this.cmbTableDesigns.Name = "cmbTableDesigns";
            this.cmbTableDesigns.DropDown += new System.EventHandler(this.cmbTableDesigns_DropDown);
            this.cmbTableDesigns.SelectedIndexChanged += new System.EventHandler(this.cmbTableDesigns_SelectedIndexChanged);
            this.cmbTableDesigns.Enter += new System.EventHandler(this.cmbTableDesigns_Enter);
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
            // DatabaseTableDesignSelector
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMain);
            this.Name = "DatabaseTableDesignSelector";
            this.gbMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbTableDesigns;
        private System.Windows.Forms.ComboBox cmbDatabaseDesigns;
    }
}
