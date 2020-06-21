using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web.Http;
using ASPCore_Final.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsManageController : Controller
    {
        [Microsoft.AspNetCore.Mvc.HttpGet("/admin/products")]
        public IActionResult Index()
        {
            List<ProductModelView> products = GetApiProducts.GetProducts().Select(p => new ProductModelView()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductDiscount = p.ProductDiscount,
                CatName = GetApiCategories.GetCategories().SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                IsActivated = p.IsActivated,
                InitAt = p.InitAt
            }).ToList();
            return View(products);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/admin/products/edit/{productId}")]
        public IActionResult EditProduct(int productId)
        {
            ViewBag.Distributors = GetApiDistributors.GetDistributors().ToList();
            ViewBag.Categories = GetApiCategories.GetCategories().ToList();
            Product product = GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId);
            return View(product);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("/admin/products/edit/{id}")]
        public IActionResult EditProduct(int id, Product product, IFormFile productFile)
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            Product current = GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == id);

            Product update = new Product()
            {
                ProductId = id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ProductDiscount = product.ProductDiscount,
                NumberOfPurchases = current.NumberOfPurchases,
                CategoryId = product.CategoryId,
                DistributorId = product.DistributorId,
                IsActivated = true,
                SlugName = SlugHelper.GetFriendlyTitle(product.ProductName),
                InitAt = current.InitAt
            };

            // product img
            string productImg = Encryptor.RandomString(12);
            string extension = productFile != null ? Path.GetExtension(productFile.FileName) : "";
            if (productFile != null)
            {
                if (SlugHelper.CheckExtension(extension))
                {
                    update.ProductImage = productImg + extension;
                }
                else
                {
                    ViewBag.error = Constants.EXTENSION_IMG_NOT_SUPPORT;
                    return View();
                }
            }
            else
            {
                update.ProductImage = current.ProductImage;
            }
            


            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PutAsJsonAsync<Product>(Constants.PRODUCT + "/" + update.ProductId, update);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    // save img
                    if(productFile != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products", productImg + extension);
                        using (var file = new FileStream(path, FileMode.Create))
                        {
                            productFile.CopyTo(file);
                        }
                    }
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Chỉnh sửa thất bại");
                    return View();
                }
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/admin/products/changeactivated/{productId}")]
        public IActionResult ChangeActivated(int productId)
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            Product current = GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId);
            // update status
            current.IsActivated = !current.IsActivated;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PutAsJsonAsync<Product>(Constants.PRODUCT + "/" + current.ProductId, current);
                postTask.Wait();

                var result = postTask.Result;
            }

            return RedirectToAction("Index");
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("/admin/products/create")]
        public IActionResult Create()
        {
            ViewBag.Distributors = GetApiDistributors.GetDistributors().ToList();
            ViewBag.Categories = GetApiCategories.GetCategories().ToList();
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("/admin/products/create")]
        public IActionResult Create(ProductCreateModel create, IFormFile productFile)
        {
            if (create.ProductDiscount < 0 || create.ProductDiscount > 1)
                return Content(Constants.DISCOUNT_INVALID);

            Product product = new Product()
            {
                ProductName = create.ProductName,
                SlugName = SlugHelper.GetFriendlyTitle(create.ProductName),
                ProductPrice = create.ProductPrice,
                ProductDiscount = create.ProductDiscount,
                DistributorId = create.DistributorId,
                CategoryId = create.CategoryId,
                ProductDescription = create.ProductDescription,
                InitAt = DateTime.Now,
                NumberOfPurchases = 0,
                IsActivated = false
            };

            // product img
            string productImg = Encryptor.RandomString(12);
            string extension = productFile != null ? Path.GetExtension(productFile.FileName) : "";
            if (productFile != null)
            {
                if (SlugHelper.CheckExtension(extension))
                {

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/products", productImg + extension);
                    using (var file = new FileStream(path, FileMode.Create))
                    {
                        productFile.CopyTo(file);
                    }

                    product.ProductImage = productImg + extension;
                }
                else
                {
                    ModelState.AddModelError("", Constants.EXTENSION_IMG_NOT_SUPPORT);
                    return Content(Constants.EXTENSION_IMG_NOT_SUPPORT);
                }
            }
            else
            {
                ModelState.AddModelError("", Constants.IMG_REQUIRED);
                return Content(Constants.IMG_REQUIRED);
            }

            Product createdProduct = CreatedProduct(product);
            if(createdProduct != null)
            {
                string catName = GetApiCategories.GetCategories().SingleOrDefault(p => p.CategoryId == product.CategoryId).CategoryName;

                switch (catName)
                {
                    case Constants.FOOD:
                        CreateAmountFood(product.ProductId);
                        break;

                    case Constants.TOY:
                        CreateAmountToys(product.ProductId);
                        break;

                    case Constants.COSTUME:
                        CreateAmountCostume(product.ProductId);
                        break;
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        private void CreateAmountCostume(int productId)
        {
            List<CostumeProduct> costumes = new List<CostumeProduct>();
            costumes.Add(new CostumeProduct()
            {
                ProductId = productId,
                CostumeSize = Constants.S,
                CostumeAmountSize = 0
            });
            costumes.Add(new CostumeProduct()
            {
                ProductId = productId,
                CostumeSize = Constants.M,
                CostumeAmountSize = 0
            });
            costumes.Add(new CostumeProduct()
            {
                ProductId = productId,
                CostumeSize = Constants.L,
                CostumeAmountSize = 0
            });

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                foreach (var p in costumes)
                {
                    var postTask = client.PostAsJsonAsync<CostumeProduct>(Constants.COSTUME_PRODUCT, p);
                    postTask.Wait();
                }
            }

        }

        private void CreateAmountToys(int productId)
        {
            ToyProduct toy = new ToyProduct()
            {
                ProductId = productId,
                ToyAmount = 0
            };

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);
                var postTask = client.PostAsJsonAsync<ToyProduct>(Constants.TOY_PRODUCT, toy);
                postTask.Wait();
            }
        }

        private void CreateAmountFood(int productId)
        {
            FoodProduct food = new FoodProduct()
            {
                ProductId = productId,
                FoodAmount = 0,
                FoodExpiredDate = DateTime.MaxValue
            };

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);
                var postTask = client.PostAsJsonAsync<FoodProduct>(Constants.FOOD_PRODUCT, food);
                postTask.Wait();
            }
        }

        private Product CreatedProduct(Product product)
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm") != null ? HttpContext.Session.GetString("vm") : "");
            string token = credential.JwToken;

            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<Product>(Constants.PRODUCT , product);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                    return readTask.Result;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}