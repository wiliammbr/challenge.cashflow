using Challenge.CashFlow.DAL;
using Challenge.CashFlow.Models;
using Challenge.CashFlow.SearchModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Challenge.CashFlow.ApiControllers
{
    public class TransactionController : ApiController
    {
        /// <summary>
        /// Current context
        /// </summary>
        private ICashFlowContext db = new CashFlowContext();

        /// <summary>
        /// Transaction list shared across the Controller
        /// </summary>
        List<Transaction> transactions = new List<Transaction>();

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TransactionController() { }

        /// <summary>
        /// Constructor with context defined
        /// </summary>
        /// <param name="context"></param>
        public TransactionController(ICashFlowContext context)
        {
            db = context;
        }

        // GET api/Transaction/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET api/Transaction/GetTransactions?
        [ResponseType(typeof(IList<Transaction>))]
        public IHttpActionResult GetTransactions([FromUri]TransactionSearchModel searchModel)
        {
            var transactions = db.Transactions.AsQueryable();
            if (searchModel != null)
            {
                if (!string.IsNullOrEmpty(searchModel.Description))
                    transactions = transactions.Where(x => x.Description.Contains(searchModel.Description));
                if (!string.IsNullOrEmpty(searchModel.PaymentType))
                    transactions = transactions.Where(x => x.PaymentType == searchModel.PaymentType);
                if (searchModel.AmountFrom.HasValue)
                    transactions = transactions.Where(x => x.Amount >= searchModel.AmountFrom);
                if (searchModel.AmountTo.HasValue)
                    transactions = transactions.Where(x => x.Amount <= searchModel.AmountTo);
                if (searchModel.DateFrom.HasValue)
                    transactions = transactions.Where(x => x.Date >= searchModel.DateFrom);
                if (searchModel.DateTo.HasValue)
                    transactions = transactions.Where(x => x.Date <= searchModel.DateTo);

                if (searchModel.DescriptionSort == "asc")
                {
                    transactions = transactions.OrderBy(t => t.Description);
                }
                else if (searchModel.DescriptionSort == "desc")
                {
                    transactions = transactions.OrderByDescending(t => t.Description);
                }

                if (searchModel.AmountSort == "asc")
                {
                    transactions = transactions.OrderBy(t => t.Amount);
                }
                else if (searchModel.AmountSort == "desc")
                {
                    transactions = transactions.OrderByDescending(t => t.Amount);
                }

                if (searchModel.PaymentTypeSort == "asc")
                {
                    transactions = transactions.OrderBy(t => t.PaymentType);
                }
                else if (searchModel.PaymentTypeSort == "desc")
                {
                    transactions = transactions.OrderByDescending(t => t.PaymentType);
                }

                if (searchModel.DateSort == "asc")
                {
                    transactions = transactions.OrderBy(t => t.Date);
                }
                else if (searchModel.DateSort == "desc")
                {
                    transactions = transactions.OrderByDescending(t => t.Date);
                }
            }

            int count = transactions.Count();
            int currentPage = searchModel.PageNumber;
            int pageSize = searchModel.PageSize;
            int totalCount = count;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var previousPage = currentPage > 1 ? "1" : "0";
            var nextPage = currentPage < totalPages ? "1" : "0";

            List<Transaction> finalList = transactions.ToList().Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var paginationMetadata = new
            {
                totalCount = totalCount,
                pageSize = pageSize,
                currentPage = currentPage,
                totalPages = totalPages,
                previousPage,
                nextPage
            };

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            }            

            return Ok(finalList);
        }



        // GET api/Transaction/GetTransactionsOverview?
        [ResponseType(typeof(IList<Transaction>))]
        public IHttpActionResult GetTransactionsOverview(int numberOfDays)
        {
            var transactions = db.Transactions.AsQueryable();
            if (numberOfDays > 0)
            {
                numberOfDays *= -1;
            }

            DateTime today = DateTime.Today;
            DateTime sinceFrom = today.AddDays(numberOfDays);
            transactions = transactions.Where(x => x.Date >= sinceFrom);

            List<Transaction> transactionsToday = transactions.Where(t => t.Date.Day == today.Day && t.Date.Month == today.Month && t.Date.Year == today.Year).ToList();
            var reportToday = new
            {
                Title = "Today",
                Total = transactionsToday.Count(),
                TotalAmount = transactionsToday.Count > 0 ? ((Decimal?)transactionsToday.Sum(t => t.Amount)) ?? 0 : 0
            };

            var list = new[] { reportToday }.ToList();

            if (numberOfDays < 0)
            {
                var reportLastDays = new
                {
                    Title = "Last " + numberOfDays * -1 + " day(s)",
                    Total = transactions.Count(),
                    TotalAmount = transactions.Count() > 0 ? ((Decimal?)transactions.Sum(t => t.Amount)) ?? 0 : 0
                };
                list.Add(reportLastDays);
            }

            return Ok(list);
        }

        // PUT api/Transaction/5
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            db.MarkAsModified(transaction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Transaction
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.Id }, transaction);
        }

        // DELETE api/Transaction/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }
    }
}
