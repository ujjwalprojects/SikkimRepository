using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblTestimonial
    {
        [Key]
        public long TestimonialsID { get; set; }
        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string ImagePath { get; set; }
        [Required]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Required]
        [Display(Name = "School")]
        public long SchoolID { get; set; }
        [Required]
        [Display(Name = "Component")]
        public long ComponentID { get; set; }
        [Required]
        [Display(Name = "Sub Component")]
        public long SubComponentID { get; set; }
        [Required]
        [Display(Name = "Testimonial Date")]
        public DateTime TestimonialDate { get; set; }
        public bool IsPublished { get; set; }
        [Display(Name = "Published Date")]
        public DateTime? PublishedDate { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
