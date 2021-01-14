using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblImageAlbum
    {
        [Key]
        public long ImageAlbumID { get; set; }
        [Required]
        [Display(Name = "Album Title")]
        public string ImageAlbumTitle { get; set; }
        [Required]
        [Display(Name = "Album Description")]
        public string ImageAlbumDesc { get; set; }
        [Required]
        [Display(Name = "Program Date")]
        public DateTime ProgramDate { get; set; }
        [Required]
        [Display(Name = "Component Name")]
        public long ComponentID { get; set; }
        [Required]
        [Display(Name = "Sub Component Name")]
        public long SubComponentID { get; set; }
        [Required]
        [Display(Name = "School Name")]
        public long SchoolID { get; set; }
        public bool IsPublished { get; set; }
        [Display(Name = "Publish Date")]
        public DateTime? PublishDate { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
