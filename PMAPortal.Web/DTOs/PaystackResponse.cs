using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMAPortal.Web.DTOs
{
    public class PaystackResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public PaystackResponseData Data { get; set; }
    }
}
