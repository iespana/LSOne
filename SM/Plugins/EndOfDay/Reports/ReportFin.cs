using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraReports.UI;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportFin : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportFin()
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for this file are reserved at 47000 - 47049

            InitializeComponent();
            lblDateTime.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        public ReportFin(
            CompanyInfo companyInfo, 
            List<HourlyDataLine> hourlyDataSubReport, 
            List<CurrencyDataLine> currencyDataSubReport,
            XtraReport taxGroupSubReport
            ) : this()
        {
            this.bsCompany.DataSource = companyInfo;             
            bsHourly.DataSource = hourlyDataSubReport;
            bsCurrency.DataSource = currencyDataSubReport;
            decimal totalCurrencyAmount = (from curr in currencyDataSubReport
                                           select curr.Amount).Sum();
            this.totalCurrencyAmount.Text = totalCurrencyAmount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

            this.taxSubReport.ReportSource = taxGroupSubReport;
        }

        public string StoreID
        {
            get { return xrLblStoreID.Text; }
            set { xrLblStoreID.Text = value; }
        }


        public string TotalHourlyCust
        {
            get { return totalHourlyCust.Text; }
            set { totalHourlyCust.Text = value; }
        }

        public string TotalHourlyAmount
        {
            get { return totalHourlyAmount.Text; }
            set { totalHourlyAmount.Text = value; }
        }
        public string TotalHourlyUnits
        {
            get { return totalHourlyUnits.Text; }
            set { totalHourlyUnits.Text = value; }
        }

        public decimal HaettVid
        {
            set { haettVid.Text = value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)); }
        }
        public decimal HeildInnstimplun
        {
            set { heildInnstimplun.Text = value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)); }
        }
        public decimal Dagsala
        {
            set { dagsala.Text = value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)); }
        }
        public decimal LinuAfsl
        {
            set { linuAfsl.Text = value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)); }
        }
        public decimal HeildAfsl
        {
            set { heildAfsl.Text = value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)); }
        }
        public decimal HaettVidFjEin
        {
            set { haettVidFjEin.Text = value.ToString("n0"); }
        }
        public decimal HeildInnstFjEin
        {
            set { heildInnstFjEin.Text = value.ToString("n0"); }
        }
        public decimal DagsalaFjEin
        {
            set { dagsalaFjEin.Text = value.ToString("n0"); }
        }
        public decimal HeildAfslFjVid
        {
            set { heildAfslFjEin.Text = value.ToString("n0"); }
        }
        public decimal LinuAfslFjVid
        {
            set { linuAfslFjEin.Text = value.ToString("n0"); }
        }
        public decimal HaettVidFjVid
        {
            set { haettVidFjVid.Text = value.ToString("n0"); }
        }
        public decimal HeildInnstFjVi
        {
            set { heildInnstFjVi.Text = value.ToString("n0"); }
        }
        public decimal DagsalaFjVi
        {
            set { dagsalaFjVi.Text = value.ToString("n0"); }
        }

        // Notkun skanna
        public string InnsleginnFjoldi
        {
            set { innsleginnFjoldi.Text = value; }
        }
        public string ScanFjoldi
        {
            set { xrLabel87.Text = value; }
        }
        public string InnsleginFjPercent
        {
            set { xrLabel102.Text = value; }
        }
        public string ScanFjPercent
        {
            set { xrLabel103.Text = value; }
        }

        // Samtölur
        public string SkuffaOpnudAnSoluEin
        {
            set { xrLabel84.Text = value; }
        }

        public string FjoldiAfgrIMinusEin
        {
            set { fjoldiAfgrIMinus.Text = value; }
        }
        public string FjoldiAfgrIMinus
        {
            set { xrLabel77.Text = value; }
        }
        public string FjoldiVidskVina
        {
            set { xxxx.Text = value; }
        }
        public string FjoldiEininga
        {
            set { xrLabel98.Text = value; }
        }

        public ReportFinPayment PaymentSubreport
        {
            get { return reportFinPayment2; }
            set { reportFinPayment2 = value; }
        }

        //public ReportFinVAT VATSubreport
        //{
        //    get { return reportFinVAT1; }
        //    set { reportFinVAT1 = value; }
        //}

        public string PeriodDefinition
        {
            get { return periodDefinition.Text; }
            set { periodDefinition.Text = value; }
        }
    }
}
