using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ReceiptGeneratorApp
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // HERE CHANGE THE FUNCTION TO WHAT YOU NEED, EITHER MERGE PDF OR CREATE INVOICE RECEIPT PDFS =================================//////////////////

            //ConvertReceiptList();
            MergePdf();
        }




        static async void ConvertReceiptList()
        {
            string outputDir = "receipts";
            Directory.CreateDirectory(outputDir);

            //GlobalFontSettings.FontResolver = new CustomFontResolver();

            //GlobalFontSettings. UseWindowsFontsUnderWsl2 = true; // Or use custom font resolver for production
            var exchangeRates2021 = new Dictionary<int, decimal>
        {
            { 1,    10.5600m }, // January
            { 2,    10.0900m }, // February
            { 3, 9.9007m }, // March
            { 4, 10.8600m }, // April
            { 5, 11.3900m }, // May
            { 6, 11.8600m }, // June
            { 7,    12.0600m }, // July
            { 8, 11.7400m }, // August
            { 9, 11.6500m }, // September
            { 10, 11.7900m }, // October
            { 11,   12.7700m }, // November
            { 12, 14.2800m },  // December
                { 0,    14.67m }, // 2022 January
        };

            var exchangeRates2022 = new Dictionary<int, decimal>
        {
            { 1, 14.67m }, // January
            { 2, 14.86m }, // February
            { 3, 15.04m }, // March
            { 4, 15.47m }, // April
            { 5, 15.69m }, // May
            { 6, 15.86m }, // June
            { 7, 16.06m }, // July
            { 8, 16.61m }, // August
            { 9, 16.95m }, // September
            { 10, 17.08m }, // October
            { 11, 17.23m }, // November
            { 12, 17.37m }  // December
        };

            var exchangeRates2023 = new Dictionary<int, decimal>
        {
            { 0, 36.8560m }, // 20244444 January
            { 1, 23.1130m }, // January
            { 2, 23.2877m }, // February
            { 3, 22.6227m }, // March
            { 4, 23.2877m }, // April
            { 5, 24.1481m }, // May
            { 6, 24.60m }, // June
            { 7,   29.98m }, // July
            { 8, 34.64m }, // August
            { 9, 34.3743m }, // September
            { 10,   33.4323m }, // October
            { 11, 34.1381m }, // November
            { 12, 36.1376m }  // December
        };

            var exchangeRates2024 = new Dictionary<int, decimal>
        {

            { 1,    36.8560m }, // January
            { 2, 38.2383m }, // February
            { 3, 39.0863m }, // March
            { 4, 41.1194m }, // April
            { 5, 40.5295m }, // May
            { 6, 40.9074m }, // June
            { 7,    41.4287m }, // July
            { 8,    43.1400m }, // August
            { 9, 44.2254m }, // September
            { 10,   45.0662m }, // October
            { 11, 44.4057m }, // November
            { 12, 43.6487m },  // December
                { 0,    44.4968m }, // 2025 January
        };

            var transactions = new List<Transaction>
            {
new Transaction { Date = DateTime.Parse("2020-03-13"), Name = "Serkan Him", Amount = 215.74m, Description = "Website Development & Maintenance" },
    new Transaction { Date = DateTime.Parse("2020-04-15"), Name = "Fiverr", Amount = 10.06m, Description = "Website Development & Maintenance" },
    new Transaction { Date = DateTime.Parse("2020-04-20"), Name = "Ilyas Ustunkaya", Amount = 65.62m, Description = "Website Development & Maintenance" },
        };

            foreach (var transaction in transactions)
            {
                int month = transaction.Date.Month;
                transaction.ConvertedAmount = transaction.Amount * exchangeRates2021[month - 1];
                //Console.WriteLine($"new Transaction {{ Date = new DateTime({transaction.Date.Year}, {transaction.Date.Month}, {transaction.Date.Day}), Name = \"{transaction.Name}\", Amount = {transaction.Amount}m, ConvertedAmount = {transaction.Amount}m * exchangeRates[{month}] }},");
            }
            int count = 0;
            foreach (var tx in transactions)
            {
                try
                {
                    tx.Amount = tx.Amount; // Assuming the amount is already in GBP
                    string fileName = Path.Combine(outputDir, $"receipt-{tx.Name.Replace(" ", "_")}-{tx.Date:yyyy-MM-dd}@{(count)}.pdf");
                    CreatePdfReceipt(tx, fileName);
                    Console.WriteLine($"Generated: {fileName}");
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing transaction for {tx.Name} on {tx.Date:yyyy-MM-dd}: {ex.Message}");
                }
            }

            Console.WriteLine("All receipts processed.");
        }


        // This function creates PDF receipts for  given transactions and saves them to the specified file path.
        static void CreatePdfReceipt(Transaction tx, string filePath)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var smallfont = new XFont("Arial", 10.5);
            var font = new XFont("Arial", 12);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);
            var boldSmallFont = new XFont("Arial", 10.5, XFontStyle.Bold);

            // Header
            gfx.DrawString("Invoice :", new XFont("Arial", 10, XFontStyle.Regular), XBrushes.DarkSlateBlue, new XPoint(40, 40));
            gfx.DrawString(tx.Name, boldSmallFont, XBrushes.Black, new XPoint(40, 60));
            //gfx.DrawString(tx.Address, smallfont, XBrushes.Black, new XPoint(40, 80 ));

            // To section
            gfx.DrawString("To:", boldSmallFont, XBrushes.Black, new XPoint(page.Width - 200, 40));
            gfx.DrawString("Your company Ltd", boldSmallFont, XBrushes.MidnightBlue, new XPoint(page.Width - 200, 60));
            gfx.DrawString("233 City Road EC1V 2NX, London", smallfont, XBrushes.Black, new XPoint(page.Width - 200, 80));
            gfx.DrawString("hi@youremail.com", smallfont, XBrushes.Black, new XPoint(page.Width - 200, 100));

            gfx.DrawString($"InvoiceDate {tx.Date:dd.MM.yyyy}", smallfont, XBrushes.Black, new XPoint(page.Width - 200, 130));
            gfx.DrawString($"Invoice Number  {tx.Date:yyyyMMdd01}", smallfont, XBrushes.Black, new XPoint(page.Width - 200, 150));

            // Table header
            int tableStartY = 200;
            gfx.DrawLine(XPens.LightGray, 40, tableStartY, page.Width - 40, tableStartY);
            gfx.DrawString("Description", boldFont, XBrushes.Black, new XPoint(50, tableStartY + 20));
            gfx.DrawString("Quantity", boldFont, XBrushes.Black, new XPoint(250, tableStartY + 20));
            gfx.DrawString("Unit price", boldFont, XBrushes.Black, new XPoint(350, tableStartY + 20));
            gfx.DrawString("Total", boldFont, XBrushes.Black, new XPoint(450, tableStartY + 20));
            gfx.DrawLine(XPens.LightGray, 40, tableStartY + 40, page.Width - 40, tableStartY + 40);

            // Row
            gfx.DrawString($"{tx.Description}", font, XBrushes.Black, new XPoint(50, tableStartY + 60));
            gfx.DrawString("1", font, XBrushes.Black, new XPoint(260, tableStartY + 60));
            //gfx.DrawString($"{tx.ConvertedAmount:F2}TL", font, XBrushes.Black, new XPoint(360, tableStartY + 60));
            gfx.DrawString($"£{tx.Amount:F2}", font, XBrushes.Black, new XPoint(360, tableStartY + 60));
            //gfx.DrawString($"{tx.ConvertedAmount:F2}TL (£{tx.Amount:F2})", font, XBrushes.Black, new XPoint(450, tableStartY + 60));
            gfx.DrawString($"£{tx.Amount:F2}", font, XBrushes.Black, new XPoint(450, tableStartY + 60));

            // Total
            gfx.DrawLine(XPens.LightGray, 40, tableStartY + 90, page.Width - 40, tableStartY + 90);
            gfx.DrawString("Total", boldFont, XBrushes.Black, new XPoint(400, tableStartY + 110));
            //gfx.DrawString($"{tx.ConvertedAmount:F2}TL (£{tx.Amount:F2})", new XFont("Arial", 14, XFontStyle.Bold), XBrushes.Black, new XPoint(450, tableStartY + 110));
            gfx.DrawString($"£{tx.Amount:F2}", new XFont("Arial", 14, XFontStyle.Bold), XBrushes.Black, new XPoint(450, tableStartY + 110));

            doc.Save(filePath);
        }



        //   THIS FUNCTION MERGES FILES  IN A FOLDER NAMED MergePdf in your desktop. Firstly you can create a folder in your desktop named MergePdf, then put all files there, and run the app.==============
        static int MergePdf()
        {
            try
            {
                var targetDir = "C:\\Users\\User\\Desktop\\MergePdf";
                //var targetDir = args.Length > 0
                //? args[0]
                //: Directory.GetCurrentDirectory();

                if (!Directory.Exists(targetDir))
                {
                    Console.WriteLine($"ERROR: Folder not found: {targetDir}");
                    return 1;
                }

                var pdfFiles = Directory.GetFiles(targetDir, "*.pdf")
                                        .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                                        .ToList();

                if (pdfFiles.Count == 0)
                {
                    Console.WriteLine("No PDF files found.");
                    return 0;
                }

                var outPath = Path.Combine(targetDir, "Merged.pdf");
                if (File.Exists(outPath)) File.Delete(outPath);

                using var outputDoc = new PdfDocument();

                foreach (var file in pdfFiles)
                {
                    Console.WriteLine($"Adding: {Path.GetFileName(file)}");
                    using var inputDoc = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                    for (int i = 0; i < inputDoc.PageCount; i++)
                        outputDoc.AddPage(inputDoc.Pages[i]);
                }

                outputDoc.Save(outPath);
                Console.WriteLine($"\n✅  Merged {pdfFiles.Count} file(s) into: {outPath}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled error:");
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
