using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoginAuth.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=DataContext")
        {

        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}