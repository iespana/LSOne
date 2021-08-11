using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.BarCodes.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.BarCodes.Dialogs
{
    public partial class BarCodeMaskDialog : DialogBase
    {
        private bool maskHasError;
        private bool segmentChanged;
        private BarcodeMask mask;
        List<BarcodeMaskSegment> segments;
        List<BarcodeMaskSegment> deletedSegments;
        private bool suppressEvents;

        public BarCodeMaskDialog(RecordIdentifier barCodeMaskID)
            : this()
        {
            this.BarCodeMaskID = barCodeMaskID;            
        }        

        public BarCodeMaskDialog()
        {
            BarCodeMaskID = "";

            mask = null;
            BarcodeMask = mask;

            InitializeComponent();

            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarCodeType());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeEAN128());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeCode39());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeITF());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeCode128());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeUPCA());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeUPCE());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeEAN13());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeEAN8());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodePDF417());
            cmbSymbology.Items.Add(new Datalayer.DataEntities.BarCodeTypes.BarcodeMaxicode());

            cmbSymbology.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;

            tbMask.ReadOnly = false;

            lvSegments.ContextMenuStrip = new ContextMenuStrip();
            lvSegments.ContextMenuStrip.Opening += lvSegments_Opening;
        }

        private void BarCodeMaskDialog_Load(object sender, EventArgs e)
        {
            suppressEvents = true;
            mask = Providers.BarcodeMaskData.Get(PluginEntry.DataModel, BarCodeMaskID);
            BarcodeMask = mask;
            deletedSegments = new List<BarcodeMaskSegment>();
            segments = new List<BarcodeMaskSegment>();

            if (mask != null)
            {
                tbDescription.Text = mask.Text;
                cmbType.SelectedIndex = (int)mask.InternalType;
                cmbSymbology.SelectedIndex = (int)mask.Type;

                if (mask.Mask != "")
                {
                    tbMask.ReadOnly = true;
                    tbMask.TabStop = false;
                }

                tbPrefix.Text = mask.Prefix;
                tbMask.Text = mask.Mask;
                tbLength.Text = tbMask.Text.Length.ToString();

                segments = Providers.BarcodeMaskSegmentData.Get(PluginEntry.DataModel, BarCodeMaskID);

                FillList(segments);

                ConstructMask();
            }

            suppressEvents = false;
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
            bool checkMask = true;
            bool checkPrefix = true;

            if (mask != null)
            {
                if (tbMask.Text == mask.Mask)
                {
                    checkMask = false;
                }


                if (tbPrefix.Text == mask.Prefix)
                {
                    checkPrefix = false;
                }
            }

            if (checkMask)
            {
                if (Providers.BarcodeMaskData.MaskExists(PluginEntry.DataModel, tbMask.Text, (mask == null) ? RecordIdentifier.Empty : mask.ID))
                {
                    errorProvider2.SetError(tbMask, Properties.Resources.BarCodeMaskExists);
                    tbMask.Focus();
                    return;
                }
            }

            if (checkPrefix)
            {
                if (Providers.BarcodeMaskData.PrefixExists(PluginEntry.DataModel, tbPrefix.Text, (mask == null) ? RecordIdentifier.Empty : mask.ID))
                {
                    errorProvider2.SetError(tbPrefix, Properties.Resources.PrefixExists);
                    tbPrefix.Focus();
                    return;
                }
            }

            if (mask == null)
            {
                mask = new BarcodeMask();
            }
            
            mask.Text = tbDescription.Text;
            mask.InternalType = (BarcodeInternalType)cmbType.SelectedIndex;
            mask.Type = (BarcodeType)cmbSymbology.SelectedIndex;
            mask.Mask = tbMask.Text;
            mask.Prefix = tbPrefix.Text;

            Providers.BarcodeMaskData.Save(PluginEntry.DataModel, mask);

            BarCodeMaskID = mask.ID;
            BarcodeMask = mask;

            if (segmentChanged)
            {
                //Delete segments
                foreach (BarcodeMaskSegment sg in deletedSegments)
                {
                    Providers.BarcodeMaskSegmentData.Delete(PluginEntry.DataModel, sg.UniqueID);
                }

                // Prepare the mask segment data.
                var segments = new List<BarcodeMaskSegment>();

                foreach (ListViewItem item in lvSegments.Items)
                {
                    var segment = (BarcodeMaskSegment)item.Tag;

                    segment.MaskId = (string)mask.ID;

                    segments.Add(segment);
                }

                Providers.BarcodeMaskSegmentData.Save(PluginEntry.DataModel, mask.ID, segments);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        public RecordIdentifier BarCodeMaskID { get; private set; }
        public BarcodeMask BarcodeMask { get; private set; }
        private void tbPrefix_TextChanged(object sender, EventArgs e)
        {
            if (suppressEvents)
            {
                return;
            }

            ConstructMask();

            errorProvider1.Clear();

            CheckEnabled(this, EventArgs.Empty);
        }

        private void tbMask_TextChanged(object sender, EventArgs e)
        {
            if (suppressEvents)
            {
                return;
            }

            errorProvider1.Clear();
            maskHasError = false;

            CheckEnabled(this, EventArgs.Empty);
        }

        private void FillList(List<BarcodeMaskSegment> segments)
        {
            tbMask.ReadOnly = true;
            tbMask.TabStop = false;

            BarcodeMaskSegment selectedSegment = lvSegments.SelectedItems.Count > 0 ? (BarcodeMaskSegment) lvSegments.SelectedItems[0].Tag : null;

            lvSegments.Items.Clear();

            foreach (BarcodeMaskSegment segment in segments.OrderBy(p => p.SegmentNum))
            {
                var listItem = new ListViewItem((string)segment.SegmentNum.ToString());

                listItem.SubItems.Add(PluginOperations.TypeText(segment.Type));
                listItem.SubItems.Add(((int)segment.Length).ToString());
                listItem.SubItems.Add(segment.SegmentChar);
                listItem.SubItems.Add(segment.Decimals.ToString());

                if (selectedSegment != null && selectedSegment == segment)
                {
                    listItem.Selected = true;
                }

                listItem.Tag = segment;
                listItem.ImageIndex = -1;

                lvSegments.Add(listItem);
                
            }

            lvSegments.BestFitColumns();
            lvSegments_SelectedIndexChanged(this, EventArgs.Empty);
            tbPrefix.MaxLength = 22 - (tbMask.Text.Length - tbPrefix.Text.Length);
        }

        private void AddSegment(List<BarcodeMaskSegment>  segments, string sequence)
        {
            var segment = new BarcodeMaskSegment();

            segments.Add(segment);

            segment.SegmentChar = sequence[0].ToString();
            segment.Length = sequence.Length;
            segment.SegmentNum = segments.Count;


            switch (sequence[0])
            {
                case 'I':
                    segment.Type = BarcodeSegmentType.Item;
                    break;

                case 'X':
                    segment.Type = BarcodeSegmentType.AnyNumber;
                    break;

                case 'M':
                    segment.Type = BarcodeSegmentType.CheckDigit;
                    break;

                case 'Z':
                    segment.Type = BarcodeSegmentType.SizeDigit;
                    break;

                case 'C':
                    segment.Type = BarcodeSegmentType.ColorDigit;
                    break;

                case 'S':
                    segment.Type = BarcodeSegmentType.StyleDigit;
                    break;

                case 'L':
                    segment.Type = BarcodeSegmentType.EANLicenseCode;
                    break;

                case 'P':
                    segment.Type = BarcodeSegmentType.Price;
                    break;

                case 'Q':
                    segment.Type = BarcodeSegmentType.Quantity;
                    break;

                case 'E':
                    segment.Type = BarcodeSegmentType.Employee;
                    break;

                case 'D':
                    segment.Type = BarcodeSegmentType.Customer;
                    break;

                case 'A':
                    segment.Type = BarcodeSegmentType.DataEntry;
                    break;

                case 'R':
                    segment.Type = BarcodeSegmentType.SalesPerson;
                    break;

                case 'F':
                    segment.Type = BarcodeSegmentType.Pharmacy;
                    break;
                    
                case 'O':
                    segment.Type = BarcodeSegmentType.Store;
                    break;
                case 'T':
                    segment.Type = BarcodeSegmentType.Terminal;
                    break;
                case 'B':
                    segment.Type = BarcodeSegmentType.Receipt;
                    break;
            }
        }

        private void tbMask_Leave(object sender, EventArgs e)
        {
            if (suppressEvents)
            {
                return;
            }

            bool foundPrefix = false;
            string maskLetters;
            string prefix = "";
            string sequence = "";
            char lastLetterFound = ' ';
            string mask;

            if (tbPrefix.Text == tbMask.Text.Substring(0, tbMask.TextLength)) 
            {                
                prefix = tbPrefix.Text.Substring(0, tbMask.TextLength);
                mask = tbMask.Text.Substring(tbPrefix.Text.Length);
            }
            else
            {
                mask = tbMask.Text;
            }

            if (!tbMask.ReadOnly && tbMask.Text != "")
            {
                maskLetters = "IXMZCSLPQEDARFOTB";

                segments = new List<BarcodeMaskSegment>();

                foreach (char c in mask)
                {
                    if (maskLetters.Contains(c))
                    {
                        foundPrefix = true;

                        if (lastLetterFound == c || lastLetterFound == ' ')
                        {
                            sequence += c;
                        }
                        else
                        {
                            if (sequence != "")
                            {
                                AddSegment(segments, sequence);
                                sequence = "";
                            }
                            sequence += c;
                        }
                    }
                    else
                    {
                        if (!foundPrefix)
                        {
                            prefix += c;
                        }
                        else
                        {
                            // Error
                            errorProvider1.SetError(tbMask, Properties.Resources.MaskInvalidLetterError);
                            tbMask.Focus();
                            maskHasError = true;
                            return;
                        }
                    }

                    lastLetterFound = c;
                }

                if (sequence != "")
                {
                    AddSegment(segments, sequence);
                }

                segmentChanged = true;
                FillList(segments);
                tbPrefix.Text = prefix;
            }
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            bool enabled;

            errorProvider2.Clear();
            
            if (mask == null)
            {
                enabled = !maskHasError && tbDescription.Text.Length > 0;
            }
            else
            {
                enabled = !maskHasError && tbDescription.Text.Length > 0
                    && (tbDescription.Text != mask.Text || cmbType.SelectedIndex != (int)mask.InternalType ||
                        cmbSymbology.SelectedIndex != (int)mask.Type || tbMask.Text != mask.Mask ||
                        tbPrefix.Text != mask.Prefix || segmentChanged);
            }

            btnOK.Enabled = enabled;
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((BarcodeInternalType)cmbType.SelectedIndex == BarcodeInternalType.CreditMemo)
            {
                errorProvider1.Icon = System.Drawing.Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider1.SetError(tbPrefix, Resources.MatchBarcodeMask);
            }
            else
            {
                errorProvider1.Clear();
            }
            CheckEnabled(null, EventArgs.Empty);
        }

        private void lvSegments_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = lvSegments.SelectedItems.Count > 0;

            if (lvSegments.SelectedItems.Count > 0)
            {
                btnMoveUp.Enabled = (lvSegments.SelectedItems[0].Index > 0);
                btnMoveDown.Enabled = (lvSegments.SelectedItems[0].Index < lvSegments.Items.Count-1);
            }
            else
            {
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = false;
            }

            btnsContextButtons.AddButtonEnabled = (tbMask.Text.Length < 22);
        }

        private void ConstructMask()
        {
            FillList(segments);

            string mask = tbPrefix.Text;

            foreach(ListViewItem item in lvSegments.Items)
            {
                var segment = (BarcodeMaskSegment)item.Tag;

                for (int i = 0; i < segment.Length; i++)
                {
                    mask += segment.SegmentChar;
                }
            }

            tbMask.Text = mask;
            tbPrefix.MaxLength = 22 - (tbMask.Text.Length - tbPrefix.Text.Length);
            tbLength.Text = tbMask.Text.Length.ToString();

        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = lvSegments.SelectedItems[0].Index;
            ListViewItem current = lvSegments.SelectedItems[0];
            ListViewItem other = lvSegments.Items[index - 1];

            ((BarcodeMaskSegment)current.Tag).SegmentNum = (int)((BarcodeMaskSegment)current.Tag).SegmentNum - 1;
            ((BarcodeMaskSegment)other.Tag).SegmentNum = (int)((BarcodeMaskSegment)other.Tag).SegmentNum + 1;

            segmentChanged = true;

            ConstructMask();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int index = lvSegments.SelectedItems[0].Index;
            ListViewItem current = lvSegments.SelectedItems[0];
            ListViewItem other = lvSegments.Items[index + 1];

            ((BarcodeMaskSegment)current.Tag).SegmentNum = (int)((BarcodeMaskSegment)current.Tag).SegmentNum + 1;
            ((BarcodeMaskSegment)other.Tag).SegmentNum = (int)((BarcodeMaskSegment)other.Tag).SegmentNum - 1;

            segmentChanged = true;

            ConstructMask();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteSegmentQuestion,
                Properties.Resources.DeleteSegment) == DialogResult.Yes)
            {

                BarcodeMaskSegment segmentToDelete = (BarcodeMaskSegment)lvSegments.SelectedItems[0].Tag;

                int nextSegmentIndex = lvSegments.SelectedItems[0].Index + 1;

                for (int i = nextSegmentIndex; i < lvSegments.Items.Count; i++)
                {
                    ((BarcodeMaskSegment) lvSegments.Items[i].Tag).SegmentNum--;
                }

                segments.Remove(segmentToDelete);

                if(!RecordIdentifier.IsEmptyOrNull(segmentToDelete.UniqueID))
                {
                    deletedSegments.Add(segmentToDelete);
                }

                segmentChanged = true;

                ConstructMask();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new BarCodeMaskSegmentDialog(lvSegments.Items.Count + 1, 22 - tbMask.Text.Length);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var segment = dlg.Segment;
                segments.Add(segment);
                segmentChanged = true;

                ConstructMask();

                tbPrefix.MaxLength = 22 - (tbMask.Text.Length - tbPrefix.Text.Length);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var segment = (BarcodeMaskSegment)lvSegments.SelectedItems[0].Tag;
            var dlg = new BarCodeMaskSegmentDialog(segment, 22 - tbMask.Text.Length);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var listItem = lvSegments.SelectedItems[0];

                listItem.SubItems[1].Text = PluginOperations.TypeText(segment.Type);
                listItem.SubItems[2].Text = ((int)segment.Length).ToString();
                listItem.SubItems[3].Text = segment.SegmentChar;
                listItem.SubItems[4].Text = segment.Decimals.ToString();

                segmentChanged = true;

                lvSegments.BestFitColumns();

                ConstructMask();

                tbPrefix.MaxLength = 22 - (tbMask.Text.Length - tbPrefix.Text.Length);

                CheckEnabled(this, EventArgs.Empty);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            lvSegments.SmallImageList = null;
        }

        private void lvSegments_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvSegments_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvSegments.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-",350));

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveUp,
                    400,
                    btnMoveUp_Click);

            item.Image = ContextButtons.GetMoveUpButtonImage();
            item.Enabled = btnMoveUp.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    500,
                    btnMoveDown_Click);

            item.Image = ContextButtons.GetMoveDownButtonImage();
            item.Enabled = btnMoveDown.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("BarCodeMaskDialogList", lvSegments.ContextMenuStrip, lvSegments);

            e.Cancel = (menu.Items.Count == 0);
        }

        
    }
}
