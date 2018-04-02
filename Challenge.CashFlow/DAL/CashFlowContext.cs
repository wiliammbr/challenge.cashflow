using Challenge.CashFlow.Models;
using System.Data.Entity;
    
namespace Challenge.CashFlow.DAL
{
    public class CashFlowContext : DbContext, ICashFlowContext
    {
        public CashFlowContext()
            : base("CashFlowContext")
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CashFlowInitializer());
            base.OnModelCreating(modelBuilder);
        }

        public void MarkAsModified(Transaction item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(User item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}