namespace BlazorEcommerce.Shared
{
    public class PaypalCreatedResponse
    {
        public string Id { get; set; }

        public string Intent { get; set; }

        public string State { get; set; }

        public PayerResponse Payer { get; set; }

        public TransactionResponse[] Transactions { get; set; }

        public DateTime CreateTime { get; set; }

        public LinkResponse[] Links { get; set; }
    }

    public class PayerResponse
    {
        public string PaymentMethod { get; set; }
    }

    public class TransactionResponse
    {
        public AmountResponse Amount { get; set; }
        public object[] RelatedResources { get; set; }
    }

    public class AmountResponse
    {
        public string Total { get; set; }
        public string Currency { get; set; }
    }

    public class LinkResponse
    {
        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }
    }
}
