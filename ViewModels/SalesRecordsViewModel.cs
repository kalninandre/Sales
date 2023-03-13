using Sales.Enums;
using Sales.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModels
{
    public class SalesRecordMainViewModel
    {
        public SalesRecordMainViewModel()
        {
            Filter = new SalesRecordFilter();
        }

        public List<SalesRecordGroupedViewModel> SalesRecordsGroups { get; set; }

        public SalesRecordFilter Filter { get; set; }
    }

    public class SalesRecordGroupedViewModel
    {
        public string Name { get; set; }

        public int Counter { get; set; }

        public List<SalesRecordViewModel> SalesRecords { get; set; }
    }

    public class SalesRecordViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string DateString { get { return Date.ToShortDateString(); } }

        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        public string DepartmentName { get; set; }

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
    }
}
