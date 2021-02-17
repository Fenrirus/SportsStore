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

            var result = (ProductListViewModel)controller.List(null, 2).Model;

            var arrayProduct = result.Products.ToArray();

            Assert.IsTrue(arrayProduct.Length == 2);
            Assert.AreEqual(arrayProduct[0].Name, "P4");
            Assert.AreEqual(arrayProduct[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Send_Paggination_View_Model()
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
            var result = (ProductListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalPages, 2);
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

        [TestMethod]
        public void Can_filter_product()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category ="Cat1" },
                new Product{ProductId = 2, Name = "P2", Category ="Cat2" },
                new Product{ProductId = 3, Name = "P3", Category ="Cat1" },
                new Product{ProductId = 4, Name = "P4", Category ="Cat2" },
                new Product{ProductId = 5, Name = "P5", Category ="Cat3" },
            });

            var productControles = new ProductController(mock.Object);
            productControles.PageSize = 3;
            var result = ((ProductListViewModel)productControles.List("Cat2", 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category ="Cat3" },
                new Product{ProductId = 2, Name = "P2", Category ="Cat2" },
                new Product{ProductId = 3, Name = "P3", Category ="Cat1" },
                new Product{ProductId = 4, Name = "P4", Category ="Cat2" },
                new Product{ProductId = 5, Name = "P5", Category ="Cat3" },
            });

            var nav = new NavController(mock.Object);
            var result = ((IOrderedEnumerable<string>)nav.Menu().Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category ="Cat3" },
                new Product{ProductId = 2, Name = "P2", Category ="Cat2" },
                new Product{ProductId = 3, Name = "P3", Category ="Cat1" },
                new Product{ProductId = 4, Name = "P4", Category ="Cat2" },
                new Product{ProductId = 5, Name = "P5", Category ="Cat4" },
            });

            var nav = new NavController(mock.Object);

            var catToSelect = "Cat4";

            string result = nav.Menu(catToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(catToSelect, result);
        }

        [TestMethod]
        public void Generete_Category_Specific_Product_Count()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductId = 1, Name = "P1", Category ="Cat1" },
                new Product{ProductId = 2, Name = "P2", Category ="Cat2" },
                new Product{ProductId = 3, Name = "P3", Category ="Cat1" },
                new Product{ProductId = 4, Name = "P4", Category ="Cat2" },
                new Product{ProductId = 5, Name = "P5", Category ="Cat3" },
            });

            var productControles = new ProductController(mock.Object);
            productControles.PageSize = 3;
            var result = ((ProductListViewModel)productControles.List("Cat1", 1).Model).PagingInfo.TotalItems;
            var result2 = ((ProductListViewModel)productControles.List("Cat2", 1).Model).PagingInfo.TotalItems;
            var result3 = ((ProductListViewModel)productControles.List("Cat3", 1).Model).PagingInfo.TotalItems;
            var resultAll = ((ProductListViewModel)productControles.List(null, 1).Model).PagingInfo.TotalItems;

            Assert.AreEqual(result, 2);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 1);
            Assert.AreEqual(resultAll, 5);
        }
    }
}