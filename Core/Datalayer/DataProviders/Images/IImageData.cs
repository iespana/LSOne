using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.Images
{
	public interface IImageData : IDataProvider<Image>, ISequenceable
	{
		Image Get(IConnectionManager entry, RecordIdentifier pictureID, CacheType cache = CacheType.CacheTypeNone);
		List<Image> GetList(IConnectionManager entry);

		/// <summary>
		/// Returns paginated lists of images that fulfill the passed search criteria (<paramref name="imageType"/>, <paramref name="idOrDescription"/>, <paramref name="idOrDescriptionBeginsWith"/>).
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		/// <param name="imageType"></param>
		/// <param name="idOrDescription"></param>
		/// <param name="idOrDescriptionBeginsWith"></param>
		/// <returns></returns>
		List<Image> SearchList(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecords, ImageTypeEnum? imageType = null, string idOrDescription = null, bool idOrDescriptionBeginsWith = true);

		/// <summary>
		/// Returns all images that fulfill the passed search criteria ( <paramref name="imageType"/>, <paramref name="description"/>, <paramref name="descriptionBeginsWith"/>), including their <see cref="Image.BackColor"/> from POSSTYLE table if the image has a <see cref="Image.BackgroundStyle"/> set.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="description"></param>
		/// <param name="descriptionBeginsWith"></param>
		/// <param name="imageType"></param>
		/// <param name="sortBy"></param>
		/// <returns></returns>
		List<Image> SearchList(IConnectionManager entry, string description, bool descriptionBeginsWith, ImageTypeEnum? imageType, string sortBy);
		bool Exists(IConnectionManager entry, string description);
		bool GuidExists(IConnectionManager entry, Guid guid);
		Image GetByGuid(IConnectionManager entry, Guid guid, CacheType cache = CacheType.CacheTypeNone);
	}
}
