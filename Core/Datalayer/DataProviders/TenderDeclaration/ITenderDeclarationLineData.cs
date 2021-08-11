using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TenderDeclaration
{
    public interface ITenderDeclarationLineData : IDataProvider<TenderdeclarationLine>
    {
        List<TenderdeclarationLine> GetTenderDeclarationLines(IConnectionManager entry, RecordIdentifier tenderDeclarationID);
    }
}