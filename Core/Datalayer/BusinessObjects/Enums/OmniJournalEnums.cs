using LSOne.Utilities.Development;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [LSOneUsage(CodeUsage.LSCommerce)]
    [DataContract(Name = "OmniJournalStatus")]
    public enum OmniJournalStatus
    {
        /// <summary>
        /// Received from device, no processing has been started
        /// </summary>
        [EnumMember]
        Received,
        /// <summary>
        /// The journal is being processed by a thread
        /// </summary>
        [EnumMember]
        InProcess,
        /// <summary>
        /// The thread has finished processing the journal
        /// </summary>
        [EnumMember]
        Done
    }

    [LSOneUsage(CodeUsage.LSCommerce)]
    [DataContract(Name = "OmniJournalProcessingResult")]
    public enum OmniJournalProcessingResult
    {
        /// <summary>
        /// The processing of the OMNI journal was successful
        /// </summary>
        [EnumMember]
        Success,

        /// <summary>
        /// Journal was not found
        /// </summary>
        [EnumMember]
        JournalNotFound,

        /// <summary>
        /// The list of journal lines to be processed was empty
        /// </summary>
        [EnumMember]
        NoJournalLines,
        /// <summary>
        /// An error occured when processing the journal
        /// </summary>
        [EnumMember]
        ErrorProcessingJournal


    }

    [LSOneUsage(CodeUsage.LSCommerce)]
    [DataContract(Name = "SendOmniJournalResult")]
    public enum SendOmniJournalResult
    {
        /// <summary>
        /// Omni journal was sent succesfully
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The template linked to the omni journal was not found
        /// </summary>
        [EnumMember]
        TemplateNotFound,
        /// <summary>
        /// There was an error sending the omni journal
        /// </summary>
        [EnumMember]
        Error
    }
}
