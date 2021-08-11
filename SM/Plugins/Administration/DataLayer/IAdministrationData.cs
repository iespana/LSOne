using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.ViewPlugins.Administration.DataLayer
{
    internal interface IAdministrationData : IDataProviderBase<ListViewItem>
    {
        List<ListViewItem> GetSignedActions(IConnectionManager entry, Guid actionID);
    }
}