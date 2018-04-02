using Challenge.CashFlow.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Challenge.CashFlow.DAL
{
    public class CashFlowInitializer : DropCreateDatabaseAlways<CashFlowContext>
    {
        protected override void Seed(CashFlowContext context)
        {
            var users = new List<User>
            {
            new User{ Id = 1, UserName = "manager" , Password = "Boss123", Roles = "Manager" },
            new User{ Id = 2, UserName = "employee" , Password = "Employee123", Roles = "Employee" },
            };
            users.ForEach((u => context.Users.Add(u)));
            context.SaveChanges();

            Random random = new Random();
            Transaction t = new Transaction();
            for (int i = 0; i < 200; i++)
            {
                context.Transactions.Add(GenerateTransactions(i, random, t));
            }
            context.SaveChanges();


            base.Seed(context);
        }

        public Transaction GenerateTransactions(int id, Random random, Transaction t)
        {
            return new Transaction()
            {
                Id = id,
                Description = "Description " + id,
                Amount = (decimal)random.NextDouble() * 500,
                PaymentType = random.Next(2) % 2 == 0 ? "C" : "M",
                Date = DateTime.Now.AddDays(-random.Next(100))
            };
        }
    }
}