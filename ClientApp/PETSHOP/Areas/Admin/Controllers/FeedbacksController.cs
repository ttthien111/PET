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
    public class FeedbacksController : CheckAuthenticateManageController
    {
        public IActionResult Index()
        {
            List<Feedback> feedbacks = GetFeedbacks();
            return View(feedbacks);
        }

        public List<Feedback> GetFeedbacks()
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            return GetApiFeedbacks.GetFeedbacks(credential.JwToken).OrderByDescending(p => p.IsRead == false).ToList();
        }

        public IActionResult ReplyFeedback(int feedbackId,string email, string content)
        {
            if(feedbackId != 0 && email != null && content != null)
            {
                SenderEmail.SendMail(email, "PETSHOP RESPONSE FOR FEEDBACK", content);
                UpdateIsRead(feedbackId);
            }
            return Ok(email);
        }

        [HttpGet]
        public IActionResult UpdateStatusIsRead(int feedbackId)
        {
            UpdateIsRead(feedbackId);
            return NoContent();
        }

        private void UpdateIsRead(int feedbackId)
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            Feedback feedback = GetApiFeedbacks.GetFeedbacks(credential.JwToken).SingleOrDefault(p => p.FeedbackId == feedbackId);
            // update
            feedback.IsRead = !feedback.IsRead;
            GetApiFeedbacks.Update(feedback, credential.JwToken);
        }

        public IActionResult GetStatus(int feedbackId) {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            return Ok(GetApiFeedbacks.GetFeedbacks(credential.JwToken).SingleOrDefault(p => p.FeedbackId == feedbackId).IsRead);
        }

        public IActionResult UpdateAllIsRead()
        {
            CredentialManage credential = JsonConvert.DeserializeObject<CredentialManage>(HttpContext.Session.GetString(Constants.VM_MANAGE));
            List<Feedback> feedbacks = GetApiFeedbacks.GetFeedbacks(credential.JwToken).ToList();
            foreach (var feedback in feedbacks)
            {
                feedback.IsRead = true;
                GetApiFeedbacks.Update(feedback, credential.JwToken);
            }

            return RedirectToAction("Index");
        }
    }
}