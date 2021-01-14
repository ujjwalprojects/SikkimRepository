using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class CaseStudyManageModel
    {
        public IEnumerable<utblMstComponent> Components { get; set; }
        public IEnumerable<utblMstSubComponent> SubComponent { get; set; }
        public IEnumerable<utblSchool> SchoolList { get; set; }
        public IEnumerable<CaseStudyView> CaseStudyView { get; set; }
        public utblCaseStudie CaseStudy { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public CaseStudyGenViewModel GenCaseStudies { get; set; }
        public IEnumerable<GenComponentModel> GenComponentModelList { get; set; }// Side Menu
    }
    public class CaseStudyView
    {
        public long CaseStudyID { get; set; }
        public string CaseStudyTitle { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string CaseStudyFilePath { get; set; }
        public DateTime CaseStudyDate { get; set; }
        public string IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
    }
    public class CaseStudyGenViewModel
    {
        public IEnumerable<GenCaseStudyView> CaseStudyList { get; set; }

        public int TotalRecords { get; set; }
    }
    public class GenCaseStudyView
    {
        public long CaseStudyID { get; set; }
        public string CaseStudyTitle { get; set; }
        public string CaseStudyDesc { get; set; }
        public string CaseStudyFilePath { get; set; }
        public DateTime CaseStudyDate { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
    }
    public class GenCaseStudyByDist
    {
        public long CaseStudyID { get; set; }
        public string CaseStudyTitle { get; set; }
        public string CaseStudyDesc { get; set; }
        public string CaseStudyFilePath { get; set; }
        public DateTime CaseStudyDate { get; set; }
        public string SchoolName { get; set; }
        public string ComponentName { get; set; }
        public string SubComponentName { get; set; }
        public string DistCode { get; set; }
    }
}
