using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    partial class DatabaseMapView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseMapView));
			this.tbDbFrom = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupPanel2 = new LSOne.Controls.GroupPanel();
			this.lvFieldMap = new LSOne.Controls.ListView();
			this.colFrom = new LSOne.Controls.Columns.Column();
			this.colTo = new LSOne.Controls.Columns.Column();
			this.colConMethod = new LSOne.Controls.Columns.Column();
			this.colConValue = new LSOne.Controls.Columns.Column();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
			this.label1 = new System.Windows.Forms.Label();
			this.tbTableFrom = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbDbTo = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbTableTo = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.pnlBottom.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.groupBox2);
			this.pnlBottom.Controls.Add(this.groupBox1);
			this.pnlBottom.Controls.Add(this.groupPanel2);
			resources.ApplyResources(this.pnlBottom, "pnlBottom");
			// 
			// tbDbFrom
			// 
			resources.ApplyResources(this.tbDbFrom, "tbDbFrom");
			this.tbDbFrom.Name = "tbDbFrom";
			this.tbDbFrom.ReadOnly = true;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// groupPanel2
			// 
			resources.ApplyResources(this.groupPanel2, "groupPanel2");
			this.groupPanel2.Controls.Add(this.lvFieldMap);
			this.groupPanel2.Controls.Add(this.panel3);
			this.groupPanel2.Name = "groupPanel2";
			// 
			// lvFieldMap
			// 
			resources.ApplyResources(this.lvFieldMap, "lvFieldMap");
			this.lvFieldMap.BackColor = System.Drawing.Color.Transparent;
			this.lvFieldMap.BorderColor = System.Drawing.Color.DarkGray;
			this.lvFieldMap.BuddyControl = null;
			this.lvFieldMap.Columns.Add(this.colFrom);
			this.lvFieldMap.Columns.Add(this.colTo);
			this.lvFieldMap.Columns.Add(this.colConMethod);
			this.lvFieldMap.Columns.Add(this.colConValue);
			this.lvFieldMap.ContentBackColor = System.Drawing.Color.White;
			this.lvFieldMap.DefaultRowHeight = ((short)(22));
			this.lvFieldMap.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
			this.lvFieldMap.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lvFieldMap.HeaderBackColor = System.Drawing.Color.White;
			this.lvFieldMap.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lvFieldMap.HeaderHeight = ((short)(25));
			this.lvFieldMap.Name = "lvFieldMap";
			this.lvFieldMap.OddRowColor = System.Drawing.Color.White;
			this.lvFieldMap.RowLineColor = System.Drawing.Color.LightGray;
			this.lvFieldMap.SecondarySortColumn = ((short)(-1));
			this.lvFieldMap.SelectedRowColor = System.Drawing.Color.LightGray;
			this.lvFieldMap.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
			this.lvFieldMap.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
			this.lvFieldMap.SortSetting = "0:1";
			this.lvFieldMap.VerticalScrollbarValue = 0;
			this.lvFieldMap.VerticalScrollbarYOffset = 0;
			this.lvFieldMap.SelectionChanged += new System.EventHandler(this.lvFieldMap_SelectionChanged);
			this.lvFieldMap.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvFieldMap_RowDoubleClick);
			// 
			// colFrom
			// 
			this.colFrom.AutoSize = true;
			this.colFrom.DefaultStyle = null;
			resources.ApplyResources(this.colFrom, "colFrom");
			this.colFrom.InternalSort = true;
			this.colFrom.MaximumWidth = ((short)(0));
			this.colFrom.MinimumWidth = ((short)(10));
			this.colFrom.SecondarySortColumn = ((short)(-1));
			this.colFrom.Tag = null;
			this.colFrom.Width = ((short)(50));
			// 
			// colTo
			// 
			this.colTo.AutoSize = true;
			this.colTo.DefaultStyle = null;
			resources.ApplyResources(this.colTo, "colTo");
			this.colTo.InternalSort = true;
			this.colTo.MaximumWidth = ((short)(0));
			this.colTo.MinimumWidth = ((short)(10));
			this.colTo.SecondarySortColumn = ((short)(-1));
			this.colTo.Tag = null;
			this.colTo.Width = ((short)(50));
			// 
			// colConMethod
			// 
			this.colConMethod.AutoSize = true;
			this.colConMethod.Clickable = false;
			this.colConMethod.DefaultStyle = null;
			resources.ApplyResources(this.colConMethod, "colConMethod");
			this.colConMethod.MaximumWidth = ((short)(0));
			this.colConMethod.MinimumWidth = ((short)(10));
			this.colConMethod.SecondarySortColumn = ((short)(-1));
			this.colConMethod.Sizable = false;
			this.colConMethod.Tag = null;
			this.colConMethod.Width = ((short)(50));
			// 
			// colConValue
			// 
			this.colConValue.AutoSize = true;
			this.colConValue.Clickable = false;
			this.colConValue.DefaultStyle = null;
			resources.ApplyResources(this.colConValue, "colConValue");
			this.colConValue.MaximumWidth = ((short)(0));
			this.colConValue.MinimumWidth = ((short)(10));
			this.colConValue.SecondarySortColumn = ((short)(-1));
			this.colConValue.Sizable = false;
			this.colConValue.Tag = null;
			this.colConValue.Width = ((short)(50));
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.Transparent;
			this.panel3.Controls.Add(this.btnsEditAddRemove);
			resources.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			// 
			// btnsEditAddRemove
			// 
			this.btnsEditAddRemove.AddButtonEnabled = true;
			resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
			this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
			this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
			this.btnsEditAddRemove.EditButtonEnabled = false;
			this.btnsEditAddRemove.Name = "btnsEditAddRemove";
			this.btnsEditAddRemove.RemoveButtonEnabled = false;
			this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEditFieldMap_Click);
			this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnAddFieldMap_Click);
			this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemoveFieldMap_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tbTableFrom
			// 
			resources.ApplyResources(this.tbTableFrom, "tbTableFrom");
			this.tbTableFrom.Name = "tbTableFrom";
			this.tbTableFrom.ReadOnly = true;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// tbDbTo
			// 
			resources.ApplyResources(this.tbDbTo, "tbDbTo");
			this.tbDbTo.Name = "tbDbTo";
			this.tbDbTo.ReadOnly = true;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// tbTableTo
			// 
			resources.ApplyResources(this.tbTableTo, "tbTableTo");
			this.tbTableTo.Name = "tbTableTo";
			this.tbTableTo.ReadOnly = true;
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.tbDbFrom);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.tbTableFrom);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.tbDbTo);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.tbTableTo);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// DatabaseMapView
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.GrayHeaderHeight = 170;
			this.Name = "DatabaseMapView";
			this.pnlBottom.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbDbFrom;
        private System.Windows.Forms.Label label3;
        private GroupPanel groupPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbTableTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDbTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTableFrom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private ListView lvFieldMap;
        private LSOne.Controls.Columns.Column colFrom;
        private LSOne.Controls.Columns.Column colTo;
        private LSOne.Controls.Columns.Column colConMethod;
        private LSOne.Controls.Columns.Column colConValue;
        private ContextButtons btnsEditAddRemove;
    }
}
