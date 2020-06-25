using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class ChartsController : CheckAuthenticateAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
        // chart by filter

        public IActionResult BillsFilter(string from, string to)
        {
            DateTime fromValue = new DateTime();
            DateTime toValue = new DateTime();
            if (from == "nodata" && to == "nodata")
            {
                fromValue = DateTime.Now.AddDays(-6);
                toValue = DateTime.Now;
            }
            else if (from == "nodata" && to != "nodata")
            {
                fromValue = DateTime.Parse(to).AddDays(-6);
                toValue = DateTime.Parse(to);
            }
            else if (from != "nodata" && to == "nodata")
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(from).AddDays(6);
            }
            else
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(to);
            }

            List<Earning7Day> BillsFilter = new List<Earning7Day>();
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<DateTime> dateTimes = new List<DateTime>();
            int days = (toValue.Date - fromValue.Date).Days + 1;

            for (int i = 0; i < days; i++)
            {
                dateTimes.Add(fromValue.Date);
                fromValue = fromValue.AddDays(1);
            }

            dateTimes = dateTimes.OrderBy(p => p).ToList();

            // get data
            foreach (var date in dateTimes)
            {
                BillsFilter.Add(new Earning7Day()
                {
                    Label = date.ToString("dd/MM/yyyy"),
                    Data = GetBills(credential).Where(p => p.DateOfPurchase.Date == date).Count()
                });
            }

            return Json(BillsFilter);
        }

        public IActionResult PaymentMethodFilter(string from = "nodata", string to = "nodata")
        {
            DateTime fromValue = new DateTime();
            DateTime toValue = new DateTime();
            if (from == "nodata" && to == "nodata")
            {
                fromValue = DateTime.Now.AddDays(-6);
                toValue = DateTime.Now;
            }
            else if (from == "nodata" && to != "nodata")
            {
                fromValue = DateTime.Parse(to).AddDays(-6);
                toValue = DateTime.Parse(to);
            }
            else if (from != "nodata" && to == "nodata")
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(from).AddDays(6);
            }
            else
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(to);
            }

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
                    Data = GetBills(credential).Where(p => p.PaymentMethodTypeId == GetApiPaymentMethodTypes.GetPaymentMethodTypes().SingleOrDefault(k => k.PaymentMethodTypeName == payment).PaymentMethodTypeId && 
                                (p.DateOfPurchase.Date >= fromValue.Date) && 
                                (p.DateOfPurchase.Date <= toValue.Date)).Count()
                });
            }

            return Json(paymentMethod7Days.OrderBy(p => p.Label).ToList());
        }

        public IActionResult EarningFilter(string from = "nodata", string to = "nodata")
        {
            DateTime fromValue = new DateTime();
            DateTime toValue = new DateTime();
            if (from == "nodata" && to == "nodata")
            {
                fromValue = DateTime.Now.AddDays(-6);
                toValue = DateTime.Now;
            }
            else if (from == "nodata" && to != "nodata")
            {
                fromValue = DateTime.Parse(to).AddDays(-6);
                toValue = DateTime.Parse(to);
            }
            else if (from != "nodata" && to == "nodata")
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(from).AddDays(6);
            }
            else
            {
                fromValue = DateTime.Parse(from);
                toValue = DateTime.Parse(to);
            }

            List<Earning7Day> earningFilter = new List<Earning7Day>();
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<DateTime> dateTimes = new List<DateTime>();
            int days = (toValue.Date - fromValue.Date).Days + 1;

            for (int i = 0; i < days; i++)
            {
                dateTimes.Add(fromValue.Date);
                fromValue = fromValue.AddDays(1);
            }

            dateTimes = dateTimes.OrderBy(p => p).ToList();

            // get data
            foreach (var date in dateTimes)
            {
                earningFilter.Add(new Earning7Day()
                {
                    Label = date.ToString("dd/MM/yyyy"),
                    Data = GetBills(credential).Where(p => p.DateOfPurchase.Date == date).Sum(p => p.TotalPrice)
                });
            }

            return Json(earningFilter);
        }


        private List<Bill> GetBills(CredentialManage credential)
        {
            return GetApiBills.GetBills(credential).Where(p => p.IsCancel == false).ToList();
        }
    }
}