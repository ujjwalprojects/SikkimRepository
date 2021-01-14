using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.Models
{
    public class utblVideo
    {
        [Key]
        public long VideoID { get; set; }
        public string VideoTitle { get; set; }
        public long VideoAlbumID { get; set; }
        public string VideoFilePathDraft { get; set; }
        public string VideoFilePathPublished{ get; set; }
        public bool IsAlbumCover { get; set; }
        public DateTime TransDate { get; set; }
        public string UserID { get; set; }
    }
}
