using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstSubComponent
    {
        [Key]
        public long SubComponentID { get; set; }
        [Required]
        [Display(Name ="Sub Component Name")]
        public string SubComponentName { get; set; }
        [Required]
        [Display(Name = "Component Name")]
        public long ComponentID { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
