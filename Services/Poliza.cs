using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using CFDIWEB.Models;
using CFDIWEB.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Common;
using NuGet.Protocol;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using CFDIWEB.Models.ComprobanteLectura;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using CFDIWEB.CFDI4;
 

namespace CFDIWEB.Services
{
    public class Poliza: IPoliza
    {

        public object MessageBox { get; private set; }

        private MyAppDbContext _dbContext;

        //Listas de tablas
        
        public  Poliza(MyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //creacion de archivo excel
        public void CreacionExcel()
        {

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("Excel is not properly installed!!");
                return;
            }


            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);



            /*if (Cfdi != null) {
             contGlobal++;
             xlWorkSheet.Cells[contGlobal, 1] = Cfdi.Cuenta;
             xlWorkSheet.Cells[contGlobal, 2] = Cfdi.RFCEmisor;
             xlWorkSheet.Cells[contGlobal, 3] = Cfdi.NombreProveedor;
             xlWorkSheet.Cells[contGlobal, 4] = "";
             xlWorkSheet.Cells[contGlobal, 5] = "";
             xlWorkSheet.Cells[contGlobal, 6] = Cfdi.Importe;
             xlWorkSheet.Cells[contGlobal, 7] = "";
             xlWorkSheet.Cells[contGlobal, 8] = Cfdi.Foliofactura;
             xlWorkSheet.Cells[contGlobal, 9] = Cfdi.TimbreFislcal.uuid;
           }*/

            


            List<Cfdi> ListaCFDIS = leerCFDI();
            int contGlobal = 1;
            foreach (Cfdi Cfdi in ListaCFDIS)
            {
          
                if (Cfdi.Conceptos != null)
                {
                    //Contruye encabezados del EXCEL
                    xlWorkSheet.Cells[contGlobal, 1] = "Cuenta";
                    xlWorkSheet.Cells[contGlobal, 2] = "Rfc Emisor";
                    xlWorkSheet.Cells[contGlobal, 3] = "Nombre";
                    xlWorkSheet.Cells[contGlobal, 4] = "Cabrgo M.E.";
                    xlWorkSheet.Cells[contGlobal, 5] = "Abono M.E.";
                    xlWorkSheet.Cells[contGlobal, 6] = "Cargo";
                    xlWorkSheet.Cells[contGlobal, 7] = "Abono";
                    xlWorkSheet.Cells[contGlobal, 8] = "Referencia";
                    xlWorkSheet.Cells[contGlobal, 9] = "Concepto";
                   

                    foreach (Concepto Concepto in Cfdi.Conceptos)
                    {
                        contGlobal++;
                        xlWorkSheet.Cells[contGlobal, 1] = Cfdi.Cuenta;
                        xlWorkSheet.Cells[contGlobal, 2] = Cfdi.RFCEmisor;
                        xlWorkSheet.Cells[contGlobal, 3] = Concepto.ClaveOServicio;
                        xlWorkSheet.Cells[contGlobal, 4] = "";
                        xlWorkSheet.Cells[contGlobal, 5] = "";
                        xlWorkSheet.Cells[contGlobal, 6] = Concepto.Importe;
                        xlWorkSheet.Cells[contGlobal, 7] = "";
                        xlWorkSheet.Cells[contGlobal, 8] = Cfdi.Foliofactura;
                        xlWorkSheet.Cells[contGlobal, 9] = Cfdi.TimbreFislcal.uuid;

                        contGlobal++;
                        xlWorkSheet.Cells[contGlobal, 1] = Cfdi.Cuenta;
                        xlWorkSheet.Cells[contGlobal, 2] = "yhuejtyfhty";
                        xlWorkSheet.Cells[contGlobal, 3] = "Proveedores";
                        xlWorkSheet.Cells[contGlobal, 4] = "";
                        xlWorkSheet.Cells[contGlobal, 5] = "";
                        xlWorkSheet.Cells[contGlobal, 6] = "";
                        xlWorkSheet.Cells[contGlobal, 7] = Cfdi.Importe;
                        xlWorkSheet.Cells[contGlobal, 8] = Cfdi.Foliofactura;
                        xlWorkSheet.Cells[contGlobal, 9] = Cfdi.TimbreFislcal.uuid;

                        foreach (ImpuestoConcepto Impuesto in Concepto.Impuestos)
                        {
                            contGlobal++;
                            xlWorkSheet.Cells[contGlobal, 1] = Cfdi.Cuenta;
                            xlWorkSheet.Cells[contGlobal, 2] = Cfdi.RFCEmisor;
                            xlWorkSheet.Cells[contGlobal, 3] = "iva";
                            xlWorkSheet.Cells[contGlobal, 4] = "";
                            xlWorkSheet.Cells[contGlobal, 5] = "";
                            xlWorkSheet.Cells[contGlobal, 6] = Impuesto.ImporteImpuesto;
                            xlWorkSheet.Cells[contGlobal, 7] = "";
                            xlWorkSheet.Cells[contGlobal, 8] = Cfdi.Foliofactura;
                            xlWorkSheet.Cells[contGlobal, 9] = Cfdi.TimbreFislcal.uuid;

                            contGlobal++;
                            xlWorkSheet.Cells[contGlobal, 1] = Cfdi.Cuenta;
                            xlWorkSheet.Cells[contGlobal, 2] = "yhuejtyfhty";
                            xlWorkSheet.Cells[contGlobal, 3] = "Proveedores";
                            xlWorkSheet.Cells[contGlobal, 4] = "";
                            xlWorkSheet.Cells[contGlobal, 5] = "";
                            xlWorkSheet.Cells[contGlobal, 6] = "";
                            xlWorkSheet.Cells[contGlobal, 7] = Cfdi.Importe;
                            xlWorkSheet.Cells[contGlobal, 8] = Cfdi.Foliofactura;
                            xlWorkSheet.Cells[contGlobal, 9] = Cfdi.TimbreFislcal.uuid;

                        }

                       

                    }


                }


            }


            xlWorkBook.SaveAs("C:\\DesagarCFDIWEB\\Polizas\\Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);


        }

        private List<Cfdi> leerCFDI()
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\DesagarCFDIWEB\unzip");
            Console.WriteLine("No search pattern returns:");

            List<Cfdi> listaCFDIS =  new List<Cfdi>();                                         
            foreach (var FileName in di.GetFiles())
            {
                string pathxml = @$"{FileName}";


                XmlDocument doc = new XmlDocument();
                doc.Load(pathxml);

                XmlNodeList elemList = doc.GetElementsByTagName("cfdi:Comprobante");
                XmlAttributeCollection atributos = elemList[0].Attributes;
                string version = "";
                for (int j = 0; j < atributos.Count; j++)
                {
                    if (atributos[j].Name == "Version")
                    {
                        version = atributos[j].Value;
                    }

                }

                Console.WriteLine(version);

                if (version == "3.3"){

                    //version 3.3
                    // 1 paso leer el xml pasar timbrado a una clase 
                    Comprobante ocomprobante = new Comprobante();
                    XmlSerializer oSerializer = new XmlSerializer(typeof(Comprobante));

                    Cfdi Cfdi = new Cfdi();
                    using (StreamReader reader = new StreamReader(pathxml))
                    {
                        //aqui desearilizamos 

                        try
                        {
                            ocomprobante = (Comprobante)oSerializer.Deserialize(reader);


                            //Obtengo datos del complemento
                            foreach (var oComplemento in ocomprobante.Complemento)
                            {
                                foreach (var ocomplementointerior in oComplemento.Any)
                                {
                                    if (ocomplementointerior.Name.Contains("TimbreFiscalDigital"))

                                    {
                                        XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));

                                        using (var readerComplemento = new StringReader(ocomplementointerior.OuterXml))
                                        {
                                            ocomprobante.TimbreFiscalDigital =
                                                (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);

                                            if (ocomprobante != null)
                                            {

                                                Cfdi.TimbreFislcal = new TimbreFiscal();
                                                Cfdi.TimbreFislcal.uuid = ocomprobante.TimbreFiscalDigital.UUID;
                                            }

                                        }
                                    }

                                }
                            }


                            //Obtengo datos del generales del CFDI
                            Cfdi.Cuenta = "102022";
                            Cfdi.RFCEmisor = ocomprobante.Emisor.Rfc;
                            Cfdi.NombreProveedor = _dbContext
                                     .Provedores
                                     .Where(u => u.rfc == Cfdi.RFCEmisor)
                                     .Select(u => u.nombre)
                                     .SingleOrDefault();

                            Cfdi.Foliofactura = ocomprobante.Folio;
                            Cfdi.Importe = ocomprobante.Total.ToString();



                            //Obtengo datos de los conceptos
                            if (ocomprobante != null)
                            {
                                List<Concepto>? Conceptos = new List<Concepto>();
                                List<ImpuestoConcepto> Impuestos = new List<ImpuestoConcepto>();
                                Concepto Concepto = new Concepto();
                                ImpuestoConcepto ImpuestoConcepto = new ImpuestoConcepto();

                                for (int cont = 0; cont <= ocomprobante.Conceptos.Length - 1; cont++)
                                {
                                    Concepto = new Concepto();
                                    Concepto.ClaveOServicio = ocomprobante.Conceptos[cont].ClaveProdServ.ToString();    //fallo aqui 
                                    Concepto.Descripcion = ocomprobante.Conceptos[cont].Descripcion;
                                    Concepto.DescripcionCatalogo = _dbContext
                                                                   .Productos
                                                                   .Where(u => u.claveprodserv.ToString() == Concepto.ClaveOServicio)
                                                                   .Select(u => u.descripcion)
                                                                   .SingleOrDefault();
                                    Concepto.Importe = ocomprobante.Conceptos[cont].Importe.ToString();
                                    Impuestos = new List<ImpuestoConcepto>();

                                    if (ocomprobante.Conceptos[cont].Impuestos.Traslados != null)
                                    {
                                        for (int conta = 0; conta <= ocomprobante.Conceptos[cont].Impuestos.Traslados.Length - 1; conta++)
                                        {
                                            //String[] oimpuestos = new string[5];
                                            //oimpuestos = ocomprobante.Impuestos.Traslados[conta].TasaOCuota;
                                            ImpuestoConcepto = new ImpuestoConcepto();
                                            ImpuestoConcepto.ImporteImpuesto = ocomprobante.Conceptos[cont].Impuestos.Traslados[conta].Importe.ToString();
                                            ImpuestoConcepto.DescImpuesto = _dbContext.Impuestos
                                                                            .Where(u => u.tipoimpuesto == ocomprobante.Conceptos[cont].Impuestos.Traslados[conta].Impuesto.ToString())
                                                                            .Select(u => u.descripcion)
                                                                            .SingleOrDefault();
                                            Impuestos.Add(ImpuestoConcepto);
                                        }

                                    }


                                    if (ocomprobante.Conceptos[cont].Impuestos.Retenciones != null)
                                    {
                                        for (int conta = 0; conta <= ocomprobante.Conceptos[cont].Impuestos.Retenciones.Length - 1; conta++)
                                        {
                                            //String[] oimpuestos = new string[5];
                                            //oimpuestos = ocomprobante.Impuestos.Traslados[conta].TasaOCuota;
                                            ImpuestoConcepto = new ImpuestoConcepto();
                                            ImpuestoConcepto.ImporteImpuesto = ocomprobante.Conceptos[cont].Impuestos.Retenciones[conta].TasaOCuota.ToString();

                                            ImpuestoConcepto.DescImpuesto = _dbContext.Impuestos
                                                                            .Where(u => u.tipoimpuesto == ocomprobante.Conceptos[cont].Impuestos.Retenciones[conta].Impuesto.ToString())
                                                                            .Select(u => u.descripcion)
                                                                            .SingleOrDefault();
                                            Impuestos.Add(ImpuestoConcepto);
                                        }
                                    }


                                    Concepto.Impuestos = Impuestos;
                                    Conceptos.Add(Concepto);
                                }

                                Cfdi.Conceptos = Conceptos;
                            }

                            listaCFDIS.Add(Cfdi);
                        }
                        catch (Exception e)
                        {
                            Console.Write("Existe un error al leer el archivo version 3.0");
                        }

                    }//us
                }


                else if (version == "4.0"){
                    //version 4.0
                    // 1 paso leer el xml pasar timbrado a una clase 
                   CFDI4.Comprobante ocomprobante = new CFDI4.Comprobante();
                    XmlSerializer oSerializer = new XmlSerializer(typeof(CFDI4.Comprobante));

                    Cfdi Cfdi = new Cfdi();
                    using (StreamReader reader = new StreamReader(pathxml))
                    {
                        //aqui desearilizamos 

                        try
                        {
                            ocomprobante = (CFDI4.Comprobante)oSerializer.Deserialize(reader);


                            //Obtengo datos del complemento
                            var oComplemento = ocomprobante.Complemento;

                            foreach (var ocomplementointerior in oComplemento.Any)
                                {
                                    if (ocomplementointerior.Name.Contains("TimbreFiscalDigital"))

                                    {
                                        XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));

                                        using (var readerComplemento = new StringReader(ocomplementointerior.OuterXml))
                                        {
                                            ocomprobante.TimbreFiscalDigital =
                                                (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);

                                            if (ocomprobante != null)
                                            {

                                                Cfdi.TimbreFislcal = new TimbreFiscal();
                                                Cfdi.TimbreFislcal.uuid = ocomprobante.TimbreFiscalDigital.UUID;
                                            }

                                        }
                                    }

                                }
                            


                            //Obtengo datos del generales del CFDI
                            Cfdi.Cuenta = "102022";
                            Cfdi.RFCEmisor = ocomprobante.Emisor.Rfc;
                            Cfdi.NombreProveedor = _dbContext
                                     .Provedores
                                     .Where(u => u.rfc == Cfdi.RFCEmisor)
                                     .Select(u => u.nombre)
                                     .SingleOrDefault();

                            Cfdi.Foliofactura = ocomprobante.Folio;
                            Cfdi.Importe = ocomprobante.Total.ToString();



                            //Obtengo datos de los conceptos
                            if (ocomprobante != null)
                            {
                                List<Concepto>? Conceptos = new List<Concepto>();
                                List<ImpuestoConcepto> Impuestos = new List<ImpuestoConcepto>();
                                Concepto Concepto = new Concepto();
                                ImpuestoConcepto ImpuestoConcepto = new ImpuestoConcepto();

                                for (int cont = 0; cont <= ocomprobante.Conceptos.Length - 1; cont++)
                                {
                                    Concepto = new Concepto();
                                    Concepto.ClaveOServicio = ocomprobante.Conceptos[cont].ClaveProdServ.ToString();    //fallo aqui 
                                    Concepto.Descripcion = ocomprobante.Conceptos[cont].Descripcion;
                                    Concepto.DescripcionCatalogo = _dbContext
                                                                   .Productos
                                                                   .Where(u => u.claveprodserv.ToString() == Concepto.ClaveOServicio)
                                                                   .Select(u => u.descripcion)
                                                                   .SingleOrDefault();
                                    Concepto.Importe = ocomprobante.Conceptos[cont].Importe.ToString();
                                    Impuestos = new List<ImpuestoConcepto>();

                                    if (ocomprobante.Conceptos[cont].Impuestos.Traslados != null)
                                    {
                                        for (int conta = 0; conta <= ocomprobante.Conceptos[cont].Impuestos.Traslados.Length - 1; conta++)
                                        {
                                            //String[] oimpuestos = new string[5];
                                            //oimpuestos = ocomprobante.Impuestos.Traslados[conta].TasaOCuota;
                                            ImpuestoConcepto = new ImpuestoConcepto();
                                            ImpuestoConcepto.ImporteImpuesto = ocomprobante.Conceptos[cont].Impuestos.Traslados[conta].Importe.ToString();
                                            ImpuestoConcepto.DescImpuesto = _dbContext.Impuestos
                                                                            .Where(u => u.tipoimpuesto == ocomprobante.Conceptos[cont].Impuestos.Traslados[conta].Impuesto.ToString())
                                                                            .Select(u => u.descripcion)
                                                                            .SingleOrDefault();
                                            Impuestos.Add(ImpuestoConcepto);
                                        }

                                    }


                                    if (ocomprobante.Conceptos[cont].Impuestos.Retenciones != null)
                                    {
                                        for (int conta = 0; conta <= ocomprobante.Conceptos[cont].Impuestos.Retenciones.Length - 1; conta++)
                                        {
                                            //String[] oimpuestos = new string[5];
                                            //oimpuestos = ocomprobante.Impuestos.Traslados[conta].TasaOCuota;
                                            ImpuestoConcepto = new ImpuestoConcepto();
                                            ImpuestoConcepto.ImporteImpuesto = ocomprobante.Conceptos[cont].Impuestos.Retenciones[conta].TasaOCuota.ToString();

                                            ImpuestoConcepto.DescImpuesto = _dbContext.Impuestos
                                                                            .Where(u => u.tipoimpuesto == ocomprobante.Conceptos[cont].Impuestos.Retenciones[conta].Impuesto.ToString())
                                                                            .Select(u => u.descripcion)
                                                                            .SingleOrDefault();
                                            Impuestos.Add(ImpuestoConcepto);
                                        }
                                    }


                                    Concepto.Impuestos = Impuestos;
                                    Conceptos.Add(Concepto);
                                }

                                Cfdi.Conceptos = Conceptos;
                            }

                            listaCFDIS.Add(Cfdi);
                        }
                        catch (Exception e)
                        {
                            Console.Write("Existe un error al leer el archivo version 4.0");
                        }

                    }//us

                }

            }
            return listaCFDIS;
        }

    }

}

