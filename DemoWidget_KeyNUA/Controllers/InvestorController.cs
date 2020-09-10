using DemoWidget_KeyNUA.DTOs;
using DemoWidget_KeyNUA.Models;
using DemoWidget_KeyNUA.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoWidget_KeyNUA.Controllers
{
    public class InvestorController : Controller
    {

        private string urlAPI = "https://localhost:44319/api/investor/keynua/";

        public IActionResult Index()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(InvestorInfoModel investorInfo)
        {

            if (investorInfo == null)
            {
                throw new ArgumentNullException(nameof(investorInfo));
            }

            ResponseContractDTO responseContractDTO = new ResponseContractDTO();

            using (var httpClient = new HttpClient())
            {

                string jsonNewContract = JsonConvert.SerializeObject(investorInfo);

                //Convert object properties in lowercase
                var newContractObject = JObject.Parse(jsonNewContract, new JsonLoadSettings());
                JsonHelpers.ChangePropertiesToLowerCase(newContractObject);

                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                    

                    var builder = new UriBuilder(new Uri(urlAPI));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, builder.Uri);

                    request.Content = new StringContent(newContractObject.ToString(Formatting.None), Encoding.UTF8, "application/json");//CONTENT-TYPE header

                    using (var response = await httpClient.SendAsync(request))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        responseContractDTO = JsonConvert.DeserializeObject<ResponseContractDTO>(apiResponse);

                        ViewBag.TokenCustomer = responseContractDTO.Users[0].Token;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return View("SignContract");
            //return Ok(responseContractDTO);

        }


    }
}
