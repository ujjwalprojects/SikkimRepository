using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblSchool
    {
        [Key]
        public long SchoolID { get; set; }
        [Required]
        [Display(Name ="District Name")]
        public string DistCode { get; set; }
        [Required]
        [Display(Name = "Block Name")]
        public long BlockID { get; set; }
        [Required]
        [Display(Name = "UDISE Code")]
        public string UDISECode { get; set; }
        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }
        [Required]
        [Display(Name = "School's Address")]
        public string SchoolAddress { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}

