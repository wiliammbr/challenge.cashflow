using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenge.CashFlow.DAL;
using Challenge.CashFlow.Models;
using System.Data.Entity;

namespace Challenge.CashFlow.Tests
{
    public class TestCashFlowContext : ICashFlowContext
    {
        public TestCashFlowContext()
        {
            this.Transactions = new TestTransactionDbSet();
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
        
        public void MarkAsModified(Transaction item)
        {
        }

        public void MarkAsModified(User item)
        {
        }

        public void Dispose() { }
    }
}
