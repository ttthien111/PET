using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class HomeController : CheckAuthenticateManageController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Bills7Days(int days)
        {
            List<Earning7Day> Bills7Days = new List<Earning7Day>();
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime now = DateTime.Now.Date;
            for (int i = 0; i < days; i++)
            {
                dateTimes.Add(now);
                now = now.AddDays(-1);
            }

            dateTimes = dateTimes.OrderBy(p => p).ToList();

            // get data
            foreach (var date in dateTimes)
            {
                Bills7Days.Add(new Earning7Day()
                {
                    Label = date.ToString("dd/MM/yyyy"),
                    Data = GetBills(credential).Where(p => p.DateOfPurchase.Date == date).Count()
                });
            }

            return Json(Bills7Days);
        }

        public IActionResult PaymentMethod7Days(int days)
        {
            List<Earning7Day> paymentMethod7Days = new List<Earning7Day>();
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<string> paymentMethod = new List<string>();

            foreach (var p in GetApiPaymentMethodTypes.GetPaymentMethodTypes())
            {
                paymentMethod.Add(p.PaymentMethodTypeName);
            }

            // get data
            foreach (var payment in paymentMethod)
            {
                paymentMethod7Days.Add(new Earning7Day()
                {
                    Label = payment,
                    Data = GetBills(credential).Where(p => p.PaymentMethodTypeId == GetApiPaymentMethodTypes.GetPaymentMethodTypes().SingleOrDefault(k => k.PaymentMethodTypeName == payment).PaymentMethodTypeId && (DateTime.Now.Date - p.DateOfPurchase.Date).Days < days).Count()
                });
            }

            return Json(paymentMethod7Days.OrderBy(p => p.Label).ToList());
        }

        public IActionResult Earning7Days(int days)
        {
            List<Earning7Day> earning7Days = new List<Earning7Day>();
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<DateTime> dateTimes = new List<DateTime>();
            DateTime now = DateTime.Now.Date;
            for (int i = 0; i < days; i++)
            {
                dateTimes.Add(now);
                now = now.AddDays(-1);
            }

            dateTimes = dateTimes.OrderBy(p => p).ToList();

            // get data
            foreach (var date in dateTimes)
            {
                earning7Days.Add(new Earning7Day()
                {
                    Label = date.ToString("dd/MM/yyyy"),
                    Data = GetBills(credential).Where(p => p.DateOfPurchase.Date == date).Sum(p => p.TotalPrice)
                });
            }

            return Json(earning7Days);
        }


        private List<Bill> GetBills(CredentialManage credential)
        {
            return GetApiBills.GetBills(credential).Where(p => p.IsCancel == false).ToList();
        }
    }
}