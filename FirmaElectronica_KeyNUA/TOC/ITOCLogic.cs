using FirmaElectronica_KeyNUA.Model;
using System.Threading.Tasks;

namespace FirmaElectronica_KeyNUA.TOC
{
    public interface ITOCLogic
    {

        public Task<ResponseTOCContractDTO> CreateContract(ContractInvestor contractInvestor);

    }
}
