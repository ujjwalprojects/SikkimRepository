using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class GenCustomModel
    {
        public IEnumerable<GenVideoAlbumView> GenVideos { get; set; }
        public IEnumerable<GenImageAlbumView> GenImages { get; set; }
        public IEnumerable<GenTestimonialsView> GenTestimonials { get; set; }
        public IEnumerable<GenCaseStudyView> GenCaseStudies { get; set; }
        public GenContentCount GenContentCount { get; set; }
        public List<utblMstComponent> GenComponent { get; set; }
        public List<utblMstSubComponent> GenSubComponent { get; set; }
        public IEnumerable<GenImageAlbumByDist> GenImgAlbumByDist { get; set; }
        public IEnumerable<GenVideoAlbumByDist> GenVidAlbumByDist { get; set; }
        public IEnumerable<GenTestimonialsByDist> GenTestimonialByDist { get; set; }
        public IEnumerable<GenCaseStudyByDist> GenCaseStudyByDist { get; set; }
        public utblMstDistrict District { get; set; }

        public utblMstComponent Component { get; set; }
        public utblMstSubComponent SubComponent { get; set; }
        public IEnumerable<GenComponentModel> GenComponentModelList { get; set; }


        public CaseStudyGenViewModel GenCaseStudyList { get; set; }
        public TestimonialGenViewModel GenTestimonialList { get; set; }
        public VideoAlbumGenViewModel GenVideoAlbumList { get; set; }
        public ImageAlbumGenViewModel GenImageAlbumList { get; set; }
    }
    public class GenContentCount
    {
        public int Images { get; set; }
        public int Videos { get; set; }
        public int Testimonials { get; set; }
        public int CaseStudies { get; set; }
    }
    public class DashModel
    {
        public List<DashCountModel> Content { get; set; }
        public List<DashCountModel> DistCountImg { get; set; }
        public List<DashCountModel> DistCountVid { get; set; }
        public List<DashCountModel> DistCountTstm { get; set; }
        public List<DashCountModel> DistCountCS { get; set; }
        public utblSchool SchoolDetails { get; set; }
    }
    public class DashCountModel
    {
        public string Content { get; set; }
        public int PublishedCount { get; set; }
        public int DraftCount { get; set; }
        public int SlNo { get; set; }
    }
    public class GenComponentModel
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public long ComponentID { get; set; }
        public string ComponentName { get; set; }
        public long SubComponentID { get; set; }
        public string SubComponentName { get; set; }
    }
    public class GenViewListByDistrictCode
    {
        public IEnumerable<GenVideoAlbumByDist> VideoAlbumListByDist { get; set; }
        public IEnumerable<GenImageAlbumByDist> ImageAlbumListByDist { get; set; }
        public IEnumerable<GenTestimonialsByDist> TestimonialsListByDist { get; set; }
        public IEnumerable<GenCaseStudyByDist> CaseStudyListByDist { get; set; }
        public utblMstDistrict District { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LegalPagesModel
    {
        public string LegalPageName { get; set; }
    }
}
