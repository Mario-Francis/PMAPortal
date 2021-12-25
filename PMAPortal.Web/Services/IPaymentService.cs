using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.Services
{
    public interface IPaymentService
    {
        Task<(bool isSuccess, DateTimeOffset? datePaid)> VerifyPayment(long paymentId, string paymentRef);
        Task<long> AddPayment(long applicationId, decimal amount);
        Task UpdatePaidPayment(long paymentId, DateTimeOffset paidDate);
        Task LogPayment(long paymentId, string paymentRef, bool isSuccess);
        Task<Payment> GetPayment(long id);
    }
}
