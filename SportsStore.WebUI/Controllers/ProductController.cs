using SportsStore.Domain.Abstract;
using System.Web.Mvc;
using System.Linq;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(int page = 1)
        {
            return View(repository.Products.OrderBy(o => o.ProductId).Skip((page - 1) * PageSize).Take(PageSize));
        }
    }
}