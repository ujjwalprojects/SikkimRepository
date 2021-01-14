using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstComponentVM
    {
        public IEnumerable<utblMstComponent> CompList { get; set; }
        public IEnumerable<ComponentView> ComponentList { get; set; }
        public IEnumerable<CategoryDD> CategoryDDList { get; set; }
        public utblMstComponent ComponentModel { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class ComponentView
    {
        public long ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string CompCategoryName { get; set; }
    }
}
