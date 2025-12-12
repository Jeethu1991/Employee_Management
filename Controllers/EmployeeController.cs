using EmployeeManagement.Data;
using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _repo = new EmployeeRepository();

        public ActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxCreate(Employee model)
        {
            Debug.WriteLine("AjaxCreate called");
            if (ModelState.IsValid)
            {
                var rows = _repo.Save(model);
                return Json(new { success = rows > 0 });
            }

            return Json(new { success = false, errors = ModelState });
        }

        public ActionResult Edit(int id)
        {
            var e = _repo.GetById(id);
            if (e == null) return HttpNotFound();
            return View(e);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee model)
        {
            if(ModelState.IsValid)
            {
                _repo.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var e = _repo.GetById(id);
            if (e == null) return HttpNotFound();
            return View(e);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetAllJson()
        {
            var list = _repo.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}