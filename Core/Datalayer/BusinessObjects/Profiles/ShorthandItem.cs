using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class ShorthandItem : DataEntity
    {
        public RecordIdentifier ProfileID;
        public ShortHandTypeEnum ShorthandType;

        public ShorthandItem()
        {
            ID = Guid.Empty;
            ProfileID = RecordIdentifier.Empty;
            ShorthandType = ShortHandTypeEnum.Email;
        }

        public bool Empty()
        {
            return ID == Guid.Empty;
        }
    }
}
