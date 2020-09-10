using System;
using System.Collections.Generic;

namespace FirmaElectronica_KeyNUA.KeyNUA
{
    public class ResponseUserContractDTO
    {
        public int Id { get; set; }
        public string Ref { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
    }
    public class ResponseDocumentContractDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public string SHA { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
    
    public class ResponseItemContractDTO
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string State { get; set; }
        public int? UserId { get; set; }
        public string Reference { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int StageIndex { get; set; }
        public string Value { get; set; }            
      
    }

    public class ResponseContractDTO
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public IList<ResponseUserContractDTO> Users { get; set; }
        public IList<ResponseDocumentContractDTO> Documents { get; set; }
        public IList<ResponseItemContractDTO> Items { get; set; }
    }
}
