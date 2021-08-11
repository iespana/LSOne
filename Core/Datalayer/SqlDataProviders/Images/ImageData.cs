using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Images;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using Image = LSOne.DataLayer.BusinessObjects.Images.Image;

namespace LSOne.DataLayer.SqlDataProviders.Images
{
    public class ImageData : SqlServerDataProviderBase, IImageData
    {
        private static string IsImageUsedCheck = @"CASE WHEN EXISTS (SELECT 1 FROM POSMENULINE l WHERE l.PICTUREID = i.PICTUREID) OR
                                                             EXISTS (SELECT 1 FROM RBOSTORETABLE s WHERE s.PICTUREID = i.PICTUREID) OR
                                                             EXISTS (SELECT 1 FROM POSISTILLLAYOUT WHERE LOGOPICTUREID = i.PICTUREID) OR
                                                             EXISTS (SELECT 1 FROM INVENTJOURNALTRANS WHERE PICTUREID = i.PICTUREID)
                                                        THEN CAST(1 AS BIT)
                                                        ELSE CAST(0 AS BIT)
                                                   END";

		private static List<TableColumn> imageDataColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "PICTUREID " , ColumnAlias = "PICTUREID", TableAlias = "i"},
			new TableColumn {ColumnName = "PICTURE " , ColumnAlias = "PICTURE", TableAlias = "i"},
			new TableColumn {ColumnName = "DESCRIPTION " , ColumnAlias = "DESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "i"},
			new TableColumn {ColumnName = "TYPEOFIMAGE " , ColumnAlias = "TYPEOFIMAGE", TableAlias = "i"},
			new TableColumn {ColumnName = "BACKGROUNDSTYLE " , ColumnAlias = "BACKGROUNDSTYLE", IsNull = true, NullValue = "''", TableAlias = "i"},
			new TableColumn {ColumnName = "GUID " , ColumnAlias = "GUID", TableAlias = "i"},
			new TableColumn {ColumnName = IsImageUsedCheck, ColumnAlias = "ISIMAGEUSED"}
		};

		private static List<TableColumn> styleInfoColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "BACKCOLOR", ColumnAlias = "BACKCOLOR", TableAlias = "s", IsNull = true, NullValue = "-1"}
		};

		public RecordIdentifier SequenceID
		{
			get
			{
				return "IMAGES";
			}
		}

		public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
		{
			return GetExistingRecords(entry, "IMAGES", "PICTUREID", sequenceFormat, startingRecord, numberOfRecords);
		}

		public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
		{
			DeleteRecord<Image>(entry, "IMAGES", "PICTUREID", ID, Permission.ManageImageBank);
		}

		public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
		{
			return RecordExists<Image>(entry, "IMAGES", "PICTUREID", ID);
		}

		public virtual bool Exists(IConnectionManager entry, string description)
		{
			return RecordExists<Image>(entry, "IMAGES", "DESCRIPTION", description);
		}

		public virtual bool GuidExists(IConnectionManager entry, Guid guid)
		{
			return RecordExists<Image>(entry, "IMAGES", "GUID", guid);
		}

		public virtual Image Get(IConnectionManager entry, RecordIdentifier pictureID, CacheType cache = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition
				{
					ConditionValue = "i.DATAAREAID = @DATAAREAID AND i.PICTUREID = @PICTUREID",
					Operator = "AND"
				};

				cmd.CommandText = string.Format(
				  QueryTemplates.BaseQuery("IMAGES", "i"),
				  QueryPartGenerator.InternalColumnGenerator(imageDataColumns),
				  string.Empty,
				  QueryPartGenerator.ConditionGenerator(condition),
				  string.Empty
				  );

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "PICTUREID", (string)pictureID);

				return Get<Image>(entry, cmd, pictureID, PopulateImage, cache, UsageIntentEnum.Normal);
			}
		}

		public virtual Image GetByGuid(IConnectionManager entry, Guid guid, CacheType cache = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition
				{
					ConditionValue = "i.DATAAREAID = @DATAAREAID AND i.GUID = @GUID",
					Operator = "AND"
				};

				cmd.CommandText = string.Format(
				  QueryTemplates.BaseQuery("IMAGES", "i"),
				  QueryPartGenerator.InternalColumnGenerator(imageDataColumns),
				  string.Empty,
				  QueryPartGenerator.ConditionGenerator(condition),
				  string.Empty
				  );

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "GUID", guid);

				return Get<Image>(entry, cmd, new RecordIdentifier(), PopulateImage, cache, UsageIntentEnum.Normal);
			}
		}

		public virtual List<Image> GetList(IConnectionManager entry)
		{
			ValidateSecurity(entry);

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition
				{
					ConditionValue = "i.DATAAREAID = @DATAAREAID",
					Operator = "AND"
				};

				cmd.CommandText = string.Format(
				  QueryTemplates.BaseQuery("IMAGES", "i"),
				  QueryPartGenerator.InternalColumnGenerator(imageDataColumns),
				  string.Empty,
				  QueryPartGenerator.ConditionGenerator(condition),
				  string.Empty
				  );

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				return Execute<Image>(entry, cmd, CommandType.Text, PopulateImage);
			}
		}

		/// <summary>
		/// Returns paginated lists of images that fulfill the passed search criteria (<paramref name="imageType"/>, <paramref name="idOrDescription"/>, <paramref name="idOrDescriptionBeginsWith"/>), including their <see cref="Image.BackColor"/> from POSSTYLE table if the image has a <see cref="Image.BackgroundStyle"/> set.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="totalRecords"></param>
		/// <param name="imageType"></param>
		/// <param name="idOrDescription"></param>
		/// <param name="idOrDescriptionBeginsWith"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="entry"/> is <see langword="null"/></exception>
		public virtual List<Image> SearchList(IConnectionManager entry, 
											int rowFrom, int rowTo, 
											out int totalRecords, 
											ImageTypeEnum? imageType = null, 
											string idOrDescription = null, 
											bool idOrDescriptionBeginsWith = true)
		{
			if(entry == null) throw new ArgumentNullException(nameof(entry));

			ValidateSecurity(entry);

			List<TableColumn> columns = new List<TableColumn>();
			columns.AddRange(imageDataColumns);
			columns.AddRange(styleInfoColumns);
			columns.Add(new TableColumn
			{
				ColumnName = "ROW_NUMBER() OVER(order by i.PICTUREID)",
				ColumnAlias = "ROW"
			});
			columns.Add(new TableColumn
			{
				ColumnName =
					"COUNT(1) OVER(ORDER BY i.PICTUREID RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
				ColumnAlias = "ROW_COUNT"
			});

			List<Condition> externalConditions = new List<Condition>();
			externalConditions.Add(new Condition
			{
				Operator = "AND",
				ConditionValue = "T.ROW BETWEEN @ROWFROM AND @ROWTO"
			});

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();

				if (imageType != null)
				{
					conditions.Add(new Condition
					{
						ConditionValue = $"i.TYPEOFIMAGE = @IMAGETYPE",
						Operator = "AND"
					});

					MakeParam(cmd, "IMAGETYPE", (int)imageType);
				}

				if (!string.IsNullOrWhiteSpace(idOrDescription))
				{
					idOrDescription = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);

					conditions.Add(new Condition
					{
						ConditionValue = "(I.PICTUREID LIKE @searchString or I.DESCRIPTION LIKE @SEARCHSTRING)",
						Operator = "AND"
					});

					MakeParam(cmd, "SEARCHSTRING", idOrDescription);
				}

				conditions.Add(new Condition
				{
					ConditionValue = "i.DATAAREAID = @DATAAREAID",
					Operator = "AND"
				});

				List<Join> joins = new List<Join>
				{
					new Join {Table = "POSSTYLE", TableAlias = "s",  JoinType = "LEFT", Condition = "i.BACKGROUNDSTYLE = s.ID AND i.DATAAREAID = s.DATAAREAID"}
				};

				cmd.CommandText = string.Format(
					QueryTemplates.PagingQuery("IMAGES", "i", "T"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "T"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					string.Empty ); //sortBy

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "ROWFROM", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "ROWTO", rowTo, SqlDbType.Int);
				int matchingRecords = 0;

				List<Image> images = Execute<Image, int>(entry, cmd, CommandType.Text, ref matchingRecords, PopulateImageWithStyleInfoAndCount);

				totalRecords = matchingRecords;

				return images;
			}
		}

		/// <summary>
		/// Returns all images that fulfill the passed search criteria ( <paramref name="imageType"/>, <paramref name="description"/>, <paramref name="descriptionBeginsWith"/>), including their <see cref="Image.BackColor"/> from POSSTYLE table if the image has a <see cref="Image.BackgroundStyle"/> set.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="description"></param>
		/// <param name="descriptionBeginsWith"></param>
		/// <param name="imageType"></param>
		/// <param name="sortBy"></param>
		/// <returns></returns>
		public virtual List<Image> SearchList(IConnectionManager entry, string description, bool descriptionBeginsWith, ImageTypeEnum? imageType, string sortBy)
		{
			ValidateSecurity(entry);

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();

				if(imageType != null)
				{
					conditions.Add(new Condition
					{
						ConditionValue = $"i.TYPEOFIMAGE = @IMAGETYPE",
						Operator = "AND"
					});

					MakeParam(cmd, "IMAGETYPE", (int)imageType);
				}

				if(!string.IsNullOrEmpty(description))
				{
					conditions.Add(new Condition
					{
						ConditionValue = "i.DESCRIPTION LIKE @DESCRIPTION",
						Operator = "AND"
					});

					MakeParam(cmd, "DESCRIPTION", PreProcessSearchText(description, true, descriptionBeginsWith));
				}

				conditions.Add(new Condition
				{
					ConditionValue = "i.DATAAREAID = @DATAAREAID",
					Operator = "AND"
				});

				List<Join> joins = new List<Join>
				{
					new Join {Table = "POSSTYLE", TableAlias = "s",  JoinType = "LEFT", Condition = "i.BACKGROUNDSTYLE = s.ID AND i.DATAAREAID = s.DATAAREAID"}
				};

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("IMAGES", "i"),
					QueryPartGenerator.InternalColumnGenerator(imageDataColumns.Union(styleInfoColumns).ToList()),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					sortBy
					);

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				return Execute<Image>(entry, cmd, CommandType.Text, PopulateImageWithStyleInfo);
			}
		}

		public virtual void Save(IConnectionManager entry, Image image)
		{
			var statement = new SqlServerStatement("IMAGES");
			ValidateSecurity(entry, Permission.ManageImageBank);
			image.Validate();

			bool isNew = false;
			if (image.ID == RecordIdentifier.Empty)
			{
				isNew = true;
				image.ID = DataProviderFactory.Instance.GenerateNumber<IImageData, Image>(entry);
			}

			if(image.Guid == Guid.Empty)
			{
				image.Guid = Guid.NewGuid();
			}

			if (isNew || !Exists(entry, image.ID))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("PICTUREID", (string)image.ID);
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
			}
			else
			{
				statement.StatementType = StatementType.Update;
				statement.AddCondition("PICTUREID", (string)image.ID);
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			}

			statement.AddField("PICTURE", GetRawImage(image), SqlDbType.Image);
			statement.AddField("DESCRIPTION", image.Text);
			statement.AddField("TYPEOFIMAGE", (int)image.ImageType, SqlDbType.TinyInt);
			statement.AddField("BACKGROUNDSTYLE", image.BackgroundStyle == null ? null : (string)image.BackgroundStyle);
			statement.AddField("GUID", image.Guid, SqlDbType.UniqueIdentifier);
			Save(entry, image, statement);
		}

		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return Exists(entry, id);
		}

		private static void PopulateImage(IDataReader dr, Image image)
		{
			image.ID = (string)dr["PICTUREID"];
			image.Text = (string)dr["DESCRIPTION"];
			image.BackgroundStyle = (string)dr["BACKGROUNDSTYLE"];
			image.ImageType = (ImageTypeEnum)Enum.Parse(typeof(ImageTypeEnum), ((byte)dr["TYPEOFIMAGE"]).ToString());
			image.IsImageUsed = AsBool(dr["ISIMAGEUSED"]);
			image.Guid = AsGuid(dr["GUID"]);

			byte[] rawPicture = (byte[])dr["PICTURE"];
			using (var stream = new MemoryStream())
			{
				for (int i = 0; i < rawPicture.Length; i++)
				{
					stream.WriteByte(rawPicture[i]);
				}
				image.Picture = System.Drawing.Image.FromStream(stream);
			}
		}

		private static void PopulateImageWithStyleInfo(IDataReader dr, Image image)
		{
			PopulateImage(dr, image);

			image.BackColor = (int)dr["BACKCOLOR"];
		}

		private static void PopulateImageWithStyleInfoAndCount(IConnectionManager entry, IDataReader dr, Image image, ref int rowCount)
		{
			PopulateImageWithStyleInfo(dr, image);

			if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
			   entry.Connection.DatabaseVersion == ServerVersion.Unknown)
			{
				rowCount = (int)dr["ROW_COUNT"];
			}
		}

		private byte[] GetRawImage(Image image)
		{
			byte[] rawPicture;
			using (var stream = new MemoryStream())
			{
				if (image.Picture is Bitmap)
				{
					image.Picture.Save(stream, ImageFormat.Png);
				}
				else
				{
					image.Picture.Save(stream, image.Picture.RawFormat);
				}
				rawPicture = stream.ToArray();
			}

			return rawPicture;
		}
	}
}
