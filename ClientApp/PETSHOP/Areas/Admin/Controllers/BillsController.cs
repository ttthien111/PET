using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class BillsController : CheckAuthenticateManageController
    {

        public List<BillViewModel> getBills()
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));

            List<BillViewModel> bills = GetApiBills.GetBills(credential).Select(p => new BillViewModel()
            {
                BillId = p.BillId,
                UserProfileEmail = GetApiUserProfile.GetUserProfiles().SingleOrDefault(k => k.UserProfileId == p.UserProfileId).UserProfileEmail,
                DateOfPurchase = p.DateOfPurchase,
                CurrentDeliveryState = GetApiDeliveryStates.GetDeliveryProductStates().SingleOrDefault(h => h.DeliveryProductStateId == GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(k => k.DeliveryProductBillId == p.BillId).DeliveryProductStateId).DeliveryProductStateName,
                IsDelivery = p.IsDelivery,
                DateOfDelivered = p.DateOfDelivered,
                GenerateCodeCheck = p.GenerateCodeCheck,
                PaymentMethodName = GetApiPaymentMethodTypes.GetPaymentMethodTypes().SingleOrDefault(k => k.PaymentMethodTypeId == p.PaymentMethodTypeId).PaymentMethodTypeName,
                IsCancel = p.IsCancel,
                TotalPrice = p.TotalPrice,
                IsApprove = p.IsApprove,
                IsCompleted = p.IsCompleted,
                CurrentDeliveryStateId = GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(k => k.DeliveryProductBillId == p.BillId).DeliveryProductStateId
            }).ToList();

            return bills;
        }
        public IActionResult Index()
        {
            List<BillViewModel> bills = getBills();
            return View(bills);
        }

        public IActionResult WaitingBills()
        {
            List<BillViewModel> bills = getBills().Where(p=>p.IsApprove == false && p.IsCancel == false && p.IsDelivery == false).ToList();
            return View(bills);
        }

        public IActionResult ApprovedBills()
        {
            List<BillViewModel> bills = getBills().Where(p => p.IsApprove == true && p.IsCancel == false && p.IsDelivery == false).ToList();
            return View(bills);
        }

        public IActionResult FollowingBills()
        {
            ViewBag.nState = GetApiDeliveryStates.GetDeliveryProductStates().Count();
            ViewBag.State = GetApiDeliveryStates.GetDeliveryProductStates();
            List<BillViewModel> bills = getBills().Where(p => p.IsApprove == true && p.IsCancel == false && p.IsDelivery == true).ToList();
            return View(bills);
        }

        public IActionResult CompletedBills()
        {
            List<BillViewModel> bills = getBills().Where(p => p.IsCompleted == true).ToList();
            return View(bills);
        }

        public IActionResult CanceledBills()
        {
            List<BillViewModel> bills = getBills().Where(p => p.IsCancel == true).ToList();
            return View(bills);
        }

        public IActionResult BillDetail(int billId)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));

            Bill p = GetApiBills.GetBills(credential).SingleOrDefault(p => p.BillId == billId);
            BillModelView bill = new BillModelView() { 
                BillId = p.BillId,
                BillCode = p.GenerateCodeCheck,
                DateOfPurchase = p.DateOfPurchase,
                DateDelivery = Convert.ToDateTime(p.DateOfDelivered),
                TotalPrice = p.TotalPrice,
                PaymentMethodId = p.PaymentMethodTypeId,
                IsDelivery = p.IsDelivery,
                IsCancel = p.IsCancel,
                IsApprove = p.IsApprove,
                IsCompleted = p.IsCompleted,
                PaymentMethodName = GetApiPaymentMethodTypes.GetPaymentMethodTypes().SingleOrDefault(k => k.PaymentMethodTypeId == p.PaymentMethodTypeId).PaymentMethodTypeName,
            };
           
            List<BillDetailModel> details = GetApiBillDetails.GetBillDetails().Where(p => p.BillId == bill.BillId)
                                            .Select(p => new BillDetailModel()
                                            {
                                                ProductId = p.ProductId,
                                                ProductName = GetApiProducts.GetProducts().SingleOrDefault(k => k.ProductId == p.ProductId).ProductName,
                                                Amount = p.ProductAmount,
                                                Price = p.ProductPriceCurrent,
                                                NoteSize = p.NoteSize,
                                                Image = GetApiProducts.GetProducts().SingleOrDefault(k => k.ProductId == p.ProductId).ProductImage,
                                            }).ToList();

            bill.BillDetail = details;

            // get delivery
            DeliveryProduct delivery = GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(p => p.DeliveryProductBillId == bill.BillId);
            bill.delivery = delivery;

            // get state of bill
            bill.DeliveryProductStateId = delivery.DeliveryProductStateId;

            bill.DeliveryStateName = GetApiDeliveryStates.GetDeliveryProductStates().SingleOrDefault(p => p.DeliveryProductStateId == delivery.DeliveryProductStateId).DeliveryProductStateName;

            return Json(bill);
        }
        
        [HttpPost]
        public IActionResult ApproveBill(int billId)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));

            Bill bill = GetApiBills.GetBills(credential).SingleOrDefault(p => p.BillId == billId);
            // update 
            bill.IsApprove = true;

            if (Directory.Exists(Constants.EMBEDED_MAIL_URL) && System.IO.File.Exists(Constants.EMBEDED_MAIL_URL + bill.GenerateCodeCheck + ".png"))
            {
                // get credential
                string token = credential.JwToken;

                using (HttpClient client = Common.HelperClient.GetClient(token))
                {
                    client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                    var postTask = client.PutAsJsonAsync<Bill>(Constants.BILL + "/" + bill.BillId, bill);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Bill>();
                        readTask.Wait();

                        // send email
                        UserProfile user = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileId == bill.UserProfileId);

                        SenderEmail.SendMail(user.UserProfileEmail, "PETSHOP Hóa đơn #" + bill.GenerateCodeCheck, "Đơn hàng có mã: #" + bill.GenerateCodeCheck + " vừa được duyệt", bill.GenerateCodeCheck);

                        return Json(readTask.Result);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return NoContent();
            } 
        }

        public IActionResult UpdateDeliveryState(string billCode, int stateId)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            Bill bill = GetApiBills.GetBills(credential).SingleOrDefault(p => p.GenerateCodeCheck == billCode);
            DeliveryProduct delivery = GetApiDeliveryProducts.GetDeliveryProducts().SingleOrDefault(p => p.DeliveryProductBillId == bill.BillId);
            delivery.DeliveryProductStateId = stateId;

            if(GetApiDeliveryStates.GetDeliveryProductStates()
                .SingleOrDefault(p=>p.DeliveryProductStateId == stateId).DeliveryProductStateName == "Đã giao hàng")
            {
                bill.IsCompleted = true;
                bill.DateOfDelivered = DateTime.Now;
                GetApiBills.Update(bill, credential.JwToken);
            }

            GetApiDeliveryProducts.Update(delivery, credential.JwToken);

            // sender mail
            UserProfile profile = GetApiUserProfile.GetUserProfiles().SingleOrDefault(p => p.UserProfileId == bill.UserProfileId);

            string body = "Đơn hàng có mã #" + bill.GenerateCodeCheck + " đã cập nhật trạng thái vận chuyển mới: " +
                                                GetApiDeliveryStates.GetDeliveryProductStates()
                                                .SingleOrDefault(p => p.DeliveryProductStateId == stateId).DeliveryProductStateName;
            SenderEmail.SendMail(profile.UserProfileEmail, "PETSHOP: UPDATE DELIVERY STATE'S YOUR BILL", body);
            return Json(bill);
        }
    }
}