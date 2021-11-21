using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MasterDetailsPOS.Models;
using WebGrease.Css.Extensions;

namespace MasterDetailsPOS.Controllers
{
    public class OrderMasterEditController : Controller
    {
        [Authorize]

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOrderMaster(int? id)
        {
            using (MasterDetailsPOSEntities db =new MasterDetailsPOSEntities())
            {
                if (id==null)
                {
                    var orders = db.OrderMasters.Select(a => new
                    {
                        a.OrderID,
                        a.OrderName,
                        a.OrderDate,
                        a.Description
                    }).ToList();
                    return Json(orders, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var orders = db.OrderMasters.Where(a => a.OrderID == id)
                 .Select(a => new
                 {
                     a.OrderID,
                     a.OrderName,
                     a.OrderDate,
                     a.Description
                 }).ToList();
                    return Json(orders, JsonRequestBehavior.AllowGet);
                }
             }
        }        

        [HttpPost]
        
        public ActionResult GetOrderDetails(int orderId)
        {
            using (MasterDetailsPOSEntities db = new MasterDetailsPOSEntities())
            {
                var orderList = db.UpdateOrders.Where(a => a.OrderID == orderId)
                    .Select(a => new
                    {
                        a.OrderID,
                        a.CategoryName,
                        a.ProductName,
                        a.Rate,
                        a.Quantity
                    }).ToList();
                return Json(orderList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateOrder(OrderMaster orderMaster)
        {
            using (MasterDetailsPOSEntities db=new MasterDetailsPOSEntities())
            {
                var masterData = db.OrderMasters.FirstOrDefault(a => a.OrderID == orderMaster.OrderID);
                if (masterData!=null)
                {
                    masterData.OrderName = orderMaster.OrderName;
                    masterData.OrderDate = orderMaster.OrderDate;
                    masterData.Description = orderMaster.Description;
                    db.Entry(masterData);
                    db.SaveChanges();
                }
                List<OrderDetail> result = db.OrderDetails.Where(a => a.OrderID == orderMaster.OrderID).ToList();
                db.OrderDetails.RemoveRange(result);
                db.SaveChanges();
                int roweffect = 0;
                foreach (var data in orderMaster.OrderDetails)
                {
                    data.OrderID = orderMaster.OrderID;
                    db.OrderDetails.Add(data);
                    roweffect =db.SaveChanges();
                }
                return Json(roweffect, JsonRequestBehavior.AllowGet);
            }
        }
    }
}