﻿namespace BlazorEcommerce.Client.Services.PrintingService
{
    /// <summary>
    /// Adapts the <see cref="PrintOptions" /> to the JavaScript version.
    /// </summary>
    internal record PrintOptionsAdapter
    {
        public string Printable { get; init; }
        public string Type { get; init; }
        public bool ShowModal { get; init; }
        public string ModalMessage { get; init; } = "Retrieving Document...";
        public bool? Base64 { get; set; }

        public bool ScanStyles { get; init; }

        public string Style { get; set; }

        public PrintOptionsAdapter(PrintOptions options)
        {
            Printable = options.Printable;
            Type = Enum.GetName(typeof(PrintType), options.Type).ToLower();
            ShowModal = options.ShowModal;
            ModalMessage = options.ModalMessage;
            Base64 = options.Base64 == true ? true : null;
            ScanStyles = options.ScanStyles;
            Style = options.Style;
        }
    }
}
