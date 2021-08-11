using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LSOne.Controls.Dialogs.Properties;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Controls.Dialogs
{
	public partial class NumPad : UserControl
	{
		private char passwordChar = '*';
		private string inputText = "";
		private string commaChar = ",";
		private int decimalDivisor = 100;
		private int numberOfDecimals = 2;
		private bool entryStartsInDecimals; //Set to true if amount is entered as 1450 for $14.50
		private bool suppressEnterButtonEvent;
		private string validationPartOfTrack = "";

		private NumpadEntryTypes entryType;
		private IPOSApp posApp;

		private string scanStr = "";
		private string track = "";
		private string startMSR = "";
		private string endMSR = "";
		private string trackSeparator = "";
		private bool trackReady;     //Is the track ready for the timer.
		private bool errorInCardSwipe;     //Is the track ready for the timer.

		public delegate void ShortcutKeyHandler(int operationID, string numPadValue);
		public event ShortcutKeyHandler ShortcutKey;

		public delegate void KeyDownHandler(object sender, KeyEventArgs e);
		public event KeyDownHandler KeyDownEvent;

		public delegate void EnterButtonHandler();
		public event EnterButtonHandler EnterButtonPressed;

		public delegate void CardSwipedHandler(string track2);
		public event CardSwipedHandler CardSwiped;
		private Currency currency;

		public void SetInnerTextBoxFocus()
		{
			if (!enteredValue.Focused)
			{
				enteredValue.Focus();
			}
		}

		public enum ExtraChar
		{
			DoubleZero = 0,
			Asterisk = 1
		}

        public NumPad()
        {
            InitializeComponent();

            MaxNumberOfDigits = 13;
			AllowNegative = false;
            GhostTextEqualToEntryType = true;

            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            if (!DesignMode && !designMode)
            {
                MaskChar = "";
                entryType = NumpadEntryTypes.BarcodeOrQuantity;
                commaChar = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                currency = Providers.CurrencyData.Get(DLLEntry.DataModel, DLLEntry.Settings.Store.Currency, CacheType.CacheTypeApplicationLifeTime);
            }           
        }

		public bool SuppressEnterButtonEvent
		{
			get { return suppressEnterButtonEvent; }
			set { suppressEnterButtonEvent = value; }
		}

		public string ValidationPartOfTrack
		{
			get { return validationPartOfTrack; }
		}

		public int NumberOfDecimals
		{
			get { return numberOfDecimals; }
			set
			{
				if (numberOfDecimals >= 0 && numberOfDecimals < 10)
				{
					numberOfDecimals = value;
					decimalDivisor = (int)Math.Pow(10, numberOfDecimals);
				}
			}
		}        
			
		private string StartMSR
		{
			get
			{
				if (startMSR == "")
				{
					startMSR = DLLEntry.Settings.HardwareProfile.StartTrack1;
				}
				return startMSR;
			}
		}

		private string EndMSR
		{
			get
			{
				if (endMSR == "")
				{
					endMSR = DLLEntry.Settings.HardwareProfile.EndTrack1;
				}
				return endMSR;
			}
		}

		private string TrackSeparator
		{
			get
			{
				if (trackSeparator == "")
				{
					trackSeparator = DLLEntry.Settings.HardwareProfile.Separator1;
				}
				return trackSeparator;
			}
		}

		public int MaxNumberOfDigits { get; set; }

        /// <summary>
        /// Text set based on the entry type
        /// </summary>
        public string EntryTypeText { get; private set; }

        /// <summary>
        /// If true, ghost text will be set automatically based on EntryType. If false, the user can manually set the ghost text.
        /// </summary>
        public bool GhostTextEqualToEntryType { get; set; }

        public string GhostText
        {
            get
            {
                return enteredValue.GhostText;
            }
            set
            {
                enteredValue.GhostText = value;
            }
        }

		/// <summary>
		/// If set to false, the Numpad timer will be disabled.  The timer is set to support 
		/// keyboard card readers and to keep the focus on the input field on the numpad.
		/// </summary>
		[DefaultValue(false)]
		public bool TimerEnabled
		{
			get { return timer1.Enabled; }
			set { timer1.Enabled = value; }
		}

		public bool ShortcutKeysActive { get; set; }

		/// <summary>
		/// Value that is entered into the numpad.
		/// </summary>
		public string EnteredValue
		{
			get
			{
				if (entryType == NumpadEntryTypes.CardValidation)
				{
					return inputText;
				}
				return enteredValue.Text;
			}
			set
			{
				enteredValue.Text = value;
				inputText = value;
				if ((entryType == NumpadEntryTypes.CardExpireValidation) && (inputText.IndexOf("/") > 0))
				{
					inputText = inputText.Replace("/", "");
				}
			}
		}

		public decimal EnteredDecimalValue
		{
			get
			{
				if ((entryType == NumpadEntryTypes.Price) || (entryType == NumpadEntryTypes.Amount))
				{
					if (EnteredValue != "")
					{
						return Convert.ToDecimal(EnteredValue, CultureInfo.CurrentCulture);
					}
				}
				return 0;
			}
		}

		/// <summary>
		/// Quantity that was enterd with a barcode
		/// </summary>
		public decimal EnteredQuantity { get; private set; }

		/// <summary>
		/// If set to true amount is entered as 1450 for $14.50
		/// </summary>
		[DefaultValue(false)]
		public bool EntryStartsInDecimals
		{
			get
			{
				return entryStartsInDecimals;
			}
			set
			{
				entryStartsInDecimals = value;
				touchNumPad.DotKeyEnabled = !entryStartsInDecimals;
			}
		}

		/// <summary>
		/// The different entry method the numpad can have, password, price or quantity, etc.
		/// </summary>
		public NumpadEntryTypes EntryType
		{
			get
			{
				return entryType;
			}
			set
			{
				entryType = value;
				InitializeEntryType();
			}
		}

		public int MaskInterval { get; set; }

		public decimal MaximumQty { get; set; }

		public string MaskChar { get; set; }

		public bool NegativeMode { get; set; }

		/// <summary>
		/// If true and <see cref="NumpadEntryTypes">NumpadEntryTypes</see> is some numeric type like Quantity, Price, Numeric, amount then the NumPad will accept negative sign.
		/// </summary>
		public bool AllowNegative { get; set; }

		public void ClearValue()
		{
			inputText = "";
			enteredValue.Text = "";
			EnteredQuantity = 0;
		}

		public void Clear()
		{
			this.posApp_POSStatusEnabled(this, new EventArgs(), null);
		}

		[Browsable(true),DefaultValue(true)]
		public bool EnterButtonEnabled
		{
			get { return touchNumPad.EnterKeyEnabled; }
			set { touchNumPad.EnterKeyEnabled = value; }
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				timer1.Stop();
				timer1.Dispose();

				foreach (Control control in this.Controls)
				{
					control.Dispose();
				}
			}
			if (posApp != null)
			{
				posApp.POSStatusEnabled -= posApp_POSStatusEnabled;
				posApp.POSStatusDisabled -= posApp_POSStatusDisabled;
				posApp.SetFocusRequest -= OnPOSApp_SetFocusRequest;
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Adds a input text to "inputText" when buttons are pressed.
		/// </summary>
		/// <param name="input"></param>
		private void AddText(string input)
		{
			bool replaceSelection = enteredValue.SelectionLength > 0;

			if (((enteredValue.TextLength - enteredValue.SelectionStart) > 2) && (input == Convert.ToString(commaChar)))
			{
				return;
			}

			if (inputText.Length < MaxNumberOfDigits || (inputText.Length - enteredValue.SelectionLength) < MaxNumberOfDigits)
			{
				bool hasCommaChar = inputText.IndexOf(commaChar) >= 0;
				if (!hasCommaChar || replaceSelection)  //No comma || replasing selection in text box
				{
					if (replaceSelection)
					{
						inputText = inputText.Remove(enteredValue.SelectionStart, enteredValue.SelectionLength);
					}

					if(!hasCommaChar && inputText == "" && input.StartsWith(commaChar))
                    {
						SetEnteredValue("0");
						inputText = "0" + input; 
						scanStr = inputText;
					}
					else
                    {
						inputText = inputText.Substring(0, enteredValue.SelectionStart) + input + inputText.Substring(enteredValue.SelectionStart);
                    }

					SetEnteredValue(inputText, selectionReplaced: true);
					return;

				}
				int currentNoOfDecimals = inputText.Length - inputText.IndexOf(commaChar) - 1;

				if (enteredValue.SelectionStart <= inputText.IndexOf(commaChar) || (currentNoOfDecimals < numberOfDecimals) && (input != Convert.ToString(commaChar)))
				{
					inputText = inputText.Substring(0, enteredValue.SelectionStart) + input + inputText.Substring(enteredValue.SelectionStart);
					SetEnteredValue(inputText);
				}
			}
		}

		/// <summary>
		/// Sets the string that is seen by the user on the numpad.
		/// </summary>
		/// <param name="inputText">The text to be formated and shown.</param>
		/// <param name="deleteKeyPressed"></param>
		/// <param name="selectionReplaced"></param>
		private void SetEnteredValue(string inputText, bool deleteKeyPressed = false, bool selectionReplaced = false, bool charsDeleted = false)
		{
			int cursorPosition = enteredValue.SelectionStart;
			
			if (entryType == NumpadEntryTypes.Password)
			{
				enteredValue.Text = "";
				enteredValue.Text = enteredValue.Text.PadLeft(inputText.Length, this.passwordChar);
			}
			else if (entryType == NumpadEntryTypes.Price || entryType == NumpadEntryTypes.Quantity || entryType == NumpadEntryTypes.Amount)
			{
				if ((entryStartsInDecimals) && (!inputText.Contains(commaChar)))
				{
					string tmpString = "";
					if (inputText == "")
					{
						inputText = "0";
						tmpString = inputText;
						tmpString += Convert.ToString(commaChar);
						tmpString = tmpString.PadRight((inputText.Length + 1 + numberOfDecimals), Convert.ToChar("0"));
						enteredValue.Text = tmpString;
					}
					else
					{
						tmpString = Convert.ToString(Convert.ToDecimal(inputText) / decimalDivisor);
						if (inputText.Length >= tmpString.Length)
						{
							if (tmpString.IndexOf(commaChar) == -1) { tmpString += Convert.ToString(commaChar); }
							tmpString = tmpString.PadRight((inputText.Length + 1), Convert.ToChar("0"));
						}
						enteredValue.Text = tmpString;
					}
					if (NegativeMode)
					{
						enteredValue.Text = "-" + tmpString;
					}
				}
				else
				{
					enteredValue.Text = inputText;
					if (NegativeMode)
					{
						enteredValue.Text = "-" + inputText;
					}
				}
			}
			// clear
			else if (((inputText.Length == enteredValue.Text.Length && inputText.Length != 0 && enteredValue.SelectionStart != 0) || (inputText.Length < enteredValue.Text.Length && enteredValue.SelectionStart != 0)) && !selectionReplaced)
			{
				bool inputSelected = (enteredValue.SelectionLength > 0);
				bool selectionCleared = (enteredValue.Text.Length - inputText.Length) > 1;
				enteredValue.Text = inputText;
				enteredValue.SelectionStart = deleteKeyPressed || selectionCleared || inputSelected ? cursorPosition : cursorPosition - 1;
			}
			// Replace selection
			else if (selectionReplaced)
			{
				enteredValue.Text = inputText;
				enteredValue.SelectionStart = cursorPosition + 1;
			}
			// enter values
			else if (inputText.Length > enteredValue.Text.Length)
			{
				enteredValue.Text = inputText;
				enteredValue.SelectionStart = cursorPosition +1;
			}
			// clear in position 0 or empty textbox
			else
			{
				enteredValue.Text = inputText;
			}
			
			if (MaskChar != "")
			{
				AddMask();
			}
			Color negColor = ColorPalette.POSRedLight;
			Color posColor = ColorPalette.POSBlack;
			enteredValue.ForeColor = NegativeMode ? negColor : posColor;
			if(!charsDeleted)
			{
				enteredValue.SelectionStart = cursorPosition + 1;
			}
			enteredValue.SelectionLength = 0;
		}

		private void InitializeEntryType()
		{
			try
			{
				ClearValue();

                if (entryType == NumpadEntryTypes.BarcodeOrQuantity)
                {
                    EntryTypeText = Resources.BarCodeOrQuantity;
                    SetGhostText(Resources.BarcodeOrQuantityGhost);
                    numberOfDecimals = numberOfDecimals != 2 && numberOfDecimals != DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits ? numberOfDecimals : DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits;
                    MaxNumberOfDigits = 20;
                    touchNumPad.MultiplyButtonIsZeroZero = false;
                }
                else if (entryType == NumpadEntryTypes.Barcode)
                {
                    EntryTypeText = Resources.ItemBarCode;
                    SetGhostText(Resources.ItemBarcodeGhost);
                    numberOfDecimals = numberOfDecimals != 2 && numberOfDecimals != DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits ? numberOfDecimals : DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits;
                    MaxNumberOfDigits = 20;
                    touchNumPad.MultiplyButtonIsZeroZero = true;

                }
                else if (entryType == NumpadEntryTypes.Price)
                {
                    EntryTypeText = Resources.EnterAmount + " (" + currency.Symbol + "):";
                    SetGhostText(Resources.Amount + " (" + currency.Symbol + ")");
                    numberOfDecimals = numberOfDecimals != 2 && numberOfDecimals != DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits ? numberOfDecimals : DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits;
                    MaxNumberOfDigits = 12;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                }                
                else if (entryType == NumpadEntryTypes.Integer || entryType == NumpadEntryTypes.IntegerPositive || entryType == NumpadEntryTypes.Numeric)
                {
                    EntryTypeText = Resources.EnterNumber;
                    SetGhostText(Resources.Number);
                    numberOfDecimals = 0;
                    MaxNumberOfDigits = 8;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                    touchNumPad.DotKeyEnabled = false;
                }
                else if (entryType == NumpadEntryTypes.CardExpireValidation)
                {
                    EntryTypeText = Resources.EnterExpireDate;
                    SetGhostText(Resources.ExpiryDate);
                    numberOfDecimals = 0;
                    MaxNumberOfDigits = 4;
                    MaskChar = "/";
                    MaskInterval = 2;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                }
                else if (entryType == NumpadEntryTypes.CardValidation)
                {
                    EntryTypeText = Resources.EnterNumber;
                    SetGhostText(Resources.Number);
                    numberOfDecimals = 0;
                    MaxNumberOfDigits = 19;
                    MaskChar = "-";
                    MaskInterval = 4;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                    TimerEnabled = true;

                }
                else if (entryType == NumpadEntryTypes.Quantity)
                {
                    EntryTypeText = Resources.EnterQuantity;
                    SetGhostText(Resources.QuantityGhost);
                    numberOfDecimals = numberOfDecimals != 2 && numberOfDecimals != DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits ? numberOfDecimals : DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits;
                    MaxNumberOfDigits = 6;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                }
                else if (entryType == NumpadEntryTypes.Amount)
                {
                    EntryTypeText = Resources.EnterAmount;
                    SetGhostText(Resources.Amount);
                    numberOfDecimals = numberOfDecimals != 2 && numberOfDecimals != DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits ? numberOfDecimals : DLLEntry.Settings.CultureInfo.NumberFormat.NumberDecimalDigits;
                    MaxNumberOfDigits = 7;
                    MaskChar = "";
                    MaskInterval = 0;
                    touchNumPad.MultiplyButtonIsZeroZero = true;
                }
                else if (entryType == NumpadEntryTypes.QR)
                {
                    MaxNumberOfDigits = 5000;
                    MaskChar = "";
                    MaskInterval = 0;
                }
            }
            catch (Exception)
            {
            }
        }

        private void SetGhostText(string text)
        {
            if(GhostTextEqualToEntryType)
            {
                enteredValue.GhostText = text;
            }
        }

		private void DeleteChar(bool deleteKeyPressed)
		{
			int cursorPosition = enteredValue.SelectionStart;
			int selectionLength = enteredValue.SelectionLength;
			int beforeDeletionInputLength = inputText.Length;
			if (inputText.Length > 0)
			{
				if (selectionLength > 0)
				{
					inputText = inputText.Substring(0, cursorPosition) + inputText.Substring(cursorPosition + selectionLength, inputText.Length - (cursorPosition + selectionLength));
				}
				else if ((cursorPosition == 0 && deleteKeyPressed)  || (cursorPosition < inputText.Length && cursorPosition != 0))
				{
					inputText = inputText.Remove(deleteKeyPressed ? cursorPosition : cursorPosition - 1, 1);
				}
				else if (cursorPosition != 0)
				{
					if (deleteKeyPressed)
					{
						return;
					}
					inputText = inputText.Substring(0, inputText.Length - 1);
				}

				// Move cursor to the correct position after deletion
				if (selectionLength == beforeDeletionInputLength || cursorPosition == 0)
				{
					enteredValue.SelectionStart = 0;
				}
				else if (cursorPosition == 1 && !deleteKeyPressed)
				{
					enteredValue.SelectionStart = cursorPosition - 1;
				}
			}

			SetEnteredValue(inputText, deleteKeyPressed, charsDeleted: true);

			if (scanStr.Length > 0)
			{
				scanStr = scanStr.Substring(0, cursorPosition) + scanStr.Substring(cursorPosition + selectionLength, scanStr.Length - (cursorPosition + selectionLength));
			}
		}

		/// <summary>
		/// Returns true if a key is found in the key mapping table
		/// </summary>
		/// <param name="keyPressed">The character that was pressed on the keyboard</param>
		/// <returns>Returns true if the key pressed is found</returns>
		private bool IsShortcutKey(char keyPressed)
		{
			try
			{
				int charId = Convert.ToInt32(keyPressed);

				return DLLEntry.Settings.KeyboardMapping.ContainsKey(charId);
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Get the shortcut key data
		/// </summary>
		/// <param name="keyPressed">The character that was pressed on the keyboard</param>
		/// <returns>Returns the shortcut key data</returns>
		private KeyboardMapping ShortcutKeyData(char keyPressed)
		{
			try
			{
				int charId = Convert.ToInt32(keyPressed);

				return DLLEntry.Settings.KeyboardMapping.ContainsKey(charId) ? DLLEntry.Settings.KeyboardMapping[charId] : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// Remove start and end MSR strings 
		/// </summary>
		/// <param name="track">The track that should be stripped</param>
		/// <returns>The track without the start and end MSR strings</returns>
		private string RemoveStartEndMSR(string track)
		{
			try
			{
				if (StartMSR.Length > 0)
				{
					track = track.Replace(StartMSR.ToLower(), "");
					track = track.Replace(StartMSR.ToUpper(), "");
				}

				if (EndMSR.Length > 0)
				{
					track = track.Replace(EndMSR.ToLower(), "");
					track = track.Replace(EndMSR.ToUpper(), "");
				}
				return track;
			}
			catch (Exception)
			{
				return track;
			}
		}

        // When a key is pressed on the keyboard
        private void OnEnteredValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!suppressEnterButtonEvent)
                {
                    if (errorInCardSwipe)
                    {
                        errorInCardSwipe = false;
                        Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.UnableToReadCardStripe, MessageBoxButtons.OK, MessageDialogType.Generic);                        
                        return;
                    }

					if (entryType == NumpadEntryTypes.QR)
					{
						SetEnteredValue(scanStr);
					}

					if (!trackReady)
					{
						scanStr = "";
                        EnterButtonPressed?.Invoke();   //else raise the enter button pressed, if input text is not empty

                        e.Handled = true;
                        EntryType = NumpadEntryTypes.BarcodeOrQuantity;
                        return;
					}
				}
			}
			else if (e.KeyChar == '*' && entryType == NumpadEntryTypes.BarcodeOrQuantity)
			{
				SetBarcodeOrQuantityMode();
			}
			else if (scanStr.Length == 4 && scanStr == "<qr>")
			{
				EntryType = NumpadEntryTypes.QR;
				scanStr += e.KeyChar;
			}
			else if (entryType == NumpadEntryTypes.QR)
			{
				scanStr += e.KeyChar;
			}
			else
			{
				if (IsShortcutKey(e.KeyChar))
				{
					KeyboardMapping mapping = ShortcutKeyData(e.KeyChar);

					if (mapping.Action != 0)
					{
						if (ShortcutKeysActive)
                        {
                            RaiseShortcutKeyEvent(mapping);
                        }
                    }
				}
				else
				{
					if (e.KeyChar == (char)Keys.Back)
					{
						DeleteChar(false);
					}
					else //Other characters
					{
						if (entryType == NumpadEntryTypes.Quantity
							|| entryType == NumpadEntryTypes.Price
							|| entryType == NumpadEntryTypes.Numeric
							|| entryType == NumpadEntryTypes.Integer
							|| entryType == NumpadEntryTypes.CardValidation
							|| entryType == NumpadEntryTypes.CardExpireValidation
							|| entryType == NumpadEntryTypes.IntegerPositive
							|| entryType == NumpadEntryTypes.Amount)
						{
							string allowedCharacters = "";
							switch (entryType)
							{
								case NumpadEntryTypes.CardValidation:
								case NumpadEntryTypes.CardExpireValidation:
								case NumpadEntryTypes.IntegerPositive:
									allowedCharacters = "0123456789"; 
									break;
								default:
									allowedCharacters = "0123456789" + commaChar;
									if(AllowNegative)
									{
										allowedCharacters += "-";
									}
									break;
							}

							scanStr += e.KeyChar;
							// if cardnumber is contained in the scanstring
							if ((scanStr.ToLower().IndexOf(StartMSR.ToLower()) >= 0 || scanStr.ToUpper().IndexOf(StartMSR.ToUpper()) >= 0) && (StartMSR.Length > 0))
							{
								// Do nothing
							}
							else if (allowedCharacters.IndexOf(e.KeyChar.ToString()) > -1)
							{
								AddText(e.KeyChar.ToString());
							}
						}
						else if (e.KeyChar.ToString() == CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
						{
							e.Handled = true;
						}
						else
						{
							scanStr += e.KeyChar;
							// if cardnumber is contained in the scanstring
							if ((StartMSR.Length == 0)
								||
								((scanStr.ToLower().IndexOf(StartMSR.ToLower()) < 0 && scanStr.ToUpper().IndexOf(StartMSR.ToUpper()) < 0)
								 &&
								 (scanStr.Length >= StartMSR.Length)))
							{
								AddText(e.KeyChar.ToString());
							}
						}
					}
				}

				if ((StartMSR.Length > 0) &&
					(EndMSR.Length > 0) &&
					(scanStr.ToLower().IndexOf(StartMSR.ToLower()) >= 0) &&
					(scanStr.ToLower().IndexOf(EndMSR.ToLower()) >= 0) &&
					(track == ""))
				{

					
					
					DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "Start or end chars detected: s=" + StartMSR + " and e=" + EndMSR + ".  Scanstring now: " + scanStr.HideNumbers('*'), this.ToString());

					// Cards can have tracks without the track seperator, where you have track data like:
					//     <startChar>123456<endChar>                    

					if (scanStr.ToLower().IndexOf(TrackSeparator.ToLower()) > 0)
					{
						DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "Card separator detected: sep=" + TrackSeparator + ".  Scanstring now: " + scanStr.HideNumbers('*'), this.ToString());
						
						track = RemoveStartEndMSR(scanStr);
						trackReady = true; 

						int seperatorCharPos = scanStr.IndexOf(TrackSeparator);
						validationPartOfTrack = scanStr.Substring(seperatorCharPos + 1, 4);


					}
					else
					{
						if (scanStr.Length < 4)
						{
							// In this case, there was likely an error in reading the card stripe and the card reader has presented us with empty track data
							// or some sort of error code
							DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "Invalid card entry: s=" + StartMSR + " and e=" + EndMSR + ".  Scanstring now: " + scanStr.HideNumbers('*'), this.ToString());
							errorInCardSwipe = true;
							scanStr = "";
						}
						else
						{
							DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, "No card separator detected, single track card. Scanstring now: " + scanStr.HideNumbers('*'), this.ToString());

							track = RemoveStartEndMSR(scanStr);
							trackReady = true;

							int length = track.Length > 4 ? 4 : track.Length;
							validationPartOfTrack = track.Substring(0, length);
						}
					}
				}
			}

			e.Handled = true;
			OnPOSApp_SetFocusRequest();
		}

        private void RaiseShortcutKeyEvent(KeyboardMapping mapping)
        {
            string extraInfo = (POSOperations)mapping.Action == POSOperations.LogOff || (POSOperations)mapping.Action == POSOperations.LogOffForce ? "true" : mapping.ActionProperty;
            ShortcutKey(mapping.Action, extraInfo); //Raise event for a shortcut key pressed
        }

        [DllImport("user32", EntryPoint = "MapVirtualKey")]
		public static extern int MapVirtualKeyA(int wCode, int wMapType);

		// When a function key (F1 - F24) is pressed on the keyboard
		private void OnEnteredValue_KeyDown(object sender, KeyEventArgs e)
		{

			bool isFunctionKey = false;
			if (e.KeyCode.ToString().IndexOf('F') == 0 && e.KeyCode.ToString().Length > 1)
			{
				Keys[] functionKeys = {   Keys.F1, 
										  Keys.F2, 
										  Keys.F3, 
										  Keys.F4, 
										  Keys.F5, 
										  Keys.F6, 
										  Keys.F7, 
										  Keys.F8, 
										  Keys.F9, 
										  Keys.F10, 
										  Keys.F11, 
										  Keys.F12, 
										  Keys.F13, 
										  Keys.F14, 
										  Keys.F15, 
										  Keys.F16, 
										  Keys.F17, 
										  Keys.F18, 
										  Keys.F19, 
										  Keys.F20, 
										  Keys.F21, 
										  Keys.F22, 
										  Keys.F23, 
										  Keys.F24 };
				for (int i = 0; i < functionKeys.Length; i++)
				{
					if (e.KeyCode == functionKeys[i])
					{
						isFunctionKey = true;
						break;
					}
				}
			}
			if ((int)e.KeyCode == (int)Keys.Delete)
			{
				e.Handled = true;
				DeleteChar(true);
			}
			if (!isFunctionKey)
			{
				return;
			}
			int nonVirtualKey = MapVirtualKeyA((int)e.KeyCode, 2);

			if (IsShortcutKey(Convert.ToChar(nonVirtualKey)))
			{
				KeyboardMapping mapping = ShortcutKeyData(Convert.ToChar(nonVirtualKey));

				if (mapping.Action != 0)
				{
					if (ShortcutKeysActive)
					{
                        RaiseShortcutKeyEvent(mapping);
					}
				}
			}
			else if (DLLEntry.Settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Keyboard)
			{
				KeyDownEvent(sender, e);
			}
		}

		public void AddMask()
		{
			if (MaskChar == "")
				return;

			string initValue = enteredValue.Text.Trim();
			initValue = StripNumber(initValue);

			if (initValue.Length < MaskInterval)
			{
				return;
			}
			if (initValue.Length == MaskInterval)
			{
				enteredValue.Text = initValue + MaskChar;
				return;
			}
			int nextInterval = MaskInterval;
			while (nextInterval < initValue.Length)
			{
				initValue = initValue.Insert(nextInterval, MaskChar);
				nextInterval = nextInterval + MaskInterval + 1;
			}
			enteredValue.Text = initValue;
		}

		/// <summary>
		/// Strips the input string and returns only the numeric characters as new string
		/// </summary>
		/// <param name="str">The string to convert</param>
		/// <returns>The result string with only numeric characters</returns>
		private static string StripNumber(string str)
		{
			string result = "";
			for (int i = 0; i < str.Length; i++)
			{
				if ((str[i] > 47) && (str[i] < 58))
				{
					result = result + str[i];
				}
			}
			return result;
		}

		protected virtual void OnPOSApp_SetFocusRequest()
		{
			bool okToProceed = false;
			Control currentControl = enteredValue.Parent;
			// Scanning up the control parent tree to check the window state of the underlying form
			while (currentControl != null)
			{
				if (currentControl is Form)
				{
					okToProceed = ((Form) currentControl).WindowState != FormWindowState.Minimized;
					break;
				}
				currentControl = currentControl.Parent;
			}

			if (okToProceed)
			{
				enteredValue.Focus();
				enteredValue.SelectionLength = 0;
				enteredValue.Select();
			}
		}

		public IPOSApp Subject
		{
			set
			{
				posApp = value;
				posApp.POSStatusEnabled += posApp_POSStatusEnabled;
				posApp.POSStatusDisabled += posApp_POSStatusDisabled;
				posApp.SetFocusRequest += OnPOSApp_SetFocusRequest;
			}
		}

		// m_subject.IssueDataChanged
		protected virtual void posApp_POSStatusDisabled(object sender, EventArgs e, IPosTransaction posTransaction)
		{
		}

		// m_subject.IssueDataChanged
		protected virtual void posApp_POSStatusEnabled(object sender, EventArgs e, IPosTransaction posTransaction)
		{
			enteredValue.Text = "";
			EnteredQuantity = 0;
			inputText = "";
			InitializeEntryType();
		}

		//WTF
		public void SetEnteredValueFocus()
		{
			OnPOSApp_SetFocusRequest();
		}

		private void touchNumPad_ClearPressed(object sender, EventArgs e)
		{
			InitializeEntryType();
		}

		private void OnTouchNumPad_TouchKeyPressed(object sender, TouchKeyKeyEventArgs args)
		{
			if (args.Key == "*" && entryType == NumpadEntryTypes.BarcodeOrQuantity)
			{
				SetBarcodeOrQuantityMode();

				args.Handled = true;
			}
		   
			enteredValue.Focus();
		}

        private void SetBarcodeOrQuantityMode()
        {
            if (enteredValue.Text == "")
            {
                return;
            }
            try
            {
                EnteredQuantity = Convert.ToDecimal(enteredValue.Text);
            }
            catch (Exception)
            {
                return;
            }

            // is the quantity entered higher than the maximum limit
            if ((MaximumQty > 0) && (EnteredQuantity > MaximumQty))
            {
                Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Resources.MaxQtyReached, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            EntryTypeText = Resources.Quantity + enteredValue.Text + " - " + Resources.BarCode;
            enteredValue.GhostText = EntryTypeText;

			enteredValue.Text = "";
			inputText = "";
			SetEnteredValue(inputText);
			touchNumPad.MultiplyButtonIsZeroZero = true;
		}

		private void OnTouchNumPad_BackspacePressed(object sender, EventArgs e)
		{
			enteredValue.Focus();
		}

		private void ntbInput_Enter(object sender, EventArgs e)
		{
			touchNumPad.KeystrokeMode = true;
		}

		private void ntbInput_Leave(object sender, EventArgs e)
		{
			touchNumPad.KeystrokeMode = false;
		}

        private void touchNumPad_EnterPressed(object sender, EventArgs e)
        {
            if (EnterButtonPressed != null)
            {
                EnterButtonPressed();
            }
            else
            {
                Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage("The EnterButtonPressed event has not been instantiated");
            }
        }

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Enabled = false;

			try
			{
				if (trackReady)
				{
                    CardSwiped?.Invoke(track); //If it is a valid card string the raise a CardSwept event.
                    track = "";
					trackReady = false;
					scanStr = "";
				}
			}
			finally
			{
				timer1.Enabled = true;
			}
		}
	}
}
