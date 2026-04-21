using System.Reflection;

namespace orchid_backend_net.Infrastructure.Service.PdfGenerator.Template
{
    public static class TemplateLoader
    {
        public static async Task<string> LoadTemplateAsync(string templateName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullResourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith(templateName))
                ?? throw new FileNotFoundException(
                    $"Template '{templateName}' not found in embedded resources.");

            using var stream = assembly.GetManifestResourceStream(fullResourceName)!;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
