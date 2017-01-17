using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Exchange.WebServices.Data;
using SendEmail.Models;

namespace SendEmail.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(UserData userData)
        {
            if (ModelState.IsValid)
            {
                var service = Service.GetService(userData, null);

                TempData["Message"] = (service == null) ?
                    "Connection to Exchange services failed. Check data and try again." :
                    Service.SendBatchEmails(service, userData);    
            }
            TempData["Message"] = "There are some incorrect data in form. Kindly check and try again.";
            TempData.Keep();
            return RedirectToAction("Index");
        }
    }
}