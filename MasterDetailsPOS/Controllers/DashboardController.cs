using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterDetailsPOS.Models;

namespace MasterDetailsPOS.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            Session["username"] = User.Identity.Name;
            ArrayList list = new ArrayList();
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var categorys = db.Categories.Count();
                var products = db.Products.Count();
                var customers = db.OrderMasters.Count(a => a.OrderDate == DateTime.Now);
                list.Add(categorys);
                list.Add(products);
                list.Add(customers);
                ViewBag.Data = list;
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetTopSaleData()
        {
            using (MasterDetailsPOSEntities db=new MasterDetailsPOSEntities())
            {
                var listOfSale=db.TopSaleProducts.OrderByDescending(a => a.Total).Take(5).ToList();
                return Json(listOfSale, JsonRequestBehavior.AllowGet);
            }
        }
    }
}