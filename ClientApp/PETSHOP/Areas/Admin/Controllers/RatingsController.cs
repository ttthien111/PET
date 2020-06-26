using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using PETSHOP.Utils;

namespace PETSHOP.Areas.Admin.Controllers
{
    [Area(Constants.ADMIN)]
    public class RatingsController : CheckAuthenticateManageController
    {
        public IActionResult Index()
        {
            List<UserComment> comments = GetApiUserComments.GetUserComments()
                                    .OrderByDescending(p => p.UserCommentPostedDate).ToList();
            return View(comments);
        }

        public IActionResult GetRatingsByFilter(int type)
        {
            List<UserComment> comments = new List<UserComment>();
            if (type == 0)
            {
                comments = GetApiUserComments.GetUserComments()
                                    .OrderByDescending(p => p.UserCommentPostedDate).ToList();
            }
            else
            {
                comments = GetApiUserComments.GetUserComments()
                                    .Where(p => p.Score == type).OrderByDescending(p => p.UserCommentPostedDate).ToList();
            }
            return View(comments);
        }

        public IActionResult DeActiveComment(int commentId)
        {
            //get credential
            CredentialModel credential = HttpContext.Session.GetObject<CredentialModel>(Constants.VM_MANAGE);

            UserComment comment = GetApiUserComments.GetUserComments().SingleOrDefault(p => p.UserCommentId == commentId);
            comment.UserCommentApproved = !comment.UserCommentApproved;
            GetApiUserComments.Update(comment, credential.JwToken);

            return Json(comment);
        }
    }
}