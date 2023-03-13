using Sales.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModels
{
    public class SellerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo {0} não informado")]
        [EmailAddress(ErrorMessage = "Entre com um e-mail válido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo {0} não informado")]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string BirthDateString { get { return BirthDate.ToShortDateString(); } }

        [Required(ErrorMessage = "Campo {0} não informado")]
        [Display(Name = "Salário Base")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public List<SaleRecordViewModel> Sales { get; set; } = new List<SaleRecordViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(m => m.Date >= initial && m.Date <= final).Sum(m => m.Amount);
        }
    }
}
