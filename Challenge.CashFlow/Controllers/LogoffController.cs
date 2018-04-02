using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Challenge.CashFlow.Models;
using Challenge.CashFlow.DAL;
using System.Web.Security;

namespace Challenge.CashFlow.Controllers
{
    public class LogoffController : Controller
    {
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
