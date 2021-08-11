using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities.BarCodeTypes
{
    internal class BarCodeType : DataEntity
    {
        public BarCodeType()
            : base()
        {

        }

        public virtual List<DataEntity> GetValidFonts()
        {
            return new List<DataEntity>();
        }

        public virtual string Name
        {
            get
            {
                return Properties.Resources.NoBarCode;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual int MinimumLength
        {
            get
            {
                return 0;
            }
        }

        public virtual int MaximumLength
        {
            get
            {
                return 0;
            }
        }

        public virtual string DefaultFont
        {
            get
            {
                return "";
            }
        }

        public virtual int TypeNumber
        {
            get
            {
                return 0;
            }
        }


        public static BarCodeType GetBarCodeType(int barCodeType)
        {
            switch (barCodeType)
            {
                case 0:
                    return new BarCodeType();
                
                case 1:
                    return new BarcodeEAN128();

                case 2:
                    return new BarcodeCode39();

                case 3:
                    return new BarcodeITF();

                case 4:
                    return new BarcodeCode128();

                case 5:
                    return new BarcodeUPCA();
                
                case 6:
                    return new BarcodeUPCE();
            
                case 7:
                    return new BarcodeEAN13();
                
                case 8:
                    return new BarcodeEAN8();
               
                case 9: // 101 in AX
                    return new BarcodePDF417();

                case 10: // 102 in AX
                    return new BarcodeMaxicode();

            }

            return new BarCodeType();
        }
    }
}
