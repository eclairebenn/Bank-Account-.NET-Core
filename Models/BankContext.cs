using Microsoft.EntityFrameworkCore;
 
namespace bank_account.Models
{
    public class BankContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Record> Records {get;set;}

    }
}