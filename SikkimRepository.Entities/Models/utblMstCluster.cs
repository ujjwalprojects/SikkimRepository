using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstCluster
    {
        [Key]
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
