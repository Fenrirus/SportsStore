using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void CanBePaginate()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductId = 1, Name = "P1" },
                new Product{ProductId = 2, Name = "P2" },
                new Product{ProductId = 3, Name = "P3" },
                new Product{ProductId = 4, Name = "P4" },
                new Product{ProductId = 5, Name = "P5" },
            });

            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            var result = (IEnumerable<Product>)controller.List(2).Model;

            var arrayProduct = result.ToArray();

            Assert.IsTrue(arrayProduct.Length == 2);
            Assert.AreEqual(arrayProduct[0].Name, "P4");
            Assert.AreEqual(arrayProduct[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10,
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            var results = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            Assert.AreEqual(@"<a class=""btn btn-outline-secondary"" href=""Strona1"">1</a>"
                          + @"<a class=""btn btn-outline-secondary btn-primary selected"" href=""Strona2"">2</a>"
                          + @"<a class=""btn btn-outline-secondary"" href=""Strona3"">3</a>"
                          , results.ToString());
        }
    }
}