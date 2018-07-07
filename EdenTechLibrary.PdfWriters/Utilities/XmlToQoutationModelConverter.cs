using EdenTechLibrary.PdfWriters.Extensions;
using EdenTechLibrary.PdfWriters.Quatation.Models;

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EdenTechLibrary.PdfWriters.Utilities
{
    public class XmlToQoutationModelConverter
    {
        public static List<QoutationModel> Convert(XDocument xDocument)
        {
            List<QoutationModel> orders = (from quotation in xDocument.Descendants("Quotation")
                                           select ConvertToQuotation(quotation)).ToList();

            return orders;
        }

        private static QoutationModel ConvertToQuotation(XElement quotationXmlElement)
        {
            QoutationModel qoutationModel = new QoutationModel
            {
                Description = quotationXmlElement.Element("Description").Value,
                FileName = quotationXmlElement.Element("FileName").Value,
                QouteDate = quotationXmlElement.Element("QouteDate").Value.StringToDateTime(),
                Items = (from item in quotationXmlElement.Descendants("Item")
                         select CreateQouteLineModel(item)).ToList()

            };

            qoutationModel.TotalExcl = qoutationModel.Items.Where(q => q.TotalExclVat != decimal.MinValue).Sum(item => item.TotalExclVat);
            qoutationModel.TotalVat = VatCalculator.GetVatAmount(qoutationModel.TotalExcl);
            qoutationModel.TotalIncl = VatCalculator.GetAmountInclVat(qoutationModel.TotalExcl);

            return qoutationModel;
        }

        private static QouteLineModel CreateQouteLineModel(XElement qouteLineModel)
        {
            return new QouteLineModel
            {
                Code = qouteLineModel.Element("Code").Value,
                Description = qouteLineModel.Element("Description").Value,
                Quantity = qouteLineModel.Element("Quantity").Value,
                Price = qouteLineModel.Element("Price").Value,
                TotalExclVat = qouteLineModel.Element("TotalExcl").Value.StringToDecimal()
            };
        }
    }
}
