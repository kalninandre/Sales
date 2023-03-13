using Sales.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Models
{
    [Table("SalesRecord")]
    public class SalesRecord
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public SaleStatusEnum Status { get; set; }

        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        public virtual Seller Seller { get; set; }
    }
}
