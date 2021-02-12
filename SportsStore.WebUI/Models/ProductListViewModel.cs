namespace SportsStore.WebUI.Models
{
    using System.Collections.Generic;
    using SportsStore.Domain.Entities;

    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}