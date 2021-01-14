using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstBlock
    {
        [Key]
        public long BlockID { get; set; }
        [Required(ErrorMessage = "Select District")]
        [Display(Name ="District Name")]
        public string DistCode { get; set; }
        [Required(ErrorMessage = "Enter Block Name")]
        [Display(Name = "Block Name")]
        public string BlockName { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
