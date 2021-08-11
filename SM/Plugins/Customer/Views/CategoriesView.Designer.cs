using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CategoriesView
    {
        #region Component Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoriesView));
            this.lvCategories = new LSOne.Controls.ListView();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnsEditAddRemove);
            this.pnlBottom.Controls.Add(this.lvCategories);
            // 
            // lvCategories
            // 
            resources.ApplyResources(this.lvCategories, "lvCategories");
            this.lvCategories.BuddyControl = null;
            this.lvCategories.Columns.Add(this.colDescription);
            this.lvCategories.ContentBackColor = System.Drawing.Color.White;
            this.lvCategories.DefaultRowHeight = ((short)(22));
            this.lvCategories.DimSelectionWhenDisabled = true;
            this.lvCategories.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvCategories.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCategories.HeaderHeight = ((short)(25));
            this.lvCategories.Name = "lvCategories";
            this.lvCategories.OddRowColor = System.Drawing.Color.White;
            this.lvCategories.RowLineColor = System.Drawing.Color.LightGray;
            this.lvCategories.SecondarySortColumn = ((short)(-1));
            this.lvCategories.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCategories.SortSetting = "0:1";
            this.lvCategories.SelectionChanged += new System.EventHandler(this.lvCategories_SelectionChanged);
            this.lvCategories.DoubleClick += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(200));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(200));
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemove_EditButtonClicked);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            // 
            // CategoriesView
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "CategoriesView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Controls.ListView lvCategories;
        private Controls.Columns.Column colDescription;
        private Controls.ContextButtons btnsEditAddRemove;

    }
}
