using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterDetailsPOS.Models;

namespace MasterDetailsPOS.Controllers
{
    public class ProductController : Controller
    {
       [Authorize]

       [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult GetAllCategory()
        //{
        //    using (DBEntites db =new DBEntites())
        //    {
        //        var categories = db.Categories.OrderBy(a => a.CategoryName)
        //            .Select(a => new
        //            {
        //                a.CategoryID,
        //                a.CategoryName
        //            }).ToList();
        //        return Json(categories, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetAllProducts(int? categoryId)
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                if (categoryId==0 || categoryId==null)
                {
                    var products = db.Products.Join(db.Categories,
                    (product => product.CategoryID),
                    (category => category.CategoryID),
                    ((product, category) => new { Products = product, Categories = category })
                    ).OrderBy(a => a.Categories.CategoryName)
                    .Select(a => new 
                    {
                        a.Categories.CategoryName,
                        a.Categories.CategoryID,
                        a.Products.ProductID,
                        a.Products.ProductName,
                        a.Products.Rate
                    }).ToList();
                    return Json(products, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var products = db.Categories.Join(db.Products,
                         (category => category.CategoryID),
                        (product => product.CategoryID),
                        ((category, product) => new { Categories = category, Products = product })
                        ).Where(a=>a.Categories.CategoryID==categoryId)
                        .Select(a=>new
                        {
                            a.Categories.CategoryName,
                            a.Categories.CategoryID,
                            a.Products.ProductID,
                            a.Products.ProductName,
                            a.Products.Rate
                        }).ToList();
                    return Json(products, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult EditProduct(int productId)
        {
            using (MasterDetailsPOSEntities db=new MasterDetailsPOSEntities())
            {
                var product = db.Products.Where(a => a.ProductID == productId)
                    .Select(a=>new
                    {
                        a.ProductID,
                        a.CategoryID,
                        a.ProductName,
                        a.Rate
                    }).ToList();
                return Json(product, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteProduct(int productId)
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var product = db.Products.SingleOrDefault(a => a.ProductID == productId);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult SaveProduct(Product product)
        {
            using (MasterDetailsPOSEntities db =new MasterDetailsPOSEntities())
            {
                bool check =db.Products.Any(a => a.CategoryID == product.CategoryID && a.ProductName == product.ProductName);
                if (check)
                {
                    db.Products.Add(product);
                    int valuEffect = db.SaveChanges();
                    return Json(valuEffect, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateProduct(Product product)
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var pro = db.Products.FirstOrDefault(a => a.ProductID == product.ProductID);
                if (pro!=null)
                {
                    pro.ProductName = product.ProductName;
                    pro.Rate = product.Rate;
                    db.Entry(pro);
                    int rowEffetct=db.SaveChanges();
                    return Json(rowEffetct, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}