using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SikkimRepository.Web.Models;
using System;

[assembly: OwinStartupAttribute(typeof(SikkimRepository.Web.Startup))]
namespace SikkimRepository.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }
        // In this method we will create default User roles and Admin user for login   
        public void CreateUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("DeptAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "DeptAdmin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website         
                var user = new ApplicationUser();
                //user.UserName = "Kaish";
                //user.Email = "kaish@netpseq.com";
                //string userPWD = "Pass@123";
                user.UserName = "SSAdmin";
                user.Email = "samagrashikshasikkim@gmail.com";
                string userPWD = "samagrasiksha@2020";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default user to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "DeptAdmin");
                }
            }

            //var user = new ApplicationUser();
            //if (user != null)
            //{
            //    //user.UserName = "Kaish";
            //    //user.Email = "kaish@netpseq.com";
            //    //string userPWD = "Pass@123";
            //    user.UserName = "SSAdmin";
            //    user.Email = "samagrashikshasikkim@gmail.com";
            //    user.Designation = "SuperAdmin";
            //    user.Gender = "Male";
            //    user.SchoolID = 0;
            //    user.RegDate = DateTime.Now;
            //    user.IsActive = true;
            //    string userPWD = "samagrasiksha@2020";
            //    var chkUser = UserManager.Create(user, userPWD);
            //    if (chkUser.Succeeded)
            //    {
            //        var result1 = UserManager.AddToRole(user.Id, "DeptAdmin");
            //    }
            //}

            //Add default user to Role Admin


            //Create Additional Role as SchoolAdmin
            if (!roleManager.RoleExists("SchoolAdmin"))
            {
                var role = new IdentityRole();
                role.Name = "SchoolAdmin";
                roleManager.Create(role);
                
            }
        }
    }
}
