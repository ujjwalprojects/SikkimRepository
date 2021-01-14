using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class ImageManageModel
    {
        public IEnumerable<ImageView> ImageList { get; set; }
        public utblImageAlbum ImageAlbum { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class ImageSaveModel
    {
        public utblImageAlbum ImageAlbum { get; set; }
        public utblImage Images { get; set; }
        public PhotoStrs PhotoStrs { get; set; }
    }
    public class ImageView
    {
        public long ImageID { get; set; }
        public string ImageTitle { get; set; }
        public string ImageFilePathThumb { get; set; }
        public string ImageFilePathNormal { get; set; }
        public bool IsAlbumCover { get; set; }
        public long ImageAlbumID { get; set; }
    }
}
