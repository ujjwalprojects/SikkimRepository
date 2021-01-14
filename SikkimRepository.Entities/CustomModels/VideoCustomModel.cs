using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class VideoManageModel
    {
        public IEnumerable<VideoView> VideoList { get; set; }
        public utblVideoAlbum VideoAlbum { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class VideoSaveModel
    {
        public utblVideoAlbum VideoAlbum { get; set; }
        public utblVideo Videos { get; set; }
    }
    public class VideoView
    {
        public long VideoID { get; set; }
        public string VideoTitle { get; set; }
        public string VideoFilePathDraft { get; set; }
        public string VideoFilePathPublished { get; set; }
        public bool IsAlbumCover { get; set; }
        public long VideoAlbumID { get; set; }
    }
}
