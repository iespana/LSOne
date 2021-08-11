using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TenderDeclaration
{
    public interface ITenderDeclarationData : IDataProvider<Tenderdeclaration>
    {
        List<Tenderdeclaration> GetTenderDeclarations(IConnectionManager entry, RecordIdentifier storeID);

        List<Tenderdeclaration> GetAllTenderDeclarationsWithoutStatementIDForAPeriod(
            IConnectionManager entry,
            RecordIdentifier storeID,
            DateTime startingTime,
            DateTime endingTime);

        List<Tenderdeclaration> GetTenderDeclarationsForAStatementID(
            IConnectionManager entry,
            RecordIdentifier storeID,
            string statementID);

        Tenderdeclaration Get(IConnectionManager entry, RecordIdentifier id);
    }
}