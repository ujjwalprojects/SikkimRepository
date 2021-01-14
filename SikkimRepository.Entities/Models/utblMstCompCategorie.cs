using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblMstCompCategorie
    {
        [Key]
        public long CompCategoryID { get; set; }
        [Required]
        [Display(Name ="Category Name")]
        public string CompCategoryName { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
