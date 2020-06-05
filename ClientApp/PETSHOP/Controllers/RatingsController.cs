using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class RatingsController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Product> products = GetApiProducts.GetProducts();
            IEnumerable<UserComment> comment = GetApiUserComments.GetUserComments();
            IEnumerable<Category> categories = GetApiCategories.GetCategories();
            IEnumerable<Distributor> distributors = GetApiDistributors.GetDistributors();

            List<ProductModelViewDetail> productRatings = products.OrderBy(p => p.InitAt).Select(p => new ProductModelViewDetail()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductDiscount = p.ProductDiscount,
                CategoryName = categories.SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                DistributorName = distributors.SingleOrDefault(k => k.DistributorId == p.DistributorId).DistributorName,
                Rating = getRatingProduct(p.ProductId),
                No_Ratings = comment.Where(k => k.ProductId == p.ProductId).Count(),
                InitAt = p.InitAt,
                SlugName = p.SlugName
            }).ToList(); 
            return View(productRatings);
        }

        [HttpGet("ratings/{slugName}")]
        public IActionResult RatingDetail(string slugName)
        {
            IEnumerable<Product> products = GetApiProducts.GetProducts();
            Product p = products.SingleOrDefault(p => p.SlugName == slugName);

            ProductModelViewDetail res = new ProductModelViewDetail()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductDiscount = p.ProductDiscount,
                CategoryName = GetApiCategories.GetCategories().SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                DistributorName = GetApiDistributors.GetDistributors().SingleOrDefault(k => k.DistributorId == p.DistributorId).DistributorName,
                Rating = getRatingProduct(p.ProductId),
                No_Ratings = GetApiUserComments.GetUserComments().Where(k => k.ProductId == p.ProductId).Count(),
                InitAt = p.InitAt,
                Comments = new List<UserComment>()
            };

            List<UserComment> comments = GetApiUserComments.GetUserComments().Where(k => k.ProductId == p.ProductId).ToList();

            foreach (var cmt in comments)
            {
                cmt.UserProfile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileId == cmt.UserProfileId);
            }

            res.Comments = comments;

            return View(res);
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