namespace Sales.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SellerViewModel> Sellers { get; set; } = new List<SellerViewModel>();

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
