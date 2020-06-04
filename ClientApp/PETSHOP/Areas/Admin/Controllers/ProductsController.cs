using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Models;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductsController : CheckAuthenticationController
    {
        public readonly PETSHOPContext _ctx;
        public ProductsController(PETSHOPContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("/admin/products")]
        public IActionResult Index()
        {
            List<ProductModelView> products = _ctx.Product.Select(p => new ProductModelView()
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                ProductImage = p.ProductImage,
                ProductPrice = p.ProductPrice,
                ProductDiscount = p.ProductDiscount,
                CategoryName = _ctx.Category.SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                DistributorName = _ctx.Distributor.SingleOrDefault(l => l.DistributorId == p.DistributorId).DistributorName,
                IsActivated = p.IsActivated
            }).ToList();
            return View(products);
        }

        public IActionResult Search(string textSearch = null) {
            List<ProductModelView> products = new List<ProductModelView>();
            if (textSearch == null)
            {
               products = _ctx.Product.Select(p => new ProductModelView()
               {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImage,
                    ProductPrice = p.ProductPrice,
                    ProductDiscount = p.ProductDiscount,
                    CategoryName = _ctx.Category.SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                    DistributorName = _ctx.Distributor.SingleOrDefault(l => l.DistributorId == p.DistributorId).DistributorName
               }).ToList();
                
            }
            else
            {
                products = _ctx.Product.Where(p => p.ProductId.ToString().ToUpper().Contains(textSearch.Trim()) || 
                                                   p.ProductName.ToUpper().Contains(textSearch.ToUpper().Trim()) ||
                                                   p.ProductDescription.ToUpper().Contains(textSearch.ToUpper().Trim())
                                             ).Select(p => new ProductModelView()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductImage = p.ProductImage,
                    ProductPrice = p.ProductPrice,
                    ProductDiscount = p.ProductDiscount,
                    CategoryName = _ctx.Category.SingleOrDefault(k => k.CategoryId == p.CategoryId).CategoryName,
                    DistributorName = _ctx.Distributor.SingleOrDefault(l => l.DistributorId == p.DistributorId).DistributorName
                }).ToList();
            }
            return PartialView(products);
        }
    }
}