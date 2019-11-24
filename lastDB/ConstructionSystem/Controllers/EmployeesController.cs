using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionSystem.Models;
using ConstructionSystem.Models.Entities;

namespace ConstructionSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Department).Include(e => e.employee);
            return View(employees.ToList());
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            List<Employee> employees;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                employees = db.Employees.Include(e => e.Department).Include(e => e.employee).ToList();
            }
            else
            {
                employees = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.FirstName.StartsWith(SearchTerm) || x.LastName.StartsWith(SearchTerm)).ToList();
            }

            //var employees = db.Employees.Include(e => e.Department).Include(e => e.employee);
            return View(employees);
        }

        //Email must be Unique
        public JsonResult IsUserExists(string Email)
        {

            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.Employees.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

        ////Email must be Unique
        //public JsonResult IsPhoneExists(string Phone)
        //{

        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(!db.Employees.Any(x => x.Phone == Phone), JsonRequestBehavior.AllowGet);
        //}

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //  Employee employee = db.Employees.Find(id);
            var employee = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.EmployeeId == id).FirstOrDefault();

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.SuperId = new SelectList(db.Employees, "EmployeeId", "FirstName", employee.SuperId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = db.Employees.Include(e => e.Department).Include(e => e.employee).Where(x => x.EmployeeId == id).FirstOrDefault();

            if (employee != null)
            {
                var projects = db.EmployeeProjects.Where(x => x.EmployeeId == id);
                foreach (var item in projects)
                {
                    db.EmployeeProjects.Remove(item);
                }

                var allemps = db.Employees.Where(x => x.SuperId == employee.EmployeeId);
                foreach (var item in allemps)
                {
                    item.SuperId = null;
                }
                db.Employees.Remove(employee);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return new HttpNotFoundResult();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
