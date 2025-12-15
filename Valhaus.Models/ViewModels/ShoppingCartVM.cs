using Valhaus.Models.Models;

namespace VALHAUS.Areas.Customer.Controllers
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public double OrderTotal { get; set; }
    }
}