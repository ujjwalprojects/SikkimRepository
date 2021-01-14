using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstVillageVM
    {
        public IEnumerable<MstVillageView> VillageList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MstVillageView
    {
        public long VillageID { get; set; }
        public long BlockID { get; set; }
        public string DistCode { get; set; }
        public string DistName { get; set; }
        public string BlockName { get; set; }
        public string VillageName { get; set; }
    }
    public class MstVillageManageModel
    {
        public IEnumerable<utblMstDistrict> DistList { get; set; }
        public utblMstVillage Village { get; set; }
    }
}
