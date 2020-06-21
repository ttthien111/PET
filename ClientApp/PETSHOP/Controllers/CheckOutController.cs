using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class CheckOutController : CheckAuthenForCheckOutController
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

        public IActionResult GetCart() => Json(Carts);

        public IActionResult Index()
        {
            ViewBag.DeliveryType = GetApiDeliveryType.GetDeliveryProductTypes();
            return View(Carts);
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IActionResult CheckOutSimple(string bill)
        {
            string generateCodeCheck = RandomString(20);

            CheckOut checkOut = JsonConvert.DeserializeObject<CheckOut>(bill);

            // Create Bill
            Bill createdBill = CreateBill(checkOut, generateCodeCheck);

            // Create Delivery
            DeliveryProduct delivery = CreateDeliveryProduct(checkOut, createdBill);

            // create Bill Detail
            List<BillDetail> details = CreateBillDetail(createdBill);

            //update amount from stock

            // get details with category name is food
            List<BillDetail> foodDetails = details.Where(p => IsFood(p.ProductId)).ToList();

            // get details with category name is toy
            List<BillDetail> toyDetails = details.Where(p => IsToy(p.ProductId)).ToList();

            // get details with category name is costume
            List<BillDetail> cosDetails = details.Where(p => IsCostume(p.ProductId)).ToList();

            // update each list details 
            UpdateStock(foodDetails, toyDetails, cosDetails);

            // remove cart
            HttpContext.Session.Remove("cart");

            return Ok(createdBill);
        }

        private bool IsCostume(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.COSTUME);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }

        private bool IsToy(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.TOY);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }

        private bool IsFood(int productId)
        {
            Category category = GetApiCategories.getCategoryBySlugName(Constants.FOOD);
            return GetApiProducts.GetProducts().SingleOrDefault(p => p.ProductId == productId && p.CategoryId == category.CategoryId) != null;
        }

        private void UpdateStock(List<BillDetail> foods, List<BillDetail> toys, List<BillDetail> costumes)
        {
            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            string token = credential.JwToken;

            //HTTP PUT FOOD
            foreach (var food in foods)
            {
                // get Current Inventory for ProductID
                FoodProduct stockFood = GetApiFoodProducts.GetFoodProducts().SingleOrDefault(p => p.ProductId == food.ProductId);
                    
                // update stock amount
                stockFood.FoodAmount -= food.ProductAmount;

                // request update
                GetApiFoodProducts.UpdateStock(stockFood, token);
            }

            //HTTP PUT TOY
            foreach (var toy in toys)
            {
                // get Current Inventory for ProductID
                ToyProduct stock = GetApiToyProducts.GetToyProducts().SingleOrDefault(p => p.ProductId == toy.ProductId);

                // update stock amount
                stock.ToyAmount -= toy.ProductAmount;

                // request update
                GetApiToyProducts.UpdateStock(stock, token);
            }

            //HTTP PUT COSTUME
            foreach (var costume in costumes)
            {
                // get Size of billDetail
                string size = costume.NoteSize;

                // get Current Inventory for ProductID
                CostumeProduct stock = GetApiCostumeProducts.GetCostumeProducts().SingleOrDefault(p => p.ProductId == costume.ProductId && p.CostumeSize == costume.NoteSize);

                // update stock amount
                stock.CostumeAmountSize -= costume.ProductAmount;

                // request update
                GetApiCostumeProducts.UpdateStock(stock, token);
            }

        }

        private List<BillDetail> CreateBillDetail(Bill createdBill)
        {
            List<BillDetail> details = new List<BillDetail>();
            List<BillDetail> response = new List<BillDetail>();
            foreach (var item in Carts)
            {
                details.Add(new BillDetail()
                {
                    BillId = createdBill.BillId,
                    ProductAmount = item.Amount,
                    ProductPriceCurrent = item.Price,
                    ProductTotalPrice = item.Price * item.Amount,
                    ProductId = GetApiProducts.GetProducts().SingleOrDefault(p => p.SlugName == item.CartItemSlugName).ProductId,
                    NoteSize = item.Size != "" ? item.Size : ""
                });

            }

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            string token = credential.JwToken;

            // post Delivery
            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                foreach (var detail in details)
                {
                    var postTask = client.PostAsJsonAsync<BillDetail>("billdetails", detail);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<BillDetail>();
                        readTask.Wait();

                        response.Add(readTask.Result);
                    }
                }

                return response;
            }
        }

        private DeliveryProduct CreateDeliveryProduct(CheckOut checkOut, Bill bill)
        {
            DeliveryProduct delivery = new DeliveryProduct()
            {
                DeliveryProductAddress = checkOut.DeliveryProductAddress,
                DeliveryProductBillId = bill.BillId,
                DeliveryProductPhoneNumber = checkOut.DeliveryProductPhoneNumber,
                DeliveryProductNote = checkOut.DeliveryProductNote,
                DeliveryProductStateId = checkOut.DeliveryProductStateId,
                DeliveryProductTypeId = checkOut.DeliveryProductTypeId
            };

            CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
            string token = credential.JwToken;

            // post Delivery
            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<DeliveryProduct>("deliveryproducts", delivery);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return delivery;
                }
                else
                {
                    return null;
                }
            }
        }

        private Bill CreateBill(CheckOut checkOut, string generateCodeCheck)
        {
            if (ModelState.IsValid)
            {
                Bill response = null;

                Bill bill = new Bill()
                {
                    UserProfileId = checkOut.UserProfileId,
                    DateOfPurchase = checkOut.DateOfPurchase,
                    TotalPrice = checkOut.TotalPrice,
                    IsDelivery = false,
                    PaymentMethodTypeId = checkOut.PaymentMethodTypeId,
                    GenerateCodeCheck = generateCodeCheck
                };
                CredentialModel credential = JsonConvert.DeserializeObject<CredentialModel>(HttpContext.Session.GetString("vm"));
                string token = credential.JwToken;

                // post comment
                using (HttpClient client = Common.HelperClient.GetClient(token))
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PostAsJsonAsync<Bill>("bills", bill);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Bill>();
                        readTask.Wait();

                        response = readTask.Result;
                    }
                }

                return response;
            }

            return null; 
        }
    }
}