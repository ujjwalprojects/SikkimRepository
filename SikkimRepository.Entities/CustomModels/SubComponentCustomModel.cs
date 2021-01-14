using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstSubComponentVM
    {
        public IEnumerable<MstSubComponentView> SubComponentList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class MstSubComponentView
    {
        public long SubComponentID { get; set; }
        public string SubComponentName { get; set; }
        public string ComponentName { get; set; }
        public string CompCategoryName { get; set; }
    }
    public class MstSubComponentManageModel
    {
        public IEnumerable<utblMstComponent> ComponentList { get; set; }
        public utblMstSubComponent SubComponent { get; set; }
    }
}
