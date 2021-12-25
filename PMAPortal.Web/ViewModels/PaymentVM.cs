using PMAPortal.Web.Extensions;
using PMAPortal.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.ViewModels
{
    public class PaymentVM
    {
        public long Id { get; set; }
        public long ApplicationId { get; set; }
        public string Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTimeOffset DatePaid { get; set; }
        public string FormattedDatePaid { get
            {
                return DatePaid.ToString("MMM d, yyyy");
            }
        }

        public static PaymentVM FromPayment(Payment payment)
        {
            return new PaymentVM
            {
                Id = payment.Id,
                ApplicationId = payment.ApplicationId,
                Amount = payment.Amount.Format(),
                IsPaid = payment.IsPaid,
                DatePaid = payment.DatePaid
            };
        }
    }
}
