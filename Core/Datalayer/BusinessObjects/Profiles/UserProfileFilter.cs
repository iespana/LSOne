using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class UserProfileFilter
    {
        public UserProfileFilter()
        {
            StoreID = RecordIdentifier.Empty;
            LayoutID = RecordIdentifier.Empty;
            VisualProfileID = RecordIdentifier.Empty;
        }

        public string Description { get; set; }
        public bool DescriptionBeginsWith { get; set; }
        public string LanguageCode { get; set; }
        public bool LanguageCodeBeginsWith { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public RecordIdentifier LayoutID { get; set; }
        public RecordIdentifier VisualProfileID { get; set; }
        public UserProfileSortEnum Sort { get; set; }
        public bool SortBackwards { get; set; }
    }

    public enum UserProfileSortEnum
    {
        Description,
        Store,
        VisualProfile,
        Layout,
        LanguageCode,
        KeyboardLanguage
    }
}
