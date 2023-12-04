namespace EcommerceAPI.Models
{
    using PayPal.Api;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPayPalService
    {
        // Creates a payment and returns the approval URL to redirect the user to PayPal
        Task<string> CreatePayment(decimal amount, string currency, string returnUrl, string cancelUrl);

        // Executes an approved PayPal payment
        Task<bool> ExecutePayment(string paymentId, string payerId);

        // Optional: Add other methods as required, like refunding a payment, getting payment details, etc.
    }
}