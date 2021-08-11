namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class TableMapSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (DesignModel model = new DesignModel())
            {
                return model.ExistsTableMap((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscTabMap"; }
        }
    }

}
