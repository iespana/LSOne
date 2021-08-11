namespace LSOne.ViewPlugins.Scheduler.Sequences
{
    internal class JobTypeSequence : ISequenceable
    {
        public bool SequenceExists(IConnectionManager entry, Utilities.DataTypes.RecordIdentifier id)
        {
            using (JobModel jobModel = new JobModel())
            {
                return jobModel.ExistsJobType((string)id);
            }
        }

        public Utilities.DataTypes.RecordIdentifier SequenceID
        {
            get { return "JscJobType"; }
        }
    }
}
