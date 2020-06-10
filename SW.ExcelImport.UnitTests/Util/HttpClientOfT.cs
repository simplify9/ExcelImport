using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SW.Pmm.UnitTests
{
    public class HttpClient<TRequest,TOk,TBadRequest>
    {
        static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public class Result
        {
            public string ResponseBodyText { get; set; }

            public HttpStatusCode StatusCode { get; set; }
            
            public TOk Ok { get; set; }

            public TBadRequest BadRequest { get; set; }
        }

        readonly HttpClient _httpClient;

        public enum Format
        {
            Xml,
            Json
        }

        public HttpClient(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));

        }

        private static string GetMimeType(Format format) => format == Format.Xml ? "text/xml" : "application/json";

        private static HttpRequestMessage BuildRequest(HttpMethod method, string url,
            string body, Format inputFormat, Format outputFormat)
        {
            var rq = new HttpRequestMessage(method, url)
            {
                Content = new StringContent(body, Encoding.UTF8, GetMimeType(inputFormat))
            };
            rq.Headers.TryAddWithoutValidation("Accept", GetMimeType(outputFormat));
            return rq;
        }

        public async Task<Result> PostJson(string url, TRequest requestBody)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var stringPayload = JsonConvert.SerializeObject(requestBody, _jsonSettings);
            var response = await _httpClient.SendAsync(BuildRequest(HttpMethod.Post, url, stringPayload, Format.Json, Format.Json));

            //var stringPayload = JsonConvert.SerializeObject(requestBody);
            //var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode,
                    Ok = JsonConvert.DeserializeObject<TOk>(body, _jsonSettings)
                };
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode,
                    BadRequest = JsonConvert.DeserializeObject<TBadRequest>(body, _jsonSettings)
                };
            }
            else
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode
                };
            }
        }

        public async Task<Result> PostXml(string url, TRequest requestBody)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            XmlSerializer xsSubmit = new XmlSerializer(typeof(TRequest));
            var subReq = requestBody;
            string xml = null;

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = true,
                }))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }

            var response = await _httpClient.SendAsync(BuildRequest(HttpMethod.Post, url, xml, Format.Xml, Format.Xml));



        
            var body = await response.Content.ReadAsStringAsync();
           
            if (response.IsSuccessStatusCode)
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode,
                    Ok =TestHelper.GetTestObjectXml<TOk>(body)
                };
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode,
                    BadRequest = TestHelper.GetTestObjectXml<TBadRequest>(body)
                   // JsonConvert.DeserializeObject<TBadRequest>(body)
                };
            }
            else
            {
                return new Result
                {
                    ResponseBodyText = body,
                    StatusCode = response.StatusCode
                };
            }
        }
    }
}
