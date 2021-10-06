using System;
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Submit
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
    }
}
