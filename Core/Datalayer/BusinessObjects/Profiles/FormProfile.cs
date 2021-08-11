using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    //uses table POSFORMPROFILE
    public class FormProfile : DataEntity
    {
        public static Guid DefaultProfileID = new Guid("0c7d790c-096c-4ed5-94b5-6b9814c87a46");
        public static Guid EmailProfileID = new Guid("F2A2744C-A885-4825-B454-CB6CD9BEDADE");

        public bool ProfileIsUsed { get; set; }

        public FormProfile(RecordIdentifier profileId, string description)
            : this()
        {
            ID = profileId;
            Text = description;
        }

        public FormProfile()
            : base() {
                ID = RecordIdentifier.Empty;
                Text = "";
            }

    }
}
