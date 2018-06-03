namespace Knarr.Core.Models.Payments
{
    public class BraintreeNewCustomer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //Credit card info
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public string CardHolderName { get; set; }

        //Billing Address
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string StreetAddress { get; set; }
        public string Region { get; set; }
    }
}
