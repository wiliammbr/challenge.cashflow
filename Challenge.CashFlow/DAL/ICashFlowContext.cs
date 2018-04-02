using Challenge.CashFlow.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.CashFlow.DAL
{
    public interface ICashFlowContext : IDisposable
    {
        DbSet<Transaction> Transactions { get; }
        DbSet<User> Users { get; }
        int SaveChanges();
        void MarkAsModified(Transaction item);
        void MarkAsModified(User item);
    }
}
