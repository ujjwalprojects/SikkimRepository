using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class VideoAlbumManageModel
    {
        public IEnumerable<utblMstComponent> Components { get; set; }
        public IEnumerable<utblMstSubComponent> SubComponent { get; set; }
        public IEnumerable<utblSchool> SchoolList { get; set; }
        public IEnumerable<VideoAlbumView> VideoAlbumList { get; set; }
        public utblVideoAlbum VideoAlbum { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public VideoAlbumGenViewModel GenVideoAlbums { get; set; }
        public IEnumerable<utblVideo> Videolist { get; set; }
        public IEnumerable<GenComponentModel> GenComponentModelList { get; set; }// Side Menu
    }
    public class VideoAlbumGenViewModel
    {
        public IEnumerable<GenVideoAlbumView> VideoAlbumList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class VideoAlbumView
    {
        public long VideoAlbumID { get; set; }
        public string VideoAlbumTitle { get; set; }
        public DateTime ProgramDate { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
    }
    public class GenVideoAlbumView
    {
        public long VideoAlbumID { get; set; }
        public string VideoAlbumTitle { get; set; }
        public string VideoAlbumDesc { get; set; }
        public string VideoTitle { get; set; }
        public DateTime ProgramDate { get; set; }
        public string VideoFilePathPublished { get; set; }
        public string VideoFilePathDraft { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
    }
    public class GenVideoAlbumByDist
    {
        public long VideoAlbumID { get; set; }
        public string VideoAlbumTitle { get; set; }
        public string VideoAlbumDesc { get; set; }
        public string VideoFilePathPublished { get; set; }
        public string VideoFilePathDraft { get; set; }
        public DateTime ProgramDate { get; set; }
        public string SchoolName { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string DistCode { get; set; }
    }
}
