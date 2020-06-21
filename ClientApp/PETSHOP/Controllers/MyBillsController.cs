using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ASPCore_Final.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Controllers
{
    public class MyBillsController : CheckAuthenController
    {
        public IActionResult Index()
        {
            //get bills
            List<BillModelView> bills = GetBills().OrderByDescending(p=>p.DateOfPurchase).ToList();

            // get All delivery states
            ViewBag.States = GetApiDeliveryStates.GetDeliveryProductStates().ToList();
            
            return View(bills);
        }

        public IActionResult CancelBill(int billId)
        {
            //get credential
            CredentialModel credential = HttpContext.Session.GetObject<CredentialModel>("vm");
            //get token
            string token = credential.JwToken;

            // get bill and billdetail
            Bill bill = GetApiBills.GetBills().SingleOrDefault(p => p.BillId == billId);
            List<BillDetailModel> billDetails = GetBills().SingleOrDefault(p => p.BillId == bill.BillId).BillDetail;

            // Update status for bill want to be canceled
            bill.IsCancel = true;
            UpdateBill(bill ,token);

            // update amount
            foreach (var detail in billDetails)
            {
                if (IsFood(detail.ProductId))
                {
                    FoodProduct food = GetApiFoodProducts.GetFoodProducts().SingleOrDefault(p => p.ProductId == detail.ProductId);
                    food.FoodAmount += detail.Amount;
                    GetApiFoodProducts.UpdateStock(food, token);
                } 
                else if (IsToy(detail.ProductId))
                {
                    ToyProduct toy = GetApiToyProducts.GetToyProducts().SingleOrDefault(p => p.ProductId == detail.ProductId);
                    toy.ToyAmount += detail.Amount;
                    GetApiToyProducts.UpdateStock(toy, token);
                }
                else
                {
                    CostumeProduct costume = GetApiCostumeProducts.GetCostumeProducts().SingleOrDefault(p => p.ProductId == detail.ProductId && p.CostumeSize == detail.NoteSize);
                    costume.CostumeAmountSize += detail.Amount;
                    GetApiCostumeProducts.UpdateStock(costume, token);
                }
            }

            return RedirectToAction("Index");
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

        private void UpdateBill(Bill bill, string token)
        {
            using (HttpClient client = Common.HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PutAsJsonAsync<Bill>("bills/"+ bill.BillId, bill);
                postTask.Wait();
            }
        }

        public List<BillModelView> GetBills()
        {
            //get credential
            CredentialModel credential = HttpContext.Session.GetObject<CredentialModel>("vm");
            // get profile
            UserProfile profile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileEmail == credential.AccountUserName);

            // get all Bill with profileID from server
            List<BillModelView> bills = GetApiBills.GetBills().Where(p => p.UserProfileId == profile.UserProfileId).Select(p => new BillModelView()
            {
                BillId = p.BillId,
                BillCode = p.GenerateCodeCheck,
                DateOfPurchase = p.DateOfPurchase,
                DateDelivery = Convert.ToDateTime(p.DateOfDelivered),
                TotalPrice = p.TotalPrice,
                PaymentMethodId = p.PaymentMethodTypeId,
                IsDelivery = p.IsDelivery,
                IsCancel = p.IsCancel
            }).ToList();

            // get payment method
            foreach (var bill in bills)
            {
                bill.PaymentMethodName = GetApiPaymentMethodTypes.GetPaymentMethodTypes().SingleOrDefault(p => p.PaymentMethodTypeId == bill.PaymentMethodId).PaymentMethodTypeName;
            }

            // get bill detail
            foreach (var item in bills)
            {
                List<BillDetailModel> details = GetApiBillDetails.GetBillDetails().Where(p => p.BillId == item.BillId)
                                                .Select(p => new BillDetailModel()
                                                {
                                                    ProductId = p.ProductId,
                                                    ProductName = GetApiProducts.GetProducts().SingleOrDefault(k => k.ProductId == p.ProductId).ProductName,
                                                    Amount = p.ProductAmount,
                                                    Price = p.ProductPriceCurrent,
                                                    NoteSize = p.NoteSize,
                                                    Image = GetApiProducts.GetProducts().SingleOrDefault(k => k.ProductId == p.ProductId).ProductImage,
                                                }).ToList();

                item.BillDetail = details;

                // get delivery of bill
                DeliveryProduct delivery = GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(p => p.DeliveryProductBillId == item.BillId);

                // get state of bill
                item.DeliveryProductStateId = delivery.DeliveryProductStateId;

                item.DeliveryStateName = GetApiDeliveryStates.GetDeliveryProductStates().SingleOrDefault(p => p.DeliveryProductStateId == delivery.DeliveryProductStateId).DeliveryProductStateName;
            }

            return bills;
        }

        // filter
        public IActionResult Filter(string sortState, string sortDate)
        {
            List<BillModelView> res = new List<BillModelView>();
            if (sortState == "cancel")
            {
                res = GetBills().Where(p => (DateTime.Now - p.DateOfPurchase).Days <= int.Parse(sortDate) && p.IsCancel == true).ToList();
            }
            else
            {
                res = GetBills().Where(p => p.DeliveryProductStateId == Int32.Parse(sortState) && (DateTime.Now - p.DateOfPurchase).Days <= Int32.Parse(sortDate)).ToList();
            }
            
            return PartialView(res);
        }

        [Microsoft.AspNetCore.Mvc.Route("mybills/detail/{billId}")]
        public IActionResult BillDetail(int billId)
        {
            BillModelView bill = GetBills().SingleOrDefault(p => p.BillId == billId);
            DeliveryProduct delivery = GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(p => p.DeliveryProductBillId == bill.BillId);
            bill.delivery = delivery;
            return View(bill);
        }
    }
}