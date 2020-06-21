using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PETSHOP.Models.ModelView
{
    public class CartItem
    {
        public string CartItemSlugName { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public string SlugCategory { get; set; }
        public string Size { get; set; }
    }
}
