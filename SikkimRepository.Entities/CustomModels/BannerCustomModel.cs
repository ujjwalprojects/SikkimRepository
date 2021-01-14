using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{
    public class BannerSaveModel
    {
        public utblBanner Banner { get; set; }
        public PhotoStrs PhotoStrs { get; set; }
    }
    public class PhotoStrs
    {
        public string PhotoThumb { get; set; }
        public string PhotoNormal { get; set; }
    }
}
