using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ConfigurationWizard
{
    public interface ITouchButtonLayoutData : IDataProviderBase<TouchButtonLayout>
    {
        /// <summary>
        /// Get layout list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>Layout list</returns>
        List<TouchButtonLayout> GetTouchButtonLayoutList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Save layouts into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutList">Layout list</param>
        void SaveLayouts(IConnectionManager entry, List<TouchButtonLayout> layoutList);

        /// <summary>
        /// Get layout list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">LayoutIds</param>
        /// <param name="cache">CacheType</param>
        /// <returns>List of Layouts</returns>
        List<TouchLayout> GetTouchLayout(IConnectionManager entry, List<RecordIdentifier> idList, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Get PosMenuHeader list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderIDs">posMenuHeaderIds</param>
        /// <param name="cacheType">CacheType</param>
        /// <returns>PosMenuHeader list</returns>
        List<PosMenuHeader> GetPosMenuHeader(IConnectionManager entry, List<RecordIdentifier> posMenuHeaderIDs, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Get PosMenuLine list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLineIDs">PosMenuLineIds</param>
        /// <returns>PosMenuLine list</returns>
        List<PosMenuLine> GetPosMenuLine(IConnectionManager entry, List<RecordIdentifier> posMenuLineIDs);

        /// <summary>
        /// Get list of PosMenuHeader with MenuId, Description values.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="condition">condition</param>
        /// <returns>list of PosMenuHeader</returns>
        List<DataEntity> Get(IConnectionManager entry, string condition);

        void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATETILLLAYOUTS");
    }
}