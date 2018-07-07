
namespace EdenTechLibrary.PdfWriters.Quatation.Models
{
    public class QouteLineModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public decimal TotalExclVat { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalInclVat { get; set; }
    }
}
