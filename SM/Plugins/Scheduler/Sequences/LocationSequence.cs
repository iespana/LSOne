namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class LocationSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (LocationModel model = new LocationModel())
            {
                return model.ExistsLocation((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscLocatio"; }
        }
    }

}
