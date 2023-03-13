using Mysqlx;
using Sales.Enums;
using Sales.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModels
{
    public class SaleRecordMainViewModel
    {
        public SaleRecordMainViewModel()
        {
            Filter = new SalesRecordFilter();
        }

        public List<SaleRecordGroupedViewModel> SalesRecordsGroups { get; set; }

        public SalesRecordFilter Filter { get; set; }
    }

    public class SaleRecordGroupedViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public List<SaleRecordViewModel> SalesRecords { get; set; }
    }

    public class SaleRecordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo {0} requerido")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string DateString { get { return Date.ToShortDateString(); } }

        [Required(ErrorMessage = "Campo {0} requerido")]
        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Campo de vendedor requerido")]
        public int SellerId { get; set; }

        public string SellerName { get; set; }

        public Seller Seller { get; set; }
    }

    public class SalesRecordFilter
    {
        public SalesRecordFilter()
        {
            DateEnd = DateTime.Now;
            DateStart = DateTime.Now.AddYears(-5);
            Status = false;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}")]
        public DateTime? DateEnd { get; set; }
        public bool Status { get; set; }
        public bool AllPeriods { get; set; }
    }
}
