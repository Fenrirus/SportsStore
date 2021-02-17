namespace SportsStore.WebUI.Controllers
{
    using SportsStore.Domain.Abstract;
    using System.Linq;
    using System.Web.Mvc;

    public class NavController : Controller
    {
        private IProductRepository _productRepository;

        public NavController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            var catergories = _productRepository.Products.Select(s => s.Category).Distinct().OrderBy(x => x);

            return PartialView(catergories);
        }
    }
}