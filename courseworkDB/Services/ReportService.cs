using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;



namespace courseworkDB.Services
{
    public class ReportService
    {
        private const string ReportFolderPath = "D:\\AD coursework\\courseworkDB\\courseworkDB\\Models\\Reports\\";

        public void GenerateDailyReport(DateTime day, decimal totalSales, List<CoffeeStatistics> topFiveCoffees, List<AddInStatistics> topFiveAddIns)
        {
            string fileName = $"DailyReport_{day:yyyyMMdd}.pdf";
            string filePath = System.IO.Path.Combine(ReportFolderPath, fileName);

            // Disable license validation
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.DefaultTextStyle(x => x.FontSize(13));

                    page.Header()
                        .Text("Daily Report")
                        .SemiBold().FontSize(25);

                    page.Content().Column(x =>
                    {
                        x.Item().Text($"Total Sales: {totalSales}");
                        x.Spacing(20);
                        x.Item().Text("Top 5 Coffees");

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Coffee Name");
                                header.Cell().Text("Total Quantity Sold");
                            });

                            foreach (var item in topFiveCoffees)
                            {
                                table.Cell().Text(item.CoffeeName);
                                table.Cell().Text(item.TotalQuantitySold.ToString());
                            }
                        });

                        x.Item().Text("Top 5 AddIns");

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("AddIn Name");
                                header.Cell().Text("Total Quantity Sold");
                            });

                            foreach (var item in topFiveAddIns)
                            {
                                table.Cell().Text(item.AddInName);
                                table.Cell().Text(item.TotalQuantitySold.ToString());
                            }
                        });
                    });

                });
            })
            .GeneratePdf(filePath);

            // Open the generated PDF
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        public void GenerateMonthlyReport(DateTime startDate, DateTime endDate, decimal totalSales, List<CoffeeStatistics> topFiveCoffees, List<AddInStatistics> topFiveAddIns)
        {
            string fileName = $"MonthlyReport_{startDate:yyyyMM}.pdf";
            string filePath = System.IO.Path.Combine(ReportFolderPath, fileName);

            // Disable license validation
            QuestPDF.Settings.License = LicenseType.Community;


            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.DefaultTextStyle(x => x.FontSize(13));

                    page.Header()
                        .Text("Monthly Report")
                        .SemiBold().FontSize(25);

                    page.Content().Column(x =>
                    {
                        x.Item().Text($"Total Sales: {totalSales}");
                        x.Item().Text($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                        x.Spacing(20);
                        x.Item().Text("Top 5 Coffees");

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Coffee Name");
                                header.Cell().Text("Total Quantity Sold");
                            });

                            foreach (var item in topFiveCoffees)
                            {
                                table.Cell().Text(item.CoffeeName);
                                table.Cell().Text(item.TotalQuantitySold.ToString());
                            }
                        });

                        x.Item().Text("Top 5 AddIns");

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(column =>
                            {
                                column.RelativeColumn();
                                column.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("AddIn Name");
                                header.Cell().Text("Total Quantity Sold");
                            });

                            foreach (var item in topFiveAddIns)
                            {
                                table.Cell().Text(item.AddInName);
                                table.Cell().Text(item.TotalQuantitySold.ToString());
                            }
                        });
                    });

                });
            })
            .GeneratePdf(filePath);

            // Open the generated PDF
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
    }

    public class CoffeeStatistics
    {
        public string CoffeeName { get; set; }
        public int TotalQuantitySold { get; set; }
    }

    public class AddInStatistics
    {
        public string AddInName { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
    
