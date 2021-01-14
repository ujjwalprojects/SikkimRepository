using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblCaseStudie
    {
        [Key]
        public long CaseStudyID { get; set; }
        [Required]
        [Display(Name ="Title")]
        public string CaseStudyTitle { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string CaseStudyDesc { get; set; }
        [Required]
        [Display(Name = "School")]
        public long SchoolID { get; set; }
        [Required]
        [Display(Name = "Component")]
        public long ComponentID { get; set; }
        [Required]
        [Display(Name = "Sub Component")]
        public long SubComponentID { get; set; }
        public string CaseStudyFilePath { get; set; }
        [Required]
        [Display(Name = "Case Study Date")]
        public DateTime CaseStudyDate { get; set; }
        public bool IsPublished { get; set; }
        [Display(Name = "Published Date")]
        public DateTime? PublishedDate { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
