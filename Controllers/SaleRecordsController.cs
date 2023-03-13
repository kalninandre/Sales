using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.Enums;
using Sales.Helpers;
using Sales.Models;
using Sales.ViewModels;

namespace Sales.Controllers
{
    public class SaleRecordsController : Controller
    {
        private readonly SalesContext _context;
        public SaleRecordsController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index(SalesRecordMainViewModel viewModel)
        {

            bool status = viewModel.Filter.Status;

            var tmpQuery = from m in _context.SalesRecords
                           select new
                           {
                               m.Id,
                               m.Date,
                               m.Amount,
                               m.Status,
                               Seller = new
                               {
                                   Id = m.Seller.Name,
                                   m.Seller.Name,
                                   m.Seller.DepartmentId,
                                   DepartmentName = m.Seller.Department.Name

                               },
                           };


            var salesRecords = new SalesRecordMainViewModel
            {
                SalesRecordsGroups = (from m in tmpQuery.ToList()
                                      group m by status ?
                                      new { Id = (int)m.Status, Name = m.Status.ToString() } : new { Id = m.Seller.DepartmentId, Name = m.Seller.DepartmentName } into g
                                      select new SalesRecordGroupedViewModel
                                      {
                                          Name = status ? ((SaleStatusEnum)Enum.Parse(typeof(SaleStatusEnum), g.Key.Name)).GetDescription() : g.Key.Name,
                                          Counter = g.Count(m => status ? (int)m.Status == g.Key.Id : m.Seller.DepartmentId == g.Key.Id),

                                          SalesRecords = (from m in g.ToList()
                                                          select new SalesRecordViewModel
                                                          {
                                                              Id = m.Id,
                                                              Date = m.Date,
                                                              Amount = m.Amount,
                                                              Status = m.Status,
                                                              SellerName = m.Seller.Name,
                                                              DepartmentName = m.Seller.DepartmentName
                                                          }).ToList()
                                      }).ToList(),

                Filter = viewModel.Filter
            };

            return View(salesRecords);
        }
    }
}
