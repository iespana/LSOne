using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public delegate void POSStatusEnabledEventHandler(object sender, EventArgs e, IPosTransaction posTransaction);
    public delegate void POSStatusDisabledEventHandler(object sender, EventArgs e, IPosTransaction posTransaction);
    public delegate void POSSetFocusRequestEventHandler();
    public delegate void POSLoadDesignHandler(RecordIdentifier layoutID = null);
    public delegate void RunOpenMenuHandler(RecordIdentifier menuID, ButtonGridsEnum buttonGrid);

    public interface IPOSApp
    {
        event POSSetFocusRequestEventHandler SetFocusRequest;
        event POSStatusEnabledEventHandler POSStatusEnabled;
        event POSStatusDisabledEventHandler POSStatusDisabled;

        RunOpenMenuHandler RunOpenMenuHandler { get; set; }
        POSLoadDesignHandler POSLoadDesignHandler { get; set; }

        bool RunOperation(POSOperations operationID, Object extraInfo);
        bool RunOperation(POSOperations operationID, object extraInfo, ref IPosTransaction posTransaction);
        bool RunOperation(POSOperations operationID, object extraInfo, OperationInfo operationInfo, ref IPosTransaction posTransaction);
        void BusinessDateSet();
        void LogOffForce();
        void RunOpenMenu(RecordIdentifier menuID, ButtonGridsEnum buttonGrid);

        /// <summary>
        /// Sets the current touch layout to the layout with the given ID
        /// </summary>
        /// <param name="layoutID">The ID of the layout to set</param>
        void LoadTouchLayout(RecordIdentifier layoutID);

        /// <summary>
        /// Sets the current touch layout to the default touch layout shown when the user logs in.
        /// </summary>
        void ResetTouchLayout();
        IWin32Window POSMainWindow { get; set; }
        void InitializeEngine(DBConnection dbConn);
    }
}
