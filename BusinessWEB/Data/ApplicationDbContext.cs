using BusinessWEB.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BusinessWEB.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }

        public DbSet<User> Users { get; set; }
        /*public DbSet<DataTable> DataTables { get; set; }*/

    }
}
