using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class ImageAlbumManageModel
    {
        public IEnumerable<utblMstComponent> Components { get; set; }
        public IEnumerable<utblMstSubComponent> SubComponent { get; set; }
        public IEnumerable<utblSchool> SchoolList { get; set; }
        public IEnumerable<ImageAlbumView> ImgAlbumList { get; set; }
        public utblImageAlbum ImageAlbum { get; set; }
        public IEnumerable<utblImage> Imageslist { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public ImageAlbumGenViewModel GenImageAlbums { get; set; }
        public IEnumerable<GenComponentModel> GenComponentModelList { get; set; }// Side Menu

    }
    public class ImageAlbumGenViewModel
    {
        public IEnumerable<GenImageAlbumView> ImageAlbumList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class ImageAlbumView
    {
        public long ImageAlbumID { get; set; }
        public string ImageAlbumTitle { get; set; }
        public DateTime ProgramDate { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class GenImageAlbumView
    {
        public long ImageAlbumID { get; set; }
        public string ImageAlbumTitle { get; set; }
        public string ImageAlbumDesc { get; set; }
        public DateTime ProgramDate { get; set; }
        public string ImageFilePathNormal { get; set; }
        public string ImageFilePathThumb { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
    }

    public class GenImageAlbumlist
    {
        public long ImageAlbumID { get; set; }
        public string ImageAlbumTitle { get; set; }
        public string ImageAlbumDesc { get; set; }
        public DateTime ProgramDate { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
    }

    public class GenImageAlbumByDist
    {
        public long ImageAlbumID { get; set; }
        public string ImageAlbumTitle { get; set; }
        public string ImageAlbumDesc { get; set; }
        public string ImageFilePathNormal { get; set; }
        public string ImageFilePathThumb { get; set; }
        public DateTime ProgramDate { get; set; }
        public string SchoolName { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string DistCode { get; set; }
    }

}
