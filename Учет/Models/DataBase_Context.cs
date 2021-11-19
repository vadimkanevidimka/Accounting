using Accounting;
using Microsoft.EntityFrameworkCore;
using Accounting.Models;

namespace Accounting
{
    class DataBase_Context : DbContext
    {
        public DbSet<AccountingForOriginals_Model> Originals_Models { get; set; }
        public DbSet<AccountingForNotifications_Model> Notifications_Models { get; set; }

        public DataBase_Context()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename = Accounting.db");
        }
    }
}
