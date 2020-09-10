using FirmaElectronica_KeyNUA.KeyNUA;
using FirmaElectronica_KeyNUA.Model;
using FirmaElectronica_KeyNUA.TOC;
using FirmaElectronica_KeyNUA.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FirmaElectronica_KeyNUA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorController : ControllerBase
    {
        private readonly IKeyNUALogic _keyNUALogic;
        private readonly ITOCLogic _tocLogic;
        public InvestorController(IKeyNUALogic keyNUALogic, ITOCLogic tocLogic)
        {
            _keyNUALogic = keyNUALogic ??
                            throw new ArgumentNullException(nameof(keyNUALogic));
            _tocLogic = tocLogic ??
                throw new ArgumentNullException(nameof(tocLogic));
        }

        [HttpGet]
        public IActionResult GetInvestor()
        {
            return Ok();
        }

        [HttpPost]
        [Route("keynua")]
        public async Task<ResponseContractDTO> GenerateDocumentKeyNUA(InvestorInfo investorInfo)
        {

            if (investorInfo == null)
            {
                throw new ArgumentNullException(nameof(investorInfo));
            }

            GeneratePDF generatePDF = new GeneratePDF();

            string newDocumentFileName = generatePDF.GenerateInvestorDocument(investorInfo);

            ResponseContractDTO response = new ResponseContractDTO();

            if (!string.IsNullOrWhiteSpace(newDocumentFileName))
            {
                ContractInvestor contractInvestor = new ContractInvestor()
                {
                    FileName = newDocumentFileName,
                    Email = investorInfo.Email,
                    FullName = string.Concat(investorInfo.FirstName, " ", investorInfo.LastName)
                };

                response = await _keyNUALogic.CreateContract(contractInvestor);
            }

            return response;

        }

        [HttpGet]
        [Route("keynua/{idcontractdocument}")]
        public async Task<ResponseContractDTO> GetDocumentKeyNUA(string idContractDocument)
        {

            if (idContractDocument == null)
            {
                throw new ArgumentNullException(nameof(idContractDocument));
            }

            ResponseContractDTO response = new ResponseContractDTO();
            response = await _keyNUALogic.GetContractCreated(idContractDocument);

            return response;

        }

        [HttpPost]
        [Route("toc")]
        public async Task<ResponseTOCContractDTO> GenerateDocumentTOC(InvestorInfo investorInfo)
        {

            if (investorInfo == null)
            {
                throw new ArgumentNullException(nameof(investorInfo));
            }

            GeneratePDF generatePDF = new GeneratePDF();

            string newDocumentFileName = generatePDF.GenerateInvestorDocument(investorInfo);

            ResponseTOCContractDTO response = new ResponseTOCContractDTO();

            if (!string.IsNullOrWhiteSpace(newDocumentFileName))
            {
                ContractInvestor contractInvestor = new ContractInvestor()
                {
                    FileName = newDocumentFileName,
                    Email = investorInfo.Email,
                    FullName = string.Concat(investorInfo.FirstName, " ", investorInfo.LastName),
                    DocNumber = investorInfo.DocumentNumber
                };

                response = await _tocLogic.CreateContract(contractInvestor);
            }

            return response;

        }

    }
}
