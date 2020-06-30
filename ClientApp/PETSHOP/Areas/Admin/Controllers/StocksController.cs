using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class StocksController : CheckAuthenticateManageController
    {
        public List<InventoryRecieveItem> IRItems
        {
            get
            {
                List<InventoryRecieveItem> inventoryRecieveItems = HttpContext.Session.GetObject<List<InventoryRecieveItem>>("inventoryRecieveItems");
                if (inventoryRecieveItems == default(List<InventoryRecieveItem>))
                {
                    inventoryRecieveItems = new List<InventoryRecieveItem>();
                }

                return inventoryRecieveItems;
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Filter(string textSearch)
        {
            List<Product> filters = new List<Product>();
            if (textSearch == "" || textSearch == null)
            {
                filters = new List<Product>();
            }
            else
            {
                List<string> searchTexts = textSearch.Split(" ").ToList();
                foreach (string search in searchTexts)
                {
                    filters.AddRange(GetApiProducts.GetProducts()
                        .Where(p => p.ProductName.ToLower().Contains(search.ToLower()) ||
                                p.ProductId.ToString() == search).ToList());
                }
            }
           
            return PartialView(filters);
        }

        public IActionResult AddToInventoryRecieveItems(int productId)
        {
            List<InventoryRecieveItem> inventoryRecieveItems = new List<InventoryRecieveItem>();
            Product product = GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId);
            string slugCat = GetApiCategories.GetCategories().SingleOrDefault(p => p.CategoryId == product.CategoryId).CategoryName;
            switch (slugCat)
            {
                case Constants.FOOD:
                    inventoryRecieveItems.AddRange(GetApiFoodProducts.GetFoodProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p=> new InventoryRecieveItem() {
                                            ProductId = product.ProductId,
                                            SubProductId = p.FoodId,
                                            ProductName = product.ProductName,
                                            Size = "",
                                            Amount = 1,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;

                case Constants.TOY:
                    inventoryRecieveItems.AddRange(GetApiToyProducts.GetToyProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p => new InventoryRecieveItem()
                                        {
                                            ProductId = product.ProductId,
                                            SubProductId = p.ToyId,
                                            ProductName = product.ProductName,
                                            Size = "",
                                            Amount = 1,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;

                case Constants.COSTUME:
                    inventoryRecieveItems.AddRange(GetApiCostumeProducts.GetCostumeProducts().Where(p => p.ProductId == product.ProductId)
                                        .Select(p => new InventoryRecieveItem()
                                        {
                                            ProductId = product.ProductId,
                                            SubProductId = p.CostumeId,
                                            ProductName = product.ProductName,
                                            Size = p.CostumeSize,
                                            Amount = 1,
                                            Price = product.ProductPrice
                                        }).ToList());
                    break;
            }

            HttpContext.Session.SetObject("inventoryRecieveItems", inventoryRecieveItems);
            return PartialView(inventoryRecieveItems);
        }
    }
}   