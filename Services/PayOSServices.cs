using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayOS;
using PayOS.Models;

namespace ASM_C_6_API.Services
{
    public class PayOSServices
    {
        //public interface IPayOSServices
        //{
        //    Task<CreatePaymentResult> CreatePaymentLink(PaymentData paymentData);
        //    Task<PaymentLinkInformation> GetPaymentLinkInformation(long orderCode);
        //    Task<PaymentLinkInformation> CancelPaymentLink(long orderCode, string cancellationReason);
        //    WebhookData VerifyPaymentWebhookData(WebhookType webhookBody, string payload, string signatureHeader = null);
        //}
    }
}
