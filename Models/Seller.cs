using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    [Table("Seller")]
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public double BaseSalary { get; set; }

        [ForeignKey("Deparment")]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<SalesRecord> SalesRecord { get; set; } = new List<SalesRecord>();
    }
}
