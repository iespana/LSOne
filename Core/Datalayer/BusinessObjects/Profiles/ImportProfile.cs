using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.DataLayer.BusinessObjects.Properties;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    /// <summary>
    /// Encapsulates the business object for import profile records.
    /// </summary>
    public class ImportProfile : DataEntity
    {
        /// <summary>
        /// Gets or sets the master Id value.
        /// </summary>
        public RecordIdentifier MasterID { get; set; }

        /// <summary>
        /// Gets or sets the description value.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the import type value.
        /// </summary>
        public ImportType ImportType { get; set; }
        public string ImportTypeString
        {
            get
            {
                return ImportTypeHelper.GetImportTypeString(ImportType);
            }
        }
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public byte Default { get; set; }

        /// <summary>
        /// Determins if the first line in the CSV file is dat line or header line
        /// </summary>
        public bool HasHeaders { get; set; }

        /// <summary>
        /// Constructs an empty import profile instance.
        /// </summary>
        public ImportProfile()
            : this(RecordIdentifier.Empty, "", "", 0, 0)
        {
        }

        /// <summary>
        /// Constructs an import profile instance based on the given parameters.
        /// </summary>
        public ImportProfile(
            RecordIdentifier masterID,
            string id,
            string description,
            ImportType importType,
            byte @default)
            : base(masterID, id)
        {
            this.MasterID = masterID;
            this.ID = id;
            this.Description = description;
            this.ImportType = importType;
            this.Default = @default;
        }
    }
}