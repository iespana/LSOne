using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Peripherals.OPOS;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class PrintingService
    {
        protected virtual FormLineFontTypeEnum GetFontType(string oposFont)
        {
            if (oposFont == OPOSConstants.BoldFont)
            {
                return FormLineFontTypeEnum.Bold;
            }

            if (oposFont == OPOSConstants.DoubleHighAndWideFont)
            {
                return FormLineFontTypeEnum.WideHigh;
            }

            if (oposFont == OPOSConstants.DoubleHighFont)
            {
                return FormLineFontTypeEnum.High;
            }

            if (oposFont == OPOSConstants.ItalicFont)
            {
                return FormLineFontTypeEnum.Italic;
            }

            if (oposFont == OPOSConstants.DoubleWideFont)
            {
                return FormLineFontTypeEnum.Wide;
            }

            return FormLineFontTypeEnum.Normal;

        }

        
        protected virtual List<FormLine> CreateFormLine(string text, IPosTransaction transaction, ISettings settings, List<BarcodePrintInfo> barcodePrintInfo, int lineWidth)
        {
            string firstFontSequence = "";
            
            text = text.Replace("\n", "");

            //myString += "Name :" + new string('\t', 8) + name;

            //Split the string up into font types using the End font sequence
            string[] fonts = text.Split(new[] { OPOSConstants.EndFontSequence }, StringSplitOptions.None);
            
            //Find the first font in the line to use for the printing
            foreach (string str in fonts.Where(w => w != ""))
            {
                foreach (string fontSequence in OPOSConstants.AllFontSequences.Where(w => w != OPOSConstants.EndFontSequence).Where(fontSequence => str.Contains(fontSequence)))
                {
                    firstFontSequence = fontSequence;
                    break;
                }

                if (firstFontSequence != "")
                {
                    break;
                }
            }

            HorizontalAlign textAlignment = TextIsHorizontallyAligned(text);

            text = OPOSConstants.CleanOPOSFonts(text);

            List<FormLine> result = new List<FormLine>();
            FormLine line = new FormLine();
            line.AppendType = FormLineAppendTypeEnum.AppendLine;
            if (text.Contains("<L"))
            {
                line.PrintLineType = FormLineTypeEnum.Logo;
            }
            else if (TextContainsBarcodeMarker(text))
            {
                if (barcodePrintInfo != null && barcodePrintInfo.Any())
                {
                    List<string> markers = BarcodePrintMarkers.AllBarcodeMarkers.Where(marker => text.Contains(marker)).ToList();
                    foreach (BarcodePrintInfo info in barcodePrintInfo.Where(w => !w.Printed))
                    {
                        if (markers.Any(a => a == info.BarcodeMarker))
                        {
                            line = new FormLineBarcode(info.BarcodeToPrint, settings.Store.BarcodeSymbology, Size.Empty);
                            //FormLineBarcode barcode = new FormLineBarcode(info.BarcodeToPrint, settings.Store.BarcodeSymbology, Size.Empty);
                            //result.Add(barcode);

                            //To print the barcode number beneath the barcode we create another line
                            //line.PrintLineType = FormLineTypeEnum.Text;
                            //line.AppendType = FormLineAppendTypeEnum.AppendLine;
                            //line.TextAlignment = HorizontalAlign.Center;
                            //line.FontType = FormLineFontTypeEnum.Wide;
                            //line.Line = info.BarcodeToPrint;

                            //line.Line = CenterAlignText(line, lineWidth);

                            info.Printed = true;
                        }
                    }
                }
            }
            else
            {
                line.PrintLineType = FormLineTypeEnum.Text;
                line.Line = text;
                line.TextAlignment = textAlignment;
                line.FontType = GetFontType(firstFontSequence);
            }

            result.Add(line);

            return result;
        }

        protected virtual string CenterAlignText(FormLine line, int lineWidth)
        {
            if (line.FontType == FormLineFontTypeEnum.Wide || line.FontType == FormLineFontTypeEnum.WideHigh)
            {
                lineWidth = lineWidth/2;
            }

            int charCountUsableForSpace = lineWidth - line.Line.WordLength();
            int spaceOnLeftSide = (int)charCountUsableForSpace / 2;
            int spaceOnRightSide = charCountUsableForSpace - spaceOnLeftSide;

            string parsedString = line.Line;

            parsedString = parsedString.PadLeft(spaceOnLeftSide + parsedString.Length);
            parsedString = parsedString.PadRight(spaceOnRightSide + parsedString.Length);

            return parsedString;
        }

        protected virtual bool TextContainsBarcodeMarker(string text)
        {
            return BarcodePrintMarkers.AllBarcodeMarkers.Any(marker => text.Contains(marker));
        }

        protected virtual HorizontalAlign TextIsHorizontallyAligned(string text)
        {
            if (text.IndexOf(OPOSConstants.HLeftAligned) > 0)
            {
                return HorizontalAlign.Left;
            }

            if (text.IndexOf(OPOSConstants.HRightAligned) > 0)
            {
                return HorizontalAlign.Right;
            }

            if (text.IndexOf(OPOSConstants.HCenterAligned) > 0)
            {
                return HorizontalAlign.Center;
            }

            return HorizontalAlign.Left;
        }

        public virtual List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, string receiptString, IPosTransaction posTransaction, List<BarcodePrintInfo> barcodePrintInfo, int formWidth)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                string[] receipt = receiptString.Split(new[] { '\r' }, StringSplitOptions.None);
                List<FormLine> formLines = new List<FormLine>();

                foreach (string s in receipt)
                {
                    formLines.AddRange(CreateFormLine(s, posTransaction, settings, barcodePrintInfo, formWidth));
                }

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "CreateWinPrintReceipt() END at " +
                                                                                DateTime.Now.Hour.ToString() + ":" +
                                                                                DateTime.Now.Minute.ToString() + ":" +
                                                                                DateTime.Now.Second.ToString() + "." +
                                                                                DateTime.Now.Millisecond.ToString(),
                                                                                this.ToString());
                return formLines;
            }
            catch (POSException ex)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(ex.Message);
                return new List<FormLine>();
            }
            catch (Exception ex)
            {
                POSFormsManager.ShowPOSErrorDialog(new POSException(1003, ex));
                return new List<FormLine>();
            }
        }


        public virtual List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, FormInfo formInfo, IPosTransaction posTransaction, List<BarcodePrintInfo> barcodePrintInfo)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                string[] header = formInfo.Header.Split(new[] { '\r' }, StringSplitOptions.None);
                List<FormLine> formLines = new List<FormLine>();

                foreach (string s in header)
                {
                    formLines.AddRange(CreateFormLine(s, posTransaction, settings, barcodePrintInfo, formInfo.FormWidth));
                }

                string[] details = formInfo.Details.Split(new[] { '\r' }, StringSplitOptions.None);
                foreach (string s in details)
                {
                    formLines.AddRange(CreateFormLine(s, posTransaction, settings, barcodePrintInfo, formInfo.FormWidth));
                }

                string[] footer = formInfo.Footer.Split(new[] { '\r' }, StringSplitOptions.None);
                foreach (string s in footer)
                {
                    formLines.AddRange(CreateFormLine(s, posTransaction, settings, barcodePrintInfo, formInfo.FormWidth));
                }

                entry.ErrorLogger.LogMessage(LogMessageType.Trace, "CreateWinPrintReceipt() END at " +
                                                                                DateTime.Now.Hour + ":" +
                                                                                DateTime.Now.Minute + ":" +
                                                                                DateTime.Now.Second + "." +
                                                                                DateTime.Now.Millisecond,
                                                                                this.ToString());
                return formLines;
            }
            catch (POSException ex)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(ex.Message);
                return new List<FormLine>();
            }
            catch (Exception ex)
            {
                POSFormsManager.ShowPOSErrorDialog(new POSException(1003, ex));
                return new List<FormLine>();
            }
        }


        public virtual List<FormLine> CreateWinPrintReceipt(IConnectionManager entry, FormSystemType formType, IPosTransaction posTransaction, bool copyReceipt, List<BarcodePrintInfo> barcodePrintInfo)
        {
            try
            {
                ISettings settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                FormInfo formInfo = formModulation.GetInfoForForm(formType, copyReceipt);

                formModulation.GetTransformedTransaction(entry, formType, posTransaction, formInfo);

                return CreateWinPrintReceipt(entry, formInfo, posTransaction, barcodePrintInfo);
            }
            catch (POSException ex)
            {
                Interfaces.Services.DialogService(entry).ShowMessage(ex.Message);
                return new List<FormLine>();
            }
            catch (Exception ex)
            {
                POSFormsManager.ShowPOSErrorDialog(new POSException(1003, ex));
                return new List<FormLine>();
            }

        }

        #region For future functionality

        /**************************************
        The version of CreateFormLine creates one FormLine per font type so that multiple font types can be printed within one line.
        But the current version of the WinPrinter does not support this so this is stored here for
        when the WinPrinter will support this.

        protected virtual List<FormLine> CreateFormLine(string text)
        {
            List<FormLine> formLines = new List<FormLine>();

            string[] fonts = text.Split(new[] { OPOSConstants.EndFontSequence }, StringSplitOptions.None);

            foreach (string str in fonts.Where(w => w != ""))
            {
                foreach (string fontSequence in OPOSConstants.AllFontSequences.Where(w => w != OPOSConstants.EndFontSequence).Where(fontSequence => str.Contains(fontSequence)))
                {
                    CreateWhiteSpace(str, fontSequence, formLines);

                    string currentFont = str.Substring(str.IndexOf(fontSequence) + fontSequence.Length);
                    FormLine line = new FormLine();
                    line.AppendType = FormLineAppendTypeEnum.Append;
                    line.PrintLineType = FormLineTypeEnum.Text;
                    line.Line = currentFont;
                    line.FontType = GetFontType(fontSequence);
                    formLines.Add(line);
                    break;
                }
            }

            FormLine lastLine = formLines.LastOrDefault();
            if (lastLine != null)
            {
                lastLine.AppendType = FormLineAppendTypeEnum.AppendLine;
            }

            return formLines;
        }

        protected virtual void CreateWhiteSpace(string str, string fontSequence, List<FormLine> formLines)
        {
            if (str.IndexOf(fontSequence) > 0)
            {
                FormLine whiteSpace = new FormLine();
                whiteSpace.AppendType = FormLineAppendTypeEnum.Append;
                whiteSpace.PrintLineType = FormLineTypeEnum.Text;
                whiteSpace.Line = str.Substring(0, str.IndexOf(fontSequence, StringComparison.CurrentCultureIgnoreCase));
                whiteSpace.FontType = GetFontType(fontSequence);
                formLines.Add(whiteSpace);
            }
        }
        ***************************************************/

        #endregion

    }
}
