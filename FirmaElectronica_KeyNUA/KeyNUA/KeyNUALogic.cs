using FirmaElectronica_KeyNUA.Model;
using FirmaElectronica_KeyNUA.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica_KeyNUA.KeyNUA
{
    public class KeyNUALogic : IKeyNUALogic
    {
        private KeyNUASettings _keyNUASettings { get; }
        private string EndPointURL = "/contracts/v1";
        public KeyNUALogic(IOptions<KeyNUASettings> keyNUASettings)
        {
            _keyNUASettings = keyNUASettings.Value;
        }

        public async Task<ResponseContractDTO> CreateContract(ContractInvestor contractInvestor)
        {
            NewContractDTO contractToSend = GetNewContractToSign(contractInvestor);

            ResponseContractDTO responseContractDTO = new ResponseContractDTO();

            using (var httpClient = new HttpClient())
            {

                string jsonNewContract = JsonConvert.SerializeObject(contractToSend);

                //Convert object properties in lowercase
                var newContractObject = JObject.Parse(jsonNewContract, new JsonLoadSettings());
                JsonHelpers.ChangePropertiesToLowerCase(newContractObject);

                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", _keyNUASettings.API_Token);
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _keyNUASettings.API_Key);

                    var builder = new UriBuilder(new Uri(_keyNUASettings.API_Url + EndPointURL));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, builder.Uri);
                    
                    request.Content = new StringContent(newContractObject.ToString(Formatting.None), Encoding.UTF8, "application/json");//CONTENT-TYPE header

                    using (var response = await httpClient.SendAsync(request))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        responseContractDTO = JsonConvert.DeserializeObject<ResponseContractDTO>(apiResponse);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
             
            }

            return responseContractDTO;
        }

        private NewContractDTO GetNewContractToSign(ContractInvestor contractInvestor)
        {
            DocumentContractDTO documentToSign = new DocumentContractDTO()
            {
                Name = contractInvestor.FileName,
                Base64 = GetFileInBytes(contractInvestor.FileName)
            };

            UserContractDTO userSign = new UserContractDTO()
            {
                Name = contractInvestor.FullName,
                Email = contractInvestor.Email
            };

            NewContractDTO newContractDTO = new NewContractDTO()
            {
                Title = Constants.Contract.TitleContractInvestor,
                Language = Constants.Contract.Language_Spanish,
                TemplateId = Constants.Contract.TemplateIdInvesor
            };

            newContractDTO.Documents.Add(documentToSign);
            newContractDTO.Users.Add(userSign);

            return newContractDTO;
        }

        private byte[] GetFileInBytes(string fileName)
        {
            string filePath = @"Template_Investor\";
            Byte[] bytesFile = File.ReadAllBytes(filePath + fileName);

            //String file = Convert.ToBase64String(bytes);

            return bytesFile;
        }


        public async Task<ResponseContractDTO> GetContractCreated(string idContract)
        {
           

            ResponseContractDTO responseContractDTO = new ResponseContractDTO();

            using (var httpClient = new HttpClient())
            {

                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", _keyNUASettings.API_Token);
                    httpClient.DefaultRequestHeaders.Add("x-api-key", _keyNUASettings.API_Key);

                    var builder = new UriBuilder(new Uri(_keyNUASettings.API_Url + EndPointURL + '/' + idContract));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
 
                    using (var response = await httpClient.SendAsync(request))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        responseContractDTO = JsonConvert.DeserializeObject<ResponseContractDTO>(apiResponse);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return responseContractDTO;
        }


    }
}
