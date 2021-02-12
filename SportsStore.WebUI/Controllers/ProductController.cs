namespace SportsStore.WebUI.Controllers
{
    using SportsStore.Domain.Abstract;
    using SportsStore.WebUI.Models;
    using System.Web.Mvc;
    using System.Linq;

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
            ProductListViewModel model = new ProductListViewModel()
            {
                Products = repository.Products.OrderBy(o => o.ProductId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = repository.Products.Count() }
            };

            return View(model);
        }
    }
}