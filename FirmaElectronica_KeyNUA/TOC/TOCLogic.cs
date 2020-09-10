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

namespace FirmaElectronica_KeyNUA.TOC
{
    public class TOCLogic : ITOCLogic
    {
        private TOCSettings _tocSettings { get; }

        private string EndPointURL = "/upload";

        public TOCLogic(IOptions<TOCSettings> tocSettings)
        {
            _tocSettings = tocSettings.Value;
        }

        public async Task<ResponseTOCContractDTO> CreateContract(ContractInvestor contractInvestor)
        {
            NewRequestContractDTO contractToSend = GetNewContractToSign(contractInvestor);

            ResponseTOCContractDTO responseContractDTO = new ResponseTOCContractDTO();

            using (var httpClient = new HttpClient())
            {

                string jsonSigners = JsonConvert.SerializeObject(contractToSend.Signers);

                ////Convert object properties in lowercase
                var newContractObject = JObject.Parse(jsonSigners, new JsonLoadSettings());
                JsonHelpers.ChangePropertiesToLowerCase_TOC(newContractObject);

                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", _tocSettings.API_Key);

                    using (var form = new MultipartFormDataContent())
                    {
                        string filePath = @"Template_Investor\" + contractInvestor.FileName;
                        
                            using (var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath)))
                            {
                                
                                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                                form.Add(fileContent, "file", Path.GetFileName(filePath));
                                form.Add(new StringContent(newContractObject.ToString()), "signers");
                                form.Add(new StringContent(contractToSend.Message), "message");
                                form.Add(new StringContent(contractToSend.Lang), "lang");
                                var builder = new UriBuilder(new Uri(_tocSettings.API_Url + EndPointURL));

                                using (var response = await httpClient.PostAsync(builder.Uri, form))
                                {
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    responseContractDTO = JsonConvert.DeserializeObject<ResponseTOCContractDTO>(apiResponse);
                                }                               
                            }      

                    }
                  
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return responseContractDTO;
        }

        private NewRequestContractDTO GetNewContractToSign(ContractInvestor contractInvestor)
        {

            SignerDTO signerDTO = new SignerDTO();
            signerDTO.Firmantes.Add(contractInvestor.Email);
            signerDTO.Identificadores.Add(contractInvestor.DocNumber);
            signerDTO.Copies.Add(Constants.Contract.CopyEmailTo);

            NewRequestContractDTO newContractDTO = new NewRequestContractDTO()
            {
                File = GetFileInBytes(contractInvestor.FileName),
                Signers = signerDTO,
                Message = Constants.Contract.MessageWelcome,
                Lang = Constants.Contract.Language_Spanish
            };

            return newContractDTO;
        }

        private byte[] GetFileInBytes(string fileName)
        {
            string filePath = @"Template_Investor\";
            Byte[] bytesFile = File.ReadAllBytes(filePath + fileName);

            return bytesFile;
        }
    }
}
