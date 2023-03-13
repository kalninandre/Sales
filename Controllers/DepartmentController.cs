using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sales.Models;
using Sales.ViewModels;
using System.Diagnostics;

namespace Sales.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly SalesContext _context;
        public DepartmentController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var departments = (from m in _context.Departments
                               orderby m.Name
                               select new DepartmentViewModel
                               {
                                   Id = m.Id,
                                   Name = m.Name,
                               }).ToList();

            return View(departments);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new DepartmentViewModel
            {
                Name = department.Name,
            };

            return View(viewModel);

        }

        [HttpPost]
        public ActionResult Edit(SellerViewModel viewModel)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == viewModel.Id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            department.Name = viewModel.Name;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel viewModel)
        {
            var department = new Department
            {
                Name = viewModel.Name,
            };

            _context.Departments.Add(department);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DepartmentViewModel viewModel)
        {
            try
            {
                var department = _context.Departments.FirstOrDefault(m => m.Id == viewModel.Id);
                if (department == null)
                {
                    throw new Exception("Vendedor não encontrado");
                }

                _context.Departments.Remove(department);
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

        public static List<SelectListItem> ListAll(SalesContext context, bool addEmpty)
        {
            var departments = (from m in context.Departments
                               select new SelectListItem
                               {
                                   Text = m.Name,
                                   Value = m.Id.ToString()
                               }).ToList();
            if (addEmpty)
            {
                departments.Insert(0, new SelectListItem
                {
                    Text = "Selecione",
                    Value = "0"
                });
            }
            return departments;
        }
    }
}
