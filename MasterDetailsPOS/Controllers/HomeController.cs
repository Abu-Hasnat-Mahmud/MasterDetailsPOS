using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MasterDetailsPOS.Models;

namespace MasterDetailsPOS.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AllCategories()
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var categories = db.Categories.OrderBy(a => a.CategoryName).Select(a => new
                {
                    a.CategoryID,
                    a.CategoryName
                }).ToList();
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProducts(int categoryId)
        {
            using (MasterDetailsPOSEntities db =new MasterDetailsPOSEntities())
            {
                var products =
                    db.Products.OrderBy(a => a.ProductName)
                        .Where(a => a.CategoryID == categoryId)
                        .Select(a => new
                        {
                            a.ProductID,
                            a.ProductName
                        }).ToList();
                return Json(products, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProductRate(int productId)
        {
            using (MasterDetailsPOSEntities db=new MasterDetailsPOSEntities())
            {
                var rate = db.Products.Where(a => a.ProductID == productId)
                    .Select(a => new
                    {
                        a.Rate  
                    }).ToList();
                return Json(rate, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Save(OrderMaster order)
        {
            using (MasterDetailsPOSEntities db =new MasterDetailsPOSEntities())
            {
                db.OrderMasters.Add(order);
                var rowEffect=db.SaveChanges();
                return Json(rowEffect, JsonRequestBehavior.AllowGet);
            }
        }
    }
}