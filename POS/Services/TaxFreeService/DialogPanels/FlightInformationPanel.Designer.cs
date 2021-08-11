using LSOne.Controls;
using LSOne.Controls.Dialogs;
using LSOne.POS.Processes.WinControls;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.DialogPanels
{
    partial class FlightInformationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlightInformationPanel));
            this.tdbHeader = new LSOne.Controls.TouchDialogBanner();
            this.tbDepartureFlight = new LSOne.Controls.ShadeTextBoxTouch();
            this.lblDepartureFlight = new System.Windows.Forms.Label();
            this.lblArrivalDate = new System.Windows.Forms.Label();
            this.dtDepartureDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.lblDepartureDate = new System.Windows.Forms.Label();
            this.dtArrivalDate = new LSOne.Controls.Dialogs.DatePickerTouch();
            this.errorProvider = new LSOne.Controls.Dialogs.TouchErrorProvider();
            this.SuspendLayout();
            // 
            // tdbHeader
            // 
            this.tdbHeader.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbHeader, "tdbHeader");
            this.tdbHeader.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbHeader.Name = "tdbHeader";
            this.tdbHeader.TabStop = false;
            // 
            // tbDepartureFlight
            // 
            this.tbDepartureFlight.BackColor = System.Drawing.Color.White;
            this.tbDepartureFlight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.tbDepartureFlight, "tbDepartureFlight");
            this.tbDepartureFlight.MaxLength = 20;
            this.tbDepartureFlight.Name = "tbDepartureFlight";
            // 
            // lblDepartureFlight
            // 
            resources.ApplyResources(this.lblDepartureFlight, "lblDepartureFlight");
            this.lblDepartureFlight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDepartureFlight.Name = "lblDepartureFlight";
            // 
            // lblArrivalDate
            // 
            resources.ApplyResources(this.lblArrivalDate, "lblArrivalDate");
            this.lblArrivalDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblArrivalDate.Name = "lblArrivalDate";
            // 
            // dtDepartureDate
            // 
            this.dtDepartureDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dtDepartureDate, "dtDepartureDate");
            this.dtDepartureDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.dtDepartureDate.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtDepartureDate.MinDate = new System.DateTime(((long)(0)));
            this.dtDepartureDate.Name = "dtDepartureDate";
            // 
            // lblDepartureDate
            // 
            resources.ApplyResources(this.lblDepartureDate, "lblDepartureDate");
            this.lblDepartureDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lblDepartureDate.Name = "lblDepartureDate";
            // 
            // dtArrivalDate
            // 
            this.dtArrivalDate.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.dtArrivalDate, "dtArrivalDate");
            this.dtArrivalDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.dtArrivalDate.MaxDate = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dtArrivalDate.MinDate = new System.DateTime(((long)(0)));
            this.dtArrivalDate.Name = "dtArrivalDate";
            // 
            // errorProvider
            // 
            this.errorProvider.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            this.errorProvider.Name = "errorProvider";
            // 
            // FlightInformationPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.errorProvider);
            this.Controls.Add(this.dtArrivalDate);
            this.Controls.Add(this.lblDepartureDate);
            this.Controls.Add(this.dtDepartureDate);
            this.Controls.Add(this.lblArrivalDate);
            this.Controls.Add(this.lblDepartureFlight);
            this.Controls.Add(this.tbDepartureFlight);
            this.Controls.Add(this.tdbHeader);
            this.Name = "FlightInformationPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private TouchDialogBanner tdbHeader;
        private LSOne.Controls.ShadeTextBoxTouch tbDepartureFlight;
        private System.Windows.Forms.Label lblDepartureFlight;
        private System.Windows.Forms.Label lblArrivalDate;
        private LSOne.Controls.Dialogs.DatePickerTouch dtDepartureDate;
        private System.Windows.Forms.Label lblDepartureDate;
        private LSOne.Controls.Dialogs.DatePickerTouch dtArrivalDate;
        private TouchErrorProvider errorProvider;
    }
}
