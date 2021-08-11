using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.Development;
using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LSOne.Services
{
	[LSOneUsage(CodeUsage.Partner, "This code file is for partners to add new functionality to this service")]
	public partial class FormModulation
	{
		/// <summary>
		/// Returns the partner's value for existing printing variable.
		/// </summary>
		/// <param name="variable"></param>
		/// <param name="eftInfo"></param>
		/// <param name="tenderItem"></param>
		/// <param name="trans"></param>
		/// <param name="formPart"></param>
		/// <param name="variableChanged"></param>
		/// <returns></returns>
		[LSOneUsage(CodeUsage.Partner)]

        //**INICIA CONNECCION Y VALIDACION**//
        public virtual string ShowForm(string Istore, string ReceiptID, int Opcion)
        {
            string returnValue = string.Empty;
            SqlConnection sqlConn = null;
            SqlDataReader sqlDr = null;
            string ServerConn = string.Empty;

            using (var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = root.OpenSubKey(@"Software\AeSolutions", false))
                {
                    ServerConn = (string)key.GetValue("Server");
                };
            };

            try
            {
                sqlConn = new SqlConnection(ServerConn); //"server = LSROSANEGRA\\SQLEXPRESS ; database = LS_RosaNegraBR; integrated security = true");
                SqlCommand sqlCmd = new SqlCommand("[Dbo].[SSDEIAN]", sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@REceiptID", SqlDbType.NVarChar).Value = ReceiptID;
                sqlCmd.Parameters.AddWithValue("@Storeid", SqlDbType.NVarChar).Value = Istore;
                sqlCmd.Parameters.AddWithValue("@Op", SqlDbType.Int).Value = Opcion;
                sqlConn.Open();
                sqlDr = sqlCmd.ExecuteReader();
                while (sqlDr.Read())
                {
                    returnValue = (string)sqlDr["Resultado"];
                }
                sqlConn.Close();
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                }
                if (sqlDr != null)
                {
                    sqlDr.Close();
                }
            }


            return returnValue;
        }
        protected string PartnerGetInfoFromTransaction(string variable, IEFTInfo eftInfo, ITenderLineItem tenderItem, IPosTransaction trans, FormPartEnum formPart, out bool variableChanged)
		{
			variableChanged = false;
			return variable;
		}

		/// <summary>
		/// Returns the value of the <i>EXTRAINFO</i> variable (there are 20 <i>EXTRAINFO</i> variables + header and footer ones).
		/// </summary>
		/// <param name="itemInfo"></param>
		/// <param name="eftInfo"></param>
		/// <param name="tenderItem"></param>
		/// <param name="trans"></param>
		/// <param name="formPart"></param>
		/// <returns></returns>
		[LSOneUsage(CodeUsage.Partner)]
		protected string GetPartnerInfoFromTransaction(FormItemInfo itemInfo, IEFTInfo eftInfo, ITenderLineItem tenderItem, IPosTransaction trans, FormPartEnum formPart)
		{
			string returnValue = string.Empty;
			switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
			{
                case "EXTRAINFO1"://Documento Fiscal
                    switch (formPart)
                    {
                        case FormPartEnum.Header: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 1); break;
                        case FormPartEnum.Footer: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 1); break;
                        default: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 1); break;
                    }
                    break;

                case "EXTRAINFO2"://CAI
                    switch (formPart)
                    {
                        case FormPartEnum.Header: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 2); break;
                        case FormPartEnum.Footer: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 2); break;
                        default: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 2); break;
                    }
                    break;

                case "EXTRAINFO3"://Fecha Limite Emision
                    switch (formPart)
                    {
                        case FormPartEnum.Header: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 3); break;
                        case FormPartEnum.Footer: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 3); break;
                        default: returnValue = ShowForm(trans.StoreId, trans.ReceiptId, 3); break;
                    }
                    break;

                case "EXTRAINFO4"://Desde + Hasta numeracion fiscal
                    switch (formPart)
                    {
                        case FormPartEnum.Header: returnValue = "Desde: " + ShowForm(trans.StoreId, trans.ReceiptId, 4) + Environment.NewLine +
                                                                " Hasta: " + ShowForm(trans.StoreId, trans.ReceiptId, 5); break;
                        case FormPartEnum.Footer: returnValue = "Desde: " + ShowForm(trans.StoreId, trans.ReceiptId, 4) + Environment.NewLine +
                                                                " Hasta: " + ShowForm(trans.StoreId, trans.ReceiptId, 5); break;
                        default: returnValue = "Desde: " + ShowForm(trans.StoreId, trans.ReceiptId, 4) + Environment.NewLine +
                                               " Hasta: " + ShowForm(trans.StoreId, trans.ReceiptId, 5); break;
                    }
                    break;

                case "EXTRAINFO5"://info SAG,Exento...
                    //var infocode = (LSOne.DataLayer.BusinessObjects.Transactions.Line.InfoCodeLineItem)trans;
                    var exo = string.Empty;
                   
                        exo ="No. Registro SAG : "+ Environment.NewLine+
                             " No. Orden de Compra Exenta : " + "\r" +
                             " No. Registro Exonerado : " + "\r" +
                             " No. Carnet Exonerado : ";

                    switch (formPart)
                    {

                    case FormPartEnum.Header: returnValue = exo; break;
                    case FormPartEnum.Footer: returnValue = exo; break;
                    default: returnValue = exo; break;

                    }
                    break;

                case "EXTRAINFO6"://Referencia nota de Credito
                    bool dev = IsReturnTransaction(trans);
                    if (dev == true)
                    {
                        switch (formPart)
                        {
                            case FormPartEnum.Header:
                                returnValue = "Factura: " + ShowForm(trans.StoreId, trans.ReceiptId, 7) + Environment.NewLine
                                            + " Fecha y Hora: " + ShowForm(trans.StoreId, trans.ReceiptId, 8) + "\r"
                                            + " CAI: " + ShowForm(trans.StoreId, trans.ReceiptId, 9); break;
                            case FormPartEnum.Footer:
                                returnValue = "Factura: " + ShowForm(trans.StoreId, trans.ReceiptId, 7) + Environment.NewLine
                                            + " Fecha y Hora: " + ShowForm(trans.StoreId, trans.ReceiptId, 8) + "\r"
                                            + " CAI: " + ShowForm(trans.StoreId, trans.ReceiptId, 9); break;
                            default:
                                returnValue = "Factura: " + ShowForm(trans.StoreId, trans.ReceiptId, 7) + Environment.NewLine
                                 + " Fecha y Hora: " + ShowForm(trans.StoreId, trans.ReceiptId, 8) + "\r"
                                 + " CAI: " + ShowForm(trans.StoreId, trans.ReceiptId, 9); break;
                        }
                        break;

                    }

                    break;

                case "EXTRAINFO7"://numeros a letras
                    decimal transaction = ((LSOne.DataLayer.TransactionObjects.RetailTransaction)trans).NetAmountWithTax;

                    string numtoword = FormModulation.ConvertToWords(transaction);
                    string wrapped = FormModulation.WordWrap(numtoword,itemInfo.Length);
                    
                    switch (formPart)
                    {

                        case FormPartEnum.Header: returnValue = wrapped; break;
                        case FormPartEnum.Footer: returnValue = wrapped; break;
                        default: returnValue = wrapped; break;


                    }

                    break;

                case "EXTRAINFO8"://titulo
                    var titulo = string.Empty;
                    bool devolucion = IsReturnTransaction(trans);

                    if (devolucion == true)
                    {
                        titulo = "NOTA DE CREDITO";

                    }
                    else
                    {
                        titulo = "FACTURA CONTADO";
                    }

                    switch (formPart)

                    {
                        case FormPartEnum.Header: returnValue = titulo; break;
                        case FormPartEnum.Footer: returnValue = titulo; break;
                        default: returnValue = titulo; break;
                    }
                    break;

                case "EXTRAINFO9": //Impuestos
                    string Exonerado = string.Empty;
                    string tx15 = string.Empty;
                    string Imp15 = string.Empty;
                    string tx18 = string.Empty;
                    string Imp18 = string.Empty;
                    string Imp0 = string.Empty;

                    var retailTransaction = ((DataLayer.TransactionObjects.RetailTransaction)trans).ITaxLines;
                    var retailTrans = ((DataLayer.TransactionObjects.RetailTransaction)trans);
                    if (retailTrans != null)
                    {
                        if (retailTrans.Customer.TaxExempt.Equals(true))
                        {
                            Exonerado =decimal.ToDouble(retailTrans.NetAmount).ToString("F");
                            //tx15 = " ISV 15% L " + decimal.ToDouble(decimal.Zero).ToString("F") + "\r\n";
                            //Imp15 = " Importe Gravado 15%  L " + decimal.ToDouble(decimal.Zero).ToString("F") + "\r\n";
                            //tx18 = " ISV 18% L " + decimal.ToDouble(decimal.Zero).ToString("F") + "\r\n";
                            //Imp18 = " Importe Gravado 18%  L " + decimal.ToDouble(decimal.Zero).ToString("F") + "\r\n";
                            //Imp0 = " Importe Exento L " + decimal.ToDouble(decimal.Zero).ToString("F") + "\r\n";
                        }
                        else
                        {
                            Exonerado =decimal.ToDouble(decimal.Zero).ToString("F");
                        }

                            foreach (var codigo in retailTransaction)
                            {
                                if (codigo.ItemTaxGroup == "15")
                                {
                                     tx15 =decimal.ToDouble(codigo.Amount).ToString("F");

                                    Imp15 =(decimal.ToDouble(codigo.PriceWithTax) - decimal.ToDouble(codigo.Amount)).ToString("F");
                                    break;
                                }
                                else
                                {
                                     tx15 =decimal.ToDouble(decimal.Zero).ToString("F");
                                    Imp15 =decimal.ToDouble(decimal.Zero).ToString("F");

                                }   
                            }

                            foreach (var codigo in retailTransaction)
                            {

                                if (codigo.ItemTaxGroup == "18")
                                {
                                    tx18 =decimal.ToDouble(codigo.Amount).ToString("F");
                                   Imp18 =(decimal.ToDouble(codigo.PriceWithTax) - decimal.ToDouble(codigo.Amount)).ToString("F");
                                    break;
                                }
                                else
                                {
                                     tx18 =decimal.ToDouble(decimal.Zero).ToString("F");
                                    Imp18 =decimal.ToDouble(decimal.Zero).ToString("F");

                            }

                            }

                            foreach (var codigo in retailTransaction)
                            {
                                if (codigo.ItemTaxGroup == "0")
                                {
                                    Imp0 =decimal.Round(codigo.PriceWithTax).ToString("F");
                                    break;
                                }
                                else
                                {
                                    Imp0 =decimal.ToDouble(decimal.Zero).ToString("F");

                            }

                            }
                        
                    }

                    switch (formPart)
                    {

                        case FormPartEnum.Header:
                            returnValue +="Importe Exonerado                 L " + Exonerado.PadLeft(11, ' ') + Environment.NewLine +
                                          "Importe EXENTO                    L " + Imp0.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 15%               L " + Imp15.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 18%               L " + Imp18.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 15%                        L " + tx15.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 18%                        L " + tx18.PadLeft(11, ' '); break;
                        case FormPartEnum.Footer:
                            returnValue +="Importe Exonerado                 L " + Exonerado.PadLeft(11, ' ') + Environment.NewLine +
                                          "Importe EXENTO                    L " + Imp0.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 15%               L " + Imp15.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 18%               L " + Imp18.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 15%                        L " + tx15.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 18%                        L " + tx18.PadLeft(11, ' '); break;
                        default:
                            returnValue +="Importe Exonerado                 L " + Exonerado.PadLeft(11, ' ') +Environment.NewLine+
                                          "Importe EXENTO                    L " + Imp0.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 15%               L " + Imp15.PadLeft(11, ' ') + "\r" +
                                          "Importe Gravado 18%               L " + Imp18.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 15%                        L " + tx15.PadLeft(11, ' ') + "\r" +
                                          "I.S.V. 18%                        L " + tx18.PadLeft(11, ' '); break;
                    }

                    break;

				case "EXTRAINFO10"://Descuentos y Rebajas
                    var desc = decimal.ToDouble(((DataLayer.TransactionObjects.RetailTransaction)trans).TotalDiscountWithTax + 
                               ((DataLayer.TransactionObjects.RetailTransaction)trans).LineDiscountWithTax + 
                               ((DataLayer.TransactionObjects.RetailTransaction)trans).PeriodicDiscountWithTax).ToString("F");

                    switch (formPart)
                    {
                        case FormPartEnum.Header: returnValue ="Descuentos y Rebajas Otorgadas:   L "+ desc.PadLeft(11,' '); break;
                        case FormPartEnum.Footer: returnValue ="Descuentos y Rebajas Otorgadas:   L " + desc.PadLeft(11,' '); break;
                        default: returnValue ="Descuentos y Rebajas Otorgadas:   L " + desc.PadLeft(11,' '); break;
                    }
                    
					
					break;
				case "EXTRAINFO11":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO11_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO11_FOOTER"; break;
						default: returnValue = "EXTRAINFO11"; break;
					}
					break;

				case "EXTRAINFO12":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO12_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO12_FOOTER"; break;
						default: returnValue = "EXTRAINFO12"; break;
					}
					break;

				case "EXTRAINFO13":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO13_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO13_FOOTER"; break;
						default: returnValue = "EXTRAINFO13"; break;
					}
					break;

				case "EXTRAINFO14":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO14_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO14_FOOTER"; break;
						default: returnValue = "EXTRAINFO14"; break;
					}
					break;

				case "EXTRAINFO15":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO15_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO15_FOOTER"; break;
						default: returnValue = "EXTRAINFO15"; break;
					}
					break;
				case "EXTRAINFO16":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO16_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO16_FOOTER"; break;
						default: returnValue = "EXTRAINFO16"; break;
					}
					break;

				case "EXTRAINFO17":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO17_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO17_FOOTER"; break;
						default: returnValue = "EXTRAINFO17"; break;
					}
					break;

				case "EXTRAINFO18":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO18_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO18_FOOTER"; break;
						default: returnValue = "EXTRAINFO18"; break;
					}
					break;

				case "EXTRAINFO19":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO19_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO19_FOOTER"; break;
						default: returnValue = "EXTRAINFO19"; break;
					}
					break;

				case "EXTRAINFO20":
					switch (formPart)
					{
						case FormPartEnum.Header: returnValue = "EXTRAINFO20_HEADER"; break;
						case FormPartEnum.Footer: returnValue = "EXTRAINFO20_FOOTER"; break;
						default: returnValue = "EXTRAINFO20"; break;
					}
					break;
			}

			return returnValue;
		}

		[LSOneUsage(CodeUsage.Partner)]
		protected string GetPartnerInfoFromSaleLineItem(IConnectionManager entry, FormItemInfo itemInfo, SaleLineItem saleLine, out bool skipIfEmptyLine)
		{
			skipIfEmptyLine = false;
			string returnValue = string.Empty;

			switch (itemInfo.Variable.ToUpperInvariant().Replace(" ", string.Empty))
			{
				case "EXTRAINFO1": returnValue = "EXTRAINFO1"; break;
				case "EXTRAINFO2": returnValue = "EXTRAINFO2"; break;
				case "EXTRAINFO3": returnValue = "EXTRAINFO3"; break;
				case "EXTRAINFO4": returnValue = "EXTRAINFO4"; break;
				case "EXTRAINFO5": returnValue = "EXTRAINFO5"; break;
				case "EXTRAINFO6": returnValue = "EXTRAINFO6"; break;
				case "EXTRAINFO7": returnValue = "EXTRAINFO7"; break;
				case "EXTRAINFO8": returnValue = "EXTRAINFO8"; break;
				case "EXTRAINFO9": returnValue = "EXTRAINFO9"; break;
				case "EXTRAINFO10": returnValue = "EXTRAINFO10"; break;
				case "EXTRAINFO11": returnValue = "EXTRAINFO11"; break;
				case "EXTRAINFO12": returnValue = "EXTRAINFO12"; break;
				case "EXTRAINFO13": returnValue = "EXTRAINFO13"; break;
				case "EXTRAINFO14": returnValue = "EXTRAINFO14"; break;
				case "EXTRAINFO15": returnValue = "EXTRAINFO15"; break;
				case "EXTRAINFO16": returnValue = "EXTRAINFO16"; break;
				case "EXTRAINFO17": returnValue = "EXTRAINFO17"; break;
				case "EXTRAINFO18": returnValue = "EXTRAINFO18"; break;
				case "EXTRAINFO19": returnValue = "EXTRAINFO19"; break;
				case "EXTRAINFO20": returnValue = "EXTRAINFO20"; break;
			}

			return returnValue;
		}

		[LSOneUsage(CodeUsage.Partner)]
		protected string GetPartnerInfoFromTaxItem(FormItemInfo itemInfo, TaxItem taxLine)
		{
			string returnValue = string.Empty;
			return returnValue;
		}

		[LSOneUsage(CodeUsage.Partner)]
		protected string GetPartnerInfoFromTenderLineItem(FormItemInfo itemInfo, ITenderLineItem tenderLine)
		{
			string returnValue = string.Empty;
			return returnValue;
		}
	}
}