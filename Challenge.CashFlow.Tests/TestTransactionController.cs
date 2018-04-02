using Challenge.CashFlow.ApiControllers;
using Challenge.CashFlow.Models;
using Challenge.CashFlow.SearchModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace Challenge.CashFlow.Tests
{
    [TestClass]
    public class TestTransactionController
    {
        [TestMethod]
        public void PostTransaction_ShouldReturnSameTransaction()
        {
            var controller = new TransactionController(new TestCashFlowContext());

            var item = GetTestTransaction();

            var result =
                controller.PostTransaction(item) as CreatedAtRouteNegotiatedContentResult<Transaction>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "DefaultApi");
            Assert.AreEqual(result.RouteValues["id"], result.Content.Id);
            Assert.AreEqual(result.Content.Description, item.Description);
        }

        [TestMethod]
        public void PutTransaction_ShouldReturnStatusCode()
        {
            var controller = new TransactionController(new TestCashFlowContext());

            var item = GetTestTransaction();

            var result = controller.PutTransaction(item.Id, item) as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public void PutTransaction_ShouldFail_WhenDifferentID()
        {
            var controller = new TransactionController(new TestCashFlowContext());

            var badResult = controller.PutTransaction(999, GetTestTransaction());
            Assert.IsInstanceOfType(badResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetTransaction_ShouldReturnTransactionWithSameID()
        {
            var context = new TestCashFlowContext();
            GetTestTransactions().ForEach((t => context.Transactions.Add(t)));

            var controller = new TransactionController(context);
            var result = controller.GetTransaction(3) as OkNegotiatedContentResult<Transaction>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.Id);
        }

        [TestMethod]
        public void GetTransactions_ShouldReturnAllTransactions()
        {
            var context = new TestCashFlowContext();
            GetTestTransactions().ForEach((t => context.Transactions.Add(t)));

            var controller = new TransactionController(context);
            var result = controller.GetTransactions(new TransactionSearchModel()) as OkNegotiatedContentResult<List<Transaction>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Content.Count);
            
            result = controller.GetTransactions(new TransactionSearchModel() { PageSize = 20 }) as OkNegotiatedContentResult<List<Transaction>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(20, result.Content.Count);

            result = controller.GetTransactions(new TransactionSearchModel() { PageNumber = 2, PageSize = 10 }) as OkNegotiatedContentResult<List<Transaction>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Content.Count);

            result = controller.GetTransactions(new TransactionSearchModel() { AmountFrom = 5 }) as OkNegotiatedContentResult<List<Transaction>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Content.Count);
        }

        [TestMethod]
        public void DeleteTransaction_ShouldReturnOK()
        {
            var context = new TestCashFlowContext();
            var item = GetTestTransaction();
            context.Transactions.Add(item);

            var controller = new TransactionController(context);
            var result = controller.DeleteTransaction(1) as OkNegotiatedContentResult<Transaction>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.Id, result.Content.Id);
        }

        private Transaction GetTestTransaction()
        {
            return new Transaction { Id = 1, Description = "Description 1", Amount = 5, PaymentType = "C" };
        }

        private List<Transaction> GetTestTransactions()
        {
            var testTransactions = new List<Transaction>();
            
            testTransactions.Add(new Transaction { Id = 2, Description = "Description 2", Amount = 10, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 3, Description = "Description 3", Amount = 10, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 4, Description = "Description 4", Amount = 50, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 5, Description = "Description 5", Amount = 1M, PaymentType = "D" });
            testTransactions.Add(new Transaction { Id = 6, Description = "Description 6", Amount = 5, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 7, Description = "Description 7", Amount = 1, PaymentType = "D" });
            testTransactions.Add(new Transaction { Id = 8, Description = "Description 8", Amount = 10, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 9, Description = "Description 9", Amount = 15, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 10, Description = "Description 10", Amount = 30, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 11, Description = "Description 11", Amount = 10, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 12, Description = "Description 12", Amount = 3, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 13, Description = "Description 13", Amount = 8, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 15, Description = "Description 15", Amount = 5, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 14, Description = "Description 14", Amount = 10, PaymentType = "D" });
            testTransactions.Add(new Transaction { Id = 16, Description = "Description 16", Amount = 10, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 17, Description = "Description 17", Amount = 25, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 18, Description = "Description 18", Amount = 30, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 19, Description = "Description 19", Amount = 5, PaymentType = "C" });
            testTransactions.Add(new Transaction { Id = 20, Description = "Description 20", Amount = 8, PaymentType = "M" });
            testTransactions.Add(new Transaction { Id = 21, Description = "Description 21", Amount = 5, PaymentType = "C" });

            return testTransactions;
        }
    }
}
