using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;

namespace PETSHOP.Controllers
{
    public class PaypalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public List<CartItem> Cart
        {
            get
            {
                var data = HttpContext.Session.GetObject<List<CartItem>>("cart");
                if (data == null)
                {
                    data = new List<CartItem>();
                }

                return data;
            }
        }
        public async Task<IActionResult> Checkout(string bill)
        {
            string generateCodeCheck = Encryptor.RandomString(20);

            CheckOut checkOut = JsonConvert.DeserializeObject<CheckOut>(bill);

            //SandboxEnvironment(clientId, clientSerect)
            var environment = new SandboxEnvironment("AXQGDKCc8Q4HNj3CmqQzipkha8qZy_RdJXKihaZ8Pp5sPChWW8RyCQ544XJCrGXgc42VDD4DcD7KIKcR-f1Lkl", "EGL1Sorwr0fxp7cRXxVL12VaI9rAH9RysBslu9KiDnuxEESkf8DVC-DjqAXrqOn9Ufi1dcYzC34rLFye");
            var client = new PayPalHttpClient(environment);

            //Đọc thông tin đơn hàng từ Session
            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            var TotalPrice = checkOut.TotalPrice.ToString();
            foreach (var item in Cart)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.Name,
                    Currency = "USD",
                    Price = item.Price.ToString(),
                    Quantity = item.Amount.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = TotalPrice.ToString(),
                            Currency = "USD",
                            Details = new AmountDetails
                            {
                                Tax = "0",
                                Shipping = "0",
                                Subtotal = TotalPrice.ToString()
                            }
                        },
                        ItemList = itemList,
                        Description = "Order " + generateCodeCheck,
                        InvoiceNumber = DateTime.Now.Ticks.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = "http://localhost:44337/Paypal/Fail",
                    ReturnUrl = "http://localhost:44337/Paypal/Success"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                HttpResponse response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);

            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                return RedirectToAction("Fail");
            }
        }

        public IActionResult Success()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Đã thanh toán, phương thức: Paypal
            return Content("Thanh toán thành công");
        }

        public IActionResult Fail()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Chưa thanh toán, phương thức: 
            return Content("Thanh toán thất bại");
        }

    }
}