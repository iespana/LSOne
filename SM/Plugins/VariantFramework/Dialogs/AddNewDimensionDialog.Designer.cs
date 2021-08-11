namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    partial class AddNewDimensionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewDimensionDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.rbCreateNew = new System.Windows.Forms.RadioButton();
            this.rbSelectFromTemplate = new System.Windows.Forms.RadioButton();
            this.lblDimensionAttributes = new System.Windows.Forms.Label();
            this.lvDimensionAttributes = new LSOne.Controls.ListView();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.lvDimensions = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
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
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // rbCreateNew
            // 
            resources.ApplyResources(this.rbCreateNew, "rbCreateNew");
            this.rbCreateNew.Checked = true;
            this.rbCreateNew.Name = "rbCreateNew";
            this.rbCreateNew.TabStop = true;
            this.rbCreateNew.UseVisualStyleBackColor = true;
            this.rbCreateNew.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // rbSelectFromTemplate
            // 
            resources.ApplyResources(this.rbSelectFromTemplate, "rbSelectFromTemplate");
            this.rbSelectFromTemplate.Name = "rbSelectFromTemplate";
            this.rbSelectFromTemplate.UseVisualStyleBackColor = true;
            this.rbSelectFromTemplate.CheckedChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // lblDimensionAttributes
            // 
            resources.ApplyResources(this.lblDimensionAttributes, "lblDimensionAttributes");
            this.lblDimensionAttributes.Name = "lblDimensionAttributes";
            // 
            // lvDimensionAttributes
            // 
            resources.ApplyResources(this.lvDimensionAttributes, "lvDimensionAttributes");
            this.lvDimensionAttributes.BuddyControl = null;
            this.lvDimensionAttributes.Columns.Add(this.column2);
            this.lvDimensionAttributes.Columns.Add(this.column3);
            this.lvDimensionAttributes.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensionAttributes.DefaultRowHeight = ((short)(22));
            this.lvDimensionAttributes.DimSelectionWhenDisabled = true;
            this.lvDimensionAttributes.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDimensionAttributes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDimensionAttributes.HeaderHeight = ((short)(25));
            this.lvDimensionAttributes.Name = "lvDimensionAttributes";
            this.lvDimensionAttributes.OddRowColor = System.Drawing.Color.White;
            this.lvDimensionAttributes.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDimensionAttributes.SecondarySortColumn = ((short)(-1));
            this.lvDimensionAttributes.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensionAttributes.SortSetting = "0:1";
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
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
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // lvDimensions
            // 
            resources.ApplyResources(this.lvDimensions, "lvDimensions");
            this.lvDimensions.BuddyControl = null;
            this.lvDimensions.Columns.Add(this.column1);
            this.lvDimensions.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensions.DefaultRowHeight = ((short)(22));
            this.lvDimensions.DimSelectionWhenDisabled = true;
            this.lvDimensions.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDimensions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDimensions.HeaderHeight = ((short)(25));
            this.lvDimensions.Name = "lvDimensions";
            this.lvDimensions.OddRowColor = System.Drawing.Color.White;
            this.lvDimensions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDimensions.SecondarySortColumn = ((short)(-1));
            this.lvDimensions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensions.SortSetting = "0:1";
            this.lvDimensions.SelectionChanged += new System.EventHandler(this.lvDimensions_SelectionChanged);
            this.lvDimensions.Load += new System.EventHandler(this.lvDimensions_Load);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // AddNewDimensionDialog
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.lblDimensionAttributes);
            this.Controls.Add(this.rbSelectFromTemplate);
            this.Controls.Add(this.rbCreateNew);
            this.Controls.Add(this.lvDimensionAttributes);
            this.Controls.Add(this.lvDimensions);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.HelpButton = true;
            this.Name = "AddNewDimensionDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lblDescription, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.lvDimensions, 0);
            this.Controls.SetChildIndex(this.lvDimensionAttributes, 0);
            this.Controls.SetChildIndex(this.rbCreateNew, 0);
            this.Controls.SetChildIndex(this.rbSelectFromTemplate, 0);
            this.Controls.SetChildIndex(this.lblDimensionAttributes, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblDimensionAttributes;
        private System.Windows.Forms.RadioButton rbSelectFromTemplate;
        private System.Windows.Forms.RadioButton rbCreateNew;
        private Controls.ListView lvDimensionAttributes;
        private Controls.ListView lvDimensions;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
    }
}