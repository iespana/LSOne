using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscLocationMember 
    {
        private JscLocation member;
        
        public RecordIdentifier OwnerLocation { get; set; }
        public RecordIdentifier MemberLocation { get; set; }
        public int Sequence { get; set; }

        public JscLocation Member
        {
            get { return member; }
            set
            {
                member = value;
                if ((value != null))
                {
                    MemberLocation = value.ID;
                }
                else
                {
                    MemberLocation = default(System.Guid);
                }

            }
        }
    }
}