using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Application.ExperimentLog.Dto.Report;
using orchid_backend_net.Infrastructure.Service.PdfGenerator.Template;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace orchid_backend_net.Infrastructure.Service.PdfGenerator
{
    public class PdfReportGenerator: IPdfReportGenerator
    {
        public async Task<byte[]> GenerateProcessLogAsync(
        ExperimentProcessLogReportModel model,
        CancellationToken cancellationToken = default)
        => await RenderToPdfAsync("ExperimentProcessLog.html", model);

        public async Task<byte[]> GenerateSummaryReportAsync(
            ExperimentSummaryReportModel model,
            CancellationToken cancellationToken = default)
            => await RenderToPdfAsync("ExperimentSummaryReport.html", model);

        // Method cũ — giữ lại để không breaking change nếu có caller khác
        public static async Task<byte[]> GenerateAsync(object model)
            => await RenderToPdfAsync("ExperimentReport.html", model);

        private static async Task<byte[]> RenderToPdfAsync(
            string templateName,
            object model)
        {
            var templateHtml = await TemplateLoader.LoadTemplateAsync(templateName);    

            var scribanTemplate = Scriban.Template.Parse(templateHtml);
            var html = await scribanTemplate.RenderAsync(model, member => ToSnakeCase(member.Name));

            var browserFetcher = new BrowserFetcher();
            var revisionInfo = await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = revisionInfo.GetExecutablePath(),
                Args = new[] { "--no-sandbox"}
            });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            return await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Right = "20px",
                    Bottom = "20px",
                    Left = "20px"
                }
            });
        }

        private static string ToSnakeCase(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, "(?<=[a-z0-9])([A-Z])", "_$1").ToLower();
        }
    }
}
