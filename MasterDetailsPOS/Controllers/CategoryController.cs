using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterDetailsPOS.Models;

namespace MasterDetailsPOS.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCategory()
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var categories = db.Categories.OrderBy(a => a.CategoryName)
                    .Select(a => new
                    {
                        a.CategoryID,
                        a.CategoryName
                    }).ToList();
                return Json(categories, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveCategory(Category category)
        {
            using (MasterDetailsPOSEntities db =new MasterDetailsPOSEntities())
            {
                bool name=db.Categories.Any(a => a.CategoryName == category.CategoryName);
                if (name==false)
                {
                    db.Categories.Add(category);
                    int valueEffect = db.SaveChanges();
                    return Json(valueEffect, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult UpdateCategory(Category category)
        {
            using (MasterDetailsPOSEntities db=new MasterDetailsPOSEntities())
            {
                var findCategory = db.Categories.FirstOrDefault(a => a.CategoryID == category.CategoryID);
                if (findCategory!=null)
                {
                    findCategory.CategoryName = category.CategoryName;
                    db.Entry(findCategory);
                    int rowEffect=db.SaveChanges();
                    return Json(rowEffect, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}