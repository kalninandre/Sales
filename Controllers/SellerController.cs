using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using Sales.Models;
using Sales.ViewModels;
using System.Diagnostics;

namespace Sales.Controllers
{
    public class SellerController : Controller
    {
        private readonly SalesContext _context;
        public SellerController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var sellers = (from m in _context.Sellers
                           orderby m.Id
                           select new SellerViewModel
                           {
                               Id = m.Id,
                               Name = m.Name,
                               Email = m.Email,
                               BirthDate = m.BirthDate,
                               BaseSalary = m.BaseSalary,
                               Department = m.Department
                           }).ToList();

            return View(sellers);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                throw new Exception(string.Format("Identificador #{0} não encontrado", id));
            }

            var seller = _context.Sellers.Include(i => i.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            var sellers = new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                Department = seller.Department
            };

            return View(sellers);

        }

        public ActionResult Create()
        {
            var departments = DepartmentController.ListAll(_context, true);
            ViewBag.Departments = new SelectList(departments, "Value", "Text");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SellerViewModel viewModel)
        {
            var seller = new Seller
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                BirthDate = viewModel.BirthDate,
                BaseSalary = viewModel.BaseSalary,
                DepartmentId = viewModel.DepartmentId,
            };

            _context.Sellers.Add(seller);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            var viewModel = new SellerViewModel
            {
                Id = seller.Id,
                Email = seller.Email,
                Name = seller.Name,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                DepartmentId = seller.DepartmentId
            };

            var departments = DepartmentController.ListAll(_context, true);
            ViewBag.Departments = new SelectList(departments, "Value", "Text");

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Edit(SellerViewModel viewModel)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            seller.Name = viewModel.Name;
            seller.Email = viewModel.Email;
            seller.BirthDate = viewModel.BirthDate;
            seller.BaseSalary = viewModel.BaseSalary;
            seller.DepartmentId = viewModel.DepartmentId;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            var viewModel = new SellerViewModel
            {
                Id = seller.Id,
                Email = seller.Email,
                Name = seller.Name,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                Department = seller.Department,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SellerViewModel viewModel)
        {
            try
            {
                var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
                if (seller == null)
                {
                    throw new Exception("Vendedor não encontrado");
                }
                _context.Sellers.Remove(seller);
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