using System.Collections.Generic;

namespace FirmaElectronica_KeyNUA.TOC
{
    public class SignerDTO
    {

        public SignerDTO()
        {
            Firmantes = new List<string>();
            Identificadores = new List<string>();
            Copies = new List<string>();
        }

        public IList<string> Firmantes { get; set; }
        public IList<string> Identificadores { get; set; }
        public IList<string> Copies { get; set; }
    }

    public class NewRequestContractDTO
    {
        public byte[] File { get; set; }
        public SignerDTO Signers { get; set; }
        public string Message { get; set; }
        public string Lang { get; set; }

    }
}
