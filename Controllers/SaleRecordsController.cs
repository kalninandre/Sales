using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sales.Enums;
using Sales.Helpers;
using Sales.Models;
using Sales.ViewModels;
using System.Diagnostics;

namespace Sales.Controllers
{
    public class SaleRecordsController : Controller
    {
        private readonly SalesContext _context;
        public SaleRecordsController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index(SaleRecordMainViewModel viewModel)
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

            if (!viewModel.Filter.AllPeriods)
            {
                if (viewModel.Filter.DateStart != null)
                {
                    tmpQuery = tmpQuery.Where(m => m.Date >= viewModel.Filter.DateStart);
                }

                if (viewModel.Filter.DateEnd != null)
                {
                    tmpQuery = tmpQuery.Where(m => m.Date <= viewModel.Filter.DateEnd);
                }
            }


            var salesRecords = new SaleRecordMainViewModel
            {
                SalesRecordsGroups = (from m in tmpQuery.ToList()
                                      group m by status ?
                                      new { Id = (int)m.Status, Name = m.Status.ToString() } : new { Id = m.Seller.DepartmentId, Name = m.Seller.DepartmentName } into g
                                      select new SaleRecordGroupedViewModel
                                      {
                                          Name = status ? ((SaleStatusEnum)Enum.Parse(typeof(SaleStatusEnum), g.Key.Name)).GetDescription() : g.Key.Name,
                                          Counter = g.Count(m => status ? (int)m.Status == g.Key.Id : m.Seller.DepartmentId == g.Key.Id),

                                          SalesRecords = (from m in g.ToList()
                                                          orderby m.Seller.Name
                                                          select new SaleRecordViewModel
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

        public ActionResult Create()
        {
            var sellers = SellerController.ListAll(_context, true);
            ViewBag.Sellers = new SelectList(sellers, "Value", "Text");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaleRecordViewModel viewModel)
        {
            if (viewModel.SellerId == 0)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor inválido" });
            }

            var saleRecord = new SalesRecord
            {
                Date = viewModel.Date,
                Amount = viewModel.Amount,
                Status = SaleStatusEnum.Pending,
                SellerId = viewModel.SellerId,
            };

            _context.SalesRecords.Add(saleRecord);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var saleRecord = _context.SalesRecords.FirstOrDefault(m => m.Id == id);
            if (saleRecord == null)
            {
                throw new Exception("Venda não encontrada");
            }

            var viewModel = new SaleRecordViewModel
            {
                Id = saleRecord.Id,
                Date = saleRecord.Date,
                Amount = saleRecord.Amount,
                Status = saleRecord.Status,
                SellerId = saleRecord.SellerId,
            };

            var enums = EnumHelpers.GenerateSelectList<SaleStatusEnum>();
            ViewBag.Status = new SelectList(enums, "Value", "Text");

            var sellers = SellerController.ListAll(_context, true);
            ViewBag.Sellers = new SelectList(sellers, "Value", "Text");

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Edit(SaleRecordViewModel viewModel)
        {
            var saleRecord = _context.SalesRecords.FirstOrDefault(m => m.Id == viewModel.Id);
            if (saleRecord == null)
            {
                throw new Exception("Venda não encontrada");
            }

            saleRecord.Date = viewModel.Date;
            saleRecord.Amount = viewModel.Amount;
            saleRecord.Status = viewModel.Status;
            saleRecord.SellerId = viewModel.SellerId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var saleRecord = _context.SalesRecords.Include(m => m.Seller).FirstOrDefault(m => m.Id == id);
            if (saleRecord == null)
            {
                throw new Exception("Venda não encontrada");
            }

            var viewModel = new SaleRecordViewModel
            {
                Id = saleRecord.Id,
                Date = saleRecord.Date,
                Status = saleRecord.Status,
                Amount = saleRecord.Amount,
                SellerName = saleRecord.Seller.Name,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SaleRecordViewModel viewModel)
        {
            try
            {
                var saleRecord = _context.SalesRecords.FirstOrDefault(m => m.Id == viewModel.Id);
                if (saleRecord == null)
                {
                    throw new Exception("Vendedor não encontrada");
                }
                _context.SalesRecords.Remove(saleRecord);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Error), new { message = "Não foi possível deletar este item em razão de chaves primárias no banco de dados" });
            }
        }

        [HttpGet]
        public ActionResult Error(string message)
        {
            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            };

            return View(error);
        }
    }
}
