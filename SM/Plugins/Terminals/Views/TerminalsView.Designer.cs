using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.Terminals.Views
{
    partial class TerminalsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalsView));
            this.btnContextButtons = new ContextButtons();
            this.lvTerminals = new ListView();
            this.column3 = new Column();
            this.column1 = new Column();
            this.column2 = new Column();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lvTerminals);
            this.pnlBottom.Controls.Add(this.btnContextButtons);
            // 
            // btnContextButtons
            // 
            this.btnContextButtons.AddButtonEnabled = true;
            resources.ApplyResources(this.btnContextButtons, "btnContextButtons");
            this.btnContextButtons.BackColor = System.Drawing.Color.Transparent;
            this.btnContextButtons.Context = ButtonTypes.EditAddRemove;
            this.btnContextButtons.EditButtonEnabled = false;
            this.btnContextButtons.Name = "btnContextButtons";
            this.btnContextButtons.RemoveButtonEnabled = false;
            this.btnContextButtons.EditButtonClicked += new System.EventHandler(this.btnContextButtons_EditButtonClicked);
            this.btnContextButtons.AddButtonClicked += new System.EventHandler(this.btnContextButtons_AddButtonClicked);
            this.btnContextButtons.RemoveButtonClicked += new System.EventHandler(this.btnContextButtons_RemoveButtonClicked);
            // 
            // lvTerminals
            // 
            resources.ApplyResources(this.lvTerminals, "lvTerminals");
            this.lvTerminals.BuddyControl = null;
            this.lvTerminals.Columns.Add(this.column3);
            this.lvTerminals.Columns.Add(this.column1);
            this.lvTerminals.Columns.Add(this.column2);
            this.lvTerminals.ContentBackColor = System.Drawing.Color.White;
            this.lvTerminals.DefaultRowHeight = ((short)(22));
            this.lvTerminals.DimSelectionWhenDisabled = true;
            this.lvTerminals.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvTerminals.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTerminals.HeaderHeight = ((short)(25));
            this.lvTerminals.Name = "lvTerminals";
            this.lvTerminals.OddRowColor = System.Drawing.Color.White;
            this.lvTerminals.RowLineColor = System.Drawing.Color.LightGray;
            this.lvTerminals.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;
            this.lvTerminals.HeaderClicked += new HeaderDelegate(this.lvTerminals_HeaderClicked);
            this.lvTerminals.SelectionChanged += new System.EventHandler(this.lvTerminals_SelectionChanged);
            this.lvTerminals.RowDoubleClick += new RowClickDelegate(this.lvTerminals_RowDoubleClick);
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = true;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(10));
            this.column3.Sizable = true;
            this.column3.Tag = null;
            this.column3.Width = ((short)(50));
            // 
            // column1
            // 
            this.column1.AutoSize = false;
            this.column1.Clickable = true;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(200));
            this.column1.Sizable = true;
            this.column1.Tag = null;
            this.column1.Width = ((short)(200));
            // 
            // column2
            // 
            this.column2.AutoSize = false;
            this.column2.Clickable = true;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(250));
            this.column2.Sizable = true;
            this.column2.Tag = null;
            this.column2.Width = ((short)(250));
            // 
            // TerminalsView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TerminalsView";
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextButtons btnContextButtons;
        private ListView lvTerminals;
        private Column column1;
        private Column column2;
        private Column column3;
    }
}
