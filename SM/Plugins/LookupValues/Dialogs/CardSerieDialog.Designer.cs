using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    partial class CardSerieDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardSerieDialog));
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ntbFrom = new NumericTextBox();
            this.ntbTo = new NumericTextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.errorProvider1.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.panel2.Font = null;
            this.errorProvider1.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.errorProvider1.SetError(this.btnOK, resources.GetString("btnOK.Error"));
            this.btnOK.Font = null;
            this.errorProvider1.SetIconAlignment(this.btnOK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnOK.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnOK, ((int)(resources.GetObject("btnOK.IconPadding"))));
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider1.SetError(this.btnCancel, resources.GetString("btnCancel.Error"));
            this.btnCancel.Font = null;
            this.errorProvider1.SetIconAlignment(this.btnCancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnCancel.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnCancel, ((int)(resources.GetObject("btnCancel.IconPadding"))));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider1.SetError(this.label2, resources.GetString("label2.Error"));
            this.label2.Font = null;
            this.errorProvider1.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider1.SetError(this.label4, resources.GetString("label4.Error"));
            this.label4.Font = null;
            this.errorProvider1.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // ntbFrom
            // 
            this.ntbFrom.AccessibleDescription = null;
            this.ntbFrom.AccessibleName = null;
            this.ntbFrom.AllowDecimal = false;
            this.ntbFrom.AllowNegative = false;
            resources.ApplyResources(this.ntbFrom, "ntbFrom");
            this.ntbFrom.BackgroundImage = null;
            this.ntbFrom.DecimalLetters = 2;
            this.errorProvider1.SetError(this.ntbFrom, resources.GetString("ntbFrom.Error"));
            this.ntbFrom.Font = null;
            this.errorProvider1.SetIconAlignment(this.ntbFrom, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ntbFrom.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.ntbFrom, ((int)(resources.GetObject("ntbFrom.IconPadding"))));
            this.ntbFrom.MaxValue = 0;
            this.ntbFrom.Name = "ntbFrom";
            this.ntbFrom.Value = 0;
            this.ntbFrom.TextChanged += new System.EventHandler(this.CheckValues);
            // 
            // ntbTo
            // 
            this.ntbTo.AccessibleDescription = null;
            this.ntbTo.AccessibleName = null;
            this.ntbTo.AllowDecimal = false;
            this.ntbTo.AllowNegative = false;
            resources.ApplyResources(this.ntbTo, "ntbTo");
            this.ntbTo.BackgroundImage = null;
            this.ntbTo.DecimalLetters = 2;
            this.errorProvider1.SetError(this.ntbTo, resources.GetString("ntbTo.Error"));
            this.ntbTo.Font = null;
            this.errorProvider1.SetIconAlignment(this.ntbTo, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("ntbTo.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.ntbTo, ((int)(resources.GetObject("ntbTo.IconPadding"))));
            this.ntbTo.MaxValue = 0;
            this.ntbTo.Name = "ntbTo";
            this.ntbTo.Value = 0;
            this.ntbTo.TextChanged += new System.EventHandler(this.CheckValues);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // CardSerieDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = null;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.ntbTo);
            this.Controls.Add(this.ntbFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.Name = "CardSerieDialog";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.ntbFrom, 0);
            this.Controls.SetChildIndex(this.ntbTo, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private NumericTextBox ntbFrom;
        private NumericTextBox ntbTo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}