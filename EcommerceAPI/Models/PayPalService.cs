namespace EcommerceAPI.Models
{
    using PayPal.Api;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PayPalService : IPayPalService // Actualiza con el espacio de nombres real de tu modelo
    {
        private readonly PayPalSettings _settings;

        public PayPalService(PayPalSettings settings)
        {
            _settings = settings;
        }

        public async Task<string> CreatePayment(decimal amount, string currency, string returnUrl, string cancelUrl)
        {
            var apiContext = GetApiContext();

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction> {new Transaction
                {
                    amount = new Amount
                    {
                        total = amount.ToString("N2"),
                        currency = currency,
                        details = new Details
                        {
                            tax = "0",
                            shipping = "0",
                            subtotal = amount.ToString("N2")
                        }
                    },
                    description = "buy on Ecommerce",
                    invoice_number = new Random().Next(999999).ToString()
                }},
                redirect_urls = new RedirectUrls
                {
                    return_url = returnUrl,
                    cancel_url = cancelUrl
                }
            };

            var createdPayment = await Task.FromResult(payment.Create(apiContext));

            // Extraer la URL de aprobación
            var approvalUrl = createdPayment.links.Find(link => link.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).href;
            return approvalUrl;
        }

        public async Task<bool> ExecutePayment(string paymentId, string payerId)
        {
            var apiContext = GetApiContext();
            var paymentExecution = new PaymentExecution { payer_id = payerId };
            var payment = new Payment { id = paymentId };

            var executedPayment = await Task.FromResult(payment.Execute(apiContext, paymentExecution));
            return executedPayment.state.ToLower() == "approved";
        }

        private APIContext GetApiContext()
        {
            var config = new Dictionary<string, string>
            {
                { "clientId", _settings.ClientId },
                { "clientSecret", _settings.ClientSecret }
            };

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken);
        }
    }
}
