using FirmaElectronica_KeyNUA.Model;
using System.Threading.Tasks;

namespace FirmaElectronica_KeyNUA.KeyNUA
{
    public interface IKeyNUALogic
    {
        public Task<ResponseContractDTO> CreateContract(ContractInvestor contractInvestor);

        public Task<ResponseContractDTO> GetContractCreated(string idContract);
    }
}