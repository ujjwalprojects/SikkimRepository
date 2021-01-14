using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstComponent
    {
        [Key]
        public long ComponentID { get; set; }
        [Required]
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public long CompCategoryID { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
