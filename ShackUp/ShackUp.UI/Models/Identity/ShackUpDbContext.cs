﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace ShackUp.UI.Models.Identity
{
    public class ShackUpDbContext : IdentityDbContext<AppUser>
    {
        public ShackUpDbContext() : base("ShackUp")
        {
        }
        
        public static ShackUpDbContext Create()
        {
            return new ShackUpDbContext();
        }
    }
}