using CloudProductsBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloudProductsBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudProductController : ControllerBase
    {

        private readonly ILogger<CloudProductController> _logger;

        private  IConfiguration _configuration;

        public CloudProductController(ILogger<CloudProductController> logger, IConfiguration iConfig)
        {
            _logger = logger;
            _configuration = iConfig;
        }
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            string dbConn = _configuration.GetSection("MySettings").GetSection("ApiUrl").Value;

            double markupvalue = Convert.ToDouble(_configuration.GetSection("MySettings").GetSection("markupvalue").Value);
            string ApiUrl = _configuration.GetSection("MySettings").GetSection("ApiUrl").Value;
            string Header = _configuration.GetSection("MySettings").GetSection("Header").Value; 

            var products =  GetJsonAsync<List<Product>>(ApiUrl, Header).ConfigureAwait(false).GetAwaiter().GetResult();

            products.ForEach(p =>
            {
                p.markupPrice = p.unitPrice * markupvalue;
                p.maximumQuantity = p.maximumQuantity == null ? 0 : p.maximumQuantity;
            });

            return products;
        }

        public async Task<T> GetJsonAsync<T>(string url, string Header)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(Header))
                {
                    httpClient.DefaultRequestHeaders.Add("api-key", Header);
                }

                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
        }
    }
}
