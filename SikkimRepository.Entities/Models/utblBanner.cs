using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblBanner
    {
        [Key]
        public long BannerID { get; set; }
        [Required(ErrorMessage = "Enter Title")]
        [Display(Name = "Banner Title")]
        public string BannerTitle { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [Display(Name = "Banner Description")]
        public string BannerDescription { get; set; }
        public string BannerThumb { get; set; }
        public string BannerPath { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
