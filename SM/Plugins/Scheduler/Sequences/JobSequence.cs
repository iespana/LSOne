namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class JobSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (JobModel jobModel = new JobModel())
            {
                return jobModel.ExistsJob((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscJob"; }
        }
    }
}
