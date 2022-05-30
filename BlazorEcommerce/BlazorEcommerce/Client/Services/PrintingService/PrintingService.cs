using Microsoft.JSInterop;

namespace BlazorEcommerce.Client.Services.PrintingService
{
    public class PrintingService : IPrintingService
    {
        private IJSObjectReference module;
        private readonly IJSRuntime jsRuntime;
        private readonly HttpClient _http;

        public PrintingService(IJSRuntime jsRuntime, HttpClient http)
        {
            this.jsRuntime = jsRuntime;
            _http = http;
        }

        public async Task Print(PrintOptions options)
        {
            if (module is null)
                await ImportModule();

            await module.InvokeVoidAsync("print", new PrintOptionsAdapter(options));
        }

        public Task Print(string printable, PrintType printType = PrintType.Pdf)
        {
            return Print(new PrintOptions(printable) { Printable = printable, Type = printType, ScanStyles = true });
        }

        public Task Print(string printable, bool showModal, PrintType printType = PrintType.Pdf)
        {
            return Print(new PrintOptions(printable) { ShowModal = showModal, Type = printType, ScanStyles = true });
        }

        internal async ValueTask ImportModule()
        {
            module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Append.Blazor.Printing/scripts.js");
        }

        public Task Print(string printable, PrintType printType = PrintType.Html, bool scanStyles = false)
        {
            return Print(new PrintOptions(printable) { Printable = printable, Type = printType, ScanStyles = scanStyles });
        }

        public Task Print(string printable, PrintType printType = PrintType.Html, string style = null)
        {
            return Print(new PrintOptions(printable) { Printable = printable, Type = printType, ScanStyles = true, Style = style });
        }

        public async Task PrintAsync(string printable, string rootLocation, PrintType printType = PrintType.Html)
        {
            var template = await GetFileTextFromRootAsync(rootLocation);

            await Print(new PrintOptions(printable) { Printable = printable, Type = printType, ScanStyles = true, Style = template });

            return;
        }

        private async Task<string> GetFileTextFromRootAsync(string rootLocation)
        {
            if (string.IsNullOrWhiteSpace(rootLocation))
            {
                return string.Empty;
            }

            var templateText = string.Empty;

            templateText = await _http.GetStringAsync(rootLocation);

            templateText = "html { -webkit-print-color-adjust: exact; }" + templateText.Trim();

            return templateText;
        }
    }
}
