using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    /// <summary>
    /// Defines methods to manipulate import profile lines.
    /// </summary>
    public interface IImportProfileLineData : IDataProvider<ImportProfileLine>
    {
        /// <summary>
        /// Get the list of ImportProflieLine instances associated with the given id.
        /// </summary>
        List<ImportProfileLine> GetSelectList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get the list of Field values that are unavailable for a given id, as they are already in use.
        /// </summary>
        List<Field> GetUnavailableFieldList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get the instance of ImportProfileLine having the given id.
        /// </summary>
        ImportProfileLine Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Swap the sequence values for two ImportProfileLine items having the given ids.
        /// </summary>
        void SwapSequenceValues(IConnectionManager entry, RecordIdentifier firstRowId, RecordIdentifier secondRowId);
    }
}
