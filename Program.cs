using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using HtmlAgilityPack;

namespace IsraBlogScraper
{
    public class Options
    {
        [Option('y', "year", Required = true, HelpText = "last post published year")]
        public int Year { get; set; }

        [Option('b', "blogId", Required = true, HelpText = "the ID of the blog")]
        public int BlogId { get; set; }

        [Option('o', "outputDirectory", Required = false, HelpText = "the output directory for the blog results")]
        public string OutputDirectory { get; set; }
    }

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<Options>(args) as Parsed<Options>;

            if (options == null)
            {
                return 1;
            }

            Console.WriteLine("Hello World!");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var blogId = options.Value.BlogId;

            var client = new HttpClient();
            var hasMorePages = true;
            var hasMoreYears = true;
            var root = options.Value.OutputDirectory ?? "blog";
            for (var year = options.Value.Year; hasMoreYears && year > 1990; year--)
            {
                Directory.CreateDirectory(Path.Combine(root, year.ToString()));
                hasMoreYears = false;
                for (var month = 12; month > 0; month--)
                {
                    var monthName = new DateTimeFormatInfo().GetMonthName(month);
                    Directory.CreateDirectory(Path.Combine(root, year.ToString(), monthName));

                    hasMorePages = true;

                    for (int pageNumber = 1; hasMorePages; pageNumber++)
                    {
                        Console.WriteLine($"Scraping: {month}/{year}, page {pageNumber}");

                        var response = await client.GetAsync(
                        $"http://israblog.nana10.co.il/blogread.asp?blog={blogId}&year={year}&month={month}&pagenum={pageNumber}&catdesc=");

                        var content = await response.Content.ReadAsByteArrayAsync();

                        var pageDocument = new HtmlDocument();

                        var str = Encoding.GetEncoding(1255).GetString(content);


                        pageDocument.LoadHtml(str);

                        var nodes = pageDocument.DocumentNode.SelectNodes("//td[contains(@class,'blog')]");

                        hasMorePages = nodes.Count > 1;
                        if (hasMorePages)
                        {
                            hasMoreYears = true;
                            var filePath = Path.Combine(root, year.ToString(), monthName, $"{pageNumber}.html");
                            File.WriteAllText(filePath, str);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
