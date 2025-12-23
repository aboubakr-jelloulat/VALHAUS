using Microsoft.AspNetCore.Mvc;
using Valhaus.Data.Repository.IRepository;

namespace VALHAUS.ViewComponents
{
    public class ShoppingCartViewComponents : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponents(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
