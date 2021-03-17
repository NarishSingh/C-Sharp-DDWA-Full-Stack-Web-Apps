using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShackUp.UI.Models.Identity;

namespace ShackUp.UI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ShackUp.UI.Models.Identity.ShackUpDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        
        protected override void Seed(ShackUpDbContext context)
        {
            //load user and role managers w custom models
            var userMgr = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));

            //check if roles loaded, if not, create admin
            if (roleMgr.RoleExists("admin"))
            {
                return;
            }

            roleMgr.Create(new AppRole
                {
                    Name = "admin"
                }
            );

            //default user
            var user = new AppUser
            {
                UserName = "admin"
            };

            //create with manager class, add to admin role
            userMgr.Create(user, "testing123");
            userMgr.AddToRole(user.Id, "admin");
        }
    } 
}