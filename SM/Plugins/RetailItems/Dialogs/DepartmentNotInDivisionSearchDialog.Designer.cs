using LSOne.Controls;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    partial class DepartmentNotInDivisionSearchDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DepartmentNotInDivisionSearchDialog));
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblToManyError = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lvDepartments = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.Controls.Add(this.lblToManyError);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Name = "panel2";
            // 
            // lblToManyError
            // 
            resources.ApplyResources(this.lblToManyError, "lblToManyError");
            this.lblToManyError.Name = "lblToManyError";
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
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
            // lvDepartments
            // 
            resources.ApplyResources(this.lvDepartments, "lvDepartments");
            this.lvDepartments.BuddyControl = null;
            this.lvDepartments.Columns.Add(this.column1);
            this.lvDepartments.Columns.Add(this.column2);
            this.lvDepartments.Columns.Add(this.column3);
            this.lvDepartments.ContentBackColor = System.Drawing.Color.White;
            this.lvDepartments.DefaultRowHeight = ((short)(22));
            this.lvDepartments.DimSelectionWhenDisabled = true;
            this.lvDepartments.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDepartments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDepartments.HeaderHeight = ((short)(25));
            this.lvDepartments.HorizontalScrollbar = true;
            this.lvDepartments.Name = "lvDepartments";
            this.lvDepartments.OddRowColor = System.Drawing.Color.White;
            this.lvDepartments.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDepartments.SecondarySortColumn = ((short)(-1));
            this.lvDepartments.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvDepartments.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDepartments.SortSetting = "-1:1";
            this.lvDepartments.SelectionChanged += new System.EventHandler(this.lvDepartments_SelectionChanged);
            this.lvDepartments.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvDepartments_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
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
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // DepartmentNotInDivisionSearchDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lvDepartments);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbSearch);
            this.HasHelp = true;
            this.Name = "DepartmentNotInDivisionSearchDialog";
            this.Controls.SetChildIndex(this.tbSearch, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvDepartments, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblToManyError;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ListView lvDepartments;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
    }
}