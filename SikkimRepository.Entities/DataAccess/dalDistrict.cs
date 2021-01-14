using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.DataAccess
{
    public class dalDistrict
    {
        EFDBContext db = new EFDBContext();
        public List<utblMstDistrict> GetDistrictList(string sTerm)
        {
            if ((sTerm == null || sTerm == ""))
            {
                return db.utblMstDistricts.ToList();
            }
            else
            {
                return db.utblMstDistricts.Where(x => x.DistName.Contains(sTerm)).ToList();
            }
        }
        public utblMstDistrict GetDistByCode(string DistCode)
        {
            return db.utblMstDistricts.Where(x => x.DistCode == DistCode).FirstOrDefault();
        }
    }
}
