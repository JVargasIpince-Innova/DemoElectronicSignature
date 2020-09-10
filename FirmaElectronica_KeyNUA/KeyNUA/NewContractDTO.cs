using System.Collections.Generic;

namespace FirmaElectronica_KeyNUA.KeyNUA
{
    public class DocumentContractDTO
    {
        public string Name { get; set; }
        public byte[] Base64 { get; set; }
    }
    public class UserContractDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class NewContractDTO
    {

        public NewContractDTO()
        {
            Documents = new List<DocumentContractDTO>();
            Users = new List<UserContractDTO>();
        }

        public string Title { get; set; }
        public string Language { get; set; }
        public string TemplateId { get; set; }

        //Min: 1 - Max: 10
        public IList<DocumentContractDTO> Documents { get; set; }
        public IList<UserContractDTO> Users { get; set; }

    }
}
