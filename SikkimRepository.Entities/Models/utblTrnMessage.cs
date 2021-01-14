using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblTrnMessage
    {
        [Key]
        public long MessageID { get; set; }
        public DateTime MessageDate { get; set; }
        public string MessageSubject { get; set; }
        public string MessageBody { get; set; }
        public string SMSContent { get; set; }
        public string SentBy { get; set; }
        public string SentTo { get; set; }
        public bool IsNew { get; set; }
        public long ServiceDtlID { get; set; }
        public string TransStatus { get; set; }
        public string TransRemarks { get; set; }
        public string RefTable { get; set; }
        public string RefTransID { get; set; }
    }
}
