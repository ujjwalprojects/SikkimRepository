using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class TestimonialManageModel
    {
        public IEnumerable<utblMstComponent> Components { get; set; }
        public IEnumerable<utblMstSubComponent> SubComponent { get; set; }
        public IEnumerable<utblSchool> SchoolList { get; set; }
        public IEnumerable<TestimonialView> TestimonialView { get; set; }
        public utblTestimonial Testimonial { get; set; }
        public PhotoStrs PhotoStrs { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public TestimonialGenViewModel GenTestimonials { get; set; }
        public IEnumerable<GenComponentModel> GenComponentModelList { get; set; }// Side Menu
    }
    public class TestimonialView
    {
        public long TestimonialsID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public DateTime TestimonialDate { get; set; }
        public string IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class TestimonialGenViewModel
    {
        public IEnumerable<GenTestimonialsView> TestimonialList { get; set; }
       
        public int TotalRecords { get; set; }
    }
    public class GenTestimonialsView
    {
        public long TestimonialsID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        public string Remarks { get; set; }
        public DateTime TestimonialDate { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }

    }
    public class GenTestimonialsByDist
    {
        public long TestimonialsID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        public string Remarks { get; set; }
        public DateTime TestimonialDate { get; set; }
        public string SchoolName { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string DistCode { get; set; }
    }
}
