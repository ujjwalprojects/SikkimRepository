using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class MstCategoryVM
    {
        public IEnumerable<utblMstCompCategorie> CategoryList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
    public class CategoryDD
    {
        public long CompCategoryID { get; set; }
        public string CompCategoryName { get; set; }
    }
}
