using Aviasales.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.DAL.DataAccess
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
