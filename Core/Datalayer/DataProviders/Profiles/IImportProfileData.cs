using LSOne.DataLayer.BusinessObjects;
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
    /// Defines methods to manipulate import profiles.
    /// </summary>
    public interface IImportProfileData : IDataProvider<ImportProfile>, ISequenceable
    {
        /// <summary>
        /// Get the list of all ImportProflie instances.
        /// </summary>
        List<ImportProfile> GetSelectList(IConnectionManager entry);

        /// <summary>
        /// Get the list of all ImportProfile instances that have more than one line.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        List<ImportProfile> GetSelectListOfNonEmptyProfiles(IConnectionManager entry);

        /// <summary>
        /// Get the ImportProfile instance having the given id.
        /// </summary>
        ImportProfile Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Set the default import profile.
        /// </summary>
        void SetAsDefault(IConnectionManager entry, RecordIdentifier id);

        ImportProfile GetDefaultImportProfile(IConnectionManager entry, ImportType importType);
    }
}
