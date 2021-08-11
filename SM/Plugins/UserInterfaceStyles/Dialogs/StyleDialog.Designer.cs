using LSOne.Controls;
using LSOne.ViewPlugins.UserInterfaceStyles.Controls;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Dialogs
{
    partial class StyleDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleDialog));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupPanel3 = new LSOne.Controls.GroupPanel();
            this.contextStyleControl = new LSOne.ViewPlugins.UserInterfaceStyles.Controls.ContextStyleControl();
            this.styleControl = new LSOne.ViewPlugins.UserInterfaceStyles.Controls.StyleControl();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSystemStyle = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupPanel3
            // 
            resources.ApplyResources(this.groupPanel3, "groupPanel3");
            this.groupPanel3.Controls.Add(this.contextStyleControl);
            this.groupPanel3.Controls.Add(this.styleControl);
            this.groupPanel3.Name = "groupPanel3";
            // 
            // contextStyleControl
            // 
            resources.ApplyResources(this.contextStyleControl, "contextStyleControl");
            this.contextStyleControl.BackColor = System.Drawing.Color.Transparent;
            this.contextStyleControl.Name = "contextStyleControl";
            this.contextStyleControl.ValueChanged += new System.EventHandler(this.contextStyleControl_ValueChanged);
            // 
            // styleControl
            // 
            resources.ApplyResources(this.styleControl, "styleControl");
            this.styleControl.BackColor = System.Drawing.Color.Transparent;
            this.styleControl.IsNew = false;
            this.styleControl.Name = "styleControl";
            this.styleControl.ValueChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // tbDescription
            // 
            resources.ApplyResources(this.tbDescription, "tbDescription");
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.TextChanged += new System.EventHandler(this.CheckEnabled);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(41)))), ((int)(((byte)(102)))));
            this.label13.Name = "label13";
            // 
            // mainLayout
            // 
            resources.ApplyResources(this.mainLayout, "mainLayout");
            this.mainLayout.Name = "mainLayout";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            // chkSystemStyle
            // 
            resources.ApplyResources(this.chkSystemStyle, "chkSystemStyle");
            this.chkSystemStyle.BackColor = System.Drawing.Color.Transparent;
            this.chkSystemStyle.Name = "chkSystemStyle";
            this.chkSystemStyle.UseVisualStyleBackColor = false;
            // 
            // StyleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.ControlBox = true;
            this.Controls.Add(this.chkSystemStyle);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HasHelp = true;
            this.Name = "StyleDialog";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.groupPanel3, 0);
            this.Controls.SetChildIndex(this.mainLayout, 0);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.chkSystemStyle, 0);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private GroupPanel groupPanel3;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        //private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel2;
        private Controls.StyleControl styleControl;
        private Controls.ContextStyleControl contextStyleControl;
        private System.Windows.Forms.CheckBox chkSystemStyle;
    }
}