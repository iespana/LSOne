using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.EventArguments;

namespace LSOne.Services.Interfaces.Delegates
{
    public delegate DialogResult ShowMessageHandler(string message, MessageBoxButtons btns = MessageBoxButtons.OK, MessageDialogType dialogType = MessageDialogType.Generic);
    public delegate DialogResult ShowKeyboardInputHandler(ref string inputText, string promptText, string ghostText, int maxLength, InputTypeEnum inputType);
    public delegate DialogResult ShowDateInputHandler(ref string inputText, string promptText);
    public delegate DialogResult ShowNumpadInputHandler(ref string inputText, string promptText, NumpadEntryTypes numpadEntryTypes);

    /// <summary>
    /// Occurs when a page on the logon form performs a predefined operation
    /// </summary>
    /// <param name="sender">The sender page</param>
    /// <param name="args">Contains information about which operation is being performed</param>
    public delegate void LogonFormEventHandler(object sender, LogonFormEventArguments args);

    public delegate void InsertedAmountDelegate(decimal totalAmount, CashGuardStaus status, short mode);
    public delegate void LevelStatusDelegate(CashGuardWarningType warningType, int denomination, short numberOf, string typeString, string denominationString, string warningMessage, string extInfo);
    public delegate void ErrorEventDelegate(int errorCode, string error);
}
