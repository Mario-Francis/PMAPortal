using Microsoft.Extensions.Options;
using PMAPortal.Web.Data.Repositories;
using PMAPortal.Web.DTOs;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services.Implementations
{
    public class PaymentService:IPaymentService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILoggerService<PaymentService> logger;
        private readonly IOptionsSnapshot<AppSettings> appSettingsDelegate;
        private readonly IRepository<Payment> paymentRepo;
        private readonly IRepository<PaymentLog> paymentLogRepo;

        public PaymentService(IHttpClientFactory clientFactory,
            ILoggerService<PaymentService> logger,
            IOptionsSnapshot<AppSettings> appSettingsDelegate,
            IRepository<Payment> paymentRepo,
            IRepository<PaymentLog> paymentLogRepo)
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
            this.appSettingsDelegate = appSettingsDelegate;
            this.paymentRepo = paymentRepo;
            this.paymentLogRepo = paymentLogRepo;
        }

        public async Task<(bool isSuccess, DateTimeOffset? datePaid)> VerifyPayment(long paymentId, string paymentRef)
        {
            var isSuccess = false;
            DateTimeOffset? date = null;
            if (string.IsNullOrEmpty(paymentRef))
            {
                throw new AppException("Payment ref is required");
            }

            var url = $"{appSettingsDelegate.Value.Paystack.BaseUrl}/transaction/verify/{paymentRef}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", appSettingsDelegate.Value.Paystack.SKey);
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var resObj = JsonSerializer.Deserialize<PaystackResponse>(resContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if(resObj.Status && resObj.Data!=null && resObj.Data.Status == "success")
                {
                    isSuccess = true;
                    date = resObj.Data.PaidAt;
                }
            }
            else
            {
                throw new Exception(resContent);
            }

            return (isSuccess, date);
        }

        public async Task<long> AddPayment(long applicationId, decimal amount)
        {
            var payment = new Payment
            {
                ApplicationId = applicationId,
                Amount = amount,
                CreatedDate = DateTimeOffset.Now,
                UpdatedDate = DateTimeOffset.Now
            };
            await paymentRepo.Insert(payment);
            return payment.Id;
        }

        public async Task UpdatePaidPayment(long paymentId, DateTimeOffset paidDate)
        {
            var payment = await paymentRepo.GetById(paymentId);
            if (payment == null)
            {
                throw new AppException("Payment id is not valid");
            }
            payment.IsPaid = true;
            payment.DatePaid = paidDate;
            payment.UpdatedDate = DateTimeOffset.Now;

            await paymentRepo.Update(payment);
        }

        public async Task LogPayment(long paymentId, string paymentRef, bool isSuccess)
        {
            var paymentLog = new PaymentLog
            {
                IsSuccess = isSuccess,
                PaymentId = paymentId,
                PaymentRef = paymentRef,
                CreatedDate = DateTimeOffset.Now
            };
            await paymentLogRepo.Insert(paymentLog);
        }

        public async Task<Payment> GetPayment(long id)
        {
            return await paymentRepo.GetById(id);
        }
    
    }
}
