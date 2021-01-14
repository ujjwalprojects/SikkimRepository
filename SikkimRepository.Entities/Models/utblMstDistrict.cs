using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstDistrict
    {
        [Key]
        public string DistCode { get; set; }
        public string DistName { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
