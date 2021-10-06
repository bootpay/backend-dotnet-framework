using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{
    public class LinkController : Controller
    {
        // 6. 결제 링크 생성 
        //[HttpGet("link")]
        public async Task<IActionResult> Index()
        {
            Payload payload = new Payload();
            payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            payload.price = 1000;
            payload.name = "테스트 결제";
            payload.pg = "nicepay";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.requestPayment(payload);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }
    }

    
}
