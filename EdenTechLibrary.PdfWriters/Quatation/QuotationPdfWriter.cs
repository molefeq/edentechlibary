using EdenTechLibrary.PdfWriters.Quatation.Models;
using EdenTechLibrary.PdfWriters.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EdenTechLibrary.PdfWriters.Quatation
{
    public class QuotationPdfWriter
    {
        static QuotationPdfWriter _instance;

        public static QuotationPdfWriter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QuotationPdfWriter();
                }
                return _instance;
            }
        }

        public void Write(MemoryStream pdfOutputStream, QoutationModel qoutationModel)
        {
            using (Document pdfDocument = new Document(iTextSharp.text.PageSize.A4))
            {
                try
                {
                    iTextSharp.text.pdf.PdfWriter pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocument, pdfOutputStream);

                    pdfDocument.Open();

                    WritePdfContent(pdfDocument, qoutationModel);

                    pdfWriter.Flush();
                    pdfWriter.CloseStream = true;
                }
                finally
                {
                    pdfDocument.Close();
                }
            }
        }

        public void Write(string fileName, QoutationModel qoutationModel)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document pdfDocument = new Document(iTextSharp.text.PageSize.A4))
                {
                    try
                    {
                        iTextSharp.text.pdf.PdfWriter pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDocument, fileStream);

                        pdfDocument.Open();
                        WritePdfContent(pdfDocument, qoutationModel);


                        pdfWriter.Flush();
                        pdfWriter.CloseStream = true;
                    }
                    finally
                    {
                        pdfDocument.Close();
                    }
                }
            }
        }

        #region Private Methods

        private void WritePdfContent(Document pdfDocument, QoutationModel qoutationModel)
        {
            PdfPTable companyHeadingTable = new PdfPTable(1);
            PdfPTable quotationTable = new PdfPTable(5);
            PdfPTable summaryBoxTable = new PdfPTable(3);
            PdfPTable notesTable = new PdfPTable(1);

            List<PdfPTable> documentTables = new List<PdfPTable>();

            companyHeadingTable.WidthPercentage = 100f;
            documentTables.Add(companyHeadingTable);

            WriteCompanyHeading(companyHeadingTable);
            WriteLine(companyHeadingTable, 1);

            quotationTable.SetWidths(new int[] { 1, 3, 1, 1, 1 });
            quotationTable.WidthPercentage = 100f;
            documentTables.Add(quotationTable);

            WriteQuotation(quotationTable, qoutationModel);

            summaryBoxTable.SetWidths(new int[] { 2, 1, 1 });
            summaryBoxTable.WidthPercentage = 100f;
            documentTables.Add(summaryBoxTable);
            WriteSummaryBoxTable(summaryBoxTable);

            notesTable.WidthPercentage = 100f;
            documentTables.Add(notesTable);
            WriteQoutationNotesTable(notesTable);

            foreach (PdfPTable pdfPTable in documentTables)
            {
                pdfDocument.Add(pdfPTable);
                pdfPTable.FlushContent();
            }
        }

        private void WriteCompanyHeading(PdfPTable companyHeadingTable)
        {
            AddTableCell(companyHeadingTable, "ADENTEC SECURITY (PTY) LTD".ToUpper(), 18, Font.BOLD, Rectangle.NO_BORDER, null, 10f);
            AddTableCell(companyHeadingTable, "38 Sonwaba Complex, Oklahoma Street", 11, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(companyHeadingTable, "Cosmo City, Randburg", 11, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(companyHeadingTable, "2188, Gauteng, South Africa", 11, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(companyHeadingTable, "Tel: 078 818 7185", 11, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(companyHeadingTable, "Reg No: 2015/368769/07", 11, Font.NORMAL, Rectangle.NO_BORDER);
        }

        private void WriteQuotation(PdfPTable quotationTable, QoutationModel qoutationModel)
        {
            WriteQuotationSummary(quotationTable, qoutationModel);
            WriteQuotationTableHeaders(quotationTable);
            WriteQuotationLines(quotationTable, qoutationModel);
        }

        private void WriteSummaryBoxTable(PdfPTable pdfTable)
        {
            WriteBankingDetailsBox(pdfTable);
            AddTableCell(pdfTable, "".ToUpper(), 10, Font.NORMAL, Rectangle.NO_BORDER, 2);
        }

        private void WriteQoutationNotesTable(PdfPTable pdfTable)
        {
            PdfPCell pdfPCell = new PdfPCell();

            pdfPCell.PaddingLeft = 10f;
            pdfPCell.PaddingTop = 20f;
            pdfPCell.Border = Rectangle.NO_BORDER;

            pdfPCell.AddElement(WriteParagraph("All stock remains the property of ADENTEC SECURITY (PTY) LTD until final balance is paid.", 11, Font.BOLD, Rectangle.ALIGN_CENTER));
            pdfPCell.AddElement(WriteParagraph("ALL QUOTATIONS ARE VALID FOR 14 DAYS ONLY.", 11, Font.NORMAL, Rectangle.ALIGN_CENTER));
            pdfPCell.AddElement(WriteParagraph("TERMS : STRICTLY C.O.D.", 11, Font.NORMAL, Rectangle.ALIGN_CENTER));
            pdfPCell.AddElement(WriteParagraph("DEPOSITS PAYABLE BEFORE INSTALLATION. BALANCE DUE ON COMPLETION.", 11, Font.BOLD, Rectangle.ALIGN_CENTER));
            pdfPCell.AddElement(WriteParagraph("Should you require any further assistance do not hesitate to contact me.", 11, Font.NORMAL, Rectangle.ALIGN_CENTER));
            pdfPCell.AddElement(WriteParagraph("Thank You For Your Business!", 11, Font.BOLD, Rectangle.ALIGN_CENTER));

            pdfTable.AddCell(pdfPCell);
        }

        private void WriteQuotationSummary(PdfPTable quotationTable, QoutationModel qoutationModel)
        {
            Font cellFont = FontFactory.GetFont("Arial", 18, Font.BOLD, BaseColor.BLACK);
            PdfPCell pdfPCell = new PdfPCell(new Phrase(qoutationModel.Description.ToUpper(), cellFont));

            pdfPCell.PaddingLeft = 10f;
            pdfPCell.HorizontalAlignment = Rectangle.ALIGN_CENTER;

            pdfPCell.Colspan = 5;
            pdfPCell.Border = Rectangle.NO_BORDER;
            quotationTable.AddCell(pdfPCell);

            Font pdfPQouteDateCellFont = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            PdfPCell pdfPQouteDateCell = new PdfPCell(new Phrase(string.Format("Date: {0}", DateTime.Now.ToString(Constants.DATE_FORMAT).ToUpper()), pdfPQouteDateCellFont));

            pdfPQouteDateCell.PaddingLeft = 10f;
            pdfPQouteDateCell.PaddingBottom = 30f;
            pdfPQouteDateCell.HorizontalAlignment = Rectangle.ALIGN_RIGHT;

            pdfPQouteDateCell.Colspan = 5;
            pdfPQouteDateCell.Border = Rectangle.NO_BORDER;
            quotationTable.AddCell(pdfPQouteDateCell);
        }

        private void WriteQuotationTableHeaders(PdfPTable pdfTable)
        {
            AddTableCell(pdfTable, "CODE", 10, Font.BOLD, Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
            AddTableCell(pdfTable, "DESCRIPTION", 10, Font.BOLD, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
            AddTableCell(pdfTable, "QTY UNIT", 10, Font.BOLD, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
            AddTableCell(pdfTable, "PRICE", 10, Font.BOLD, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
            AddTableCell(pdfTable, "TOTAL", 10, Font.BOLD, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER);
        }

        private void WriteQuotationLines(PdfPTable pdfTable, QoutationModel qoutationModel)
        {
            foreach (QouteLineModel qouteLineModel in qoutationModel.Items)
            {
                AddTableCell(pdfTable, qouteLineModel.Code, 10, Font.NORMAL, Rectangle.NO_BORDER);
                AddTableCell(pdfTable, qouteLineModel.Description, 10, Font.NORMAL, Rectangle.NO_BORDER);
                AddTableCell(pdfTable, qouteLineModel.Quantity, 10, Font.NORMAL, Rectangle.NO_BORDER);
                AddTableCell(pdfTable, qouteLineModel.Price, 10, Font.NORMAL, Rectangle.NO_BORDER);
                AddTableCell(pdfTable, qouteLineModel.TotalExclVat == decimal.MinValue ? "" : qouteLineModel.TotalExclVat.ToString(Constants.MONEY_FORMAT), 10, Font.NORMAL, Rectangle.NO_BORDER);
            }

            WriteLine(pdfTable, 5);

            AddTableCell(pdfTable, "".ToUpper(), 10, Font.NORMAL, Rectangle.NO_BORDER, 3);
            AddTableCell(pdfTable, "Total Excl", 10, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(pdfTable, string.Format("R {0}", qoutationModel.TotalExcl.ToString(Constants.MONEY_FORMAT)), 10, Font.NORMAL, Rectangle.NO_BORDER);

            AddTableCell(pdfTable, "".ToUpper(), 10, Font.NORMAL, Rectangle.NO_BORDER, 3);
            AddTableCell(pdfTable, "Vat @ 14%", 10, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(pdfTable, string.Format("R {0}", qoutationModel.TotalVat.ToString(Constants.MONEY_FORMAT)), 10, Font.NORMAL, Rectangle.NO_BORDER);

            AddTableCell(pdfTable, "".ToUpper(), 10, Font.NORMAL, Rectangle.NO_BORDER, 3);
            AddTableCell(pdfTable, "Total Incl", 10, Font.NORMAL, Rectangle.NO_BORDER);
            AddTableCell(pdfTable, string.Format("R {0}", qoutationModel.TotalIncl.ToString(Constants.MONEY_FORMAT)), 10, Font.NORMAL, Rectangle.NO_BORDER);
        }

        private void AddTableCell(PdfPTable pdfTable, string cellText, int fontSize, int fontStyle, int border, int? colSpan = null, float? paddingBottom = null, float? paddingTop = null)
        {
            Font cellFont = FontFactory.GetFont("Arial", fontSize, fontStyle, BaseColor.BLACK);
            PdfPCell pdfPCell = new PdfPCell(new Phrase(cellText, cellFont));

            pdfPCell.PaddingLeft = 10f;
            pdfPCell.Border = border;

            if (colSpan != null)
            {
                pdfPCell.Colspan = colSpan.Value;
            }

            if (paddingBottom != null)
            {
                pdfPCell.PaddingBottom = paddingBottom.Value;
            }

            if (paddingTop != null)
            {
                pdfPCell.PaddingTop = paddingTop.Value;
            }

            pdfTable.AddCell(pdfPCell);
        }

        private void WriteLine(PdfPTable pdfTable, int colspan)
        {
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(2.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            p.PaddingTop = 10f;

            PdfPCell pdfPCell = new PdfPCell(p);

            pdfPCell.Colspan = colspan;
            pdfPCell.Border = Rectangle.NO_BORDER;
            pdfTable.AddCell(pdfPCell);
        }

        private void WriteBankingDetailsBox(PdfPTable pdfTable)
        {
            PdfPTable bankingDetailsTable = new PdfPTable(1);

            AddTableCell(bankingDetailsTable, "Banking Details".ToUpper(), 11, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Bank: FNB", 10, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Account Holder: ADENTEC SECURITY PTY LTD", 10, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Account No: 62766243142", 10, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Account Type: Cheque", 10, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Branch Name: NORTHGATE", 10, Font.BOLD, Rectangle.NO_BORDER);
            AddTableCell(bankingDetailsTable, "Branch Code: 256755", 10, Font.BOLD, Rectangle.NO_BORDER);

            PdfPTable containerTable = new PdfPTable(1);
            PdfPCell containerTableCell = new PdfPCell(bankingDetailsTable);
            containerTable.SpacingAfter = 15;

            containerTable.TotalWidth = 50f;
            containerTableCell.Border = Rectangle.BOX;
            containerTableCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            containerTable.AddCell(containerTableCell);

            PdfPCell pdfPCell = new PdfPCell(containerTable);

            pdfPCell.Border = Rectangle.NO_BORDER;
            pdfPCell.PaddingLeft = 10f;
            pdfPCell.PaddingTop = 50f;
            pdfPCell.HorizontalAlignment = Rectangle.ALIGN_LEFT;
            pdfTable.AddCell(pdfPCell);
        }

        private Paragraph WriteParagraph(string paragraphContent, int fontSize, int fontStyle, int alignment)
        {
            Font cellFont = FontFactory.GetFont("Arial", fontSize, fontStyle, BaseColor.BLACK);
            var paragraph = new Paragraph(new Phrase(paragraphContent, cellFont));
            paragraph.Alignment = alignment;

            return paragraph;
        }

        #endregion
    }
}
