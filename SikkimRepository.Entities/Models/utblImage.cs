using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblImage
    {
        [Key]
        public long ImageID { get; set; }
        [Required]
        [Display(Name = "Image Title")]
        public string ImageTitle { get; set; }
        public long ImageAlbumID { get; set; }
        public string ImageFilePathThumb { get; set; }
        public string ImageFilePathNormal { get; set; }
        public bool IsAlbumCover { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
