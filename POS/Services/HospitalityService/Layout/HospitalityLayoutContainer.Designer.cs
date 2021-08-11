using LSOne.POS.Processes.WinControls;
using System.Windows.Forms;

namespace LSOne.Services.Layout
{
    partial class HospitalityLayoutContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HospitalityLayoutContainer));
            this.tlpForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tlpOperations = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHospitalityTypes = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSelectedTableValue = new System.Windows.Forms.Label();
            this.lblSelectedTable = new System.Windows.Forms.Label();
            this.touchDialogBanner = new LSOne.Controls.TouchDialogBanner();
            this.touchErrorProviderLeft = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.touchErrorProviderRight = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.tlpForm.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpForm
            // 
            resources.ApplyResources(this.tlpForm, "tlpForm");
            this.tlpForm.Controls.Add(this.touchErrorProviderLeft, 0, 3);
            this.tlpForm.Controls.Add(this.tlpTableLayout, 0, 2);
            this.tlpForm.Controls.Add(this.tlpOperations, 2, 2);
            this.tlpForm.Controls.Add(this.tlpHospitalityTypes, 0, 1);
            this.tlpForm.Controls.Add(this.touchErrorProviderRight, 0, 3);
            this.tlpForm.Controls.Add(this.panelHeader, 0, 0);
            this.tlpForm.Name = "tlpForm";
            this.tlpForm.Tag = "1";
            // 
            // tlpTableLayout
            // 
            this.tlpTableLayout.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.tlpTableLayout, "tlpTableLayout");
            this.tlpForm.SetColumnSpan(this.tlpTableLayout, 2);
            this.tlpTableLayout.Name = "tlpTableLayout";
            this.tlpTableLayout.VisibleChanged += new System.EventHandler(this.tlpTableLayout_VisibleChanged);
            // 
            // tlpOperations
            // 
            resources.ApplyResources(this.tlpOperations, "tlpOperations");
            this.tlpOperations.Name = "tlpOperations";
            // 
            // tlpHospitalityTypes
            // 
            resources.ApplyResources(this.tlpHospitalityTypes, "tlpHospitalityTypes");
            this.tlpForm.SetColumnSpan(this.tlpHospitalityTypes, 3);
            this.tlpHospitalityTypes.Name = "tlpHospitalityTypes";
            // 
            // panelHeader
            // 
            this.tlpForm.SetColumnSpan(this.panelHeader, 3);
            this.panelHeader.Controls.Add(this.lblSelectedTableValue);
            this.panelHeader.Controls.Add(this.lblSelectedTable);
            this.panelHeader.Controls.Add(this.touchDialogBanner);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            // 
            // lblSelectedTableValue
            // 
            resources.ApplyResources(this.lblSelectedTableValue, "lblSelectedTableValue");
            this.lblSelectedTableValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSelectedTableValue.Name = "lblSelectedTableValue";
            // 
            // lblSelectedTable
            // 
            resources.ApplyResources(this.lblSelectedTable, "lblSelectedTable");
            this.lblSelectedTable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblSelectedTable.Name = "lblSelectedTable";
            // 
            // touchDialogBanner
            // 
            resources.ApplyResources(this.touchDialogBanner, "touchDialogBanner");
            this.touchDialogBanner.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner.Name = "touchDialogBanner";
            this.touchDialogBanner.TabStop = false;
            // 
            // touchErrorProviderLeft
            // 
            this.touchErrorProviderLeft.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.touchErrorProviderLeft, "touchErrorProviderLeft");
            this.touchErrorProviderLeft.Name = "touchErrorProviderLeft";
            this.touchErrorProviderLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.touchErrorProviderLeft_Paint);
            // 
            // touchErrorProviderRight
            // 
            this.touchErrorProviderRight.BackColor = System.Drawing.Color.White;
            this.tlpForm.SetColumnSpan(this.touchErrorProviderRight, 2);
            resources.ApplyResources(this.touchErrorProviderRight, "touchErrorProviderRight");
            this.touchErrorProviderRight.Name = "touchErrorProviderRight";
            this.touchErrorProviderRight.Paint += new System.Windows.Forms.PaintEventHandler(this.touchErrorProviderRight_Paint);
            // 
            // HospitalityLayoutContainer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpForm);
            this.Name = "HospitalityLayoutContainer";
            this.tlpForm.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpForm;
        private System.Windows.Forms.TableLayoutPanel tlpOperations;
        private System.Windows.Forms.TableLayoutPanel tlpHospitalityTypes;
        private System.Windows.Forms.TableLayoutPanel tlpTableLayout;
        private Controls.Dialogs.TouchErrorProvider touchErrorProviderRight;
        private Label lblSelectedTableValue;
        private Label lblSelectedTable;
        private Controls.TouchDialogBanner touchDialogBanner;
        private Controls.Dialogs.TouchErrorProvider touchErrorProviderLeft;
        private Panel panelHeader;
    }
}