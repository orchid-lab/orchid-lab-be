using System.Reflection;

namespace orchid_backend_net.Infrastructure.Service.PdfGenerator.Template
{
    public static class TemplateLoader
    {
        public static async Task<string> LoadTemplateAsync()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Service.PdfGenerator.Template.ExperimentReport.html";
            var fullResourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith(resourceName));

            if (fullResourceName == null)
                throw new FileNotFoundException($"Template {resourceName} not found in embedded resources.");

            using var stream = assembly.GetManifestResourceStream(fullResourceName)!;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
