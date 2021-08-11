namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class DatabaseDesignSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (DesignModel model = new DesignModel())
            {
                return model.ContainsDatabaseDesign((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscDbDesig"; }
        }
    }

}
