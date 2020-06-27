using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PETSHOP.Areas.Admin.Models;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class CustomersController : CheckAuthenticateManageController
    {
        public IActionResult Index()
        {
            return View(GetProfiles());
        }

        public List<UserProfile> GetProfiles(bool isActive = true)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<UserProfile> profiles = GetApiUserProfile.GetUserProfiles()
                            .Where(p=> GetApiAccounts.GetAccounts().SingleOrDefault(k=> k.AccountId == p.AccountId) != null )
                            .ToList();

            foreach (var prof in profiles)
            {
                Account account = GetApiAccounts.GetAccounts().SingleOrDefault(p => p.AccountId == prof.AccountId);
                prof.Account = account;
            }

            return profiles;
        }

        public IActionResult DeActiveAccount(int accountId)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            Account account = GetApiAccounts.GetAccounts().SingleOrDefault(p => p.AccountId == accountId);
            account.IsActive = !account.IsActive;

            // update
            GetApiAccounts.Update(account, credential.JwToken);

            return RedirectToAction("Index");
        }

        public IActionResult ExportExcel()
        {
            List<UserProfile> profiles = GetApiUserProfile.GetUserProfiles().ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("ProfilesData");

                //custom format header
                worksheet.Row(1).Height = 20;
                worksheet.Column(1).Width = 5;
                worksheet.Column(2).Width = 30;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 30;
                worksheet.Column(5).Width = 20;
                worksheet.Column(6).Width = 20;
                worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Row(1).Style.Font.Bold = true;

                //custom color
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#e8d39e");
                for (int i = 1; i <= 6; i++)
                {
                    worksheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(colFromHex);
                }


                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Tên khách hàng";
                worksheet.Cells[1, 4].Value = "Ngày sinh";
                worksheet.Cells[1, 5].Value = "Địa chỉ";
                worksheet.Cells[1, 6].Value = "Số điện thoại";

                //body of table  
                int recordindex = 2;
                int idx = 1;

                foreach (var prof in profiles)
                {
                    worksheet.Cells[recordindex, 1].Value = idx;
                    worksheet.Cells[recordindex, 2].Value = prof.UserProfileEmail;
                    worksheet.Cells[recordindex, 3].Value = prof.UserProfileFirstName + " " + prof.UserProfileMiddleName + " " + prof.UserProfileLastName;
                    worksheet.Cells[recordindex, 4].Value = prof.UserProfileDob != null ? DateTime.Parse(prof.UserProfileDob).ToString("dd/MM/yyyy") : "_";
                    worksheet.Cells[recordindex, 5].Value = prof.UserProfileAddress != null ? prof.UserProfileAddress : "_";
                    worksheet.Cells[recordindex, 6].Value = prof.UserProfilePhoneNumber != null ? prof.UserProfilePhoneNumber : "_";


                    worksheet.Row(recordindex).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Row(recordindex).Style.Font.Bold = false;
                    idx++;
                    recordindex++;
                }

                package.Save();
            }

            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProfilesData.xlsx");
        }
    }
}