using System;
using System.Collections.Generic;

namespace EdenTechLibrary.PdfWriters.Quatation.Models
{
    public class QoutationModel
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime QouteDate { get; set; }
        public Decimal TotalExcl { get; set; }
        public Decimal TotalVat { get; set; }
        public Decimal TotalIncl { get; set; }
        public List<QouteLineModel> Items { get; set; }

        public QoutationModel()
        {
            Items = new List<QouteLineModel>();
        }
    }
}
