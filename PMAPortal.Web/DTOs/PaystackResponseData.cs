using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMAPortal.Web.DTOs
{
    public class PaystackResponseData
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        [JsonPropertyName("gateway_response")]
        public string GatewayResponse { get; set; }
        [JsonPropertyName("paid_at")]
        public DateTimeOffset PaidAt { get; set; }
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        public string Channel { get; set; }
        public string Currency { get; set; }
        public decimal Fees { get; set; }
    }
}
