using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{
    public partial class BarCodeMaskSegmentDialog : DialogBase
    {
        public BarCodeMaskSegmentDialog(BarcodeMaskSegment segment, int maxLength)
            : this()
        {
            this.Segment = segment;
            tbSegmentNum.Text = segment.SegmentNum.ToString();
            ntbLength.MaxValue = maxLength + (int)segment.Length;
            ntbLength.Value = (double)segment.Length;
            ntbDecimals.Value = (double)segment.Decimals;
            cmbType.SelectedIndex = (int)segment.Type;
        }

        public BarCodeMaskSegmentDialog(int segmentNum, int maxLength)
            : this()
        {
            Segment = null;
            tbSegmentNum.Text = segmentNum.ToString();
            ntbLength.MaxValue = maxLength;
        }

        public BarCodeMaskSegmentDialog()
        {
            InitializeComponent();
            ntbDecimals.Enabled = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Segment == null)
            {
                Segment = new BarcodeMaskSegment {SegmentNum = Convert.ToInt32(tbSegmentNum.Text)};
            }

            Segment.Type = (BarcodeSegmentType)cmbType.SelectedIndex;
            Segment.SegmentChar = tbCharacter.Text;
            Segment.Length = (int)ntbLength.Value;
            Segment.Decimals = (int)ntbDecimals.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        public BarcodeMaskSegment Segment { get; private set; }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (Segment == null)
            {
                btnOK.Enabled = (cmbType.SelectedIndex >= 0 && ntbLength.Value > 0);
            }
            else
            {
                btnOK.Enabled = (cmbType.SelectedIndex >= 0 && ntbLength.Value > 0) &&
                    (cmbType.SelectedIndex != (int)Segment.Type ||
                    tbCharacter.Text != Segment.SegmentChar ||
                    ntbLength.Value != (double)Segment.Length ||
                    ntbDecimals.Value != (double)Segment.Decimals);
            }
            ntbDecimals.Enabled = (((BarcodeSegmentType) cmbType.SelectedIndex) == BarcodeSegmentType.Price) ||
                                  (((BarcodeSegmentType) cmbType.SelectedIndex) == BarcodeSegmentType.Quantity);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCharacter.Text = BarcodeMaskSegment.TypeToCharacter((BarcodeSegmentType)cmbType.SelectedIndex);
            
            CheckEnabled(this, EventArgs.Empty);
        }
    }
}
