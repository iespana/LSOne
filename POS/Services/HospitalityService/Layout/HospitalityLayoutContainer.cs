using System;
using System.Windows.Forms;
using LSOne.Services.Interfaces.Delegates;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Controls;
using System.Collections.Generic;
using System.Drawing;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Layout
{
    public partial class HospitalityLayoutContainer : TouchBaseForm
    {
        public event VisibleChangedDelegate HospitalityVisibleChanged;

        IConnectionManager dataModel;

        public HospitalityLayoutContainer(IConnectionManager entry)
        {
            InitializeComponent();
            dataModel = entry;
        }

        public TableLayoutPanel HospPanel
        {
            get { return this.tlpForm; }
        }

        public TableLayoutPanel HospTypesPanel
        {
            get { return this.tlpHospitalityTypes; }
        }

        public TableLayoutPanel OperationsPanel
        {
            get { return this.tlpOperations; }
        }

        public TableLayoutPanel DiningTableLayoutPanel
        {
            get { return this.tlpTableLayout; }
        }

        public string HospitalityTypeText
        {
            set{ touchDialogBanner.BannerText = value;}
        }

        public string SelectedTableText
        {
            set { lblSelectedTableValue.Text = value; }
        }

        public string ErrorMessageLeft
        {
            set
            {
                touchErrorProviderLeft.ErrorText = value;
                touchErrorProviderLeft.Visible = value != "";
                AdjustErrorMessageCells();
            }
        }

        public string ErrorMessageRight
        {
            set
            {
                touchErrorProviderRight.ErrorText = value;
                touchErrorProviderRight.Visible = value != "";
                AdjustErrorMessageCells();
            }
        }

        public void AdjustErrorMessageCells()
        {
            if(!touchErrorProviderLeft.Visible && !touchErrorProviderRight.Visible)
            {
                tlpForm.SetColumnSpan(touchErrorProviderRight, 2);
                tlpForm.SetColumn(touchErrorProviderRight, 1);
                tlpForm.SetColumnSpan(touchErrorProviderLeft, 1);
                tlpForm.SetColumn(touchErrorProviderLeft, 0);
                return;
            }

            if(touchErrorProviderLeft.Visible && !touchErrorProviderRight.Visible)
            {
                tlpForm.SetColumn(touchErrorProviderRight, 1);
                tlpForm.SetColumn(touchErrorProviderLeft, 0);
                tlpForm.SetColumnSpan(touchErrorProviderLeft, 3);
                touchErrorProviderLeft.Margin = new Padding(7, touchErrorProviderLeft.Margin.Top, 7, touchErrorProviderLeft.Margin.Bottom);
            }
            else if(!touchErrorProviderLeft.Visible && touchErrorProviderRight.Visible)
            {
                tlpForm.SetColumn(touchErrorProviderRight, 0);
                tlpForm.SetColumnSpan(touchErrorProviderRight, 3);
                touchErrorProviderRight.Margin = new Padding(7, touchErrorProviderRight.Margin.Top, 7, touchErrorProviderRight.Margin.Bottom);
            }
            else
            {
                tlpForm.SetColumnSpan(touchErrorProviderRight, 2);
                tlpForm.SetColumn(touchErrorProviderRight, 1);
                tlpForm.SetColumnSpan(touchErrorProviderLeft, 1);
                tlpForm.SetColumn(touchErrorProviderLeft, 0);

                touchErrorProviderLeft.Margin = new Padding(7, touchErrorProviderLeft.Margin.Top, 2, touchErrorProviderLeft.Margin.Bottom);
                touchErrorProviderRight.Margin = new Padding(3, touchErrorProviderRight.Margin.Top, 7, touchErrorProviderRight.Margin.Bottom);
            }
        }

        private void tlpTableLayout_VisibleChanged(object sender, EventArgs e)
        {
            if (HospitalityVisibleChanged != null)
            {
                HospitalityVisibleChanged(dataModel);
            }
        }

        private void touchErrorProviderLeft_Paint(object sender, PaintEventArgs e)
        {
            DrawBorder(e);
        }

        private void touchErrorProviderRight_Paint(object sender, PaintEventArgs e)
        {
            DrawBorder(e);
        }

        private static void DrawBorder(PaintEventArgs e)
        {
            Pen borderPen = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(borderPen, new Rectangle(e.ClipRectangle.Location, new Size(e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)));
            borderPen.Dispose();
        }
    }
}
