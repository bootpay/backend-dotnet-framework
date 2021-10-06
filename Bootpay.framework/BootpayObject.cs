﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text; 
using System.Threading.Tasks;
using Bootpay.converter;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay
{
    public class BootpayObject
    {
        protected string _applicationId;
        protected string _privateKey;
        protected string _baseUrl;
        private string _token;


        public const int MODE_DEVELOPMENT = 0;
        public const int MODE_TEST = 1;
        public const int MODE_STAGE = 2;
        public const int MODE_PRODUCTION = 3;


        private readonly Dictionary<int, string> _URL = new Dictionary<int, string>()
        {
            { MODE_DEVELOPMENT, "https://dev-api.bootpay.co.kr/" },
            { MODE_TEST, "https://test-api.bootpay.co.kr/" },
            { MODE_STAGE, "https://stage-api.bootpay.co.kr/" },
            { MODE_PRODUCTION, "https://api.bootpay.co.kr/" },
        };

        public BootpayObject(string applicationId, string privateKey, int mode = MODE_PRODUCTION)
        {
            _applicationId = applicationId;
            _privateKey = privateKey;
            _baseUrl = _URL[mode];
        }

        /***
         * ## 1. 토큰 발급 
         *
         *   부트페이와 서버간 통신을 하기 위해서는 부트페이 서버로부터 토큰을 발급받아야 합니다.  
         *   발급된 토큰은 30분간 유효하며, 최초 발급일로부터 30분이 지날 경우 토큰 발급 함수를 재호출 해주셔야 합니다.
         */
        public async Task<ResToken> GetAccessToken()
        { 
            Token token = new Token()
            {
                applicationId = _applicationId,
                privateKey = _privateKey
            };

            string json = JsonConvert.SerializeObject(token,
                        Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });


            var res = await SendAsync<ResToken>("request/token", HttpMethod.Post, json);
            _token = res.data.token;
            return res;
        }
         

        public async Task<TRes> SendAsync<TRes>(string url, HttpMethod method, string json = "")
        //public async Task<string> SendAsync(string url, HttpMethod method, string json = "")
        { 
            using (HttpRequestMessage request = new HttpRequestMessage())
            using (HttpClient client = new HttpClient())
            {                     
                request.Method = method;
                request.RequestUri = new Uri(_baseUrl + url);
                Console.WriteLine("json length: " + json.Length);

                if (json.Length > 0) {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");  
                } 

                if (_token != null && _token.Length > 0) { client.DefaultRequestHeaders.Add("Authorization", _token); }
                
                var res = await client.SendAsync(request);
                string resJson = await res.Content.ReadAsStringAsync();

                Console.WriteLine(resJson);
                

                return JsonConvert.DeserializeObject<TRes>(resJson);

                //return await res.Content.ReadAsStringAsync();
            }
        }
    }
}
