using System;

namespace EdenTechLibrary.PdfWriters.Utilities
{
    public class VatCalculator
    {
        public static decimal GetVatAmount(decimal amountExclVat)
        {
            return Math.Round(amountExclVat * Constants.VAT_PERCENTAGE, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal GetAmountInclVat(decimal amountExclVat)
        {
            decimal vatAmount = GetVatAmount(amountExclVat);

            return Math.Round(amountExclVat, 2, MidpointRounding.AwayFromZero) + vatAmount;
        }
    }
}
