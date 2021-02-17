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

        public ViewResult List(string category, int page = 1)
        {
            ProductListViewModel model = new ProductListViewModel()
            {
                Products = repository.Products.OrderBy(o => o.ProductId)
                .Where(w => category == null || w.Category == category)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(w => w.Category == category).Count()
                },
                CurrentCategory = category,
            };

            return View(model);
        }
    }
}