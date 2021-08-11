using System;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JobValidationResult
    {
        public JobValidationMessage Message { get; set; }
        public string Param0 { get; set; }
        public string Param1 { get; set; }

        public string MessageText
        {
            get
            {
                if (Message == JobValidationMessage.OK)
                    return null;
                 
                var EnumType = typeof (JobValidationMessage);
                string name = EnumType.Name + "_" + Enum.GetName(EnumType, Message);
                string resString = Properties.Resources.ResourceManager.GetString(name, System.Threading.Thread.CurrentThread.CurrentUICulture);
                if (Param0 != null && Param1 != null)
                {
                    return string.Format(resString, Param0, Param1);
                }
                else if (Param0 != null)
                {
                    return string.Format(resString, Param0);
                }
                else
                {
                    return resString;
                }
            }
        }
    }

    public enum JobValidationMessage
    {
        OK,
        JobIsNotEnabled,
        SourceLocationIsNotEnabled,
        SourceLocationHasNoDataDirector,
        SourceLocationHasNoDBDesign,
        SourceLocationsHaveMultipleDBDesigns,
        SourceLocationsHaveNoDBDesigns,
        SubjobDBMismatch,
        DestinationLocationTableMapMissing,
        DescriptionMissing,
        SourceLocationMissing,
        DestinationLocationMissing,
        NoSubjobs,
        MissingSubJobActionTable,
    }
}
