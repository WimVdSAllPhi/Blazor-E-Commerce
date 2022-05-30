namespace BlazorEcommerce.Client.Services.PrintingService
{
    public interface IPrintingService
    {
        Task Print(PrintOptions options);

        Task Print(string printable, PrintType printType = PrintType.Pdf);

        Task Print(string printable, bool showModal, PrintType printType = PrintType.Pdf);

        Task Print(string printable, PrintType printType = PrintType.Html, bool scanStyles = false);

        Task Print(string printable, PrintType printType = PrintType.Html, string style = null);

        Task PrintAsync(string printable, string location, PrintType printType = PrintType.Html);
    }
}
