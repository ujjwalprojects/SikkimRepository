using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkimRepository.Entities.CustomModels
{

    public class RegisteredUserVM
    {
        public IEnumerable<RegisteredUserView> UserList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public MyProfile MyProfileView { get; set; }
    }
    public class RegisteredUserView
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string SchoolName { get; set; }
    }
    public class MyProfile
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string SchooName { get; set; }
        public long SchooID { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsActive { get; set; }
    }
}
