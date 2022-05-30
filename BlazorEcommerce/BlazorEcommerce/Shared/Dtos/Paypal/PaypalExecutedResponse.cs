namespace BlazorEcommerce.Shared
{
    public class PaypalExecutedResponse
    {
        public string Id { get; set; }

        public string Intent { get; set; }

        public string State { get; set; }

        public string Cart { get; set; }

        public Payer Payer { get; set; }

        public Transaction[] Transactions { get; set; }

        public DateTime CreateTime { get; set; }

        public Link[] Links { get; set; }
    }

    public class Payer
    {
        public string PaymentMethod { get; set; }

        public string Status { get; set; }

        public PayerInfo PayerInfo { get; set; }
    }

    public class PayerInfo
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PayerId { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public string CountryCode { get; set; }

        public BillingAddress BillingAddress { get; set; }
    }

    public class ShippingAddress
    {
        public string RecipientName { get; set; }

        public string Line1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string CountryCode { get; set; }
    }

    public class BillingAddress
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class Transaction
    {
        public Amount Amount { get; set; }
        public Payee Payee { get; set; }
        public ItemList ItemList { get; set; }
        public RelatedResources[] RelatedResources { get; set; }
    }

    public class Amount
    {
        public string Total { get; set; }
        public string Currency { get; set; }
        public Details Details { get; set; }
    }

    public class Details
    {
        public string Subtotal { get; set; }
    }

    public class Payee
    {
        public string MerchantId { get; set; }

        public string Email { get; set; }
    }

    public class ItemList
    {
        public ShippingAddress1 ShippingAddress { get; set; }
    }

    public class ShippingAddress1
    {
        public string RecipientName { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class RelatedResources
    {
        public Sale Sale { get; set; }
    }

    public class Sale
    {
        public string Id { get; set; }
        public string State { get; set; }
        public Amount Amount { get; set; }
        public string PaymentMode { get; set; }
        public string ProtectionEligibility { get; set; }
        public string ProtectionElegibilityType { get; set; }
        public TransactionFee TransactionFee { get; set; }
        public string ParentPayment { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Link[] Links { get; set; }
    }

    public class TransactionFee
    {
        public string Value { get; set; }

        public string Currency { get; set; }
    }

    public class Link
    {
        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }
    }
}
