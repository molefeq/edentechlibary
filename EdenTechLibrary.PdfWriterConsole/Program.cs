using EdenTechLibrary.PdfWriters.Quatation;
using EdenTechLibrary.PdfWriters.Quatation.Models;
using EdenTechLibrary.PdfWriters.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EdenTechLibrary.PdfWriterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            XDocument xDocument = XDocument.Load("../../Data/Qoutations/quotations.xml");
            List<QoutationModel> quotations = XmlToQoutationModelConverter.Convert(xDocument);
            string baseDirectory = "C:/Temp/EdentTechPdfQuotations/";

            foreach (QoutationModel qoutationModel in quotations)
            {
                string fileName = baseDirectory + qoutationModel.FileName;
                QuotationPdfWriter.Instance.Write(fileName, qoutationModel);
            }
        }
    }
}
