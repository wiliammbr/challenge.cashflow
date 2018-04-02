using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Challenge.CashFlow.SearchModels
{
    public class TransactionSearchModel
    {
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentType { get; set; }
        public DateTime? Date { get; set; }
        
        public string DescriptionSort { get; set; }
        public string AmountSort { get; set; }
        public string PaymentTypeSort { get; set; }
        public string DateSort { get; set; }

        public int PageNumber { get; set; }

        public int PageSize
        {
            get { return _PageSize; }
            set
            {
                _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

        private int _PageSize = 10;
        const int MaxPageSize = 200;

        public TransactionSearchModel()
        {
            PageNumber = 1;
        }
    }
}