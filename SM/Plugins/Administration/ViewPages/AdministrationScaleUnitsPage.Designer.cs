using LSOne.Controls;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    partial class AdministrationScaleUnitsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationScaleUnitsPage));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ddcbOunce = new LSOne.Controls.DualDataComboBox();
            this.ddcbKiloGram = new LSOne.Controls.DualDataComboBox();
            this.ddcbGram = new LSOne.Controls.DualDataComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ddcbPound = new LSOne.Controls.DualDataComboBox();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.ddcbOunce, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ddcbKiloGram, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ddcbGram, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ddcbPound, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // ddcbOunce
            // 
            this.ddcbOunce.AddList = null;
            this.ddcbOunce.AllowKeyboardSelection = false;
            resources.ApplyResources(this.ddcbOunce, "ddcbOunce");
            this.ddcbOunce.MaxLength = 32767;
            this.ddcbOunce.Name = "ddcbOunce";
            this.ddcbOunce.NoChangeAllowed = false;
            this.ddcbOunce.OnlyDisplayID = false;
            this.ddcbOunce.RemoveList = null;
            this.ddcbOunce.RowHeight = ((short)(22));
            this.ddcbOunce.SecondaryData = null;
            this.ddcbOunce.SelectedData = null;
            this.ddcbOunce.SelectedDataID = null;
            this.ddcbOunce.SelectionList = null;
            this.ddcbOunce.SkipIDColumn = false;
            this.ddcbOunce.RequestData += new System.EventHandler(this.ddcb_RequestData);
            this.ddcbOunce.RequestClear += new System.EventHandler(this.ddcb_RequestClear);
            // 
            // ddcbKiloGram
            // 
            this.ddcbKiloGram.AddList = null;
            this.ddcbKiloGram.AllowKeyboardSelection = false;
            resources.ApplyResources(this.ddcbKiloGram, "ddcbKiloGram");
            this.ddcbKiloGram.MaxLength = 32767;
            this.ddcbKiloGram.Name = "ddcbKiloGram";
            this.ddcbKiloGram.NoChangeAllowed = false;
            this.ddcbKiloGram.OnlyDisplayID = false;
            this.ddcbKiloGram.RemoveList = null;
            this.ddcbKiloGram.RowHeight = ((short)(22));
            this.ddcbKiloGram.SecondaryData = null;
            this.ddcbKiloGram.SelectedData = null;
            this.ddcbKiloGram.SelectedDataID = null;
            this.ddcbKiloGram.SelectionList = null;
            this.ddcbKiloGram.SkipIDColumn = false;
            this.ddcbKiloGram.RequestData += new System.EventHandler(this.ddcb_RequestData);
            this.ddcbKiloGram.RequestClear += new System.EventHandler(this.ddcb_RequestClear);
            // 
            // ddcbGram
            // 
            this.ddcbGram.AddList = null;
            this.ddcbGram.AllowKeyboardSelection = false;
            resources.ApplyResources(this.ddcbGram, "ddcbGram");
            this.ddcbGram.MaxLength = 32767;
            this.ddcbGram.Name = "ddcbGram";
            this.ddcbGram.NoChangeAllowed = false;
            this.ddcbGram.OnlyDisplayID = false;
            this.ddcbGram.RemoveList = null;
            this.ddcbGram.RowHeight = ((short)(22));
            this.ddcbGram.SecondaryData = null;
            this.ddcbGram.SelectedData = null;
            this.ddcbGram.SelectedDataID = null;
            this.ddcbGram.SelectionList = null;
            this.ddcbGram.SkipIDColumn = false;
            this.ddcbGram.RequestData += new System.EventHandler(this.ddcb_RequestData);
            this.ddcbGram.RequestClear += new System.EventHandler(this.ddcb_RequestClear);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // ddcbPound
            // 
            this.ddcbPound.AddList = null;
            this.ddcbPound.AllowKeyboardSelection = false;
            resources.ApplyResources(this.ddcbPound, "ddcbPound");
            this.ddcbPound.MaxLength = 32767;
            this.ddcbPound.Name = "ddcbPound";
            this.ddcbPound.NoChangeAllowed = false;
            this.ddcbPound.OnlyDisplayID = false;
            this.ddcbPound.RemoveList = null;
            this.ddcbPound.RowHeight = ((short)(22));
            this.ddcbPound.SecondaryData = null;
            this.ddcbPound.SelectedData = null;
            this.ddcbPound.SelectedDataID = null;
            this.ddcbPound.SelectionList = null;
            this.ddcbPound.SkipIDColumn = false;
            this.ddcbPound.RequestData += new System.EventHandler(this.ddcb_RequestData);
            this.ddcbPound.RequestClear += new System.EventHandler(this.ddcb_RequestClear);
            // 
            // AdministrationScaleUnitsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox2);
            this.Name = "AdministrationScaleUnitsPage";
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DualDataComboBox ddcbOunce;
        private DualDataComboBox ddcbKiloGram;
        private DualDataComboBox ddcbGram;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private DualDataComboBox ddcbPound;


    }
}
