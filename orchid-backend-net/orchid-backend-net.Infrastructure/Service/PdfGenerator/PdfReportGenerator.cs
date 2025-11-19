using orchid_backend_net.Application.Common.Interfaces;
using orchid_backend_net.Infrastructure.Service.PdfGenerator.Template;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace orchid_backend_net.Infrastructure.Service.PdfGenerator
{
    public class PdfReportGenerator: IPdfReportGenerator
    {
        public async Task<byte[]> GenerateAsync(object model)
        {
            // Load embedded template
            var templateHtml = await TemplateLoader.LoadTemplateAsync();

            // Render with Scriban
            var scribanTemplate = Scriban.Template.Parse(templateHtml);
            var html = await scribanTemplate.RenderAsync(model, member => member.Name);

            //Get Chromium path and launch browser
            var browserFetcher = new BrowserFetcher();
            var revisionInfo = await browserFetcher.DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = revisionInfo.GetExecutablePath()
            });

            //Convert HTML -> PDF by DinkToPdf
            await using var page = await browser.NewPageAsync();

            // Settings html
            await page.SetContentAsync(html);

            // Create PDF options
            var pdfOptions = new PdfOptions
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
            };

            return await page.PdfDataAsync(pdfOptions);
        }
    }
}
