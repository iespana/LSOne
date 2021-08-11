using System;
using System.Collections.Generic;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.ValueProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;

namespace LSOne.Services
{
    public partial class LabelService : ILabelService
    {
        private static int runningCounter;

        public virtual IErrorLog ErrorLog
        {
            set { }
        }

        public string LastErrorMessage { get; private set; }

        public void Init(IConnectionManager entry)
        {
        }

        public virtual List<string> GetAvailableMacros<T>() where T : IDataEntity
        {
            LabelValueProviderBase provider = null;
            if (typeof(T) == typeof(RetailItem))
            {
                provider = new RetailItemLabelValueProvider();
            }
            else if (typeof(T) == typeof(Customer))
            {
                provider = new CustomerLabelValueProvider();
            }

            if (provider != null)
            {
                return provider.SupportedMacros;
            }

            return null;
        }

        public virtual bool Print(IConnectionManager entry, List<LabelPrintRequest> requests)
        {
            LastErrorMessage = string.Empty;
            if (requests == null || requests.Count == 0)
            {
                LastErrorMessage = Properties.Resources.NothingToPrint;
                return false;
            }

            // Accumulate labels for each printer (normally only one, but perhaps not when using queue)
            var printerDict = new Dictionary<string, StringBuilder>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var request in requests)
            {
                var tmp = ApplyMacros(entry, request.NumberOfLabels, request.Template.Template, request.Entity);
                if (tmp != null)
                {
                    if (!printerDict.ContainsKey(request.Printer))
                    {
                        printerDict[request.Printer] = new StringBuilder();
                    }

                    printerDict[request.Printer].Append(request.Template.Translate(tmp));
                }
            }

            bool res = true;
            foreach (var printer in printerDict.Keys)
            {
                var buf = printerDict[printer];
                if (buf.Length > 0)
                {
                    if (!RawPrinting.SendStringToPrinter(printer, "LABEL_" + ++runningCounter, buf.ToString()))
                    {
                        if (LastErrorMessage.Length > 0)
                        {
                            LastErrorMessage += Environment.NewLine;
                        }
                        LastErrorMessage += RawPrinting.LastErrorMessage;
                        res = false;
                    }
                }
            }
            return res;
        }

        protected virtual string ApplyMacros(IConnectionManager entry, int numLabels, string template, IDataEntity entity)
        {
            LabelValueProviderBase provider = null;
            if (entity is RetailItem)
            {
                provider = new RetailItemLabelValueProvider();
            }
            else if (entity is Customer)
            {
                provider = new CustomerLabelValueProvider();
            }

            if (provider != null)
            {
                return provider.ApplyMacros(entry, numLabels, template, entity);
            }
            return null;
        }

        public virtual void AddToPrintQueue(IConnectionManager entry, string batch, List<LabelPrintRequest> requests)
        {
            if (requests == null || requests.Count == 0)
            {
                return;
            }

            var lblPrintingService = LSOne.Services.Interfaces.Services.LabelService(entry);
            if (lblPrintingService == null)
            {
                return;
            }

            foreach (var request in requests)
            {
                RecordIdentifier entityID;
                if (request.Entity is RetailItem)
                {
                    entityID = request.Entity.ID;
                }
                else if (request.Entity is Customer)
                {
                    entityID = request.Entity.ID;
                }
                else
                {
                    entityID = GetEntityID(request.Entity);
                }

                if (entityID == null)
                {
                    break;
                }

                var data = new LabelQueue
                    {
                        NumberOfLabels = request.NumberOfLabels,
                        EntityID = entityID,
                        LabelTemplateID = request.Template.ID,
                        Batch = batch,
                        Printer = request.Printer
                    };

                Providers.LabelQueueData.Save(entry, data);
            }
        }

        public virtual bool PrintFromQueue(IConnectionManager entry, string batch)
        {
            bool res = false;
            var queueData = Providers.LabelQueueData.GetList(entry, batch);
            if (queueData != null)
            {
                var requests = new List<LabelPrintRequest>();
                foreach (var data in queueData)
                {
                    var request = new LabelPrintRequest
                        {
                            NumberOfLabels = data.NumberOfLabels,
                            Printer = data.Printer,
                            Template = Providers.LabelTemplateData.Get(entry, data.LabelTemplateID)
                        };
                    if (request.Template.Context == LabelTemplate.ContextEnum.Items)
                    {
                        request.Entity = Providers.RetailItemData.Get(entry, data.EntityID);
                    }
                    else if (request.Template.Context == LabelTemplate.ContextEnum.Customers)
                    {
                        request.Entity = Providers.CustomerData.Get(entry, data.EntityID, UsageIntentEnum.Normal);
                    }

                    requests.Add(request);
                }

                // Print the batch
                res = Print(entry, requests);

                // Mark as printed
                foreach (var data in queueData)
                {
                    Providers.LabelQueueData.SetPrinted(entry, data.ID, LastErrorMessage);
                }
            }

            return res;
        }
    }
}
