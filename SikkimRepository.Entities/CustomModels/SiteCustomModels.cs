using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class ErrorLogModel
    {
        public string ErrorDateTime { get; set; }
        public string URL { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string IPAddress { get; set; }
        public string User { get; set; }
    }
    public class AuditLogModel
    {
        public string TableName { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
        public long? ContentID { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
