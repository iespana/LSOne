namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class KitchenServiceProfile : DataEntity
    {
        public KitchenServiceProfile()
        {
            KitchenServiceAddress = "";
            KitchenServicePort = 17750;
 
        }
        /// <summary>
        /// A location of the kitchen manager server
        /// </summary>
        ///
        public string KitchenServiceAddress { get; set; }
        
        /// <summary>
        /// Port number of the kitchen manager
        /// </summary>
        /// 
        public int KitchenServicePort { get; set; }

    }
}
