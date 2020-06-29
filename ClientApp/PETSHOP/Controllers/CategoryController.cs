using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ASPCore_Final.Services;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            // get category from api

            IEnumerable<Category> categories = GetApiCategories.GetCategories();
            ViewBag.Category = categories;
            return View();
        }

        [HttpGet("category/{slugCategory}")]
        public IActionResult CategoryById(string slugCategory = null)
        {
            // get category
            Category category= GetApiCategories.getCategoryBySlugName(slugCategory);
            ViewBag.CategoryName = category.CategoryName;

            // create filter
            Filter filter = HttpContext.Session.GetObject<Filter>("filter") == null ? GetFilter() : HttpContext.Session.GetObject<Filter>("filter");
            ViewBag.Filter = filter;

            // list product view to show
            List<ProductModelView> productsByCatId = getProductsByFilter(slugCategory, filter);
            return View(productsByCatId);
        }

        public IActionResult SearchProduct(string categorySlugName = null, string textSearch = null ,long priceFrom = 0, long priceTo = 0, bool isNew = false, bool isDiscount = false, string ratings = null, bool sort = false)
        {
            // get category
            Category category = CategoryByName(categorySlugName);
            ViewBag.CategoryName = category.CategoryName;

            // get products by category
            List<Product> productsByCatId = ProductsByCatId(category.CategoryId);

            //get Filter
            Filter filter = GetFilter(priceFrom, priceTo, isNew, isDiscount, ratings, sort);

            List<ProductModelView> res = new List<ProductModelView>();
            if(textSearch == null)
            {
                res = getProductsByFilter(categorySlugName, filter);
            }
            else
            {
                string textSearch_slug = SlugHelper.GetFriendlyTitle(textSearch.Trim());
                List<string> searchs = textSearch_slug.Split("-").ToList();
                foreach (var text in searchs)
                {
                    List<ProductModelView> products = getProductsByFilter(categorySlugName, filter)
                                                    .Where(p => p.ProductName.ToLower().Contains(text.ToLower()))
                                                    .Select(p=> new ProductModelView() {
                                                        ProductId = p.ProductId,
                                                        ProductName = p.ProductName,
                                                        ProductDescription = p.ProductDescription,
                                                        ProductImage = p.ProductImage,
                                                        ProductDiscount = p.ProductDiscount,
                                                        ProductPrice = p.ProductPrice,
                                                        CategoryId = p.CategoryId,
                                                        DistributorId = p.DistributorId,
                                                        IsActivated = p.IsActivated,
                                                        InitAt = p.InitAt,
                                                        Rating = p.Rating,
                                                        SlugName = p.SlugName
                                                    }).ToList();
                    res.AddRange(products);
                }

                res = res.Distinct().ToList();
            }

            return PartialView(res);
        }

        [HttpGet("category/{slugCategory}/{slugProductName}")]
        public IActionResult ProductDetail(string slugCategory = null, string slugProductName = null)
        {
            //get category
            Category category = CategoryByName(slugCategory);
            ViewBag.CategoryName = category.CategoryName;

            // get product by slugname
            Product product = ProductBySlugName(slugProductName);

            // init 
            FoodProduct food = new FoodProduct();
            List<CostumeProduct> costume = new List<CostumeProduct>();
            ToyProduct toy = new ToyProduct();

            ProductModelViewDetail res = new ProductModelViewDetail();

            if (slugCategory == "food")
            {
                food = GetApiFoodProducts.GetFoodProducts().SingleOrDefault(p => p.ProductId == product.ProductId);
                res = new ProductModelViewDetail()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    ProductDiscount = product.ProductDiscount,
                    SlugName = product.SlugName,
                    CategoryName = category.CategoryName,
                    Rating = 0,
                    No_Ratings = GetApiUserComments.GetUserComments().Where(p=>p.ProductId == product.ProductId).Count(),
                    InitAt = product.InitAt,
                    DistributorName = GetApiDistributors.GetDistributors().SingleOrDefault(p => p.DistributorId == product.DistributorId).DistributorName,
                    FoodExpiredDate = food.FoodExpiredDate,
                    IsActivated = product.IsActivated,
                    CostumeSize = new List<CostumeSizeModel>(),
                    Amount = food.FoodAmount
                };

                res.Rating = getRatingProduct(res.ProductId);
            }
            else if(slugCategory == "costume")
            {
                costume = GetApiCostumeProducts.GetCostumeProducts().Where(p => p.ProductId == product.ProductId).Select(p=> new CostumeProduct() { 
                    ProductId = p.ProductId,
                    CostumeId = p.CostumeId,
                    CostumeAmountSize = p.CostumeAmountSize,
                    CostumeSize = p.CostumeSize
                }).ToList();

                res = new ProductModelViewDetail()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    ProductDiscount = product.ProductDiscount,
                    SlugName = product.SlugName,
                    CategoryName = category.CategoryName,
                    DistributorName = GetApiDistributors.GetDistributors().SingleOrDefault(p=>p.DistributorId == product.DistributorId).DistributorName,
                    Rating = 0,
                    No_Ratings = GetApiUserComments.GetUserComments().Where(p => p.ProductId == product.ProductId).Count(),
                    InitAt = product.InitAt,
                    IsActivated = product.IsActivated,
                    FoodExpiredDate = DateTime.Now,
                    CostumeSize = new List<CostumeSizeModel>(),
                    Amount = 0
                };

                res.CostumeSize = costume.Select(p => new CostumeSizeModel()
                {
                    CostumeSize = p.CostumeSize,
                    CostumeAmountSize = p.CostumeAmountSize
                }).ToList();

                res.Rating = getRatingProduct(res.ProductId);
            }
            else
            {
                toy = GetApiToyProducts.GetToyProducts().SingleOrDefault(p => p.ProductId == product.ProductId);
                res = new ProductModelViewDetail()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                    ProductDiscount = product.ProductDiscount,
                    SlugName = product.SlugName,
                    CategoryName = category.CategoryName,
                    DistributorName = GetApiDistributors.GetDistributors().SingleOrDefault(p => p.DistributorId == product.DistributorId).DistributorName,
                    Rating = 0,
                    No_Ratings = GetApiUserComments.GetUserComments().Where(p => p.ProductId == product.ProductId).Count(),
                    InitAt = product.InitAt,
                    IsActivated = product.IsActivated,
                    FoodExpiredDate = DateTime.Now,
                    CostumeSize = new List<CostumeSizeModel>(),
                    Amount = toy.ToyAmount
                };

                res.Rating = getRatingProduct(res.ProductId);
            }

            return View(res);
        }

        public IActionResult Filter(string categorySlugName = null,long priceFrom = 0, long priceTo = 0, bool isNew = false, bool isDiscount = false, string ratings = null, bool sort = false)
        {
            List<ProductModelView> products = new List<ProductModelView>();
            HttpContext.Session.SetObject("filter", GetFilter(priceFrom, priceTo, isNew, isDiscount, ratings, sort));
            
            // create filter
            Filter filter = GetFilter(priceFrom, priceTo, isNew, isDiscount, ratings, sort);

            // get products by filter
            products = getProductsByFilter(categorySlugName, filter);
            
            return PartialView(products);
        }

        // Begin Function for get data quickly 

        public List<ProductModelView> getProductsByFilter(string categorySlugName, Filter filter)
        {
            Category category = GetApiCategories.getCategoryBySlugName(categorySlugName);

            double priceTo_ = filter.PriceTo;
            if(filter.PriceTo == 0 || filter.PriceTo < filter.PriceFrom)
            {
                priceTo_ = 999999999999;
            }

            //create list products to assign 
            List<ProductModelView> products = new List<ProductModelView>();

            // get Products from api to query
            List<Product> productsApi = GetApiProducts.GetProducts().Where(p=>p.IsActivated == true).ToList();

            products = productsApi
                        .Where(p =>
                             p.CategoryId == category.CategoryId &&
                             p.ProductPrice * (1 - p.ProductDiscount) >= filter.PriceFrom &&
                             p.ProductPrice * (1 - p.ProductDiscount) <= priceTo_
                             )
                        .Select(p => new ProductModelView() {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                ProductDescription = p.ProductDescription,
                                ProductImage = p.ProductImage,
                                ProductDiscount = p.ProductDiscount,
                                ProductPrice = p.ProductPrice,
                                CategoryId = p.CategoryId,
                                DistributorId = p.DistributorId,
                                IsActivated = p.IsActivated,
                                InitAt = p.InitAt,
                                Rating = 0,
                                SlugName = p.SlugName,
                                NumberOfPurchases = p.NumberOfPurchases
                        }).ToList();

            // update rating for each product
            foreach (var p in products)
            {
                p.Rating = getRatingProduct(p.ProductId);
            }

            string[] ratings = filter.ratings == null ? new string[0] : filter.ratings.Split("-");
            List<ProductModelView> res = new List<ProductModelView>();
            // get by ratings
            if(ratings.Length > 1)
            {
                foreach (var p in products)
                {
                    bool tracking = false;
                    foreach (var rating in ratings)
                    {
                        if (rating != "" && ((int)p.Rating == Int32.Parse(rating) || ((int)p.Rating == 5 && (int)p.Rating - Int32.Parse(rating) == 1) || (int)p.Rating + 1 == Int32.Parse(rating)))
                        {
                            tracking = true;
                            break;
                        }
                    }

                    // if track check point at product has rating equal ratings list 
                    if (tracking) res.Add(p);
                }
            }
            else
            {
                res = products;
            }
            

            if (filter.sort)
            {
                res = res.OrderByDescending(p => p.ProductPrice).ToList();
            }
            else
            {
                res = res.OrderBy(p => p.ProductPrice).ToList();
            }
            if (filter.isDiscount) res = res.Where(p => p.ProductDiscount > 0).ToList();
            if (filter.isNew) res = res.Where(p => p.isNew == true).ToList();

            return res;
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
                rating += (double) comment.Score;
            }
            rating = rating / (userComments.Count() == 0 ? 1 : userComments.Count());
            return rating;
        }

        public Filter GetFilter(long priceFrom = 0, long priceTo = 0, bool isNew = false, bool isDiscount = false, string ratings = null, bool sort = false)
        {
            return new Filter()
            {
                PriceFrom = priceFrom,
                PriceTo = priceTo,
                isNew = isNew,
                isDiscount = isDiscount,
                ratings = ratings == null ? String.Empty : ratings,
                sort = sort
            };
        }

        public List<Product> ProductsByCatId(int catId)
        {
            // fix: dùng search theo tag
            List<Product> products = GetApiProducts.GetProducts().Where(p => p.CategoryId == catId && p.IsActivated == true).ToList();

            return products;
        }

        public Category CategoryByName(string slugCategory)
        {
            return GetApiCategories.GetCategories().SingleOrDefault(p => p.CategoryName.ToLower() == slugCategory.ToLower());
        }

        public Product ProductBySlugName(string slugProductName)
        {
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.SlugName == slugProductName && p.IsActivated == true);
        }

        // End Function for get data quickly 

        public IActionResult CheckAmountProduct(string slugName, string size = null)
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

            return Ok(amount);
        }
    }
}