using System.Windows.Forms;
using LSOne.Controls;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.LSCommerce.Views
{
    partial class LicensesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicensesView));
            this.btnsEditRemove = new LSOne.Controls.ContextButtons();
            this.lvLSCommerceLicenses = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvLSCommerceLicenses);
            this.pnlBottom.Controls.Add(this.btnsEditRemove);
            // 
            // btnsEditRemove
            // 
            this.btnsEditRemove.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsEditRemove, "btnsEditRemove");
            this.btnsEditRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditRemove.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.btnsEditRemove.EditButtonEnabled = false;
            this.btnsEditRemove.Name = "btnsEditRemove";
            this.btnsEditRemove.RemoveButtonEnabled = false;
            this.btnsEditRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // lvLSCommerceLicenses
            // 
            resources.ApplyResources(this.lvLSCommerceLicenses, "lvLSCommerceLicenses");
            this.lvLSCommerceLicenses.BuddyControl = null;
            this.lvLSCommerceLicenses.Columns.Add(this.column1);
            this.lvLSCommerceLicenses.Columns.Add(this.column2);
            this.lvLSCommerceLicenses.Columns.Add(this.column3);
            this.lvLSCommerceLicenses.Columns.Add(this.column4);
            this.lvLSCommerceLicenses.Columns.Add(this.column5);
            this.lvLSCommerceLicenses.ContentBackColor = System.Drawing.Color.White;
            this.lvLSCommerceLicenses.DefaultRowHeight = ((short)(18));
            this.lvLSCommerceLicenses.DimSelectionWhenDisabled = true;
            this.lvLSCommerceLicenses.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvLSCommerceLicenses.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLSCommerceLicenses.HeaderHeight = ((short)(25));
            this.lvLSCommerceLicenses.Name = "lvLSCommerceLicenses";
            this.lvLSCommerceLicenses.OddRowColor = System.Drawing.Color.White;
            this.lvLSCommerceLicenses.RowLineColor = System.Drawing.Color.LightGray;
            this.lvLSCommerceLicenses.SecondarySortColumn = ((short)(-1));
            this.lvLSCommerceLicenses.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvLSCommerceLicenses.SortSetting = "0:1";
            this.lvLSCommerceLicenses.SelectionChanged += new System.EventHandler(this.lvLSCommerceLicenses_SelectionChanged);
            this.lvLSCommerceLicenses.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvLSCommerceLicenses_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.InternalSort = true;
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
            this.column2.InternalSort = true;
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
            this.column3.InternalSort = true;
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.InternalSort = true;
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.InternalSort = true;
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // LicensesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LicensesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnsEditRemove;
        private ListView lvLSCommerceLicenses;
        private Controls.Columns.Column column1;
        private Controls.Columns.Column column2;
        private Controls.Columns.Column column3;
        private Controls.Columns.Column column4;
        private Controls.Columns.Column column5;
    }
}
