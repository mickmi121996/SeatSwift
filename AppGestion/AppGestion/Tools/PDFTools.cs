using AppGestion.Classes;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppGestion.Tools
{
    public class PDFTools
    {
        #region Sales Report

        /// <summary>
        /// Create a PDF file for the sales report
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="sellLines">The list of sell lines</param>
        /// <param name="reportDate">The date of the report</param>
        /// <param name="reportType">The type of the report</param>
        /// <remarks>
        /// The report type can be "Quotidien" or "Annuel"
        /// </remarks>
        /// <remarks>
        /// The sell lines are the list of sales
        /// </remarks>
        public static void CreateSalesReportPdf(
            string filePath,
            List<SellLine> sellLines,
            DateTime reportDate,
            string reportType
        )
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header()
                        .Element(container => ComposeHeader(container, reportDate, reportType));
                    page.Content().Element(content => ComposeContent(content, sellLines));
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            document.GeneratePdf(filePath);
        }

        /// <summary>
        /// Compose the header of the sales report
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="reportDate">The date of the report</param>
        /// <param name="reportType">The type of the report</param>
        /// <remarks>
        /// The report type can be "Quotidien" or "Annuel"
        /// </remarks>
        private static void ComposeHeader(
            IContainer container,
            DateTime reportDate,
            string reportType
        )
        {
            string headerText =
                reportType == "Quotidien"
                    ? $"Rapport de vente pour le {reportDate:D}"
                    : $"Rapport de vente pour {reportDate:Y}";

            container.Row(row =>
            {
                row.RelativeItem().Text(headerText).FontSize(20).Bold();
            });
        }

        /// <summary>
        /// Compose the content of the sales report
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="sellLines">The list of sell lines</param>
        /// <remarks>
        /// The content is composed of a table with the sell lines and the grand total
        /// </remarks>
        /// <remarks>
        /// The table has 7 columns: Spectacle, Date, Tickets, Montant Avant Taxes, TPS, TVQ, Montant Après Taxes
        /// </remarks>
        /// <remarks>
        /// The grand total is the sum of the Montant Après Taxes
        /// </remarks>
        private static void ComposeContent(IContainer container, List<SellLine> sellLines)
        {
            // Style for the table headers
            static IContainer HeaderCellStyle(IContainer container)
            {
                return container
                    .DefaultTextStyle(x => x.Bold())
                    .PaddingVertical(5)
                    .BorderBottom(2)
                    .BorderColor("#000000");
            }

            // Style for the table cells
            static IContainer CellStyle(IContainer container)
            {
                return container.PaddingVertical(5).BorderBottom(1).BorderColor("#CCCCCC");
            }

            // Calculating the grand total of the Montant Après Taxes
            decimal grandTotal = sellLines.Sum(line => (decimal)line.TotalAmountAfterTaxe);

            container.Table(table =>
            {
                // Definition of the columns with proportions adapted to your content
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(60);
                    columns.ConstantColumn(120);
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(60);
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(60);
                });

                // Table header with the defined style
                table.Header(header =>
                {
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("Spectacle")
                        .FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("Date").FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("Tickets")
                        .FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("Montant Avant Taxes")
                        .FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("TPS").FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("TVQ").FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .AlignRight()
                        .Text("Montant Après Taxes")
                        .FontSize(10);
                });

                // Table content with each sell line
                foreach (var line in sellLines)
                {
                    table.Cell().Element(CellStyle).Text(line.ShowName).FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .Text(line.RepresentationDate.ToString("U"))
                        .FontSize(8);
                    table.Cell().Element(CellStyle).Text(line.TicketsSold.ToString()).FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .Text($"{line.TotalAmountBeforeTaxe:C}")
                        .FontSize(8);
                    table.Cell().Element(CellStyle).Text($"{line.TPS:C}").FontSize(8);
                    table.Cell().Element(CellStyle).Text($"{line.TVQ:C}").FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignRight()
                        .Text($"{line.TotalAmountAfterTaxe:C}")
                        .FontSize(8);
                }

                //  Grand Total line with the adapted style
                table.Cell().Element(HeaderCellStyle).Text("Grand Total").FontSize(10);
                table
                    .Cell()
                    .Element(HeaderCellStyle)
                    .AlignRight()
                    .Text($"{grandTotal:C}")
                    .FontSize(10);
            });
        }

        #endregion


        #region Transaction Report

        /// <summary>
        /// Create a PDF file for the transaction report
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="transactionLines">The list of transaction lines</param>
        /// <param name="reportDate">The date of the report</param>
        /// <param name="reportType">The type of the report</param>
        /// <remarks>
        /// The report type can be "Quotidien" or "Annuel"
        /// </remarks>
        /// <remarks>
        /// The transaction lines are the list of transactions
        /// </remarks>
        /// <remarks>
        public static void CreateTransactionsReportPdf(
            string filePath,
            List<TransactionLine> transactionLines,
            DateTime reportDate,
            string reportType
        )
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header()
                        .Element(
                            container =>
                                ComposeTransactionsHeader(container, reportDate, reportType)
                        );
                    page.Content()
                        .Element(content => ComposeTransactionsContent(content, transactionLines));
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            document.GeneratePdf(filePath);
        }

        /// <summary>
        /// Compose the header of the transaction report
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="reportDate">The date of the report</param>
        /// <param name="reportType">The type of the report</param>
        /// <remarks>
        /// The report type can be "Quotidien" or "Annuel"
        /// </remarks>
        private static void ComposeTransactionsHeader(
            IContainer container,
            DateTime reportDate,
            string reportType
        )
        {
            string headerText =
                reportType == "Quotidien"
                    ? $"Rapport de transaction pour le {reportDate:D}"
                    : $"Rapport de transaction pour {reportDate:Y}";

            container.Row(row =>
            {
                row.RelativeItem().Text(headerText).FontSize(20).Bold();
            });
        }

        /// <summary>
        /// Compose the content of the transaction report
        /// </summary>
        /// <param name="container">The container</param>
        /// <param name="transactionLines">The list of transaction lines</param>
        /// <remarks>
        /// The content is composed of a table with the transaction lines and the grand total
        /// </remarks>
        /// <remarks>
        /// The table has 8 columns: Numéro de commande, Date, Email, Billets vendues, $ Avant taxe, TPS, TVQ, $ Après taxe
        /// </remarks>
        /// <remarks>
        /// The grand total is the sum of the Montant Après Taxes
        /// </remarks>
        private static void ComposeTransactionsContent(
            IContainer container,
            List<TransactionLine> transactionLines
        )
        {
            // Style for the table headers
            static IContainer HeaderCellStyle(IContainer container)
            {
                return container
                    .DefaultTextStyle(x => x.Bold())
                    .PaddingVertical(5)
                    .BorderBottom(2)
                    .BorderColor("#000000");
            }

            // Style for the table cells
            static IContainer CellStyle(IContainer container)
            {
                return container.PaddingVertical(5).BorderBottom(1).BorderColor("#CCCCCC");
            }

            // Calculating the grand total of the Montant Après Taxes
            decimal grandTotal = transactionLines.Sum(line => (decimal)line.TotalAmountAfterTaxe);

            container.Table(table =>
            {
                // Definition of the columns with proportions adapted to your content
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(65);
                    columns.RelativeColumn(120);
                    columns.RelativeColumn(120);
                    columns.RelativeColumn(50);
                    columns.RelativeColumn(65);
                    columns.RelativeColumn(50);
                    columns.RelativeColumn(50);
                    columns.RelativeColumn(65);
                });

                // Table header with the defined style
                table.Header(header =>
                {
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("commande")
                        .FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("Date").FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("Email").FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("Billets vendues")
                        .FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .Text("$ Avant taxe")
                        .FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("TPS").FontSize(10);
                    header.Cell().Element(HeaderCellStyle).AlignMiddle().Text("TVQ").FontSize(10);
                    header
                        .Cell()
                        .Element(HeaderCellStyle)
                        .AlignMiddle()
                        .AlignRight()
                        .Text("$ Après taxe")
                        .FontSize(10);
                });

                // Table content with each transaction line
                foreach (var line in transactionLines)
                {
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .Text(line.OrderNumber)
                        .FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .Text(line.TransactionDate.ToString("U"))
                        .FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .Text(line.ClientEmail)
                        .FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .Text(line.TicketsSold.ToString())
                        .FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .Text($"{line.TotalAmountBeforeTaxe:C}")
                        .FontSize(8);
                    table.Cell().Element(CellStyle).AlignMiddle().Text($"{line.TPS:C}").FontSize(8);
                    table.Cell().Element(CellStyle).AlignMiddle().Text($"{line.TVQ:C}").FontSize(8);
                    table
                        .Cell()
                        .Element(CellStyle)
                        .AlignMiddle()
                        .AlignRight()
                        .Text($"{line.TotalAmountAfterTaxe:C}")
                        .FontSize(8);
                }

                // Grand Total line with the adapted style
                table.Cell().Element(HeaderCellStyle).Text("Grand Total").FontSize(10);
                table
                    .Cell()
                    .Element(HeaderCellStyle)
                    .AlignRight()
                    .Text($"{grandTotal:C}")
                    .FontSize(10);
            });
        }

        #endregion
    }
}
