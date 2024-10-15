using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using SWD.SAPelearning.Repository.DTO.PaymentDTO;

namespace SWD.SAPelearning.Service
{
    public class PaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Payment CreatePayment(string returnUrl, string cancelUrl, decimal amount)
        {
            var apiContext = PayPalConfig.GetAPIContext(_configuration);

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
            {
                new Transaction
                {
                    amount = new Amount
                    {
                        currency = "USD",
                        total = amount.ToString("F2")
                    },
                    description = "Payment for course enrollment"
                }
            },
                redirect_urls = new RedirectUrls
                {
                    return_url = returnUrl,
                    cancel_url = cancelUrl
                }
            };

            return payment.Create(apiContext);
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            var apiContext = PayPalConfig.GetAPIContext(_configuration);
            var payment = new Payment { id = paymentId };
            var paymentExecution = new PaymentExecution { payer_id = payerId };
            return payment.Execute(apiContext, paymentExecution);
        }
    }
}
