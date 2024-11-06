using EmpManagement.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagement.Controllers
{
    public class HomeController : Controller
    {
        private TestDBEntities DbContext = new TestDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int? page, string textSearching)
        {
            IQueryable<tblEmp> emps;
            if (!string.IsNullOrEmpty(textSearching))
            {
                emps = from person in DbContext.tblEmps
                       orderby person.id
                       where person.name.Contains(textSearching)
                       select person;
            }
            else
            {
                emps = from person in DbContext.tblEmps
                       orderby person.id
                       select person;
            }

            //var emps = from person in DbContext.tblEmps orderby person.id select person;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(emps.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}