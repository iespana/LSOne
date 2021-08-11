using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// DatabaseInfo holds information about each of the databases in the SQL Server
    /// </summary>
    public class DatabaseInfo
    {
        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The size of the database
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// The owner of the database
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// When was the database created?
        /// </summary>
        public string Created { get; set; }
        /// <summary>
        /// The database status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Compatability of the database i.e. 2005, 2008 and etc.
        /// </summary>
        public string Compatability { get; set; }

        /// <summary>
        /// Default constructor which initialises all properties to an empty string
        /// </summary>
        public DatabaseInfo()
        {
            Name = "";
            Size = "";
            Owner = "";
            Created = "";
            Status = "";
            Compatability = "";
        }
    }
}
