namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    /// <summary>
    /// A business object that holds information about a <see cref="PosContext"/>
    /// </summary>
    public class PosContext : DataEntity
    {
        /// <summary>
        /// Default constructor for <see cref="PosContext"/>
        /// </summary>
        public PosContext ()
            : base ()
        {
            MenuRequired = false;
            UsedInStyleProfile = false;
        }

        /// <summary>
        /// If set to true then a Menu is also required when this context is selected
        /// </summary>
        public bool MenuRequired { get; set; }

        /// <summary>
        /// If set to true then the context is being used in a style profile
        /// </summary>
        public bool UsedInStyleProfile { get; set; }
    }
}
