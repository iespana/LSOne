namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    partial class EditDimensionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDimensionsDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvDimensions = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.lvDimensionAttribute = new LSOne.Controls.ListView();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.lblDimensions = new System.Windows.Forms.Label();
            this.lblDimensionAttribute = new System.Windows.Forms.Label();
            this.btnMoveUpDimension = new LSOne.Controls.ContextButton();
            this.btnMoveDownDimension = new LSOne.Controls.ContextButton();
            this.btnsEditAddRemoveDimension = new LSOne.Controls.ContextButtons();
            this.btnsEditAddRemoveDimensionAttribute = new LSOne.Controls.ContextButtons();
            this.btnMoveUpDimensionAttribute = new LSOne.Controls.ContextButton();
            this.btnMoveDownDimensionAttribute = new LSOne.Controls.ContextButton();
            this.panel2.SuspendLayout();
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
            this.lvDimensions.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvDimensions_RowDoubleClick);
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
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(50));
            // 
            // lvDimensionAttribute
            // 
            resources.ApplyResources(this.lvDimensionAttribute, "lvDimensionAttribute");
            this.lvDimensionAttribute.BuddyControl = null;
            this.lvDimensionAttribute.Columns.Add(this.column2);
            this.lvDimensionAttribute.Columns.Add(this.column3);
            this.lvDimensionAttribute.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensionAttribute.DefaultRowHeight = ((short)(22));
            this.lvDimensionAttribute.DimSelectionWhenDisabled = true;
            this.lvDimensionAttribute.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDimensionAttribute.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDimensionAttribute.HeaderHeight = ((short)(25));
            this.lvDimensionAttribute.Name = "lvDimensionAttribute";
            this.lvDimensionAttribute.OddRowColor = System.Drawing.Color.White;
            this.lvDimensionAttribute.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDimensionAttribute.SecondarySortColumn = ((short)(-1));
            this.lvDimensionAttribute.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensionAttribute.SortSetting = "0:1";
            this.lvDimensionAttribute.SelectionChanged += new System.EventHandler(this.lvDimensionAttribute_SelectionChanged);
            this.lvDimensionAttribute.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvDimensionAttribute_RowDoubleClick);
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
            this.column2.Sizable = false;
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
            this.column3.Sizable = false;
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // lblDimensions
            // 
            resources.ApplyResources(this.lblDimensions, "lblDimensions");
            this.lblDimensions.Name = "lblDimensions";
            // 
            // lblDimensionAttribute
            // 
            resources.ApplyResources(this.lblDimensionAttribute, "lblDimensionAttribute");
            this.lblDimensionAttribute.Name = "lblDimensionAttribute";
            // 
            // btnMoveUpDimension
            // 
            this.btnMoveUpDimension.Context = LSOne.Controls.ButtonType.MoveUp;
            resources.ApplyResources(this.btnMoveUpDimension, "btnMoveUpDimension");
            this.btnMoveUpDimension.Name = "btnMoveUpDimension";
            this.btnMoveUpDimension.Click += new System.EventHandler(this.btnMoveUpDimension_Click);
            // 
            // btnMoveDownDimension
            // 
            this.btnMoveDownDimension.Context = LSOne.Controls.ButtonType.MoveDown;
            resources.ApplyResources(this.btnMoveDownDimension, "btnMoveDownDimension");
            this.btnMoveDownDimension.Name = "btnMoveDownDimension";
            this.btnMoveDownDimension.Click += new System.EventHandler(this.btnMoveDownDimension_Click);
            // 
            // btnsEditAddRemoveDimension
            // 
            this.btnsEditAddRemoveDimension.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveDimension, "btnsEditAddRemoveDimension");
            this.btnsEditAddRemoveDimension.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveDimension.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveDimension.EditButtonEnabled = false;
            this.btnsEditAddRemoveDimension.Name = "btnsEditAddRemoveDimension";
            this.btnsEditAddRemoveDimension.RemoveButtonEnabled = false;
            this.btnsEditAddRemoveDimension.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_EditButtonClicked);
            this.btnsEditAddRemoveDimension.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_AddButtonClicked);
            this.btnsEditAddRemoveDimension.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_RemoveButtonClicked);
            // 
            // btnsEditAddRemoveDimensionAttribute
            // 
            this.btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveDimensionAttribute, "btnsEditAddRemoveDimensionAttribute");
            this.btnsEditAddRemoveDimensionAttribute.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveDimensionAttribute.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = false;
            this.btnsEditAddRemoveDimensionAttribute.Name = "btnsEditAddRemoveDimensionAttribute";
            this.btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = false;
            this.btnsEditAddRemoveDimensionAttribute.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_EditButtonClicked);
            this.btnsEditAddRemoveDimensionAttribute.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_AddButtonClicked);
            this.btnsEditAddRemoveDimensionAttribute.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked);
            // 
            // btnMoveUpDimensionAttribute
            // 
            this.btnMoveUpDimensionAttribute.Context = LSOne.Controls.ButtonType.MoveUp;
            resources.ApplyResources(this.btnMoveUpDimensionAttribute, "btnMoveUpDimensionAttribute");
            this.btnMoveUpDimensionAttribute.Name = "btnMoveUpDimensionAttribute";
            this.btnMoveUpDimensionAttribute.Click += new System.EventHandler(this.btnMoveUpDimensionAttribute_Click);
            // 
            // btnMoveDownDimensionAttribute
            // 
            this.btnMoveDownDimensionAttribute.Context = LSOne.Controls.ButtonType.MoveDown;
            resources.ApplyResources(this.btnMoveDownDimensionAttribute, "btnMoveDownDimensionAttribute");
            this.btnMoveDownDimensionAttribute.Name = "btnMoveDownDimensionAttribute";
            this.btnMoveDownDimensionAttribute.Click += new System.EventHandler(this.btnMoveDownDimensionAttribute_Click);
            // 
            // EditDimensionsDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnMoveDownDimensionAttribute);
            this.Controls.Add(this.btnMoveUpDimensionAttribute);
            this.Controls.Add(this.btnsEditAddRemoveDimensionAttribute);
            this.Controls.Add(this.btnsEditAddRemoveDimension);
            this.Controls.Add(this.btnMoveDownDimension);
            this.Controls.Add(this.btnMoveUpDimension);
            this.Controls.Add(this.lblDimensionAttribute);
            this.Controls.Add(this.lblDimensions);
            this.Controls.Add(this.lvDimensionAttribute);
            this.Controls.Add(this.lvDimensions);
            this.Controls.Add(this.panel2);
            this.HasHelp = true;
            this.Name = "EditDimensionsDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.lvDimensions, 0);
            this.Controls.SetChildIndex(this.lvDimensionAttribute, 0);
            this.Controls.SetChildIndex(this.lblDimensions, 0);
            this.Controls.SetChildIndex(this.lblDimensionAttribute, 0);
            this.Controls.SetChildIndex(this.btnMoveUpDimension, 0);
            this.Controls.SetChildIndex(this.btnMoveDownDimension, 0);
            this.Controls.SetChildIndex(this.btnsEditAddRemoveDimension, 0);
            this.Controls.SetChildIndex(this.btnsEditAddRemoveDimensionAttribute, 0);
            this.Controls.SetChildIndex(this.btnMoveUpDimensionAttribute, 0);
            this.Controls.SetChildIndex(this.btnMoveDownDimensionAttribute, 0);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private Controls.ListView lvDimensions;
        private Controls.ListView lvDimensionAttribute;
        private System.Windows.Forms.Label lblDimensions;
        private System.Windows.Forms.Label lblDimensionAttribute;
        private Controls.ContextButton btnMoveUpDimension;
        private Controls.ContextButton btnMoveDownDimension;
        private Controls.ContextButtons btnsEditAddRemoveDimension;
        private Controls.ContextButtons btnsEditAddRemoveDimensionAttribute;
        private Controls.ContextButton btnMoveUpDimensionAttribute;
        private Controls.ContextButton btnMoveDownDimensionAttribute;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
    }
}