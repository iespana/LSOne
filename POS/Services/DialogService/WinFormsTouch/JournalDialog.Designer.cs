using LSOne.Controls;
using LSOne.Controls.Columns;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class JournalDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JournalDialog));
            this.lvJournal = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.tlpCustomerAddress = new System.Windows.Forms.TableLayoutPanel();
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.pnlCustomerInfo = new System.Windows.Forms.Panel();
            this.lblAddress = new LSOne.Controls.DoubleLabel();
            this.lblCustomer = new LSOne.Controls.DoubleLabel();
            this.pnlReceipt = new LSOne.Controls.DoubleBufferedPanel();
            this.pnlCustomerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvJournal
            // 
            resources.ApplyResources(this.lvJournal, "lvJournal");
            this.lvJournal.ApplyVisualStyles = false;
            this.lvJournal.BackColor = System.Drawing.Color.White;
            this.lvJournal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvJournal.BuddyControl = null;
            this.lvJournal.Columns.Add(this.column1);
            this.lvJournal.Columns.Add(this.column2);
            this.lvJournal.Columns.Add(this.column3);
            this.lvJournal.Columns.Add(this.column4);
            this.lvJournal.Columns.Add(this.column5);
            this.lvJournal.ContentBackColor = System.Drawing.Color.White;
            this.lvJournal.DefaultRowHeight = ((short)(50));
            this.lvJournal.EvenRowColor = System.Drawing.Color.White;
            this.lvJournal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvJournal.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvJournal.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvJournal.HeaderHeight = ((short)(30));
            this.lvJournal.HideVerticalScrollbarWhenDisabled = true;
            this.lvJournal.Name = "lvJournal";
            this.lvJournal.OddRowColor = System.Drawing.Color.White;
            this.lvJournal.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvJournal.RowLines = true;
            this.lvJournal.SecondarySortColumn = ((short)(-1));
            this.lvJournal.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvJournal.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvJournal.SortSetting = "0:1";
            this.lvJournal.TouchScroll = true;
            this.lvJournal.UseFocusRectangle = false;
            this.lvJournal.VerticalScrollbarValue = 0;
            this.lvJournal.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvJournal.SelectionChanged += new System.EventHandler(this.lvJournal_SelectionChanged);
            this.lvJournal.VerticalScrollValueChanged += new System.EventHandler(this.lvJournal_VerticalScrollValueChanged);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            this.column1.FillRemainingWidth = true;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(130));
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
            this.column2.Width = ((short)(75));
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
            this.column3.Width = ((short)(120));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(85));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(150));
            // 
            // tlpCustomerAddress
            // 
            resources.ApplyResources(this.tlpCustomerAddress, "tlpCustomerAddress");
            this.tlpCustomerAddress.Name = "tlpCustomerAddress";
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ButtonHeight = 50;
            this.panel.Name = "panel";
            this.panel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panel_Click);
            // 
            // pnlCustomerInfo
            // 
            this.pnlCustomerInfo.Controls.Add(this.lblAddress);
            this.pnlCustomerInfo.Controls.Add(this.lblCustomer);
            resources.ApplyResources(this.pnlCustomerInfo, "pnlCustomerInfo");
            this.pnlCustomerInfo.Name = "pnlCustomerInfo";
            this.pnlCustomerInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCustomerInfo_Paint);
            // 
            // lblAddress
            // 
            resources.ApplyResources(this.lblAddress, "lblAddress");
            this.lblAddress.BackColor = System.Drawing.Color.White;
            this.lblAddress.HeaderText = "Address";
            this.lblAddress.Name = "lblAddress";
            // 
            // lblCustomer
            // 
            resources.ApplyResources(this.lblCustomer, "lblCustomer");
            this.lblCustomer.BackColor = System.Drawing.Color.White;
            this.lblCustomer.HeaderText = "Customer";
            this.lblCustomer.Name = "lblCustomer";
            // 
            // pnlReceipt
            // 
            resources.ApplyResources(this.pnlReceipt, "pnlReceipt");
            this.pnlReceipt.Name = "pnlReceipt";
            // 
            // JournalDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlReceipt);
            this.Controls.Add(this.pnlCustomerInfo);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.lvJournal);
            this.Name = "JournalDialog";
            this.pnlCustomerInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvJournal;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private Column column5;
        private System.Windows.Forms.TableLayoutPanel tlpCustomerAddress;
        private TouchScrollButtonPanel panel;
        private System.Windows.Forms.Panel pnlCustomerInfo;
        private DoubleLabel lblAddress;
        private DoubleLabel lblCustomer;
        private DoubleBufferedPanel pnlReceipt;
    }
}