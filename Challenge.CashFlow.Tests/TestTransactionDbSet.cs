using Challenge.CashFlow.Models;
using System.Linq;

namespace Challenge.CashFlow.Tests
{
    class TestTransactionDbSet : TestDbSet<Transaction>
    {
        public override Transaction Find(params object[] keyValues)
        {
            return this.SingleOrDefault(t => t.Id == (int)keyValues.Single());
        }
    }
}
