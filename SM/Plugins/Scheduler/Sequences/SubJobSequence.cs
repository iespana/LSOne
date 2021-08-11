namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class SubJobSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (JobModel jobModel = new JobModel())
            {
                return jobModel.ExistsSubJob((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscSubJob"; }
        }
    }
}
