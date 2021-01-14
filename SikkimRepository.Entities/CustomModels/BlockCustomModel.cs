using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstBlockVM
    {
        public IEnumerable<MstBlockView> BlockList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MstBlockView
    {
        public long BlockID { get; set; }
        public string DistCode { get; set; }
        public string DistName { get; set; }
        public string BlockName { get; set; }
    }
    public class MstBlockManageModel
    {
        public IEnumerable<utblMstDistrict> DistList { get; set; }
        public utblMstBlock Block { get; set; }
    }
}
