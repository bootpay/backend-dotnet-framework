﻿using System; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Cancel
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
        [JsonProperty("cancel_id")]
        public string cancelId { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string reason { get; set; }
        public RefundData refund { get; set; }
    }
}
