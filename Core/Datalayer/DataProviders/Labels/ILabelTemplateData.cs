using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Labels
{
    public interface ILabelTemplateData : IDataProvider<LabelTemplate>, ISequenceable
    {
        /// <summary>
        /// Retrieves a list of all templates applicable for a given context
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<LabelTemplate> GetList(IConnectionManager entry, LabelTemplate.ContextEnum context);

        List<LabelTemplate> GetList(IConnectionManager entry, LabelTemplate.ContextEnum context, string sorting);
        bool Exists(IConnectionManager entry, LabelTemplate.ContextEnum context, string labelName);

        LabelTemplate Get(IConnectionManager entry, RecordIdentifier labelTemplateID);
    }
}