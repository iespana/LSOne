using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Gets the reason code with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ReasonCode GetReasonById(LogonInfo logonInfo, RecordIdentifier id)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(id)}: {id}");

                return Providers.ReasonCodeData.GetReasonById(entry, id);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of paginated reason codes based on a search criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="description"></param>
        /// <param name="descriptionBeginsWith"></param>
        /// <param name="action"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isSystemCode"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortedBackwards"></param>
        /// <returns></returns>
        public virtual List<ReasonCode> SearchReasonList(LogonInfo logonInfo, string description, bool descriptionBeginsWith, 
            ReasonActionEnum? action, DateTime? beginDate, DateTime? endDate, bool? isSystemCode, ReasonCodeSorting sortBy, bool sortedBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, 
                    $"{Utils.Starting} {nameof(description)}: {description}, {nameof(descriptionBeginsWith)}: {descriptionBeginsWith}, {nameof(action)}: {action}, {nameof(beginDate)}: {beginDate?.ToShortDateString()}, " +
                    $"{nameof(endDate)}: {endDate?.ToShortDateString()}, {nameof(isSystemCode)}: {isSystemCode}, {nameof(sortBy)}: {sortBy}, {nameof(sortedBackwards)}: {sortedBackwards}");

                return Providers.ReasonCodeData.SearchReasonList(entry, description, descriptionBeginsWith, action, beginDate, endDate, isSystemCode, sortBy, sortedBackwards);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes the reason code with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="id"></param>
        public virtual void DeleteReasonCode(LogonInfo logonInfo, RecordIdentifier id)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(id)}: {id}");

                Providers.ReasonCodeData.Delete(entry, id);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list with all available reason codes
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<ReasonCode> GetReasonCodesList(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.ReasonCodeData.GetList(entry);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list of reason codes filtered by the given parameters and sorted by description.
        /// </summary>
        /// <param name="logonInfo">The entry into the database</param>
        /// <param name="actions">Filtering criteria - list of reason actions. If null, it returns all reason codes, disregarding their action.</param>
        /// <param name="forPOS">Filtering criteria - if true returns only reason codes with SHOWONPOS = 1</param>
        /// <param name="open">Filtering criteria - if true returns only open reason codes</param>
        /// <returns>List of <see cref="ReasonCode"/></returns>
        public virtual List<ReasonCode> GetReasonCodesList(LogonInfo logonInfo,
                                                           List<ReasonActionEnum> actions, 
                                                           bool? forPOS = null, bool open = true)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(actions)}: {string.Join(";", actions)}, {nameof(forPOS)}: {forPOS}, {nameof(open)}: {open}");

                return Providers.ReasonCodeData.GetList(entry, actions, forPOS, open);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns a list with all reason codes that can be used. This is based on the begin and end date of the reason code configurations.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<ReasonCode> GetOpenReasonCodesList(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.ReasonCodeData.GetOpenReasonCodesList(entry);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves a reason code
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="reasonCode"></param>
        public virtual void SaveReasonCode(LogonInfo logonInfo, ReasonCode reasonCode)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.ReasonCodeData.Save(entry, reasonCode);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns true if a reason code is in use; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        public virtual bool ReasonCodeIsInUse(LogonInfo logonInfo, RecordIdentifier reasonId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(reasonId)}: {reasonId}");

                return Providers.ReasonCodeData.ReasonCodeIsInUse(entry, reasonId);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the list of reason codes used for returns
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        public virtual List<ReasonCode> GetReasonCodesForReturn(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.ReasonCodeData.GetReasonCodesForReturn(entry);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}