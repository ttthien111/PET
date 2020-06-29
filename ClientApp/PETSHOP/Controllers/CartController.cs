using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPal.v1.Orders;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class CartController : Controller
    {
        public List<CartItem> Carts
        {
            get
            {
                List<CartItem> myCart = HttpContext.Session.GetObject<List<CartItem>>("cart");
                if (myCart == default(List<CartItem>))
                {
                    myCart = new List<CartItem>();
                }

                return myCart;
            }
        }

        public IActionResult Index()
        {
            int totalPrice = Carts.Sum(p => p.Amount * p.Price);
            HttpContext.Session.SetInt32("sizeCart", totalPrice);
            return View(Carts);
        }

        public IActionResult AddToCart(string cartItem)
        {
            List<CartItem> carts = Carts;
            CartItem item = JsonConvert.DeserializeObject<CartItem>(cartItem);
            item.Name = item.Name.Replace("\r\n", "");
            if (ExistInCart(item.Name))
            {
                CartItem current = carts.SingleOrDefault(p => p.CartItemSlugName == item.CartItemSlugName);
                current.Amount += item.Amount;
                HttpContext.Session.SetObject("cart", carts);
                return RedirectToAction("Index");
            }
            else
            {
                
                if (item.SlugCategory == "costume")
                {
                    string name = item.Name;
                    int idx = name.IndexOf('(');
                    item.Size = name.Substring(idx + 1, 1);
                }
                carts.Add(item);
                HttpContext.Session.SetObject("cart", carts);
                return RedirectToAction("Index");
            }
        }

        private bool ExistInCart(string Name)
        {
            if(Carts.SingleOrDefault(p=>p.Name == Name) != null)
            {
                return true;
            }
            return false;
        }

        public IActionResult EditCart(string Name, int Amount)
        {
            Name = Name.Replace("\n", "");
            List<CartItem> carts = Carts;
            CartItem item = carts.SingleOrDefault(p => p.Name == Name);

            // get current amount item cart

            int currentAmount = GetAmountProduct(item.CartItemSlugName, item.Size);
            item.Amount = Amount <= currentAmount ? Amount : currentAmount;
            HttpContext.Session.SetObject("cart", carts);
            return NoContent();
        }

        public int GetAmountProduct(string slugName, string size = null)
        {
            Product product = GetApiProducts.GetProducts().SingleOrDefault(p => p.SlugName == slugName);
            string slugCat = GetApiCategories.GetCategories().SingleOrDefault(p => p.CategoryId == product.CategoryId).CategoryName;

            int amount = 0;

            switch (slugCat)
            {
                case Constants.FOOD:
                    amount = GetApiFoodProducts.GetFoodProducts().SingleOrDefault(p => p.ProductId == product.ProductId).FoodAmount;
                    break;
                case Constants.TOY:
                    amount = GetApiToyProducts.GetToyProducts().SingleOrDefault(p => p.ProductId == product.ProductId).ToyAmount;
                    break;
                case Constants.COSTUME:
                    amount = GetApiCostumeProducts.GetCostumeProducts().SingleOrDefault(p => p.ProductId == product.ProductId && p.CostumeSize == size).CostumeAmountSize;
                    break;
            }

            return amount;
        }

        public IActionResult DeleteItemCart(string Name)
        {
            List<CartItem> carts = Carts;
            CartItem item = carts.SingleOrDefault(p => p.Name == Name);
            carts.Remove(item);
            HttpContext.Session.SetObject("cart", carts);
            return RedirectToAction("Index");
        }
    }
}