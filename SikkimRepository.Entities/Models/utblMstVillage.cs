using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstVillage
    {
        [Key]
        public long VillageID { get; set; }
        public string VillageName { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
