using LSOne.Controls;

namespace LSOne.Services.DialogPanels
{
    partial class FormPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PassportPanel));
            this.tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.tbPassportNumber = new System.Windows.Forms.TextBox();
            this.lblPassportNumber = new System.Windows.Forms.Label();
            this.lblPassportIssuedBy = new System.Windows.Forms.Label();
            this.tbPassportIssuedBy = new System.Windows.Forms.TextBox();
            this.ddfcbIssueDate = new LSOne.Controls.DropDownFormComboBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tdbHeader
            // 
            this.tdbHeader.BackColor = System.Drawing.Color.SteelBlue;
            resources.ApplyResources(this.tdbHeader, "tdbHeader");
            this.tdbHeader.Icon = ((System.Drawing.Image)(resources.GetObject("tdbHeader.Icon")));
            this.tdbHeader.Name = "tdbHeader";
            this.tdbHeader.TabStop = false;
            // 
            // tbPassportNumber
            // 
            resources.ApplyResources(this.tbPassportNumber, "tbPassportNumber");
            this.tbPassportNumber.Name = "tbPassportNumber";
            // 
            // lblPassportNumber
            // 
            resources.ApplyResources(this.lblPassportNumber, "lblPassportNumber");
            this.lblPassportNumber.Name = "lblPassportNumber";
            // 
            // lblPassportIssuedBy
            // 
            resources.ApplyResources(this.lblPassportIssuedBy, "lblPassportIssuedBy");
            this.lblPassportIssuedBy.Name = "lblPassportIssuedBy";
            // 
            // tbPassportIssuedBy
            // 
            resources.ApplyResources(this.tbPassportIssuedBy, "tbPassportIssuedBy");
            this.tbPassportIssuedBy.Name = "tbPassportIssuedBy";
            // 
            // ddfcbIssueDate
            // 
            this.ddfcbIssueDate.AddList = null;
            resources.ApplyResources(this.ddfcbIssueDate, "ddfcbIssueDate");
            this.ddfcbIssueDate.MaxLength = 32767;
            this.ddfcbIssueDate.Name = "ddfcbIssueDate";
            this.ddfcbIssueDate.RemoveList = null;
            this.ddfcbIssueDate.RowHeight = ((short)(22));
            this.ddfcbIssueDate.SecondaryData = null;
            this.ddfcbIssueDate.SelectedData = null;
            this.ddfcbIssueDate.SelectionList = null;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // PassportPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblPassportNumber);
            this.Controls.Add(this.ddfcbIssueDate);
            this.Controls.Add(this.lblPassportIssuedBy);
            this.Controls.Add(this.tbPassportIssuedBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPassportNumber);
            this.Controls.Add(this.tdbHeader);
            this.DoubleBuffered = true;
            this.Name = "PassportPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TouchDialogBanner tdbHeader;
        private System.Windows.Forms.TextBox tbPassportNumber;
        private System.Windows.Forms.Label lblPassportNumber;
        private System.Windows.Forms.Label lblPassportIssuedBy;
        private System.Windows.Forms.TextBox tbPassportIssuedBy;
        private DropDownFormComboBox ddfcbIssueDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
    }
}
