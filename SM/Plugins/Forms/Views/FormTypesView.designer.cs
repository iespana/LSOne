using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Forms.Views
{
    partial class FormTypesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTypesView));
            this.btnsContextButtons = new ContextButtons();
            this.lvFormTypes = new ListView();
            this.colDescription = new Column();
            this.colSystemType = new Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvFormTypes);
            this.pnlBottom.Controls.Add(this.btnsContextButtons);
            // 
            // btnsContextButtons
            // 
            this.btnsContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsContextButtons, "btnsContextButtons");
            this.btnsContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnsContextButtons.EditButtonEnabled = false;
            this.btnsContextButtons.Name = "btnsContextButtons";
            this.btnsContextButtons.RemoveButtonEnabled = false;
            this.btnsContextButtons.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsContextButtons.AddButtonClicked += new System.EventHandler(this.btnAdd_Click);
            this.btnsContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvFormTypes
            // 
            resources.ApplyResources(this.lvFormTypes, "lvFormTypes");
            this.lvFormTypes.BuddyControl = null;
            this.lvFormTypes.Columns.Add(this.colDescription);
            this.lvFormTypes.Columns.Add(this.colSystemType);
            this.lvFormTypes.ContentBackColor = System.Drawing.Color.White;
            this.lvFormTypes.DefaultRowHeight = ((short)(22));
            this.lvFormTypes.DimSelectionWhenDisabled = true;
            this.lvFormTypes.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvFormTypes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvFormTypes.HeaderHeight = ((short)(25));
            this.lvFormTypes.Name = "lvFormTypes";
            this.lvFormTypes.OddRowColor = System.Drawing.Color.White;
            this.lvFormTypes.RowLineColor = System.Drawing.Color.LightGray;
            this.lvFormTypes.SelectionModel = ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvFormTypes.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvFormTypes.SortSetting = "0:1";
            this.lvFormTypes.HeaderClicked += new HeaderDelegate(this.lvForms_HeaderClicked);
            this.lvFormTypes.SelectionChanged += new System.EventHandler(this.lvForms_SelectionChanged);
            this.lvFormTypes.RowDoubleClick += new RowClickDelegate(this.lvFormTypes_RowDoubleClick);
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.Clickable = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.Sizable = true;
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(150));
            // 
            // colSystemType
            // 
            this.colSystemType.AutoSize = true;
            this.colSystemType.Clickable = true;
            this.colSystemType.DefaultStyle = null;
            resources.ApplyResources(this.colSystemType, "colSystemType");
            this.colSystemType.MaximumWidth = ((short)(0));
            this.colSystemType.MinimumWidth = ((short)(10));
            this.colSystemType.Sizable = true;
            this.colSystemType.Tag = null;
            this.colSystemType.Width = ((short)(150));
            // 
            // FormTypesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "FormTypesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsContextButtons;
        private ListView lvFormTypes;
        private Column colDescription;
        private Column colSystemType;

    }
}
