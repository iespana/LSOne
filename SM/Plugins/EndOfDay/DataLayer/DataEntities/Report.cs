using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    //Pending refactoring
    public class Report : BasePropertyClass
    {
        private decimal dagsSala;
        private int dagsSalaAfgreidslur;
        private decimal dagsSalaEiningar;
        private decimal heildarInnst;
        private int heildarInnstAfgreidslur;
        private decimal heildarInnstEiningar;
        private decimal lagad;
        private int lagadAfgreidslur;
        private decimal lagadEiningar;
        private decimal haettVid;
        private int haettVidAfgreidslur;
        private decimal haettVidEiningar;
        private decimal skilad;
        private int skiladAfgreidslur;
        private decimal skiladEiningar;
        private int linuAfslFjVid;
        private decimal linuAfsl;
        private int heildAfslFjVid;
        private decimal heildAfsl;
        private decimal heildarSummur;
        private decimal innsleginnFjoldi;
        private int fjoldiAfgrIMinus;
        private DataTable tenderTable;
        private DataTable subTenderTable;
                
        public int FjoldiAfgrIMinus
        {
            get { return fjoldiAfgrIMinus; }
            set { fjoldiAfgrIMinus = value; }
        }
        public decimal InnsleginnFjoldi
        {
            get { return innsleginnFjoldi; }
            set { innsleginnFjoldi = value; }
        }
        public decimal DagsSala
        {
            get { return dagsSala; }
            set { dagsSala = value; }
        }
        public int DagsSalaAfgreidslur
        {
            get { return dagsSalaAfgreidslur; }
            set { dagsSalaAfgreidslur = value; }
        }
        public decimal DagsSalaEiningar
        {
            get { return dagsSalaEiningar; }
            set { dagsSalaEiningar = value; }
        }
        public decimal HeildarInnst
        {
            get { return heildarInnst; }
            set { heildarInnst = value; }
        }
        public int HeildarInnstAfgreidslur
        {
            get { return heildarInnstAfgreidslur; }
            set { heildarInnstAfgreidslur = value; }
        }
        public decimal HeildarInnstEiningar
        {
            get { return heildarInnstEiningar; }
            set { heildarInnstEiningar = value; }
        }
        public decimal Lagad
        {
            get { return lagad; }
            set { lagad = value; }
        }
        public int LagadAfgreidslur
        {
            get { return lagadAfgreidslur; }
            set { lagadAfgreidslur = value; }
        }
        public decimal LagadEiningar
        {
            get { return lagadEiningar; }
            set { lagadEiningar = value; }
        }
        public decimal HaettVid
        {
            get { return haettVid; }
            set { haettVid = value; }
        }
        public int HaettVidAfgreidslur
        {
            get { return haettVidAfgreidslur; }
            set { haettVidAfgreidslur = value; }
        }
        public decimal HaettVidEiningar
        {
            get { return haettVidEiningar; }
            set { haettVidEiningar = value; }
        }
        public decimal Skilad
        {
            get { return skilad; }
            set { skilad = value; }
        }
        public int SkiladAfgreidslur
        {
            get { return skiladAfgreidslur; }
            set { skiladAfgreidslur = value; }
        }
        public decimal SkiladEiningar
        {
            get { return skiladEiningar; }
            set { skiladEiningar = value; }
        }
        public int LinuAfslFjVid
        {
            get { return linuAfslFjVid; }
            set { linuAfslFjVid = value; }
        }
        public decimal LinuAfsl
        {
            get { return linuAfsl; }
            set { linuAfsl = value; }
        }
        public int HeildAfslFjVid
        {
            get { return heildAfslFjVid; }
            set { heildAfslFjVid = value; }
        }
        public decimal HeildAfsl
        {
            get { return heildAfsl; }
            set { heildAfsl = value; }
        }
        public decimal HeildarSummur
        {
            get { return heildarSummur; }
            set { heildarSummur = value; }
        }
        public DataTable SubTenderTable
        {
            get { return subTenderTable; }
            set { subTenderTable = value; }
        }
        public DataTable TenderTable
        {
            get { return tenderTable; }
            set { tenderTable = value; }
        }

        public override string[] GetIDs()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
