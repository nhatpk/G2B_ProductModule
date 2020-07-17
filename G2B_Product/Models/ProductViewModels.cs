using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace G2B_Product.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ShortDesc { get; set; }
        public string FullDesc { get; set; }
    }
}
