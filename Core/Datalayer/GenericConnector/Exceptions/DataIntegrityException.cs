using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector.Exceptions
{
    /// <summary>
    /// An Exception that should be used when data does not exists that other data references.
    /// </summary>
    public class DataIntegrityException : DatabaseException
    {
        Type entityType;
        RecordIdentifier id;

        /// <summary>
        /// A constructor that takes type of data entity and id as RecordIdentifier.
        /// </summary>
        /// <param name="entityType">Type of the object for example typeof(Store)</param>
        /// <param name="id">ID of the object, this can be deep nested RecordIdentifier</param>
        public DataIntegrityException(Type entityType,RecordIdentifier id)
            : base("Object does not exist: " + entityType.Name + " with id: " + id.FullString())
        {
            this.entityType = entityType;
            this.id = id;
        }

        public DataIntegrityException(string message)
            :base (message)
        {
        }

        /// <summary>
        /// Returns the entity type
        /// </summary>
        public Type EntityType
        {
            get
            {
                return entityType;
            }
        }

        /// <summary>
        /// Returns the ID
        /// </summary>
        public RecordIdentifier ID
        {
            get
            {
                return id;
            }
        }
    }
}
