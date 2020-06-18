using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Category = GetApiCategories.GetCategories().ToList(); // api
            List<ProductModelView> newProducts = GetApiProducts.GetProducts().Select(p => new ProductModelView() {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductDiscount = p.ProductDiscount,
                ProductPrice = p.ProductPrice,
                SlugName = p.SlugName,
                CategoryId = p.CategoryId,
                DistributorId = p.DistributorId,
                Rating = 0,
                InitAt = p.InitAt,
                IsActivated = p.IsActivated,
                NumberOfPurchases = p.NumberOfPurchases,
                CatName = ""
            }).ToList();

            foreach (var p in newProducts)
            {
                p.Rating = getRatingProduct(p.ProductId);
                p.CatName = GetApiCategories.GetCategories().SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName;
            }

            ViewBag.NewProducts = newProducts.Where(p => p.isNew || p.ProductDiscount != 0).Take(5).ToList();

            List<ProductModelView> mostBoughts = GetApiProducts.GetProducts().OrderByDescending(p=>p.NumberOfPurchases).Select(p => new ProductModelView()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductDiscount = p.ProductDiscount,
                ProductPrice = p.ProductPrice,
                SlugName = p.SlugName,
                CategoryId = p.CategoryId,
                DistributorId = p.DistributorId,
                Rating = 0,
                InitAt = p.InitAt,
                IsActivated = p.IsActivated,
                NumberOfPurchases = p.NumberOfPurchases,
                CatName = ""
            }).Take(5).ToList();

            foreach (var p in mostBoughts)
            {
                p.Rating = getRatingProduct(p.ProductId);
                p.CatName = GetApiCategories.GetCategories().SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName;
            }

            ViewBag.MostBought = mostBoughts.Take(5).ToList();

            return View();
        }

        private double getRatingProduct(int productId)
        {
            // get all usercomments by productId 
            List<UserComment> userComments = GetApiUserComments.GetUserComments()
                                            .Where(p => p.ProductId == productId)
                                            .ToList();
            // create rating to return
            double rating = 0.0;

            foreach (var comment in userComments)
            {
                rating += (double)comment.Score;
            }
            rating = rating / (userComments.Count() == 0 ? 1 : userComments.Count());
            return rating;
        }
    }
}
