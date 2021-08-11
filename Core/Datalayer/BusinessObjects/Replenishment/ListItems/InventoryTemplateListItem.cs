using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Replenishment.ListItems
{
    [DataContract]
    public class InventoryTemplateListItem : DataEntity
    {
        [DataMember]
        public int StoreCount { get; internal set; }
        [DataMember]
        public bool AllStores { get; internal set; }
        [DataMember]
        public TemplateEntryTypeEnum Type { get; internal set; }

        [StringLength(100)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

    }
}
