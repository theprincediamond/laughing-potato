
using itec420.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itec420.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            List<Department> dps = _context.Departments.ToList();
            return View(dps);
        }
        public ActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateDepartment(DepartmentCreateModel model)
        {

            if (ModelState.IsValid)
            {
                var dep_entity = new Department
                {
                    DepartmentName = model.DepartmentName,
                    DepartmentDescription = model.DepartmentDescription,
                };

                _context.Departments.Add(dep_entity);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult EditDepartment(int id)
        {
            var entity = _context.Departments.Select(i => new DepartmentEditModel
            {
                Id = i.Id,
                DepartmentName = i.DepartmentName,
                DepartmentDescription = i.DepartmentDescription
            }).FirstOrDefault(i => i.Id == id);

            return View(entity);
        }

        [HttpPost]
        public ActionResult EditDepartment(int id, DepartmentEditModel model)
        {

            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = _context.Departments.FirstOrDefault(i => i.Id == model.Id);

                if (entity != null)
                {
                    entity.DepartmentName = model.DepartmentName;
                    entity.DepartmentDescription = model.DepartmentDescription;

                    _context.SaveChanges();

                    TempData["Message"] = $"{entity.DepartmentName} has been updated.";

                    return RedirectToAction("Index");
                }
            }

            return View(model);

        }

        public ActionResult DeleteDepartment(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var entity = _context.Departments.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                return View(entity);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteDepartmentConfirm(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var entity = _context.Departments.FirstOrDefault(i => i.Id == id);

            if (entity != null)
            {
                _context.Departments.Remove(entity);
                _context.SaveChanges();

                TempData["Message"] = $"{entity.DepartmentName} has been deleted.";

            }
            return RedirectToAction("Index");
        }

    }
}