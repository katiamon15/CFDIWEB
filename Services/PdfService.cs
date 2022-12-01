using CFDIWEB.Interfaces;
using Microsoft.AspNetCore.Razor.Language;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.IdentityModel.Tokens;
using RazorEngineCore;
using CFDIWEB.CFDI4;
using System.Diagnostics;
using CFDIWEB.Models;
using CFDIWEB.Pages;
using System.Text;
using System;
using System.IO;
using iTextSharp.text.pdf.qrcode;
using Azure;
using System.Threading;

namespace CFDIWEB.Services
{
    public class PdfService : IPdf
    {

        public async Task<List<byte[]>> Pdfversion(List<byte[]> files)
        {
            List<MemoryStream> streams = new List<MemoryStream>();

            List<byte[]> rutaspdf = new List<byte[]>();

            foreach (byte[] file in files)
            {
                XmlDocument xmlDoc= new XmlDocument();
                MemoryStream ms = new MemoryStream(file);
                xmlDoc.Load(ms);
                streams.Add(ms);


                XmlNodeList elemList = xmlDoc.GetElementsByTagName("cfdi:Comprobante");
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

                if (version == "4.0")
                {
                    CFDI4.Comprobante ocomprobantes = new CFDI4.Comprobante();
                    XmlSerializer oSerializer = new XmlSerializer(typeof(CFDI4.Comprobante));


                    using (MemoryStream reader = new MemoryStream(file))
                    {
                        streams.Add(reader);
                        //aqui desearilizamos
                        ocomprobantes = (CFDI4.Comprobante)oSerializer.Deserialize(reader);  //fallo aqui 

                        //complementos
                        var oComplemento = ocomprobantes.Complemento;

                        foreach (var complementointerior in oComplemento.Any)
                        {
                            if (complementointerior.Name.Contains("TimbreFiscalDigital"))

                            //iftipo comprobante == "I"
                            //else{new Exception("El documento XML no es un ingreso")}

                            {
                                XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));

                                using (var readerComplemento = new StringReader(complementointerior.OuterXml))
                                {
                                    ocomprobantes.TimbreFiscalDigital =
                                        (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);
                                }
                            }

                        }

                    }//using


                    //paso 2 proceso de lectura apñicandolo con razor y haciendo pdf  

                    string path = @"C:\DesagarCFDIWEB\Plantillas\";
                    string pathHTMLTemp = path + "mihtml.html"; //temporal 
                    string pathplantilla = path + "Plantilla.html";
                    string shtml = Razor.GetStringOffile(pathplantilla);
                    string resulthtml = "";
                    //resulthtml = RazorEngineCore.Razor.Parse(shtml, ocomprobante);

                    IRazorEngine razorEngine = new RazorEngineCore.RazorEngine();
                    IRazorEngineCompiledTemplate template = razorEngine.Compile(shtml);

                    string result = template.Run(ocomprobantes);


                    Console.WriteLine(result);
                    Console.Read();

                    //creamos el temporal
                    File.WriteAllText(pathHTMLTemp, result);

                    string pathhtmlpdf = @"C:\CFDIWEB\Pdf\wkhtmltopdf\wkhtmltopdf.exe";

                    ProcessStartInfo oprocessStartInfo = new ProcessStartInfo();
                    oprocessStartInfo.UseShellExecute = false;
                    oprocessStartInfo.FileName = pathhtmlpdf;
                    oprocessStartInfo.Arguments = "mihtml.html " + ocomprobantes.TimbreFiscalDigital.UUID + ".pdf";

                    using (Process oProcess = Process.Start(oprocessStartInfo))
                    {
                        oProcess.WaitForExit();

                    }

                    var rutapdf = @"C:\DesagarCFDIWEB\Plantillas\" + ocomprobantes.TimbreFiscalDigital.UUID + ".pdf";
                    byte[] bytes = await File.ReadAllBytesAsync(rutapdf);

                    rutaspdf.Add(bytes);

                    //elimminar el temporral
                    File.Delete(pathHTMLTemp);

                   

                }//if 4.0

                else if (version == "3.3")
                {
                    Comprobante ocomprobante = new Comprobante();
                    XmlSerializer oSerializer = new XmlSerializer(typeof(Comprobante));

                    using (MemoryStream reader = new MemoryStream(file))
                    {

                        streams.Add(reader);
                        //aqui desearilizamos
                        ocomprobante = (Comprobante)oSerializer.Deserialize(reader);

                        //complementos
                        foreach (var oComplemento in ocomprobante.Complemento)
                        {
                            foreach (var ocomplementointerior in oComplemento.Any)
                            {
                                if (ocomplementointerior.Name.Contains("TimbreFiscalDigital"))

                                //iftipo comprobante == "I"
                                //else{new Exception("El documento XML no es un ingreso")}

                                {
                                    XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));

                                    using (var readerComplemento = new StringReader(ocomplementointerior.OuterXml))
                                    {
                                        ocomprobante.TimbreFiscalDigital =
                                            (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);
                                    }
                                }

                            }
                        }
                    }//using CFDIWEB.Services.PdfService



                    //paso 2 proceso de lectura apñicandolo con razor y haciendo pdf  

                    string path = @"C:\DesagarCFDIWEB\Plantillas\";
                    string pathHTMLTemp = path + "mihtml.html"; //temporal 
                    string pathplantilla = path + "Plantilla.html";
                    string shtml = Razor.GetStringOffile(pathplantilla);
                    string resulthtml = "";
                    //resulthtml = RazorEngineCore.Razor.Parse(shtml, ocomprobante);

                    IRazorEngine razorEngine = new RazorEngineCore.RazorEngine();
                    IRazorEngineCompiledTemplate template = razorEngine.Compile(shtml);

                    string result = template.Run(ocomprobante);


                    Console.WriteLine(result);

                    //creamos el temporal
                    File.WriteAllText(pathHTMLTemp, result);


                    string pathhtmlpdf = @"C:\CFDIWEB\Pdf\wkhtmltopdf\wkhtmltopdf.exe";

                    
                    ProcessStartInfo oprocessStartInfo = new ProcessStartInfo();
                    oprocessStartInfo.WorkingDirectory = path;
                    oprocessStartInfo.UseShellExecute = false;
                    oprocessStartInfo.FileName = pathhtmlpdf;
                    oprocessStartInfo.Arguments = "mihtml.html " + ocomprobante.TimbreFiscalDigital.UUID + ".pdf";

                    using (Process oProcess = Process.Start(oprocessStartInfo))
                    {
                        oProcess.WaitForExit();
                    }

                    var rutapdf = @"C:\DesagarCFDIWEB\Plantillas\" + ocomprobante.TimbreFiscalDigital.UUID + ".pdf";
                    byte[] bytes = await File.ReadAllBytesAsync(rutapdf);

                    rutaspdf.Add(bytes);

                    //elimminar el temporral
                    File.Delete(pathHTMLTemp);

                }// else if 3.3
                else
                {
                    Console.WriteLine("Error en la version");
                }//else 
                streams.ForEach(s => s.Dispose());
            }

            return rutaspdf;

        }

         

        public class Razor
        {
            public static string GetStringOffile(string pathFile)
            {
                string contenido = File.ReadAllText(pathFile);
                return contenido;
            }

        }// public class Razor


    }// servic


}//namespace


