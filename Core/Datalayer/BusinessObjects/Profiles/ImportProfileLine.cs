using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    /// <summary>
    /// Encapsulates the business object for import profile line records.
    /// </summary>
    public class ImportProfileLine : DataEntity
    {
        /// <summary>
        /// Gets or sets the master Id value.
        /// </summary>
        public RecordIdentifier MasterId { get; set; }

        /// <summary>
        /// Gets or sets the master import profile Id value.
        /// </summary>
        public RecordIdentifier ImportProfileMasterId { get; set; }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        public Field Field { get; set; }

        /// <summary>
        /// Gets or sets the field type value.
        /// </summary>
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Gets or sets the sequence value.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Constructs an empty import profile line instance.
        /// </summary>
        public ImportProfileLine()
            : this(RecordIdentifier.Empty, RecordIdentifier.Empty, 0, 0, 0)
        {
        }

        /// <summary>
        /// Constructs an import profile line instance based on the given parameters.
        /// </summary>
        public ImportProfileLine(
            RecordIdentifier masterId,
            RecordIdentifier importProfileMasterId,
            Field field,
            FieldType fieldType,
            int sequence)
            : base ()
        {
            this.MasterId = masterId;
            this.ImportProfileMasterId = importProfileMasterId;
            this.Field = field;
            this.FieldType = fieldType;
            this.Sequence = sequence;
        }
    }
}
