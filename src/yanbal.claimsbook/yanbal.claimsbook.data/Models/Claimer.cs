using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models.Behavior;

namespace yanbal.claimsbook.data.Models
{
    public class Claimer : IGuid
    {
        public Guid ID { get; set; }
        public DocumentType TipoDocumento { get; set; }
        public string NumDocumento { get; set; }
        public string Name { get; set; }
        public string SurnameFather { get; set; }
        public string SurnameMother { get; set; }
        public string Telephone { get; set; }
        public AnswerType AnswerType { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }
        public string Departament { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
