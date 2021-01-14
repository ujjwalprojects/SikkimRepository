using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstSchoolVM
    {
        public IEnumerable<MstSchoolView> SchoolList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MstSchoolView
    {
        public long SchoolID { get; set; }
        public string DistName { get; set; }
        public string BlockName { get; set; }
        public string SchoolName { get; set; }
        public string UDISECode { get; set; }
    }
    public class MstSchoolManageModel
    {
        public IEnumerable<utblMstDistrict> DistList { get; set; }
        public IEnumerable<utblMstBlock> Blocklist { get; set; }
        public utblSchool School { get; set; }
    }
}
